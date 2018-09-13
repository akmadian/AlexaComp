using System;
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
                string programPath = AlexaComp.GetProgramPath(req.PRIMARY);
                AlexaComp._log.Info("ProgramPath - " + programPath);
                AlexaComp._log.Info("req Program - " + req.PRIMARY);
                Process.Start(AlexaComp.GetProgramPath(req.PRIMARY));
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
            // Hardwaremonitor code
        }

    }

    public class Request{
        public string AUTH { get; set; }
        public string COMMAND { get; set; }
        public string PRIMARY { get; set; }
        public string SECONDARY { get; set; }
    }
}
