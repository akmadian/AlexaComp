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
                AlexaCompSERVER.stopServer();
                Process.Start("shutdown", "/s /t .5");
            } 

            // For starting a
            try {
                string programPath = GetProgramPath(programName);
                clog("ProgramPath - " + programPath);
                clog("req Program - " + programName);
                Process.Start(GetProgramPath(programName));
                clog("Program Launched");
                Response res = new Response(true, "Program Launched!", "", "");
                AlexaCompSERVER.stopServer(); // Restart Server to Handle Next Request
            }
            catch (NullReferenceException) {
                clog("NullReferenceException caught during attempt to launch program, " +
                    "null most likely returned from GetProgramPath.");
                Response res = new Response(false, "");
                AlexaCompSERVER.stopServer();
            }
            catch (System.ComponentModel.Win32Exception e) {
                clog("Win32Exception Caught during attempt to launch program " + e.Message);
                Response res = new Response(false, "");
                AlexaCompSERVER.stopServer();
            }catch (InvalidOperationException e) {
                clog("InvalidOperationException Caught during attempt to launch program " + e.Message);
                Response res = new Response(false, "");
                AlexaCompSERVER.stopServer();
            }
        }

        static string GetProgramPath(string program) {
            XmlDocument doc = new XmlDocument();
            doc.Load("pathDir.xml");
            clog("pathDIR loaded");
            if (doc.SelectSingleNode("//path[@programName='" + program + "']") is XmlElement elem) {
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
}