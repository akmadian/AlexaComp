using System;
using System.Xml;
using System.Diagnostics;
using System.Runtime.InteropServices;
using AlexaComp.Controllers;
using System.Threading;

namespace AlexaComp {
    class AlexaCompREQUEST : AlexaCompCore {

        [DllImport("user32.dll")]
        private static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        [DllImport("PowrProf.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);

        private static string failMessage = "There was an error, please check the Alexa Comp log file.";
        public static void processRequest(Request req) {
            switch (req.COMMAND) {
                case "LAUNCH":         launchRequest(req);   break;
                case "COMPUTERCOMMAND":        commandRequest(req);  break;
                case "GETCOMPSTAT":    compStatRequest(req); break;
                case "AUDIOCOMMAND":   audioRequest(req);    break;
                case "RGBCOMMAND":     RGBRequest(req);      break; // TODO : Implement RGBCommand
            }
            req.logTimeElapsed();
        }
        
        
        public static void RGBRequest(Request req) {
            RGBColor priColor;
            RGBColor secColor;
            if (req.OPTIONS != null) { priColor = new RGBColor(req.OPTIONS); } else { priColor = null; };
            if (req.OPTIONS2 != null) { secColor = new RGBColor(req.OPTIONS2); } else { secColor = null; };
            LightingController.lightingProcess(priColor, secColor, req.PRIMARY);
        }

        static void audioRequest(Request req) {
            try {
                switch (req.PRIMARY) {
                    // All are working
                    case "PLAYPAUSE":  AudioController.togglePlayPause();                         break;
                    case "NEXTTRACK":  AudioController.nextTrack();                               break;
                    case "PREVTRACK":  AudioController.prevTrack();                               break;
                    case "SETVOLUME":  AudioController.setVolume(Convert.ToInt32(req.SECONDARY)); break;
                    case "VOLUMEUP":   AudioController.volUp();                                   break;
                    case "VOLUMEDOWN": AudioController.volDown();                                 break;
                    case "TOGGLEMUTE": break; // TODO: Implement Mute Functionality
                }
                Response res = new Response(true, "Done!");
            } catch (Exception e) {
                clog(e.ToString());
                Response res = new Response(false, "Oops! Something went wrong...");
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
                clog("ProgramPath - " + programPath);
                clog("req Program - " + req.PRIMARY);
                Process.Start(GetProgramPath(req.PRIMARY));
                clog("Program Launched");
                Response res = new Response(true, "Program Launched!", "", "");
                AlexaCompSERVER.stopServer(); // Restart Server to Handle Next Request
            }
            catch (NullReferenceException) {
                clog("NullReferenceException caught during attempt to launch program, " +
                    "null most likely returned from GetProgramPath.");
                Response res = new Response(false, failMessage);
                AlexaCompSERVER.stopServer();
            }
            catch (System.ComponentModel.Win32Exception e) {
                clog("Win32Exception Caught during attempt to launch program " + e.Message);
                Response res = new Response(false, failMessage);
                AlexaCompSERVER.stopServer();
            }catch (InvalidOperationException e) {
                clog("InvalidOperationException Caught during attempt to launch program " + e.Message);
                Response res = new Response(false, failMessage);
                AlexaCompSERVER.stopServer();
            }
        }

        static void commandRequest(Request req) {
            clog("CommandResuest");
            clog(req.PRIMARY);
            switch (req.PRIMARY) {
                case "SHUTDOWN":  stopApplication(); Process.Start("shutdown", "/s /t .5");                         break; // To Test
                case "LOCK":
                    Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation"); break; // Working
                case "RESTART":   stopApplication(); Process.Start("shutdown", "/r /t .5");                         break; // To Test
                case "SLEEP":     SetSuspendState(false, true, true);                                               break; // To Test
                case "LOGOFF":    ExitWindowsEx(0, 0);                                                              break; // To Test
                case "HIBERNATE": SetSuspendState(true, true, true);                                                break; // To Test
            }
        }

        static void compStatRequest(Request req) {
            // TODO : Fix Compstat System
            // TODO : Implement switch case
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
            clog("pathDIR loaded");
            XmlElement elem = doc.SelectSingleNode("//path[@programName='" + program + "']") as XmlElement;
            if (elem != null) {
                string path = elem.GetAttribute("programPath");
                clog("Path - " + path);
                return path;
            }
            else {
                clog("null returned");
                return null;
            }
        }
    }

    [DebuggerDisplay("[AlexaComp Request Object -- {{COMMAND: {COMMAND}, PRIMARY: {PRIMARY}, SECONDARY: {SECONDARY}, TERTIARY: {TERTIARY}}}]")]
    class Request : AlexaCompCore{
        public string AUTH { get; set; }
        public string COMMAND { get; set; }
        public string PRIMARY { get; set; }
        public string SECONDARY { get; set; }
        public string TERTIARY { get; set; }
        public string SESSID { get; set; }
        public string[] OPTIONS { get; set; }
        public string[] OPTIONS2 { get; set; }

        public Stopwatch sw = new Stopwatch();

        public Request() {
            sw.Start();
        }

        public Request(string AUTH_, string COMMAND_, string PRIMARY_, string SECONDARY_, string TERTIARY_, string[] options, string[] options2) {
            sw.Start();
            AUTH = AUTH_;
            COMMAND = COMMAND_;
            PRIMARY = PRIMARY_;
            SECONDARY = SECONDARY_;
            TERTIARY = TERTIARY_;
            OPTIONS = options;
            OPTIONS2 = options2;
        }

        public void logTimeElapsed() {
            sw.Stop();
            clog("Request Completed, Time Elapsed(ms) - " + sw.ElapsedMilliseconds.ToString());
        }

        /*
        public void printRequest() {
            string str = string.Format("New Request - {{Command: {0}, Primary: {1}, Secondary: {2}, Tertiary: {3}}}", COMMAND, PRIMARY, SECONDARY, TERTIARY);
            clog(str);
        }*/
    }

    class Response : AlexaCompCore{
        public bool passorfail;
        public string message;
        public string primary;
        public string secondary;

        public string json;

        public Response(bool passorfail_, string message_, string primary_ = "", string secondary_ = "") {
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

