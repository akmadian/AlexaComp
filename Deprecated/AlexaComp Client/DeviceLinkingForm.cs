using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

using Newtonsoft.Json;

namespace AlexaComp {
    public partial class DeviceLinkingForm : Form {

        private static Request reqObj;
        private static NetworkStream nwStream;

        public DeviceLinkingForm() {
            InitializeComponent();
            this.ShowDialog();
        }

        public static void startDeviceLinking(object options) {
            Console.WriteLine("Starting device linking form");
            Options options_ = (Options)options;
            reqObj = options_.req;
            nwStream = options_.nwStream;
            Application.Run(new DeviceLinkingForm());
        }

        private void button1_Click(object sender, EventArgs e) {
            string keyEntered = authKeyBox.Text;
            string IPAddress = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();

            if (keyEntered == reqObj.PRIMARY) {
                Response res = new Response(true, "devicelinking", IPAddress, "");
                res.sendResponse(nwStream);
            } else {
                Response res = new Response(false, "devicelinking", IPAddress, "");
                res.sendResponse(nwStream);
            }
           

            //AlexaCompSERVER.sendToLambda(json, fullmessage: true);

        }

        private void button2_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void authKeyBox_TextChanged(object sender, EventArgs e) {}
    }
}
