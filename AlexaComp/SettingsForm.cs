using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlexaComp
{
    public partial class SettingsForm : Form
    {

        private Dictionary<string, string> textBoxDict = new Dictionary<string, string>();
        public SettingsForm(){
            InitializeComponent();
            this.AcceptButton = this.UpdateSettingsButton;

            genTextBoxDict();
            fillTextBoxes();
        }

        private void genTextBoxDict(){
            foreach (Control c in this.Controls){
                if (c.GetType().ToString() == "System.Windows.Forms.TextBox"){
                    textBoxDict[c.Name.ToString()] = c.Text;
                }
            }
        }

        private void fillTextBoxes(){
            foreach (Control c in this.Controls){
                if (c.GetType().ToString() == "System.Windows.Forms.TextBox"){
                    Console.WriteLine("TextBox Detected: " + c.Name.ToString());
                    c.Text = AlexaComp.settingsDict[c.Name.ToString()];
                    Console.WriteLine(AlexaComp.settingsDict[c.Name.ToString()]);
                }
            }
        }

        private void UpdateSettingsButton_Click(object sender, EventArgs e){
            genTextBoxDict();
            foreach(KeyValuePair<string, string> pair in textBoxDict){
                if (pair.Value != AlexaComp.settingsDict[pair.Key]){
                    AlexaComp._log.Info("diff: " + pair.Key + " - " + pair.Value);
                    AlexaComp.UpdateConfigValue(pair.Key, pair.Value);
                }
            }
        }

        private void AlexaCompSettingsForm_Closing(object sender, FormClosingEventArgs e) {
            AlexaComp._log.Info("Closing SettingsForm");
        }
    }
}
