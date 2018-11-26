using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Linq;
using System.Diagnostics;
using Microsoft.Win32;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using AlexaComp.Controllers;
using AlexaComp.Forms;
using AlexaComp.Core;
using System.Management;

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

            // Log Paths
            _log.Info(exePath.ToString());
            _log.Info("PathToDebug - " + pathToDebug);
            _log.Info("PathToProject - " + pathToProject);

            AppDomain.CurrentDomain.UnhandledException += (s, e) => {
                var exception = (Exception)e.ExceptionObject;
                clog(exception.ToString(), "FATAL");
                throw exception;
            };
            clog("Exception Handler Registered");

            getExternalIP();
            Console.ReadLine();
            // testThing();

            // LoadingScreenThread.Start();
            /*
            LightingControlThread.Start();
            Console.WriteLine("Waiting Before Effect Start");
            Thread.Sleep(1000);
            Request requ = new Request("testAuth", "RGBCOMMAND", "RAINBOWFADEEFFECT", "", "", opt1, opt2);
            AlexaCompREQUEST.processRequest(requ);
            Thread.Sleep(5000);
            Request requ_ = new Request("testAuth", "RGBCOMMAND", "ERROREFFECT", "", "", opt1, opt2);
            AlexaCompREQUEST.processRequest(requ_);*/
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

