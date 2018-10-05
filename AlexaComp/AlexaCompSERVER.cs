using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

using Newtonsoft.Json;
using Mono.Nat;

namespace AlexaComp{
    class AlexaCompSERVER{
        // TODO: Work on abstraction.

        private static bool newServerFlag = false;
        private static TcpListener server;
        private static TcpClient client;
        private static NetworkStream nwStream;

        private static int PORT = int.Parse(AlexaComp.GetConfigValue("PORT"));
        private static INatDevice device;

        public static void startServer() {
            AlexaComp._log.Info("Server Thread Started");
            AlexaComp._log.Info("Active Threads - " + Process.GetCurrentProcess().Threads.Count.ToString());

            string AUTH = AlexaComp.GetConfigValue("AUTH");
            string HOST = AlexaComp.GetConfigValue("HOST");
            try {
                IPAddress host = IPAddress.Parse(HOST);

                server = new TcpListener(host, PORT);
                server.Start(); // Start Server
                AlexaComp._log.Info("Listening...");
                Console.WriteLine("Listening...");
                if (AlexaComp.stopProgramFlag == true) {
                    return;
                }
            } catch (FormatException ex) {
                AlexaComp._log.Debug(ex);
            }

            try {
                client = server.AcceptTcpClient(); // Accept Client Connection

            } catch (SocketException) { AlexaComp._log.Debug("SocketException Caught when accepting client");
            } catch (NullReferenceException) { AlexaComp._log.Debug("NullReferenceException Caught when accepting client");
            } catch (InvalidOperationException) { AlexaComp._log.Debug("InvalidOperationException Caught when trying to accept tcp Client, most likely server not listening.");
                startServer();
            }
            
            AlexaComp._log.Info("Client Connected");

            try{
                nwStream = client.GetStream(); // Get Incoming Data
                byte[] buffer = new byte[client.ReceiveBufferSize];
                int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize); // Read Data
                string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead); // Convert Data to String
                Console.WriteLine(dataReceived);
                
                Request req = JsonConvert.DeserializeObject<Request>(dataReceived);
                AlexaComp._log.Info("New Request - {Command: " + req.COMMAND + ", Primary: " + req.PRIMARY +
                          ", Secondary: " + req.SECONDARY + ", Tertiary: " + req.TERTIARY + "}");
                Console.WriteLine("New Request - {Command: " + req.COMMAND + ", Primary: " + req.PRIMARY +
                          ", Secondary: " + req.SECONDARY + ", Tertiary: " + req.TERTIARY + "}");
                
                if (req.AUTH != AUTH){
                    AlexaComp._log.Info("Auth Invalid");
                    newServerFlag = true;
                    AlexaComp._log.Info("newServerFlag set to true");
                } else {
                    AlexaComp._log.Info("Auth Valid");
                    Console.WriteLine("Auth Valid");
                    if (req.COMMAND == "DEVICELINK") {
                        Console.WriteLine("Devicelinkrequest");
                        Options opt = new Options(req, nwStream);
                        Thread DeviceLinkingThread = new Thread(new ParameterizedThreadStart(DeviceLinkingForm.startDeviceLinking));
                        DeviceLinkingThread.Start(opt);
                        Thread.Sleep(8000);
                    } else {
                        Console.WriteLine("not devicelink");
                    }

                    AlexaCompREQUEST.processRequest(req);
                    stopServer();
                }
            } catch (NullReferenceException ex){
                AlexaComp._log.Debug(ex);
            } catch (ObjectDisposedException ex){
                AlexaComp._log.Debug(ex);
            }
        }

        public static void sendToLambda(string json, NetworkStream customStream) {
            AlexaComp._log.Info("Sending Back to Lambda - " + json);
            Console.WriteLine("Sending Back to Lambda - " + json);
            byte[] message = Encoding.UTF8.GetBytes(json);
            int messageLength = Encoding.UTF8.GetBytes(json).Length;
            if (customStream != null) {
                customStream.Write(message, 0, messageLength);
            } else {
                nwStream.Write(message, 0, messageLength);
            }
        }

        // Port Forwarding
        public static void onDeviceFound(object sender, DeviceEventArgs args) {
            Console.WriteLine("Port Forwarding Device Found");
            device = args.Device;
            device.CreatePortMap(new Mapping(Protocol.Tcp, PORT, PORT)); // Forward Port
            AlexaComp._log.Info("Port - " + PORT.ToString() + " - Forwarded Successfully");

            foreach (Mapping portMap in device.GetAllMappings()) {
                AlexaComp._log.Info("Port Map - " + portMap.ToString());
            }
        }

        public static void onDeviceLost(object sender, DeviceEventArgs args) {
            device = args.Device;
            AlexaComp._log.Info("Forwarding Device Lost");
        }

        public static void delPortMap() {
            try {
                device.DeletePortMap(new Mapping(Protocol.Tcp, PORT, PORT));
            } catch (MappingException ex) {
                AlexaComp._log.Debug(ex.ToString());
            }
        }

        public static void forwardPort() {
            NatUtility.DeviceFound += new EventHandler<DeviceEventArgs>(onDeviceFound);
            NatUtility.DeviceLost += new EventHandler<DeviceEventArgs>(onDeviceLost);
            NatUtility.StartDiscovery();
            AlexaComp._log.Info("Starting Port Forwarding Device Discovery");
        }

        // Server Management
        public static void stopServer(){
            AlexaComp._log.Info("Stopping Server");
            try{
                client.Close();
                AlexaComp._log.Info("Client closed");
                server.Stop();
                AlexaComp._log.Info("Server Stopped");
            } catch (NullReferenceException ex) {
                AlexaComp._log.Debug(ex);
            }
            
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
