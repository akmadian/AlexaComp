using System;
using System.Xml;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace AlexaComp {
    public class AlexaCompREQUEST {

        [DllImport("user32.dll")]
        private static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        [DllImport("PowrProf.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);

        private static string failMessage = "There was an error, please check the Alexa Comp log file.";
        public static void processRequest(Request req) {
            switch (req.COMMAND) {
                case "LAUNCH":
                    launchRequest(req);
                    req.logTimeElapsed();
                    break;
                case "COMMAND":
                    commandRequest(req);
                    req.logTimeElapsed();
                    break;
                case "GETCOMPSTAT":
                    compStatRequest(req);
                    req.logTimeElapsed();
                    break;
                case "AUDIOCOMMAND":
                    audioRequest(req);
                    break;
                case "NETWORKCOMMAND":
                    break;
            }
        }

        static void audioRequest(Request req) {
            try {
                switch (req.PRIMARY) {
                    case "PLAYPAUSE":
                        AudioController.togglePlayPause();
                        break;
                    case "NEXTTRACK":
                        AudioController.nextTrack();
                        break;
                    case "PREVTRACK":
                        AudioController.prevTrack();
                        break;
                    case "SETVOLUME":
                        AudioController.setVolume(Convert.ToInt32(req.SECONDARY));
                        break;
                    case "VOLUMEUP":
                        AudioController.volUp();
                        break;
                    case "VOLUMEDOWN":
                        AudioController.volDown();
                        break;
                    case "TOGGLEMUTE":
                        break;
                }
                Response res = new Response(true, "Done!", "", "");
            } catch (Exception e) {
                AlexaComp._log.Error(e);
                Response res = new Response(false, "Oops! Something went wrong...", "", "");
            }
        }

        static void launchRequest(Request req) {
            if (req.PRIMARY == "SHUTDOWN") {
                AlexaCompSERVER.stopServer();
                Process.Start("shutdown", "/s /t .5");
            } 

            // For starting a
            try {
                string programPath = GetProgramPath(req.PRIMARY);
                AlexaComp._log.Info("ProgramPath - " + programPath);
                AlexaComp._log.Info("req Program - " + req.PRIMARY);
                Process.Start(GetProgramPath(req.PRIMARY));
                AlexaComp._log.Info("Program Launched");
                Response res = new Response(true, "Program Launched!", "", "");
                AlexaCompSERVER.stopServer(); // Restart Server to Handle Next Request
            }
            catch (NullReferenceException) {
                AlexaComp._log.Error("NullReferenceException caught during attempt to launch program, " +
                    "null most likely returned from GetProgramPath.");
                Response res = new Response(false, failMessage, "", "");
                AlexaCompSERVER.stopServer();
            }
            catch (System.ComponentModel.Win32Exception e) {
                AlexaComp._log.Error("Win32Exception Caught during attempt to launch program " + e.Message);
                Response res = new Response(false, failMessage, "", "");
                AlexaCompSERVER.stopServer();
            }catch (InvalidOperationException e) {
                AlexaComp._log.Error("InvalidOperationException Caught during attempt to launch program " + e.Message);
                Response res = new Response(false, failMessage, "", "");
                AlexaCompSERVER.stopServer();
            }
        }

        static void commandRequest(Request req) {
            switch (req.PRIMARY) {
                case "SHUTDOWN":
                    AlexaComp.stopApplication();
                    Process.Start("shutdown", "/s /t .5");
                    break;
                case "LOCK":
                    Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
                    break;
                case "RESTART":
                    AlexaComp.stopApplication();
                    Process.Start("shutdown", "/r /t .5");
                    break;
                case "SLEEP":
                    SetSuspendState(false, true, true);
                    break;
                case "LOGOFF":
                    ExitWindowsEx(0, 0);
                    break;
                case "HIBERNATE":
                    SetSuspendState(true, true, true);
                    break;
            }
        }

        static void compStatRequest(Request req) {
            if (req.PRIMARY == "RAM") {
                Response res = new Response(true, "5.3 Gigabites", "", "");
            }
            else if (req.PRIMARY == "GPU") {
                Response res = new Response(true, "38 degrees celcius", "", "");
            } else {
                Console.WriteLine(AlexaCompHARDWARE.partStat(req.PRIMARY, req.SECONDARY, req.TERTIARY));
                Response res = new Response(true, AlexaCompHARDWARE.partStat(req.PRIMARY, req.SECONDARY, req.TERTIARY), "", "");
            }
        }

        static string GetProgramPath(string program) {
            XmlDocument doc = new XmlDocument();
            doc.Load("pathDir.xml");
            AlexaComp._log.Info("pathDIR loaded");
            XmlElement elem = doc.SelectSingleNode("//path[@programName='" + program + "']") as XmlElement;
            if (elem != null) {
                string path = elem.GetAttribute("programPath");
                AlexaComp._log.Info("Path - " + path);
                return path;
            }
            else {
                AlexaComp._log.Info("null returned");
                return null;
            }
        }
    }

    public class Request {
        public string AUTH { get; set; }
        public string COMMAND { get; set; }
        public string PRIMARY { get; set; }
        public string SECONDARY { get; set; }
        public string TERTIARY { get; set; }

        public Stopwatch sw = new Stopwatch();

        public Request() {
            sw.Start();
            printRequest();
        }

        public void logTimeElapsed() {
            sw.Stop();
            AlexaComp._log.Info("Request Completed, Time Elapsed(ms) - " + sw.ElapsedMilliseconds.ToString());
        }

        public void printRequest() {
            string str = string.Format("New Request - {{Command: {0}, Primary: {1}, Secondary: {2}, Tertiary: {3}}}", COMMAND, PRIMARY, SECONDARY, TERTIARY);
            Console.WriteLine(str);
        }
    }

    public class Response {
        public bool passorfail;
        public string message;
        public string primary;
        public string secondary;

        public string json;

        public Response(bool passorfail_, string message_, string primary_, string secondary_) {
            passorfail = passorfail_;
            message = message_;
            primary = primary_;
            secondary = secondary_;

            json = Newtonsoft.Json.JsonConvert.SerializeObject(this);
            Console.WriteLine(json);
        }

        public void sendResponse(System.Net.Sockets.NetworkStream customStream) {
            if (customStream != null) {
                AlexaCompSERVER.sendToLambda(json, customStream);
            } else {
                AlexaCompSERVER.sendToLambda(json, null);
            }
        }
    }
}

