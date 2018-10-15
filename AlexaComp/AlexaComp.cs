using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Linq;
using System.Diagnostics;
using Microsoft.Win32;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.XPath;

using log4net;
using log4net.Config;

// TODO: Implement MQTT for server response
// TODO: Experiment with HTTP responses instead of MQTT
// TODO: Documentation

namespace AlexaComp {
    public class AlexaComp {

        // Paths
        private static string exePath = System.Reflection.Assembly.GetEntryAssembly().Location;
        private static string[] splitExePath = exePath.Split('\\').ToArray();
        public static string pathToDebug = string.Join("\\\\", splitExePath.Reverse().Skip(1).Reverse());
        public static string pathToProject = string.Join("\\\\", splitExePath.Reverse().Skip(3).Reverse());

        // Threads
        public static Thread AppWindowThread = new Thread(AlexaCompGUI.StartAppWindow);
        public static Thread ServerThread = new Thread(AlexaCompSERVER.startServer);
        public static Thread ServerLoopThread = new Thread(AlexaCompSERVER.ServerLoop);
        private static Thread LoadingScreenThread = new Thread(LoadingScreenForm.startLoadingScreen);

        // Misc
        public static bool updateLogBoxFlag = false;
        public static bool stopProgramFlag = false;
        public static Dictionary<string, string> settingsDict = new Dictionary<string, string>();
        public static readonly ILog _log = LogManager.GetLogger(typeof(AlexaComp));


        [STAThread]
        static void Main(string[] args) {
            XmlConfigurator.Configure();
            _log.Info("Start Program");

            foreach (string arg in args) {
                if ("--LogSensors".Contains(arg)) {
                    Thread LogSensorsThread = new Thread(new ParameterizedThreadStart(AlexaCompHARDWARE.getAllSensors));
                    LogSensorsThread.Start(false);
                }
            }
            getExternalIP();
            ServerThread.Name = "ServerThread";
            ServerLoopThread.Name = "ServerLoopThread";
            AppWindowThread.Name = "AppWindowThread";

            LoadingScreenThread.Start();

            // Log Paths
            _log.Info(exePath.ToString());
            _log.Info("PathToDebug - " + pathToDebug);
            _log.Info("PathToProject - " + pathToProject);
            
        }

        /// <summary>
        /// Reads all user configured values into a dictionary.
        /// </summary>
        public static void readConfig(){
            foreach (string key in ConfigurationManager.AppSettings.AllKeys){
                settingsDict[key] = GetConfigValue(key);
            }
        }

        public static void getExternalIP() {
            string data = new WebClient().DownloadString("http://checkip.dyndns.org/");
            Match match = Regex.Match(data, @"\b(?:[0-9]{1,3}\.){3}[0-9]{1,3}\b");
            if (match.Success) {
                _log.Info("extip - " + match);
            }
        }

        /// <summary>
        /// Verifies that the necessary config values are entered and correctly formatted.
        /// </summary>
        public static void verifyConfig() {
            foreach (KeyValuePair<string, string> pair in settingsDict){
                Console.WriteLine("pair: '" + pair.Key + "' - " + pair.Value);
                if (pair.Value == "" || pair.Value == " " || pair.Value == "null"){
                    _log.Fatal("Config Value" + pair.Key + " not configured correctly or are missing. AlexaComp will not work.");
                } else if (pair.Key == "HOST"){ // Is Host IP formatted properly?
                    System.Net.IPAddress ipAdd = null;
                    bool isValidIp = System.Net.IPAddress.TryParse(pair.Value, out ipAdd);
                    if (isValidIp != true){
                        _log.Fatal("Config value \"HOST\" exists, but is not configured or formatted properly.");
                    }
                } 
            }
        }
        
        /// <summary>
        /// Gets a config value.
        /// </summary>
        /// <param name="key">The name or key of the config value to get.</param>
        /// <returns></returns>
        public static string GetConfigValue(string key) {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// Updates a config value of name {key} with value {value}.
        /// </summary>
        /// <param name="key">The name of the config value to update.</param>
        /// <param name="value">The value to update to</param>
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

        public static void inventoryPrograms() {
            string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key)) {
                foreach (string subkey_name in key.GetSubKeyNames()) {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name)) {
                        if (subkey.GetValue("DisplayName") != null) {
                            string subkeyValue = subkey.GetValue("DisplayName").ToString();
                            if (subkeyValue.Contains("Microsoft")  || subkeyValue.Contains("SDK") || 
                                subkeyValue.Contains("WinRT") || subkeyValue.Contains("vs_") ||
                                subkeyValue.Contains("Visual C++") || subkeyValue.Contains("CRT") ||
                                subkeyValue.Contains("Windows")){
                                
                            } else {
                                Console.WriteLine(subkey.GetValue("DisplayName"));
                            }
                        }
                    }
                }
            }
        }
    }

    public class Options {
        public Request req;
        public System.Net.Sockets.NetworkStream nwStream;

        public Options(Request req_, System.Net.Sockets.NetworkStream nwStream_) {
            req = req_;
            nwStream = nwStream_;
        }
    }
}

