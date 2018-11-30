using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AlexaComp.Core.Requests {

    public class SystemCommand : AlexaCompCore {
        
        #region DllImports
        [DllImport("user32.dll")]
        private static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        [DllImport("PowrProf.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);
        #endregion

        #region Properties
        private string command;
        #endregion

        #region Constructors

        public SystemCommand(string command_) {
            command = command_;
            process();
        }

        #endregion

        public void process() {
            clog("CommandResuest");
            clog(this.command);
            switch (this.command) {
                case "SHUTDOWN": 
                    stopApplication(); Process.Start("shutdown", "/s /t .5");                         
                    break; // To Test

                case "LOCK":
                    Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation"); 
                    break; // Working

                case "RESTART":   
                    stopApplication(); Process.Start("shutdown", "/r /t .5");                         
                    break; // To Test

                case "SLEEP":     
                    SetSuspendState(false, true, true);                                               
                    break; // To Test

                case "LOGOFF":    
                    ExitWindowsEx(0, 0);                                                              
                    break; // To Test

                case "HIBERNATE": 
                    SetSuspendState(true, true, true);                                                
                    break; // To Test
            }
        }
    }
}