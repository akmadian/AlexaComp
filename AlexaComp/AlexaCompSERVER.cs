using System;
using System.Text;
using System.Net;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

using Newtonsoft.Json;
using Mono.Nat;

using AlexaComp.Core;

namespace AlexaComp{

    class AlexaCompSERVER : AlexaCompCore {
        // TODO: Work on abstraction.
        #region Properties
        public static bool newServerFlag = false;

        public static TcpListener server;
        public static TcpClient client;
        private static NetworkStream nwStream;

        public static int PORT = int.Parse(GetConfigValue("PORT"));
        public static string AUTH = GetConfigValue("AUTH");
        public static string HOST = "10.0.0.59";

        private static INatDevice device;
        #endregion

        #region Methods
        public static void StartServer() {
            try {
                IPAddress host = IPAddress.Parse(HOST);
                server = new TcpListener(host, PORT);
                server.Start(); // Start Server
                Clog("Server Started");
                Clog("Listening...");
                if (stopProgramFlag == true) {
                    return;
                }
            } catch (SocketException ex) {
                System.Windows.Forms.MessageBox.Show("AlexaComp Encountered a Fatal Error and Had To Stop.\n" +
                    "Please Check The AlexaComp Log File For More Information.\n" + 
                    "EXCEPTION: " + ex.ToString());
                Environment.Exit(1);
            }
            catch (FormatException ex) {
                AlexaComp._log.Debug(ex);
            }

            AcceptClient();
        }

        public static void AcceptClient() {
            try {
                client = server.AcceptTcpClient(); // Accept Client Connection
                Stopwatch reqTimer = new Stopwatch();
                reqTimer.Start();

                try {
                    nwStream = client.GetStream(); // Get Incoming Data
                    byte[] buffer = new byte[client.ReceiveBufferSize];
                    int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize); // Read Data
                    string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead); // Convert Data to String
                    Clog(dataReceived);
                    DeserializeRequest(dataReceived, reqTimer);
                }
                catch (NullReferenceException ex) { AlexaComp._log.Debug(ex); }
                catch (ObjectDisposedException ex) { AlexaComp._log.Debug(ex); }
            }
            catch (SocketException) { Clog("SocketException Caught when accepting client"); }
            catch (NullReferenceException) { Clog("NullReferenceException Caught when accepting client"); }
            catch (InvalidOperationException) {
                Clog("InvalidOperationException Caught when trying to accept tcp Client, most likely server not listening.");
                StartServer();
            }
        }

        public static void DeserializeRequest(string data, Stopwatch timer) {
            Request req = JsonConvert.DeserializeObject<Request>(data);
            req.sw = timer;
            string reqString = String.Format("New Request - [Command: {0}, Primary: {1}, Secondary: {2}, Tertiary: {3}]", req.COMMAND, req.PRIMARY, req.SECONDARY, req.TERTIARY);
            Clog(reqString);

            if (req.AUTH != AUTH) {
                Clog("Auth Invalid");
                newServerFlag = true;
                Clog("destoryServerFlag set to true");
            }
            else {
                Clog("Auth Valid.");
                req.ProcessRequest(req);
                StopServer();
            }
        }

        public static void StopServer() {
            Clog("Stopping Server");
            try {
                client.Close();
                Clog("Client closed");
                server.Stop();
                Clog("Server Stopped");
            }
            catch (NullReferenceException ex) {
                AlexaComp._log.Debug(ex);
            }

            if (stopProgramFlag == false) {
                newServerFlag = true;
                Clog("newServerFlag Raised");
            }
        }

        /*
        * Description
        * @param json - The json string to send back to the lambda instance, usually sent from a request object.
        * @param customStream - If defined, sends the response over a provided network stream instead of the most current stream.
        */
        public static void SendToLambda(string json, NetworkStream customStream) {
            Clog("Sending Back To Lambda - " + json);
            byte[] message = Encoding.UTF8.GetBytes(json);
            int messageLength = Encoding.UTF8.GetBytes(json).Length;
            if (customStream != null) {
                customStream.Write(message, 0, messageLength);
            }
            else {
                nwStream.Write(message, 0, messageLength);
            }
        }

        #region PortForwarding
        /*
        * Start the device discovery process
        */
        public static void ForwardPort() {
            NatUtility.DeviceFound += new EventHandler<DeviceEventArgs>(OnDeviceFound);
            NatUtility.DeviceLost += new EventHandler<DeviceEventArgs>(OnDeviceLost);
            Clog("Starting Port Forwarding Device Discovery");
            NatUtility.StartDiscovery();
        }

        /*
        * When device is found, forward the port set up in App.config.
        */
        public static void OnDeviceFound(object sender, DeviceEventArgs args) {
            Console.WriteLine("Port Forwarding Device Found");
            device = args.Device;
            device.CreatePortMap(new Mapping(Protocol.Tcp, PORT, PORT)); // Forward Port
            Clog("Port - " + PORT.ToString() + " - Forwarded Successfully");

            foreach (Mapping portMap in device.GetAllMappings()) { // Log all portmaps
                Clog("Port Map - " + portMap.ToString());
            }
        }

        /*
        * Do when device is lost.
        */
        public static void OnDeviceLost(object sender, DeviceEventArgs args) {
            device = args.Device;
            Clog("Forwarding Device Lost");
        }

        /*
        * Deletes all port maps.
        */
        public static void DelPortMap() {
            try {
                device.DeletePortMap(new Mapping(Protocol.Tcp, PORT, PORT));
            } catch (MappingException ex) {
                AlexaComp._log.Debug(ex.ToString());
            }
        }
        #endregion

        /*
        * Checks to see if the server is stopped, if true, restarts it.
        */
        public static void ServerLoop(){
            if (stopProgramFlag == false) {
                while (true){
                    Thread.Sleep(150);
                    if (newServerFlag) {
                        Thread StartServerThread = new Thread(StartServer) {
                            Name = "ServerThread"
                        };
                        StartServerThread.Start();
                        newServerFlag = false;
                    }
                }
            }
        }

        #endregion
    }
}
