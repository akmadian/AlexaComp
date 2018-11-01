using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace AlexaComp
{
    public partial class ProgramListForm : Form
    {
        public ProgramListForm()
        {
            InitializeComponent();
            this.AcceptButton = this.addToListButton;
            dataListView.View = View.Details;
            dataListView.GridLines = true;
            dataListView_Load();
        }

        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }

        private void dataListView_Load()
        {
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
            foreach (XmlNode node in list)
            {
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

        private void addToListButton_Click(object sender, EventArgs e)
        {
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
            doc.Save(AlexaComp.pathToDebug + "\\pathDir.xml");
            doc.Save(AlexaComp.pathToProject + "\\pathDir.xml");
            AlexaComp._log.Info("pathDir appended");

            dataListView_Load();

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            string programName = programNameTextBox.Text;
            string programPath = programPathTextBox.Text;

            if (string.IsNullOrEmpty(programName) && string.IsNullOrEmpty(programPath)) { // If both are empty.
                errorLabel.Text = "Please specify a program name and path.";
            } else if (string.IsNullOrEmpty(programPath)) { // If only path is empty.
                errorLabel.Text = "Please specify a program path.";
            } else if (string.IsNullOrEmpty(programName)) { // If only name is empty.
                errorLabel.Text = "Please specify a program name.";
            } else { // If both name and path are filled.
                XmlDocument doc = new XmlDocument();
                doc.Load("pathDir.xml");

                programNameTextBox.Clear();
                programPathTextBox.Clear();

                programName = programName.Replace(" ", string.Empty).ToUpper(); // Format string

                string log = "{Name: " + programName + ", Path: " + programPath + "}";
                AlexaComp._log.Info("Add Program - " + log);

                XmlDocumentFragment frag = doc.CreateDocumentFragment();
                frag.InnerXml = "<path programName=\"" + programName +
                                "\" programPath=\"" + programPath + "\"/>";

                AlexaComp._log.Info("Add new program - " + frag.InnerXml);
                doc.DocumentElement.AppendChild(frag);
                doc.Save(AlexaComp.pathToDebug + "\\pathDir.xml");
                doc.Save(AlexaComp.pathToProject + "\\pathDir.xml");
                AlexaComp._log.Info("pathDir appended");

                dataListView_Load(); // Reset program list

                errorLabel.Text = "";
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e) {

        }
    }
}
