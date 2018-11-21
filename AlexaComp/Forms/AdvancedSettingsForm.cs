using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AlexaComp;

namespace AlexaComp.Forms {
    public partial class AdvancedSettingsForm : Form {
        public AdvancedSettingsForm() {
            InitializeComponent();
        }

        public static void startAdvToolsThread() {
            Application.Run(new AdvancedSettingsForm());
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            Console.WriteLine("CB1 Changed - " + comboBox1.SelectedItem);
            PRIMARYCB.Items.Clear();
            SECONDARYCB.Items.Clear();
            TERTIARYCB.Items.Clear();
            switch (comboBox1.SelectedItem) {
                case "LAUNCH": Launch(); break;
                case "COMPUTERCOMMAND": ComputerCommand(); break;
                case "GETCOMPSTAT": break;
                case "AUDIOCOMMAND": AudioCommand();  break;
                case "RGBCOMMAND": RGBCommand(); break;
            }
        }

        private void submitRequestButton_Click(object sender, EventArgs e) {
            var COMMAND = comboBox1.SelectedItem;
            var PRIMARY = PRIMARYCB.SelectedItem;
            var SECONDARY = SECONDARYCB.SelectedItem;
            var TERTIARY = TERTIARYCB.SelectedItem;
            if (PRIMARY == null) { PRIMARY = ""; }
            if (SECONDARY == null) { SECONDARY = ""; }
            if (TERTIARY == null) { TERTIARY = ""; }
            AlexaCompCore.clog(String.Format("Custom Request Submitted - COMMAND: {0}, PRIMARY: {1}, SECONDARY: {2}, TERTIARY: {3}", 
                COMMAND, PRIMARY, SECONDARY, TERTIARY));
            Request custReq = new Request("testAuth", COMMAND.ToString(), 
                                                      PRIMARY.ToString(), 
                                                      SECONDARY.ToString(), 
                                                      TERTIARY.ToString());
            AlexaCompREQUEST.processRequest(custReq);
        }


        private void Launch() {
            PRIMARYCB.Items.Add("GOOGLECHROME");
            PRIMARYCB.Items.Add("STEAM");
        }

        private void ComputerCommand() {
            PRIMARYCB.Items.Add("SHUTDOWN");
            PRIMARYCB.Items.Add("LOCK");
            PRIMARYCB.Items.Add("RESTART");
            PRIMARYCB.Items.Add("SLEEP");
            PRIMARYCB.Items.Add("LOGOFF");
            PRIMARYCB.Items.Add("HIBERNATE");
        }

        private void AudioCommand() {
            PRIMARYCB.Items.Add("PLAYPAUSE");
            PRIMARYCB.Items.Add("NEXTTRACK");
            PRIMARYCB.Items.Add("PREVTRACK");
            PRIMARYCB.Items.Add("SETVOLUME");
            PRIMARYCB.Items.Add("VOLUMEUP");
            PRIMARYCB.Items.Add("VOLUMEDOWN");
            PRIMARYCB.Items.Add("TOGGLEMUTE");
        }

        private void RGBCommand() {
            PRIMARYCB.Items.Add("STATICCOLOR");
            PRIMARYCB.Items.Add("ERROREFFECT");
            PRIMARYCB.Items.Add("BREATHINGEFFECT");
            PRIMARYCB.Items.Add("RAINBOWFADEEFFECT");
            PRIMARYCB.Items.Add("PULSEEFFECT");
            PRIMARYCB.Items.Add("ALTERNATINGEFFECT");
            PRIMARYCB.Items.Add("BLINKINGEFFECT");
            PRIMARYCB.Items.Add("ALLLEDOFF");

            SECONDARYCB.Items.Add("COLOR HERE");

            TERTIARYCB.Items.Add("SECONDARY COLOR HERE");
        }
    }
}
