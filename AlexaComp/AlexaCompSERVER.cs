using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

using Newtonsoft.Json;

namespace AlexaComp
{
    class AlexaCompSERVER{
        public static bool newServerFlag = false;
        public static TcpListener server;
        public static TcpClient client;

        public static void startServer(){
            AlexaComp._log.Info("Server Thread Started");
            AlexaComp._log.Info("Active Threads - " + Process.GetCurrentProcess().Threads.Count.ToString());

            string AUTH = AlexaComp.GetConfigValue("AUTH");
            string HOST = AlexaComp.GetConfigValue("HOST");
            try{
                int PORT = int.Parse(AlexaComp.GetConfigValue("PORT"));
                IPAddress host = IPAddress.Parse(HOST);

                server = new TcpListener(host, PORT);
                server.Start(); // Start Server
                AlexaComp._log.Info("Listening");
                if (AlexaComp.stopProgramFlag == true){
                    return;
                }
            }
            catch (FormatException) {
                AlexaComp._log.Debug("FormatException Caught during attempt to read PORT value from config, most likely no value configured");
            }

            try{
                client = server.AcceptTcpClient(); // Accept Client Connection
            } catch (SocketException) {
                AlexaComp._log.Debug("SocketException Caught when accepting client");
            } catch (NullReferenceException){
                AlexaComp._log.Debug("NullReferenceException Caught when accepting client");
            }
            AlexaComp._log.Info("Client Connected");
            try
            {
                NetworkStream nwStream = client.GetStream(); // Get Incoming Data
                byte[] buffer = new byte[client.ReceiveBufferSize];
                int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize); // Read Data
                string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead); // Convert Data to String

                Request req = JsonConvert.DeserializeObject<Request>(dataReceived);
                AlexaComp._log.Info("New Request - {Command: " + req.COMMAND + ", Primary: " + req.PRIMARY +
                          ", Secondary: " + req.SECONDARY + "}");

                if (req.AUTH != AUTH)
                {
                    AlexaComp._log.Info("Auth Valid");
                    newServerFlag = true;
                    AlexaComp._log.Info("newServerFlag set to true");
                }
                else
                {
                    AlexaComp._log.Info("Auth Valid");
                    AlexaCompREQUEST.processRequest(req);
                    stopServer();
                }
            } catch (NullReferenceException)
            {
                AlexaComp._log.Debug("NullReferenceException Caught in AlexaCompSERVER");
            }
        }

        public static void stopServer(){
            AlexaComp._log.Info("Stopping Server");
            try{
                client.Close();
                AlexaComp._log.Info("Client closed");
            } catch (NullReferenceException){
                AlexaComp._log.Debug("NullReferenceException when closing client, object is null or does not exist");
            }
            try{
                server.Stop();
            } catch (NullReferenceException){
                AlexaComp._log.Debug("NullReferenceException caught when stopping server");
            }
            AlexaComp._log.Info("Server Stopped");
            if (AlexaComp.stopProgramFlag == false){
                newServerFlag = true;
                AlexaComp._log.Info("newServerFlag Raised");
            }
        }

        public static void ServerLoop(){
            if (AlexaComp.stopProgramFlag == false) {
                while (true){
                    Thread.Sleep(150);
                    if (newServerFlag == true){
                        AlexaComp._log.Info("Active Threads - " + Process.GetCurrentProcess().Threads.Count.ToString());
                        Thread StartServerThread = new Thread(startServer);
                        StartServerThread.Name = "startServerThread";
                        StartServerThread.Start();
                        newServerFlag = false;
                    }
                }
            }
        }
    }
}
