using System;
using System.Windows.Forms;
using System.Threading;

namespace AlexaComp{

    public partial class AlexaCompGUI : Form {
        
        public static void StartAppWindow() {
            Application.Run(new AlexaCompGUI());
            AlexaComp._log.Info("StartAppWinodw Thread Started");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AlexaComp._log.Info("StartAppWindow Task Completed");
        }

        public AlexaCompGUI() {
            InitializeComponent();
            this.notifyIcon.DoubleClick += new EventHandler(this.notifyIcon_DoubleClick);
        }

        // Control Callbacks
        /*
        * Starts the ProgramList Form
        */
        private void editProgramListButton_Click(object sender, EventArgs e) {
            AlexaComp._log.Info("EditProgramListButton Clicked");
            startProgramListForm();
        }

        /*
        * If repo link clicked, start a new browser window and go to the repo page.
        */
        private void RepoLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            System.Diagnostics.Process.Start("https://github.com/akmadian/AlexaComp");
        }

        /*
        * Logs all discoverable hardware sensors and opens the resulting log.
        */
        private void LogAllSensorsButton_Click(object sender, EventArgs e) {
            Thread logSensorsThread = new Thread(new ParameterizedThreadStart(AlexaCompHARDWARE.getAllSensors));
            logSensorsThread.Name = "logSensorsThread";
            logSensorsThread.Start(true);
        }

        /*
        * Opens the AlexaComp log file.
        */
        private void openLogFileButton_Click(object sender, EventArgs e) {
            AlexaComp._log.Info("OpenLogFileButtonClicked");
            System.Diagnostics.Process.Start(AlexaComp.pathToDebug + "\\AlexaCompLOG.log");
        }

        private void startProgramListForm() {
            AlexaComp._log.Info("StartProgramListFormThread Started");
            ProgramListForm ProgramListForm = new ProgramListForm();
            ProgramListForm.ShowDialog();
        }


        // Application Winodow Event Handlers
        /*
        * When notify icon is double clicked, show app.
        */
        private void notifyIcon_DoubleClick(object sender, EventArgs e) {
            AlexaComp._log.Info("Restore Clicks Detected");
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
            this.ShowInTaskbar = true;
        }

        /*
        * Handler for X button click.
        */
        protected override void OnFormClosing(FormClosingEventArgs e) {
            base.OnFormClosing(e);
            AlexaComp._log.Info("CLOSING PROGRAM");
            AlexaComp.stopProgramFlag = true;
            AlexaCompSERVER.stopServer();
            AlexaCompSERVER.delPortMap();

            Application.Exit();
            Environment.Exit(1);
        }

        /*
        * Handler for minimize button click.
        */
        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            if (this.WindowState == FormWindowState.Minimized) {
                AlexaComp._log.Info("Minimized to system tray");
                Hide();
                notifyIcon.Visible = true;
                this.ShowInTaskbar = false;
            }
        }

        /*
        * Waits for ` press, then opens console.
        */
        private void AlexaCompGUI_KeyPress(object sender, KeyPressEventArgs e) {
            Console.WriteLine("Keypress detected");
            Console.WriteLine(e.KeyChar);
            Console.WriteLine(e.KeyChar.ToString());
            switch (e.KeyChar.ToString()) {
                case "`":
                    Thread ConsoleWindowThread = new Thread(ConsoleForm.StartConsoleWindow);
                    ConsoleWindowThread.Name = "ConsoleWindowThread";
                    ConsoleWindowThread.Start();
                    break;
    }
        }

        // Copied from settings form, will adapt soon.
        /* 
        private void runOnStartCheck_CheckedChanged(object sender, EventArgs e) {
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (runOnStartCheck.Checked) {
                regKey.SetValue("AlexaComp", AlexaComp.pathToDebug + "\\AlexaComp.exe");
            }
            else {
                regKey.DeleteValue("AlexaComp", false);
            }
        }*/

        private void label1_Click(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void programNameTextBox_TextChanged(object sender, EventArgs e) { }
        private void programPathTextBox_TextChanged(object sender, EventArgs e) { }
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e) { }
        private void dataListView_SelectedIndexChanged(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void pictureBox1_Click_1(object sender, EventArgs e) {}
        private void logListView_SelectedIndexChanged(object sender, EventArgs e){}
        private void logTextBox_TextChanged(object sender, EventArgs e){ }
        private void textBox1_TextChanged(object sender, EventArgs e) {}
    }
}
