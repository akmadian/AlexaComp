using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using System.Xml;
using OpenHardwareMonitor.Hardware;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

using log4net;
using log4net.Config;
using Newtonsoft.Json;
using uPLibrary.Networking.M2Mqtt;

// TODO: Look into threading for running server and GUI at the same time


namespace AlexaComp { 
    class AlexaCompBackend{

        public List<Thread> ServerThreads = new List<Thread>();
        private static readonly ILog _log = LogManager.GetLogger(typeof(AlexaCompBackend));
        public static bool newServerFlag = false;
        public static MqttClient mqttClient = new MqttClient("m11.cloudmqtt.com:15387");

        [STAThread]
        static void Main(){
            XmlConfigurator.Configure();
            _log.Info("Start Program 1");
            
            byte code = mqttClient.Connect(Guid.NewGuid().ToString(), GetConfigValue("mqttUSER"), GetConfigValue("mqttPASS"));
            mqttClient.Subscribe(new string[] { "AlexaComp" }, new byte[] { 2 });

            Thread StartAppWindowThread = new Thread(StartAppWindow);
            StartAppWindowThread.Start();

            Thread StartServerThread = new Thread(StartServer);
            StartServerThread.Start();

            Thread newServerFlagThread = new Thread(ServerLoop);
            newServerFlagThread.Start();

            // Computer _computer = new Computer() { CPUEnabled = true};
            // _computer.Open();
            // GetTemperaturesInCelsius(_computer);
        }

        public static void StartAppWindow(){
            _log.Info("StartAppWinodw Thread Started");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new AlexaComp_Config_Window());
            _log.Info("StartAppWindow Task Completed");
        }

        public static string GetConfigValue(string key) {
            return ConfigurationManager.AppSettings[key];
        }

        public static void ServerLoop(){
            while (true){
                if (newServerFlag == true){
                    _log.Info("Active Threads - " + Process.GetCurrentProcess().Threads.Count.ToString());
                    Thread StartServerThread = new Thread(StartServer);
                    StartServerThread.Start();
                    newServerFlag = false;
                } 
            }
        }
        
        public static void GetTemperaturesInCelsius(Computer _computer){
            Console.WriteLine("Get Temperatures Started");
            var coreAndTemperature = new Dictionary<string, float>();

            foreach (var hardware in _computer.Hardware){
                hardware.Update(); //use hardware.Name to get CPU model
                Console.WriteLine(hardware.Name);
                foreach (var sensor in hardware.Sensors)
                {
                    Console.WriteLine(sensor);
                    Console.WriteLine(sensor.Value);
                    Console.WriteLine(sensor.SensorType);
                    Console.WriteLine(sensor.Name);
                    if (sensor.SensorType == SensorType.Temperature && sensor.Value.HasValue)
                    {
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

        public static void closeServer(TcpListener server, TcpClient client){
            client.Close();
            server.Stop();
            _log.Info("Server Stopped");
            newServerFlag = true;
            _log.Info("newServerFlag Raised");
        }

        public static void StartServer(){
            _log.Info("Active Threads - " + Process.GetCurrentProcess().Threads.Count.ToString());
            string HOST = GetConfigValue("HOST");
            int PORT = int.Parse(GetConfigValue("PORT"));
            string AUTH = GetConfigValue("AUTH");

            IPAddress localAdd = IPAddress.Parse(HOST); // Listen at Specified IP and Port
            TcpListener listener = new TcpListener(localAdd, PORT);
            Console.WriteLine("Listening...");
            _log.Info("Listening...");
            listener.Start();
            
            TcpClient client = listener.AcceptTcpClient(); // Client Connected
            _log.Info("Client Connected");

            NetworkStream nwStream = client.GetStream(); // Get Incoming Data
            byte[] buffer = new byte[client.ReceiveBufferSize];

            int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize); // Read Data

            string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead); // Convert Data to String

            alexaRequest req = JsonConvert.DeserializeObject<alexaRequest>(dataReceived);
            _log.Info("New Request - {Command: " + req.COMMAND + ", Primary: " + req.PRIMARY +
                      ", Secondary: " + req.SECONDARY + "}");

            if (req.AUTH != AUTH){
                _log.Info("Auth Invalid");
                newServerFlag = true;
                _log.Info("newServerFlag set to true");
            }
            else{
                _log.Info("Auth Valid");
                if (req.COMMAND == "LAUNCH"){ // Launch Program
                    try{
                        string programPath = GetProgramPath(req.PRIMARY);
                        _log.Info("ProgramPath - " + programPath);
                        _log.Info("req Program - " + req.PRIMARY);
                        Process.Start(GetProgramPath(req.PRIMARY));
                        _log.Info("Program Launched");
                        byte code = mqttClient.Connect(Guid.NewGuid().ToString(), GetConfigValue("mqttUSER"), GetConfigValue("mqttPASS"));
                        ushort msgID = mqttClient.Publish("AlexaComp/confirmations", Encoding.UTF8.GetBytes("1"));
                        closeServer(listener, client); // Restart Server to Handle Next Request
                    }
                    catch (NullReferenceException){
                        _log.Error("NullReferenceException caught during attempt to launch program, " +
                            "null most likely returned from GetProgramPath.");
                        closeServer(listener, client); 
                    }
                    catch (System.ComponentModel.Win32Exception e){
                        _log.Error("Win32Exception Caught during attempt to launch program " + e.Message);
                        closeServer(listener, client); 
                    }
                }

                else if (req.COMMAND == "GETCOMPSTAT"){ // Get Hardware Stat
                    // hardwaremonitor code here
                }

                else if (req.COMMAND == "COMPUTERCOMMAND"){ // Issue Command
                    if (req.PRIMARY == "SHUTDOWN"){
                        Process.Start("shutdown", "/s /t 0");
                    } else if (req.PRIMARY == "RESTART"){
                        Process.Start("shutdown", "/r /t 0");
                    } else if (req.PRIMARY == "SLEEP"){
                        // Sleep Code Here
                    }
                }
            }
            /*---write back the text to the client---
            Console.WriteLine("Sending back : " + dataReceived);
            nwStream.Write(buffer, 0, bytesRead);
            client.Close();
            listener.Stop();
            Console.ReadLine();*/
        }
    }

    public class alexaRequest{
        public string AUTH { get; set; }
        public string COMMAND { get; set; }
        public string PRIMARY { get; set; }
        public string SECONDARY { get; set; }
    } 
}
