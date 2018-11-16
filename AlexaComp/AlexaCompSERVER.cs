using System;
using System.Text;
using System.Net;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

using Newtonsoft.Json;
using Mono.Nat;

namespace AlexaComp{

   class AlexaCompServerInstance : AlexaCompSERVER {

        public bool newServerFlag = false;
        
        private static TcpListener server;
        private static TcpClient client;
        private static NetworkStream nwStream;

        private static int PORT = int.Parse(AlexaComp.GetConfigValue("PORT"));
        private static string AUTH = AlexaComp.GetConfigValue("AUTH");
        private static string HOST = AlexaComp.GetConfigValue("HOST");

        public AlexaCompServerInstance() {
            AlexaCompCore.serverInstances.Add(this);
            startServer();
        }

        public void startServer() {
            try {
                IPAddress host = IPAddress.Parse(HOST);

                server = new TcpListener(host, PORT);
                server.Start(); // Start Server
                AlexaComp._log.Info("Listening...");
                Console.WriteLine("Listening...");
                if (AlexaComp.stopProgramFlag == true) {
                    return;
                }
            }
            catch (FormatException ex) {
                AlexaComp._log.Debug(ex);
            }

            acceptClient();
        }

        public void acceptClient() {
            try {
                client = server.AcceptTcpClient(); // Accept Client Connection

                try {
                    nwStream = client.GetStream(); // Get Incoming Data
                    byte[] buffer = new byte[client.ReceiveBufferSize];
                    int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize); // Read Data
                    string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead); // Convert Data to String
                    Console.WriteLine(dataReceived);
                    deserializeRequest(dataReceived);
                }
                catch (NullReferenceException ex) { AlexaComp._log.Debug(ex); }
                catch (ObjectDisposedException ex) { AlexaComp._log.Debug(ex); }
            }
            catch (SocketException) { clog("SocketException Caught when accepting client");}
            catch (NullReferenceException) { clog("NullReferenceException Caught when accepting client");}
            catch (InvalidOperationException) { clog("InvalidOperationException Caught when trying to accept tcp Client, most likely server not listening.");
                startServer();
            }
        }

        public void deserializeRequest(string data) {
            Request req = JsonConvert.DeserializeObject<Request>(data);
            string reqString = String.Format("New Request - [Command: {0}, Primary: {1}, Secondary: {2}, Tertiary: {3}]", req.COMMAND, req.PRIMARY, req.SECONDARY, req.TERTIARY);
            clog(reqString);

            if (req.AUTH != AUTH) {
                clog("Auth Invalid");
                newServerFlag = true;
                clog("destoryServerFlag set to true");
            }
            else {
                clog("Auth Valid.");
                AlexaCompREQUEST.processRequest(req);
                stopServer();
            }
        }

        /*
        * Description
        * @param json - The json string to send back to the lambda instance, usually sent from a request object.
        * @param customStream - If defined, sends the response over a provided network stream instead of the most current stream.
        */
        public static void sendToLambda(string json, NetworkStream customStream) {
            AlexaComp._log.Info("Sending Back to Lambda - " + json);
            Console.WriteLine("Sending Back to Lambda - " + json);
            byte[] message = Encoding.UTF8.GetBytes(json);
            int messageLength = Encoding.UTF8.GetBytes(json).Length;
            if (customStream != null) {
                customStream.Write(message, 0, messageLength);
            }
            else {
                nwStream.Write(message, 0, messageLength);
            }
        }

        public void stopServer() {
            AlexaComp._log.Info("Stopping Server");
            try {
                client.Close();
                AlexaComp._log.Info("Client closed");
                server.Stop();
                AlexaComp._log.Info("Server Stopped");
            }
            catch (NullReferenceException ex) {
                AlexaComp._log.Debug(ex);
            }

            if (AlexaComp.stopProgramFlag == false) {
                newServerFlag = true;
                AlexaComp._log.Info("newServerFlag Raised");
            }
        }

        public static void clog(string message) {
            AlexaCompCore._log.Info(message);
            Console.WriteLine(message);
        }
    }

    class AlexaCompSERVER : AlexaCompCore {
        // TODO: Work on abstraction.
        protected static List<AlexaCompServerInstance> serverInstances = new List<AlexaCompServerInstance>();

        protected static int PORT = int.Parse(AlexaComp.GetConfigValue("PORT"));
        protected static string AUTH = AlexaComp.GetConfigValue("AUTH");
        protected static string HOST = AlexaComp.GetConfigValue("HOST");

        private static INatDevice device;

        // PORT FORWARDING
        /*
        * Start the device discovery process
        */
        public static void forwardPort() {
            // NatUtility.DeviceFound += new EventHandler<DeviceEventArgs>(onDeviceFound);
            NatUtility.DeviceLost += new EventHandler<DeviceEventArgs>(onDeviceLost);
            AlexaComp._log.Info("Starting Port Forwarding Device Discovery");
            NatUtility.StartDiscovery();
        }

        /*
        * When device is found, forward the port set up in App.config.
        */
        public static void onDeviceFound(object sender, DeviceEventArgs args) {
            Console.WriteLine("Port Forwarding Device Found");
            device = args.Device;
            device.CreatePortMap(new Mapping(Protocol.Tcp, PORT, PORT)); // Forward Port
            AlexaComp._log.Info("Port - " + PORT.ToString() + " - Forwarded Successfully");

            foreach (Mapping portMap in device.GetAllMappings()) { // Log all portmaps
                AlexaComp._log.Info("Port Map - " + portMap.ToString());
            }
        }

        /*
        * Do when device is lost.
        */
        public static void onDeviceLost(object sender, DeviceEventArgs args) {
            device = args.Device;
            AlexaComp._log.Info("Forwarding Device Lost");
        }

        /*
        * Deletes all port maps.
        */
        public static void delPortMap() {
            try {
                device.DeletePortMap(new Mapping(Protocol.Tcp, PORT, PORT));
            } catch (MappingException ex) {
                AlexaComp._log.Debug(ex.ToString());
            }
        }

        /*
        * Checks to see if the server is stopped, if true, restarts it.
        */
        /*
        public static void ServerLoop(){
            if (AlexaComp.stopProgramFlag == false) {
                while (true){
                    foreach (AlexaCompServerInstance instance in serverInstances) {
                        Thread.Sleep(150);
                        bool nServerFlag = instance.newServerFla
                        if (instance. == true) {
                            AlexaComp._log.Info("Active Threads - " + Process.GetCurrentProcess().Threads.Count.ToString());
                            Thread StartServerThread = new Thread(startServer);
                            StartServerThread.Name = "startServerThread";
                            StartServerThread.Start();
                            newServerFlag = false;
                        }
                    }
                }
            }
        }*/
    }
}
