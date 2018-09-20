using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using OpenHardwareMonitor.Hardware;
using System.Threading;
using System.Linq;
using Microsoft.Win32;

using log4net;
using log4net.Config;

// TODO: Implement MQTT for server response
// TODO: Add calls to mqttPublish
// TODO: Figure out Hardwaremonitor

namespace AlexaComp {
    public class AlexaComp {

        private static string exePath = System.Reflection.Assembly.GetEntryAssembly().Location;

        private static string[] splitExePath = exePath.Split('\\').ToArray();
        private List<Thread> ServerThreads = new List<Thread>();

        public static readonly ILog _log = LogManager.GetLogger(typeof(AlexaComp));
        public static string pathToDebug = string.Join("\\\\", splitExePath.Reverse().Skip(1).Reverse());
        public static string pathToProject = string.Join("\\\\", splitExePath.Reverse().Skip(3).Reverse());

        public static Thread AppWindowThread = new Thread(AlexaComp_Config_Window.StartAppWindow);
        public static Thread ServerThread = new Thread(AlexaCompSERVER.startServer);
        public static Thread ServerLoopThread = new Thread(AlexaCompSERVER.ServerLoop);

        public static bool stopProgramFlag = false;
        public static Dictionary<string, string> settingsDict = new Dictionary<string, string>();

        public static List<string> postQueue = new List<string>();
        public static bool postQueueFlag = false;


        [STAThread]
        static void Main(string[] args) {
            XmlConfigurator.Configure();
            _log.Info("Start Program");
            foreach (string arg in args){
                if ("--LogSensors".Contains(arg)){
                    Thread LogSensorsThread = new Thread(new ParameterizedThreadStart(AlexaCompHARDWARE.getAllSensors));
                    LogSensorsThread.Start(false);
                }
            }
            readConfig();
            verifyConfig();

            AppWindowThread.Name = "AppWindowThread";
            AppWindowThread.Start();
            // Log Paths
            _log.Info(exePath.ToString());
            _log.Info("PathToDebug - " + pathToDebug);
            _log.Info("PathToProject - " + pathToProject);

            //Start Threads
            ServerThread.Name = "ServerThread";
            ServerLoopThread.Name = "ServerLoopThread";
            
            ServerThread.Start();
            ServerLoopThread.Start();

            try{
                AlexaCompMQTT.makeMQTT();
            }
            catch (TypeInitializationException){
                _log.Info("TypeInitializationException Caught during attempt to make MQTT, most likely no settings are configured");
            }
            
        }

        public static void verifyConfig() {
            foreach (KeyValuePair<string, string> pair in settingsDict){
                Console.WriteLine("pair: '" + pair.Key + "' - " + pair.Value);
                if (pair.Value == "" || pair.Value == " " || pair.Value == "null"){
                    _log.Fatal("Config Value" + pair.Key + " not configured correctly or are missing. AlexaComp will not work.");
                }
            }
        }
        

        public static string GetConfigValue(string key) {
            return ConfigurationManager.AppSettings[key];
        }

        public static void UpdateConfigValue(string key, string value){
            Console.WriteLine(key + " - " + value);
            ExeConfigurationFileMap debugMap = new ExeConfigurationFileMap { ExeConfigFilename = pathToDebug + "\\AlexaComp.exe.config" };
            ExeConfigurationFileMap projectMap = new ExeConfigurationFileMap { ExeConfigFilename = pathToProject + "\\App.config" };
            var config = ConfigurationManager.OpenMappedExeConfiguration(debugMap, ConfigurationUserLevel.None);
            var config2 = ConfigurationManager.OpenMappedExeConfiguration(projectMap, ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove(key);
            config2.AppSettings.Settings.Remove(key);
            config.AppSettings.Settings.Add(key, value);
            config2.AppSettings.Settings.Add(key, value);
            config.Save(ConfigurationSaveMode.Modified);
            config2.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("appSettings");
        }

        public static void readConfig() {
            foreach (string key in ConfigurationManager.AppSettings.AllKeys) {
                settingsDict[key] = GetConfigValue(key);
            }
        }
    }
}

