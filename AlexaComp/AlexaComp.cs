using System;
using System.Diagnostics;
using System.Windows.Forms;

using log4net.Config;

using AlexaComp.Core;
using AlexaComp.Forms;
using AlexaComp.Core.Requests;
using AlexaComp.Core.Controllers;

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
            
            if (!IsConnectedToInternet()) {
                Clog("No Internet Connection Detected... Quitting...", "FATAL");
                MessageBox.Show("No Internet Connection Detected, Stopping AlexaComp...", 
                    "AlexaComp",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                StopApplication();
            }

            // MDBController.makeInstance();

            // Set Internal IP for port mapping and server
            try {
                string host = GetInternalIP();
                Clog("Internal IP Found - " + host);
                ServerConfig.HOST = host;
            } catch (Exception) {
                Clog("No Network Adapters With IPv4 Addresses Detected, Cannot Start Server... Quitting...", "FATAL");
                MessageBox.Show("No Network Adapters With IPv4 Addresses Detected, Cannot Start Server. Stopping AlexaComp...",
                    "AlexaComp",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                StopApplication();
            }

            GetExternalIP();

            Clog("Initializing Hardware Sensors");
            
            loadingScreenThread.Start(timer);
        }
    }
}
