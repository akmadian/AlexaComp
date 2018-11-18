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

            LoadingScreenThread.Start();
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
        
        /*
        * Verifies that the necessary config values are entered and correctly formatted.
        */
        public static void verifyConfig() {
            foreach (KeyValuePair<string, string> pair in settingsDict){
                // Console.WriteLine("pair: '" + pair.Key + "' - " + pair.Value);
                if (String.IsNullOrEmpty(pair.Value)){
                    _log.Fatal("Config Value" + pair.Key + " not configured correctly or are missing. AlexaComp will not work.");
                } else if (pair.Key == "HOST"){ // Is Host IP formatted properly?
                    bool isValidIp = IPAddress.TryParse(pair.Value, out IPAddress ipAdd);
                    if (isValidIp != true){
                        _log.Fatal("Config value \"HOST\" exists, but is not configured or formatted properly.");
                    }
                } 
            }
        }
        
        /*
        * Updates a config value of name {key} with value {value}.
        * @ param key - The name of the config value to update.
        * @ param value - The value to update to.
        */
        public static void UpdateConfigValue(string key, string value){
            Console.WriteLine(key + " - " + value);
            ExeConfigurationFileMap debugMap = new ExeConfigurationFileMap { ExeConfigFilename = pathToDebug + "\\AlexaComp.exe.config" };
            ExeConfigurationFileMap projectMap = new ExeConfigurationFileMap { ExeConfigFilename = pathToProject + "\\App.config" };
            var config = ConfigurationManager.OpenMappedExeConfiguration(debugMap, ConfigurationUserLevel.None);
            var config2 = ConfigurationManager.OpenMappedExeConfiguration(projectMap, ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove(key);
            config2.AppSettings.Settings.Remove(key);
            config.AppSettings.Settings.Add(key, value);
            config2.AppSettings.Settings.Add(key, value);
            config.Save(ConfigurationSaveMode.Modified);
            config2.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("appSettings");
        }

        /*
        * Inventories all user installed programs.
        */
        public static void inventoryPrograms() {
            string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key)) {
                foreach (string subkey_name in key.GetSubKeyNames()) {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name)) {
                        if (subkey.GetValue("DisplayName") != null) {
                            string subkeyValue = subkey.GetValue("DisplayName").ToString();
                            if (subkeyValue.Contains("Microsoft")  || subkeyValue.Contains("SDK") || 
                                subkeyValue.Contains("WinRT") || subkeyValue.Contains("vs_") ||
                                subkeyValue.Contains("Visual C++") || subkeyValue.Contains("CRT") ||
                                subkeyValue.Contains("Windows")){
                                
                            } else {
                                Console.WriteLine(subkey.GetValue("DisplayName"));
                            }
                        }
                    }
                }
            }
        }
    }

    class Options {
        public Request req;
        public System.Net.Sockets.NetworkStream nwStream;

        public Options(Request req_, System.Net.Sockets.NetworkStream nwStream_) {
            req = req_;
            nwStream = nwStream_;
        }
    }
}

