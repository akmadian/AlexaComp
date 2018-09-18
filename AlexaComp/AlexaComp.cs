using System;
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
    public class AlexaComp{

        private static string exePath = System.Reflection.Assembly.GetEntryAssembly().Location;
        private static string[] splitExePath = exePath.Split('\\').ToArray();
        private List<Thread> ServerThreads = new List<Thread>();
    
        public static readonly ILog _log = LogManager.GetLogger(typeof(AlexaComp));
        public static string pathToDebug = string.Join("\\", splitExePath.Reverse().Skip(1).Reverse()) + "\\pathDir.xml";
        public static string pathToProject = string.Join("\\", splitExePath.Reverse().Skip(3).Reverse()) + "\\pathDir.xml";

        public static Thread AppWindowThread = new Thread(AlexaComp_Config_Window.StartAppWindow);
        public static Thread ServerThread = new Thread(AlexaCompSERVER.startServer);
        public static Thread ServerLoopThread = new Thread(AlexaCompSERVER.ServerLoop);

        public static bool stopProgramFlag = false;

        [STAThread]
        static void Main(string[] args){
            foreach (string arg in args){
                if ("--LogSensors".Contains(arg)){
                    Thread LogSensorsThread = new Thread(LogAllSensors);
                    LogSensorsThread.Start();
                }
            }

            XmlConfigurator.Configure();
            _log.Info("Start Program");
            AlexaCompMQTT.makeMQTT();
            
            //Start Threads
            AppWindowThread.Name = "AppWindowThread";
            ServerThread.Name = "ServerThread";
            ServerLoopThread.Name = "ServerLoopThread";

            AppWindowThread.Start();
            ServerThread.Start();
            ServerLoopThread.Start();
            

            /*
            while (true)
            { 
                float usedMem = AlexaCompHARDWARE.GpuStat("Used Memory");
                Console.ReadLine();
                Console.WriteLine(usedMem);
            }*/
        }

        public static void LogAllSensors(){
            AlexaCompHARDWARE.getAllSensors("CPU");
            AlexaCompHARDWARE.getAllSensors("GPU");
            AlexaCompHARDWARE.getAllSensors("RAM");
            AlexaCompHARDWARE.getAllSensors("MAINBOARD");
            AlexaCompHARDWARE.getAllSensors("HDD");
            AlexaCompHARDWARE.getAllSensors("FANCONTROLLER");
        }

        public static string GetConfigValue(string key) {
            return ConfigurationManager.AppSettings[key];
        }
    }
}

