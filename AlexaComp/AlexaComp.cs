using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using OpenHardwareMonitor.Hardware;
using System.Threading;
using System.Linq;

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

        [STAThread]
        static void Main(){
            XmlConfigurator.Configure();
            _log.Info("Start Program");
            AlexaCompMQTT.makeMQTT();
            
            //Start Threads
            Thread StartAppWindowThread = new Thread(AlexaComp_Config_Window.StartAppWindow);
            Thread StartServerThread = new Thread(AlexaCompSERVER.startServer);

            StartAppWindowThread.Name = "startAppWindowThread";
            StartServerThread.Name = "startServerThread";

            StartAppWindowThread.Start();
            StartServerThread.Start();
        }

        public static string GetConfigValue(string key) {
            return ConfigurationManager.AppSettings[key];
        }
        
        static void GetTemperaturesInCelsius(Computer _computer){
            Console.WriteLine("Get Temperatures Started");
            var coreAndTemperature = new Dictionary<string, float>();

            foreach (var hardware in _computer.Hardware){
                hardware.Update(); //use hardware.Name to get CPU model
                Console.WriteLine(hardware.Name);
                foreach (var sensor in hardware.Sensors){
                    Console.WriteLine(sensor);
                    Console.WriteLine(sensor.Value);
                    Console.WriteLine(sensor.SensorType);
                    Console.WriteLine(sensor.Name);
                    if (sensor.SensorType == SensorType.Temperature && sensor.Value.HasValue){
                        Console.WriteLine(sensor.Value);
                    }          
                }
            }
        }

        public static string GetProgramPath(string program){
            XmlDocument doc = new XmlDocument();
            doc.Load("pathDir.xml");
            _log.Info("pathDIR loaded");
            XmlElement elem = doc.SelectSingleNode("//path[@programName='" + program + "']") as XmlElement;
            if (elem != null){
                string path = elem.GetAttribute("programPath");
                _log.Info("Path - " + path);
                return path;
            }
            else{
                _log.Info("null returned");
                return null;
            }
        }
    }
}

