using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Xml;


namespace AlexaComp{

    public partial class AlexaComp_Config_Window : Form{


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
                Console.WriteLine(row);
                var item = new ListViewItem(row);
                dataListView.Items.Add(item);

                Console.WriteLine("Name - " + programName + " -- " + programPath);
            }
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

            XmlDocumentFragment frag = doc.CreateDocumentFragment();
            frag.InnerXml = "<path programName=\"" + programName + 
                            "\" programPath=\"" + programPath + "\"/>";
            Console.WriteLine(frag.InnerXml);
            doc.DocumentElement.AppendChild(frag);
            doc.Save("C:\\Users\\Ari\\source\\repos\\AlexaComp\\AlexaComp\\bin\\Debug\\pathDir.xml");
            doc.Save("C:\\Users\\Ari\\source\\repos\\AlexaComp\\AlexaComp\\pathDir.xml");

            dataListView_Load();
        }


        private void label1_Click(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void programNameTextBox_TextChanged(object sender, EventArgs e) { }
        private void programPathTextBox_TextChanged(object sender, EventArgs e) { }
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e) { }
        private void dataListView_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}
