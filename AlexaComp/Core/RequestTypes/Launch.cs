using System;
using System.Xml;
using System.Diagnostics;

using AlexaComp.Core;

namespace AlexaComp.Core.Requests {
    public class Launch : AlexaCompCore {
        
        private string programName;

        public Launch(string programName_) {
            Clog("LaunchCommand Constructed");
            programName = programName_;
            process();
        }

        public void process() {
            try {
                string programPath = GetProgramPath(programName);
                Clog("ProgramPath - " + programPath);
                Clog("req Program - " + programName);
                Process.Start(GetProgramPath(programName));
                Clog("Program Launched");
                Response res = new Response(true, "Program Launched!", "", "");
            }
            catch (NullReferenceException) {
                Clog("NullReferenceException caught during attempt to launch program, " +
                    "null most likely returned from GetProgramPath.");
                Response res = new Response(false, "");
            }
            catch (System.ComponentModel.Win32Exception e) {
                Clog("Win32Exception Caught during attempt to launch program " + e.Message);
                Response res = new Response(false, "");
            }catch (InvalidOperationException e) {
                Clog("InvalidOperationException Caught during attempt to launch program " + e.Message);
                Response res = new Response(false, "");
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