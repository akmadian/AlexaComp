using System;
using System.Collections.Generic;
using System.Configuration;
using AlexaComp.Controllers;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

using log4net;
using log4net.Config;

namespace AlexaComp {
    
    // MAIN TODO LIST
    // TODO : Implement input validation on all functions.
    // TODO : Create format validation functions.

    class AlexaCompCore {

        public static string exePath = System.Reflection.Assembly.GetEntryAssembly().Location;
        private static string[] splitExePath = exePath.Split('\\').ToArray();
        public static string pathToDebug = string.Join("\\\\", splitExePath.Reverse().Skip(1).Reverse());
        public static string pathToProject = string.Join("\\\\", splitExePath.Reverse().Skip(3).Reverse());

        // Threads
        public static Thread AppWindowThread = new Thread(AlexaCompGUI.StartAppWindow);
        // public static Thread ServerThread = new Thread(AlexaCompSERVER.startServer);
        // public static Thread ServerLoopThread = new Thread(AlexaCompSERVER.ServerLoop);
        public static Thread LoadingScreenThread = new Thread(LoadingScreenForm.startLoadingScreen);
        public static Thread LightingControlThread = new Thread(LightingController.startLightingThread);

        // Misc
        public static bool updateLogBoxFlag = false;
        public static bool stopProgramFlag = false;
        public static Dictionary<string, string> settingsDict = new Dictionary<string, string>();
        public static readonly ILog _log = LogManager.GetLogger(typeof(AlexaComp));

        // Controller Instances
        public static LightingController RGBController = new LightingController();
        public static AudioController SoundController = new AudioController();

        public static List<AlexaCompServerInstance> serverInstances = new List<AlexaCompServerInstance>();

        public static void clog(string tolog) {
            _log.Info(tolog);
            Console.WriteLine(tolog);
        }

        public static void stopApplication() {
            _log.Info("CLOSING PROGRAM");
            stopProgramFlag = true;
            // AlexaCompSERVER.stopServer();
            AlexaCompSERVER.delPortMap();
            
            Environment.Exit(1);
        }

        public static string[] splitStringEveryN(string toSplit, int n) {
            List<string> returnArr = new List<string>();
            for (int i = 0; i < toSplit.Length; i += n) {
                returnArr.Add(toSplit.Substring(i, Math.Min(n, toSplit.Length - i)));
            }
            return returnArr.ToArray();
        }

        public static bool validateRGB(RGBColor color) {
            int[] rgbArr = colorMethods.RGBToArr(color);
            foreach (var value in rgbArr) {
                if (255 < value || value < 0) {
                    clog("Invalid RGB - Value Greater Than 255, or Less Than 0.");
                    return false;
                }
            }
            return true;
        }

        public void ServerLoop() {
            if (AlexaCompCore.stopProgramFlag == false) {
                while (true) {
                    foreach (AlexaCompServerInstance instance in serverInstances) {
                        Thread.Sleep(150);
                        if (instance.newServerFlag == true) {
                            AlexaComp._log.Info("Active Threads - " + Process.GetCurrentProcess().Threads.Count.ToString());
                            // Thread StartServerThread = new Thread(startServer);
                            // StartServerThread.Name = "startServerThread";
                            // StartServerThread.Start();
                            // newServerFlag = false;
                        }
                    }
                }
            }
        }


        /*
        * Gets a specified config value
        * @ param key - The name or key of the config value to get.
        */
        public static string GetConfigValue(string key) {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
