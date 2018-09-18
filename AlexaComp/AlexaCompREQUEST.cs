using System;
using System.Xml;
using System.Diagnostics;

namespace AlexaComp{
    public class AlexaCompREQUEST{

        public static void processRequest(Request req){
            if(req.COMMAND == "LAUNCH") {launchRequest(req); }
            else if (req.COMMAND == "COMMAND") { commandRequest(req); }
            else if (req.COMMAND == "GETCOMPSTAT") { compStatRequest(req); }
        }

        static void launchRequest(Request req){
            if (req.PRIMARY == "SHUTDOWN") {
                AlexaCompSERVER.stopServer();
                Process.Start("shutdown", "/s /t .5");
            }

            try{
                string programPath = GetProgramPath(req.PRIMARY);
                AlexaComp._log.Info("ProgramPath - " + programPath);
                AlexaComp._log.Info("req Program - " + req.PRIMARY);
                Process.Start(GetProgramPath(req.PRIMARY));
                AlexaComp._log.Info("Program Launched");
                AlexaCompMQTT.mqttPublish("pass");
                AlexaCompSERVER.stopServer(); // Restart Server to Handle Next Request
            }
            catch (NullReferenceException){
                AlexaComp._log.Error("NullReferenceException caught during attempt to launch program, " +
                    "null most likely returned from GetProgramPath.");
                AlexaCompMQTT.mqttPublish("fail");
                AlexaCompSERVER.stopServer();
            }
            catch (System.ComponentModel.Win32Exception e){
                AlexaComp._log.Error("Win32Exception Caught during attempt to launch program " + e.Message);
                AlexaCompMQTT.mqttPublish("fail");
                AlexaCompSERVER.stopServer();
            }
        }

        static void commandRequest(Request req){
            if (req.PRIMARY == "SHUTDOWN"){
                AlexaCompSERVER.stopServer();
                Process.Start("shutdown", "/s /t .5");
            }
            else if (req.PRIMARY == "RESTART"){
                Process.Start("shutdown", "/r /t 0");
            }
            else if (req.PRIMARY == "SLEEP"){
                // Sleep Code Here
            }
        }

        static void compStatRequest(Request req){
            if (req.PRIMARY == "GPU"){
                if (req.SECONDARY == "TEMPERATURE"){
                    Console.WriteLine(AlexaCompHARDWARE.partStat("GPU", "GPU Core"));
                } 
            }
        }

        public static string GetProgramPath(string program){
            XmlDocument doc = new XmlDocument();
            doc.Load("pathDir.xml");
            AlexaComp._log.Info("pathDIR loaded");
            XmlElement elem = doc.SelectSingleNode("//path[@programName='" + program + "']") as XmlElement;
            if (elem != null){
                string path = elem.GetAttribute("programPath");
                AlexaComp._log.Info("Path - " + path);
                return path;
            }
            else{
                AlexaComp._log.Info("null returned");
                return null;
            }
        }
    }

    public class Request{
        public string AUTH { get; set; }
        public string COMMAND { get; set; }
        public string PRIMARY { get; set; }
        public string SECONDARY { get; set; }
    }
}
