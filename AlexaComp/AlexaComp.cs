using System;
using System.Configuration;
using System.Threading;
using System.Net;
using System.Diagnostics;
using System.Text.RegularExpressions;

using log4net.Config;

using AlexaComp.Core;
using AlexaComp.Forms;
using AlexaComp.Core.Requests;
using AlexaComp.Core.Controllers;

// TODO : Documentation
// TODO : Add region tags to files where appropriate.
/** Documentation format
* Description
* @param <paramname> <description>
* @return <returns>
* @throws <throws>
*/

namespace AlexaComp {
    class AlexaComp : AlexaCompCore {

        [STAThread]
        static void Main(string[] args) {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            XmlConfigurator.Configure();
            AppDomain.CurrentDomain.UnhandledException += (s, e) => {
                var exception = (Exception)e.ExceptionObject;
                Clog(exception.ToString(), "FATAL");
                throw exception;
            };
            Clog("Exception Handler Registered");

            _log.Info("Start Program");

            // Log Assembly and Environment Information
            System.Reflection.Assembly Assembly = System.Reflection.Assembly.GetEntryAssembly();
            Clog("Assembly.GetName()");
            Clog("    GetName - " + Assembly.GetName().ToString());
            Clog("    Name - " + Assembly.GetName().Name);
            Clog("    Version - " + Assembly.GetName().Version);
            Clog("    VersionCompatibility - " + Assembly.GetName().VersionCompatibility);
            Clog("    FullName - " + Assembly.FullName);
            Clog("    HostContext - " + Assembly.HostContext.ToString());
            Clog("    IsFullyTrusted - " + Assembly.IsFullyTrusted.ToString());
            Clog("System.Environment");
            Clog("    OSVersion - " + Environment.OSVersion);
            Clog("    Version - " + Environment.Version);
            Clog("    CurrentManagedThreadID - " + Environment.CurrentManagedThreadId.ToString());
            Clog("    Is64BitOS - " + Environment.Is64BitOperatingSystem.ToString());
            Clog("    Is64BitProcess - " + Environment.Is64BitProcess.ToString());
            Clog("    WorkingSet - " + Environment.WorkingSet.ToString());
            Clog("Target Framework - " + AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName);
            Clog("Executable Path - " + exePath.ToString());
            Clog("PathToDebug - " + pathToDebug);
            Clog("PathToProject - " + pathToProject);
            
            GetExternalIP();

            Clog("Initializing Hardware Sensors");
            
            loadingScreenThread.Start(timer);
        }


        /*
        * Reads all user configured values into the settingsDict.
        */
        public static void ReadConfig(){
            foreach (string key in ConfigurationManager.AppSettings.AllKeys){
                settingsDict[key] = GetConfigValue(key);
            }
        }

        /*
        * Gets the user's public IP address.
        */
        public static void GetExternalIP() {
            string data = new WebClient().DownloadString("http://checkip.dyndns.org/");
            Match match = Regex.Match(data, @"\b(?:[0-9]{1,3}\.){3}[0-9]{1,3}\b"); // Regex match for IP
            if (match.Success) {
                Clog("Public IP - " + match);
            } else {
                Clog("IP Regex Match Unsuccessful.", "ERROR");
            }
        }
    }
}
