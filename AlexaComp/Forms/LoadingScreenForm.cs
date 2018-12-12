using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

using AlexaComp.Core;
using AlexaComp.Core.Controllers;

namespace AlexaComp {
    public partial class LoadingScreenForm : Form {
        public LoadingScreenForm(Stopwatch timer) {
            InitializeComponent();

            Thread.Sleep(300);
            Thread loadingThread = new Thread(unused => StartLoading(timer));
            loadingThread.Start();
        }

        public static void StartLoadingScreen(object timer) {
            try {
                Application.Run(new LoadingScreenForm((Stopwatch)timer));
            } catch (ObjectDisposedException) {
                Console.WriteLine("Caught object disposed exception");
            }
        }

        private void StartLoading(Stopwatch timer) {
            UpdateProgress("Reading Config File");
            AlexaComp.ReadConfig();

            UpdateProgress("Creating Port Map", 400);
            ServerController.ForwardPort();

            UpdateProgress("Scanning for RGB Devices");
            AlexaComp.LightingControlThread.Start();

            UpdateProgress("Assigning Sensors");
            HardwareController.InitSensors();

            UpdateProgress("Getting Installed Programs");
            ProgramInventory.ScanDir();

            UpdateProgress("Starting Server");
            AlexaCompCore.ServerThread.Start();

            UpdateProgress("Starting Server Loop");
            AlexaCompCore.ServerLoopThread.Start();

            UpdateProgress("Starting AlexaComp", 400);
            CloseSplashScreen();

            AlexaComp.AppWindowThread.Start();
            timer.Stop();
            AlexaCompCore.Clog(String.Format("Application Window started in {0} ms.", timer.ElapsedMilliseconds));
        }

        private void UpdateProgress(string message, int wait = 300) {
            try {
                AlexaCompCore.Clog(message);
                progressLabel.Invoke(new MethodInvoker(delegate { progressLabel.Text = message; }));
            } catch (InvalidOperationException) {

            }
            // Thread.Sleep(wait);
        }

        private void CloseSplashScreen() {
            this.Invoke(new MethodInvoker(delegate { this.Close(); }));
        }

        private void LoadingScreenForm_Load(object sender, EventArgs e) {}
        private void pictureBox1_Click(object sender, EventArgs e) {}
        private void button1_Click(object sender, EventArgs e) {}
    }
}
