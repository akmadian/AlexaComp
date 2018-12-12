using System;
using System.Windows.Forms;
using System.Threading;
using System.Xml;

using AlexaComp.Core.Controllers;
using AlexaComp.Core;

namespace AlexaComp{

    public partial class AlexaCompGUI : Form {
        
        public static void StartAppWindow() {
            Application.Run(new AlexaCompGUI());
            AlexaCompCore.Clog("StartAppWinodw Thread Started");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AlexaCompCore.Clog("StartAppWindow Task Completed");
        }

        public AlexaCompGUI() {
            InitializeComponent();
            this.notifyIcon.DoubleClick += new EventHandler(this.notifyIcon_DoubleClick);
            this.AcceptButton = this.addToListButton;
            dataListView.View = View.Details;
            dataListView.GridLines = true;
            dataListView_Load();
        }

        #region MainTab
        /*
        * If repo link clicked, start a new browser window and go to the repo page.
        */
        private void RepoLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            System.Diagnostics.Process.Start("https://github.com/akmadian/AlexaComp");
        }
        #endregion

        #region SettingsTab
        /*
        * Logs all discoverable hardware sensors and opens the resulting log.
        */
        private void LogAllSensorsButton_Click(object sender, EventArgs e) {
            MessageBox.Show("This feature has been temporarily disabled.");
            // Thread logSensorsThread = new Thread(new ParameterizedThreadStart(AlexaCompHARDWARE.getAllSensors));
            // logSensorsThread.Name = "logSensorsThread";
            // logSensorsThread.Start(true);
        }

        /*
        * Opens the AlexaComp log file.
        */
        private void openLogFileButton_Click(object sender, EventArgs e) {
            AlexaCompCore.Clog("OpenLogFileButtonClicked");
            System.Diagnostics.Process.Start(AlexaComp.pathToDebug + "\\AlexaCompLOG.log");
        }

        // Copied from settings form, will adapt soon.
        private void runOnStartCheck_CheckedChanged(object sender, EventArgs e) {
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (runOnStartupCheck.Checked) {
                regKey.SetValue("AlexaComp", AlexaCompCore.exePath);
            }
            else {
                regKey.DeleteValue("AlexaComp", false);
            }
        }
        #endregion

        #region ProgramListTab
        private void dataListView_Load() {
            // Clear on refresh
            dataListView.Clear();
            dataListView.Items.Clear();
            // Add Columns
            dataListView.Columns.Add("Program Name");
            dataListView.Columns.Add("Program Path");

            XmlDocument doc = new XmlDocument();
            doc.Load("pathDir.xml");
            XmlNodeList list = doc.SelectNodes("/pathDir/path");

            // Loop over XML doc
            foreach (XmlNode node in list) {
                string programName = node.Attributes[0].Value;
                string programPath = node.Attributes[1].Value;

                // Make and add row to listview
                string[] row = { programName, programPath };
                // Console.WriteLine(row);
                var item = new ListViewItem(row);
                dataListView.Items.Add(item);

                // Console.WriteLine("Name - " + programName + " -- " + programPath);
            }
            AlexaCompCore.Clog("Data List Reloaded");
            // Reset column width
            foreach (ColumnHeader column in dataListView.Columns) { column.Width = -1; }
        }

        private void addToListButton_Click_1(object sender, EventArgs e) {
            string programName = programNameTextBox.Text;
            string programPath = programPathTextBox.Text;

            if (string.IsNullOrEmpty(programName) && string.IsNullOrEmpty(programPath)) { // If both are empty.
                errorLabel.Visible = true;
                errorLabel.Text = "Please specify a program name and path.";
            }
            else if (string.IsNullOrEmpty(programPath)) { // If only path is empty.
                errorLabel.Visible = true;
                errorLabel.Text = "Please specify a program path.";
            }
            else if (string.IsNullOrEmpty(programName)) { // If only name is empty.
                errorLabel.Visible = true;
                errorLabel.Text = "Please specify a program name.";
            }
            else { // If both name and path are filled.
                XmlDocument doc = new XmlDocument();
                doc.Load("pathDir.xml");

                programNameTextBox.Clear();
                programPathTextBox.Clear();

                programName = programName.Replace(" ", string.Empty).ToUpper(); // Format string

                string log = "{Name: " + programName + ", Path: " + programPath + "}";
                AlexaCompCore.Clog("Add Program - " + log);

                XmlDocumentFragment frag = doc.CreateDocumentFragment();
                frag.InnerXml = "<path programName=\"" + programName +
                                "\" programPath=\"" + programPath + "\"/>";

                AlexaCompCore.Clog("Add new program - " + frag.InnerXml);
                doc.DocumentElement.AppendChild(frag);
                doc.Save(AlexaCompCore.pathToDebug + "\\pathDir.xml");
                doc.Save(AlexaCompCore.pathToProject + "\\pathDir.xml");
                AlexaCompCore.Clog("pathDir appended");

                dataListView_Load(); // Reset program list

                errorLabel.Visible = false;
                errorLabel.Text = "";
            }
        }

        #endregion

        #region WindowEventHandlers

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
        protected override void OnFormClosing(FormClosingEventArgs e = null) {
            if (e != null) {
                base.OnFormClosing(e);
            }
            AlexaCompCore.Clog("CLOSING PROGRAM");
            AlexaCompCore.stopProgramFlag = true;
            ServerController.StopServer();
            ServerController.DelPortMap();

            Environment.Exit(1);
        }

        /*
        * Handler for minimize button click.
        */
        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            if (this.WindowState == FormWindowState.Minimized) {
                AlexaCompCore.Clog("Minimized to system tray");
                Hide();
                notifyIcon.Visible = true;
                this.ShowInTaskbar = false;
            }
        }

        #endregion

        #region UnusedMethods
        private void label1_Click(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void programNameTextBox_TextChanged(object sender, EventArgs e) { }
        private void programPathTextBox_TextChanged(object sender, EventArgs e) { }
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e) { }
        private void dataListView_SelectedIndexChanged(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void pictureBox1_Click_1(object sender, EventArgs e) { }
        private void logListView_SelectedIndexChanged(object sender, EventArgs e) { }
        private void logTextBox_TextChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void errorLabel_Click(object sender, EventArgs e) {}
        private void label3_Click(object sender, EventArgs e) {}
        private void RGBSystemLabel_Click(object sender, EventArgs e) {}
        private void textBox5_TextChanged(object sender, EventArgs e) {}
        #endregion
    }
}
