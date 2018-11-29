using System;
using System.Configuration;
using System.Threading;
using System.Net;
using System.Text.RegularExpressions;

using log4net;
using log4net.Config;

// TODO : Documentation
// TODO : Add region tags to files where appropriate.
/** Documentation format
* Description
* @param <paramname> <description>
* @return <returns>
* @throws <throws>
*/

namespace AlexaComp {
    class AlexaComp : AlexaCompCore{

        [STAThread]
        static void Main(string[] args) {
            XmlConfigurator.Configure();
            AppDomain.CurrentDomain.UnhandledException += (s, e) => {
                var exception = (Exception)e.ExceptionObject;
                clog(exception.ToString(), "FATAL");
                throw exception;
            };
            clog("Exception Handler Registered");

            _log.Info("Start Program");

            // Parse cli args
            foreach (string arg in args) {
                if ("-LogSensors".Contains(arg)) {
                    Thread LogSensorsThread = new Thread(new ParameterizedThreadStart(AlexaCompHARDWARE.getAllSensors));
                    LogSensorsThread.Start(false);
                }
                if ("-GetPrograms".Contains(arg)) {

                }
                if ("-AddProgram".Contains(arg)) {
                    if ("--AddProgramLiteral".Contains(arg)) {

                    }
                }
            }

            // Log Assembly and Environment Information
            System.Reflection.Assembly Assembly = System.Reflection.Assembly.GetEntryAssembly();
            clog("Assembly.GetName()");
            clog("    GetName - " + Assembly.GetName().ToString());
            clog("    Name - " + Assembly.GetName().Name);
            clog("    Version - " + Assembly.GetName().Version);
            clog("    VersionCompatibility - " + Assembly.GetName().VersionCompatibility);
            clog("    FullName - " + Assembly.FullName);
            clog("    HostContext - " + Assembly.HostContext.ToString());
            clog("    IsFullyTrusted - " + Assembly.IsFullyTrusted.ToString());
            clog("System.Environment");
            clog("    OSVersion - " + Environment.OSVersion);
            clog("    Version - " + Environment.Version);
            clog("    CurrentManagedThreadID - " + Environment.CurrentManagedThreadId.ToString());
            clog("    Is64BitOS - " + Environment.Is64BitOperatingSystem.ToString());
            clog("    Is64BitProcess - " + Environment.Is64BitProcess.ToString());
            clog("    WorkingSet - " + Environment.WorkingSet.ToString());
            clog("Target Framework - " + AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName);
            clog("Executable Path - " + exePath.ToString());
            clog("PathToDebug - " + pathToDebug);
            clog("PathToProject - " + pathToProject);
            
            getExternalIP();

            LoadingScreenThread.Start();
        }


        /*
        * Reads all user configured values into the settingsDict.
        */
        public static void readConfig(){
            foreach (string key in ConfigurationManager.AppSettings.AllKeys){
                settingsDict[key] = GetConfigValue(key);
            }
        }

        /*
        * Gets the user's public IP address.
        */
        public static void getExternalIP() {
            string data = new WebClient().DownloadString("http://checkip.dyndns.org/");
            Match match = Regex.Match(data, @"\b(?:[0-9]{1,3}\.){3}[0-9]{1,3}\b"); // Regex match for IP
            if (match.Success) {
                clog("Public IP - " + match);
            } else {
                clog("IP Regex Match Unsuccessful.", "ERROR");
            }
        }
    }
}

