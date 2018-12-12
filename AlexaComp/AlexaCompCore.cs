using System;
using System.Collections.Generic;
using System.Configuration;
using AlexaComp.Controllers;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading;

using log4net;

using AlexaComp.Core;
using AlexaComp.Core.Controllers;
using System.Net.Sockets;

namespace AlexaComp {
    
    // MAIN TODO LIST
    // TODO : Implement input validation on all functions.
    // TODO : Create format validation functions.

    #pragma warning disable CA1052 // Static holder types should be Static or NotInheritable
    public class AlexaCompCore {
    #pragma warning restore CA1052 // Static holder types should be Static or NotInheritable

        #region Properties
        public static string exePath = System.Reflection.Assembly.GetEntryAssembly().Location;
        private static string[] splitExePath = exePath.Split('\\').ToArray();
        public static string pathToDebug = string.Join("\\\\", splitExePath.Reverse().Skip(1).Reverse());
        public static string pathToProject = string.Join("\\\\", splitExePath.Reverse().Skip(3).Reverse());

        // Threads
        public static Thread AppWindowThread = new Thread(AlexaCompGUI.StartAppWindow) { Name = "AppWindowThread" };
        public static Thread ServerThread = new Thread(ServerController.StartServer) { Name = "ServerThread" };
        public static Thread ServerLoopThread = new Thread(ServerController.ServerLoop) { Name = "ServerLoopThread" };
        public static Thread loadingScreenThread = new Thread(new ParameterizedThreadStart(LoadingScreenForm.StartLoadingScreen));
        public static Thread LightingControlThread = new Thread(LightingController.StartLightingThread) { Name = "LightingControlThread" };

        // Misc
        public static bool updateLogBoxFlag = false;
        public static bool stopProgramFlag = false;
        public static Dictionary<string, string> settingsDict = new Dictionary<string, string>();
        public static readonly ILog _log = LogManager.GetLogger(typeof(AlexaComp));

        #endregion

        public static Dictionary<string, Hardware> Devices = new Dictionary<string, Hardware>();

        #region Methods
        public static void Clog(string tolog, string customLevel = "INFO") {
            switch (customLevel) {
                case "ERROR": _log.Error(tolog); break;
                case "INFO" : _log.Info(tolog);  break;
                case "WARN" : _log.Warn(tolog);  break;
                case "DEBUG": _log.Debug(tolog); break;
                case "FATAL": _log.Fatal(tolog); break;
            }
            Console.WriteLine(tolog);
        }

        public static void StopApplication() {
            Clog("CLOSING PROGRAM");
            stopProgramFlag = true;
            try {
                ServerController.StopServer();
            } catch (NullReferenceException) {
                Clog("NullReferenceException Caught When Stopping Server");
            } catch (Exception e) {
                Clog("Exception Caught When Stopping Server \n" + e);
            }

            try {
                ServerController.DelPortMap();
            } catch (NullReferenceException) {
                Clog("NullReferenceException Caught When Deleting Port Maps");
            } catch (Exception e) {
                Clog("Exception Caught When Deleting Port Maps\n" + e);
            }
            Clog("Exiting...");
            Environment.Exit(1);
        }

        public static bool IsConnectedToInternet(bool log = true) {
            Clog("Checking For Internet Connection...");
            try {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204")) {
                    if (log) { Clog("Internet Connection DOES exist."); }
                    return true;
                }
            }
            catch {
                if (log) { Clog("Internet Connection DOES NOT exist."); }
                return false;
            }
        }

        /// <summary>
        /// Encryption Methods From: https://odan.github.io/2017/08/10/aes-256-encryption-and-decryption-in-php-and-csharp.html
        /// </summary>
        #region encryption
        #endregion
        /*
        * Gets a specified config value
        * @ param key - The name or key of the config value to get.
        */
        public static string GetConfigValue(string key) {
            return ConfigurationManager.AppSettings[key];
        }


        /*
        * Gets the user's public IP address.
        */
        public static void GetExternalIP() {
            string data = new WebClient().DownloadString("http://checkip.dyndns.org/");
            Match match = Regex.Match(data, @"\b(?:[0-9]{1,3}\.){3}[0-9]{1,3}\b"); // Regex match for IP
            if (match.Success) {
                Clog("Public IP - " + match);
            }
            else {
                Clog("IP Regex Match Unsuccessful.", "ERROR");
            }
        }

        public static string GetInternalIP() {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList) {
                if (ip.AddressFamily == AddressFamily.InterNetwork) {
                    return ip.ToString();
                }
            }
            throw new Exception();
        }

        /*
        * Reads all user configured values into the settingsDict.
        */
        public static void ReadConfig() {
            foreach (string key in ConfigurationManager.AppSettings.AllKeys) {
                settingsDict[key] = GetConfigValue(key);
            }
        }

        #endregion
    }
}
