using System;
using System.Xml;
using System.Diagnostics;

using AlexaComp.Core;

namespace AlexaComp.Core.Requests {
    public class Launch : AlexaCompCore {
        
        private string programName;

        public Launch(string programName_) {
            programName = programName_;
            process();
        }

        public void process() {
            if (programName == "SHUTDOWN") {
                AlexaCompSERVER.StopServer();
                Process.Start("shutdown", "/s /t .5");
            } 

            // For starting a
            try {
                string programPath = GetProgramPath(programName);
                Clog("ProgramPath - " + programPath);
                Clog("req Program - " + programName);
                Process.Start(GetProgramPath(programName));
                Clog("Program Launched");
                Response res = new Response(true, "Program Launched!", "", "");
                AlexaCompSERVER.StopServer(); // Restart Server to Handle Next Request
            }
            catch (NullReferenceException) {
                Clog("NullReferenceException caught during attempt to launch program, " +
                    "null most likely returned from GetProgramPath.");
                Response res = new Response(false, "");
                AlexaCompSERVER.StopServer();
            }
            catch (System.ComponentModel.Win32Exception e) {
                Clog("Win32Exception Caught during attempt to launch program " + e.Message);
                Response res = new Response(false, "");
                AlexaCompSERVER.StopServer();
            }catch (InvalidOperationException e) {
                Clog("InvalidOperationException Caught during attempt to launch program " + e.Message);
                Response res = new Response(false, "");
                AlexaCompSERVER.StopServer();
            }
        }

        static string GetProgramPath(string program) {
            XmlDocument doc = new XmlDocument();
            doc.Load("pathDir.xml");
            Clog("pathDIR loaded");
            if (doc.SelectSingleNode("//path[@programName='" + program + "']") is XmlElement elem) {
                string path = elem.GetAttribute("programPath");
                Clog("Path - " + path);
                return path;
            }
            else {
                Clog("null returned");
                return null;
            }
        }
    }
}