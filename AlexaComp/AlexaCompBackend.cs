using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Configuration;



namespace AlexaComp
{
    class Program
    {

        string HOST = GetConfigValue("HOST");
        int PORT = int.Parse(GetConfigValue("PORT"));

        static void Main(string[] args)
        {
            Console.WriteLine("START");

            loopXml();

            string HOST = GetConfigValue("HOST");
            int PORT = int.Parse(GetConfigValue("PORT"));
            Main2(HOST, PORT);
        }

        public static void loopXml()
        {
            string xmlString = System.IO.File.ReadAllText("pathDir.xml");
            XmlReader rdr = XmlReader.Create(new System.IO.StringReader(xmlString));

            while (rdr.read())
            {
                if (rdr.NodeType == XmlNodeType.Element)
                {
                    Console.WriteLine(rdr.LocalName);
                }
            }

        }

        public static string GetConfigValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }


        static void Main2(string HOST, int PORT)
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
            Console.WriteLine("RECEIVED : " + dataReceived);

            /*---write back the text to the client---
            Console.WriteLine("Sending back : " + dataReceived);
            nwStream.Write(buffer, 0, bytesRead);
            client.Close();
            listener.Stop();
            Console.ReadLine();*/
        }
    }
}
