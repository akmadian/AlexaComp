using System;
using System.Collections.Generic;
using System.Configuration;
using AlexaComp.Controllers;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Security.Cryptography;

using log4net;
using log4net.Config;

using AlexaComp.Core;

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
        public static Thread ServerThread = new Thread(AlexaCompSERVER.StartServer) { Name = "ServerThread" };
        public static Thread ServerLoopThread = new Thread(AlexaCompSERVER.ServerLoop) { Name = "ServerLoopThread" };
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
            AlexaCompSERVER.StopServer();
            AlexaCompSERVER.DelPortMap();
            Environment.Exit(1);
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

        #endregion
    }
}
