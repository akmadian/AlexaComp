using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlexaComp {
    public partial class ConsoleForm : Form {

        public string ConsoleText = ">>> ";
        public string newCommandCarat = ">>> ";

        public static void StartConsoleWindow() {
            Application.Run(new ConsoleForm());
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }

        public ConsoleForm() {
            InitializeComponent();
            // consoleTextBox.Text = ConsoleText;
        }

        private void processCommand() {

        }

        private void ConsoleForm_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == Convert.ToChar(Keys.Return)) {

                consoleTextBox.AppendText("\n" + newCommandCarat);
                Console.WriteLine(consoleTextBox.Text);
            }
        }

        private void consoleTextBox_TextChanged(object sender, EventArgs e) {

        }


    }
}
