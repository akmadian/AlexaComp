using System;
using System.Windows.Forms;
using System.Xml;

namespace AlexaComp{

    public partial class AlexaComp_Config_Window : Form{

        public static void StartAppWindow() {
            AlexaComp._log.Info("StartAppWinodw Thread Started");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new AlexaComp_Config_Window());
            AlexaComp._log.Info("StartAppWindow Task Completed");
        }


        public AlexaComp_Config_Window(){
            InitializeComponent();
            dataListView.View = View.Details;
            dataListView.GridLines = true;
            dataListView_Load();
        }

        private void dataListView_Load(){
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
            foreach (XmlNode node in list){
                string programName = node.Attributes[0].Value;
                string programPath = node.Attributes[1].Value;

                // Make and add row to listview
                string[] row = { programName, programPath };
                // Console.WriteLine(row);
                var item = new ListViewItem(row);
                dataListView.Items.Add(item);

                // Console.WriteLine("Name - " + programName + " -- " + programPath);
            }
            AlexaComp._log.Info("Data List Reloaded");
            // Reset column width
            foreach (ColumnHeader column in dataListView.Columns) { column.Width = -1; }
        }
    
        private void addToListButton_Click(object sender, EventArgs e){

            XmlDocument doc = new XmlDocument();
            doc.Load("pathDir.xml");
            string programName = programNameTextBox.Text;
            string programPath = programPathTextBox.Text;
            programNameTextBox.Clear();
            programPathTextBox.Clear();

            programName = programName.Replace(" ", string.Empty).ToUpper();

            string log = "{Name: " + programName + ", Path: " + programPath + "}";
            AlexaComp._log.Info("Add Program - " + log);

            XmlDocumentFragment frag = doc.CreateDocumentFragment();
            frag.InnerXml = "<path programName=\"" + programName + 
                            "\" programPath=\"" + programPath + "\"/>";

            AlexaComp._log.Info("Add new program - " + frag.InnerXml);
            doc.DocumentElement.AppendChild(frag);
            doc.Save(AlexaComp.pathToDebug);
            doc.Save(AlexaComp.pathToProject);
            AlexaComp._log.Info("pathDir appended");

            dataListView_Load();
            
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void programNameTextBox_TextChanged(object sender, EventArgs e) { }
        private void programPathTextBox_TextChanged(object sender, EventArgs e) { }
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e) { }
        private void dataListView_SelectedIndexChanged(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void FormClosing(object sender, FormClosedEventArgs e) { }

        protected override void OnFormClosing(FormClosingEventArgs e){
            base.OnFormClosing(e);
            AlexaComp._log.Info("Closing Program");
            AlexaComp.stopProgramFlag = true;
            AlexaComp._log.Info("StopServerLoopFlag Raised");
            AlexaCompSERVER.stopServer();
            AlexaComp._log.Info("Server Stopped");
            Application.Exit();
        }
    }
}
