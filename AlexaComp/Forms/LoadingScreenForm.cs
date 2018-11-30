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

namespace AlexaComp {
    public partial class LoadingScreenForm : Form {
        public LoadingScreenForm() {
            InitializeComponent();

            Thread.Sleep(300);
            Thread loadingThread = new Thread(startLoading);
            loadingThread.Start();
        }

        public static void startLoadingScreen() {
            try {
                Application.Run(new LoadingScreenForm());
            } catch (ObjectDisposedException) {
                Console.WriteLine("Caught object disposed exception");
            }
            
        }

        private void startLoading() {
            updateProgress("Reading Config File");
            AlexaComp.readConfig();

            updateProgress("Creating Port Map", 400);
            AlexaCompSERVER.forwardPort();

            updateProgress("Scanning for RGB Devices");
            AlexaComp.LightingControlThread.Start();

            updateProgress("Assigning Sensors");
            // AlexaCompHARDWARE.assignSensors();

            updateProgress("Getting Installed Programs");
            //AlexaComp.inventoryPrograms();

            updateProgress("Starting Server");
            AlexaCompCore.ServerThread.Start();

            updateProgress("Starting Server Loop");
            AlexaCompCore.ServerLoopThread.Start();

            updateProgress("Starting AleComp", 400);
            closeSplashScreen();

            AlexaComp.AppWindowThread.Start();
        }

        private void updateProgress(string message, int wait = 300) {
            try {
                progressLabel.Invoke(new MethodInvoker(delegate { progressLabel.Text = message; }));
            } catch (InvalidOperationException) {

            }
            // Thread.Sleep(wait);
        }

        private void closeSplashScreen() {
            this.Invoke(new MethodInvoker(delegate { this.Close(); }));
        }

        private void LoadingScreenForm_Load(object sender, EventArgs e) {}
        private void pictureBox1_Click(object sender, EventArgs e) {}
        private void button1_Click(object sender, EventArgs e) {}
    }
}
