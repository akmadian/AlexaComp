using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using System.Xml;
using System.Management;
using OpenHardwareMonitor.Hardware;

using Newtonsoft.Json;

// TODO: Look into threading for running server and GUI at the same time


namespace AlexaComp{
    static class AlexaCompBackend{

        [STAThread]
        static void Main(){
            Console.WriteLine("START");

            Computer _computer = new Computer() { CPUEnabled = true};
            _computer.Open();
            GetTemperaturesInCelsius(_computer);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string HOST = GetConfigValue("HOST");
            int PORT = int.Parse(GetConfigValue("PORT"));
            string AUTH = GetConfigValue("AUTH");
            Server(HOST, PORT, AUTH);

            Application.Run(new AlexaComp_Config_Window());
          
        }

        public static void GetTemperaturesInCelsius(Computer _computer)
        {
            Console.WriteLine("Get Temperatures Started");
            var coreAndTemperature = new Dictionary<string, float>();

            foreach (var hardware in _computer.Hardware)
            {
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

            public static string GetConfigValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }


        public static string GetProgramPath(string program)
        {

            XmlDocument doc = new XmlDocument();
            doc.Load("pathDir.xml");
            XmlElement elem = doc.SelectSingleNode("//path[@programName='" + program + "']") as XmlElement;
            if (elem != null)
            {
                string path = elem.GetAttribute("programPath");
                return path;
            }
            else
            {
                return null;
            }
        }

        static void Server(string HOST, int PORT, string AUTH)
        {
            //---listen at the specified IP and port no.---
            IPAddress localAdd = IPAddress.Parse(HOST);
            TcpListener listener = new TcpListener(localAdd, PORT);
            Console.WriteLine("Listening...");
            listener.Start();

            //---incoming client connected---
            TcpClient client = listener.AcceptTcpClient();

            //---get the incoming data through a network stream---
            NetworkStream nwStream = client.GetStream();
            byte[] buffer = new byte[client.ReceiveBufferSize];

            //---read incoming stream---
            int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

            //---convert the data received into a string---
            string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            alexaRequest req = JsonConvert.DeserializeObject<alexaRequest>(dataReceived);

            if (req.AUTH != AUTH)
            {
                Console.WriteLine("Auth Invalid");
            }
            else
            {
                Console.WriteLine("Auth Valid");
                if (req.COMMAND == "LAUNCH")
                {
                    try
                    {
                        System.Diagnostics.Process.Start("start " + GetProgramPath(req.PROGRAM));
                        client.Close();
                        listener.Stop();
                        Server(HOST, PORT, AUTH);
                    }
                    catch (NullReferenceException)
                    {
                        Console.WriteLine("NullReferenceException caught during attempt to launch program, " +
                            "null most likely returned from GetProgramPath.");
                        client.Close();
                        listener.Stop();
                        Server(HOST, PORT, AUTH);
                    }
                }
                else if (req.COMMAND == "GETCOMPSTAT")
                {
                    // hardwaremonitor code here
                }
            }

            Console.WriteLine("RECEIVED : " + dataReceived);

            /*---write back the text to the client---
            Console.WriteLine("Sending back : " + dataReceived);
            nwStream.Write(buffer, 0, bytesRead);
            client.Close();
            listener.Stop();
            Console.ReadLine();*/
        }
    }

    public class alexaRequest
    {
        public string AUTH { get; set; }
        public string COMMAND { get; set; }
        public string PROGRAM { get; set; }
    }
}
