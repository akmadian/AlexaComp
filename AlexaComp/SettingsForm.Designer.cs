namespace AlexaComp
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.HostSettingLabel = new System.Windows.Forms.Label();
            this.PortSettingLabel = new System.Windows.Forms.Label();
            this.AuthSettingLabel = new System.Windows.Forms.Label();
            this.MqttUserSettingLabel = new System.Windows.Forms.Label();
            this.MqttPassSettingLabel = new System.Windows.Forms.Label();
            this.MqttServerSetttingLabel = new System.Windows.Forms.Label();
            this.MqttPortSettingLabel = new System.Windows.Forms.Label();
            this.UpdateSettingsButton = new System.Windows.Forms.Button();
            this.HOST = new System.Windows.Forms.TextBox();
            this.PORT = new System.Windows.Forms.TextBox();
            this.AUTH = new System.Windows.Forms.TextBox();
            this.mqttUSER = new System.Windows.Forms.TextBox();
            this.mqttPASS = new System.Windows.Forms.TextBox();
            this.mqttSERVER = new System.Windows.Forms.TextBox();
            this.mqttPORT = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // HostSettingLabel
            // 
            this.HostSettingLabel.AutoSize = true;
            this.HostSettingLabel.Location = new System.Drawing.Point(29, 18);
            this.HostSettingLabel.Name = "HostSettingLabel";
            this.HostSettingLabel.Size = new System.Drawing.Size(29, 13);
            this.HostSettingLabel.TabIndex = 0;
            this.HostSettingLabel.Text = "Host";
            // 
            // PortSettingLabel
            // 
            this.PortSettingLabel.AutoSize = true;
            this.PortSettingLabel.Location = new System.Drawing.Point(29, 41);
            this.PortSettingLabel.Name = "PortSettingLabel";
            this.PortSettingLabel.Size = new System.Drawing.Size(26, 13);
            this.PortSettingLabel.TabIndex = 1;
            this.PortSettingLabel.Text = "Port";
            // 
            // AuthSettingLabel
            // 
            this.AuthSettingLabel.AutoSize = true;
            this.AuthSettingLabel.Location = new System.Drawing.Point(29, 63);
            this.AuthSettingLabel.Name = "AuthSettingLabel";
            this.AuthSettingLabel.Size = new System.Drawing.Size(50, 13);
            this.AuthSettingLabel.TabIndex = 2;
            this.AuthSettingLabel.Text = "Auth Key";
            // 
            // MqttUserSettingLabel
            // 
            this.MqttUserSettingLabel.AutoSize = true;
            this.MqttUserSettingLabel.Location = new System.Drawing.Point(29, 106);
            this.MqttUserSettingLabel.Name = "MqttUserSettingLabel";
            this.MqttUserSettingLabel.Size = new System.Drawing.Size(63, 13);
            this.MqttUserSettingLabel.TabIndex = 3;
            this.MqttUserSettingLabel.Text = "MQTT User";
            // 
            // MqttPassSettingLabel
            // 
            this.MqttPassSettingLabel.AutoSize = true;
            this.MqttPassSettingLabel.Location = new System.Drawing.Point(29, 129);
            this.MqttPassSettingLabel.Name = "MqttPassSettingLabel";
            this.MqttPassSettingLabel.Size = new System.Drawing.Size(64, 13);
            this.MqttPassSettingLabel.TabIndex = 4;
            this.MqttPassSettingLabel.Text = "MQTT Pass";
            // 
            // MqttServerSetttingLabel
            // 
            this.MqttServerSetttingLabel.AutoSize = true;
            this.MqttServerSetttingLabel.Location = new System.Drawing.Point(29, 151);
            this.MqttServerSetttingLabel.Name = "MqttServerSetttingLabel";
            this.MqttServerSetttingLabel.Size = new System.Drawing.Size(72, 13);
            this.MqttServerSetttingLabel.TabIndex = 5;
            this.MqttServerSetttingLabel.Text = "MQTT Server";
            // 
            // MqttPortSettingLabel
            // 
            this.MqttPortSettingLabel.AutoSize = true;
            this.MqttPortSettingLabel.Location = new System.Drawing.Point(29, 173);
            this.MqttPortSettingLabel.Name = "MqttPortSettingLabel";
            this.MqttPortSettingLabel.Size = new System.Drawing.Size(60, 13);
            this.MqttPortSettingLabel.TabIndex = 6;
            this.MqttPortSettingLabel.Text = "MQTT Port";
            // 
            // UpdateSettingsButton
            // 
            this.UpdateSettingsButton.Location = new System.Drawing.Point(182, 203);
            this.UpdateSettingsButton.Name = "UpdateSettingsButton";
            this.UpdateSettingsButton.Size = new System.Drawing.Size(110, 23);
            this.UpdateSettingsButton.TabIndex = 7;
            this.UpdateSettingsButton.Text = "Update Settings";
            this.UpdateSettingsButton.UseVisualStyleBackColor = true;
            this.UpdateSettingsButton.Click += new System.EventHandler(this.UpdateSettingsButton_Click);
            // 
            // HOST
            // 
            this.HOST.Location = new System.Drawing.Point(115, 15);
            this.HOST.Name = "HOST";
            this.HOST.Size = new System.Drawing.Size(161, 20);
            this.HOST.TabIndex = 8;
            // 
            // PORT
            // 
            this.PORT.Location = new System.Drawing.Point(115, 38);
            this.PORT.Name = "PORT";
            this.PORT.Size = new System.Drawing.Size(161, 20);
            this.PORT.TabIndex = 9;
            // 
            // AUTH
            // 
            this.AUTH.Location = new System.Drawing.Point(115, 60);
            this.AUTH.Name = "AUTH";
            this.AUTH.Size = new System.Drawing.Size(161, 20);
            this.AUTH.TabIndex = 10;
            // 
            // mqttUSER
            // 
            this.mqttUSER.Location = new System.Drawing.Point(115, 103);
            this.mqttUSER.Name = "mqttUSER";
            this.mqttUSER.Size = new System.Drawing.Size(161, 20);
            this.mqttUSER.TabIndex = 11;
            // 
            // mqttPASS
            // 
            this.mqttPASS.Location = new System.Drawing.Point(115, 126);
            this.mqttPASS.Name = "mqttPASS";
            this.mqttPASS.Size = new System.Drawing.Size(161, 20);
            this.mqttPASS.TabIndex = 12;
            // 
            // mqttSERVER
            // 
            this.mqttSERVER.Location = new System.Drawing.Point(115, 148);
            this.mqttSERVER.Name = "mqttSERVER";
            this.mqttSERVER.Size = new System.Drawing.Size(161, 20);
            this.mqttSERVER.TabIndex = 13;
            // 
            // mqttPORT
            // 
            this.mqttPORT.Location = new System.Drawing.Point(115, 170);
            this.mqttPORT.Name = "mqttPORT";
            this.mqttPORT.Size = new System.Drawing.Size(161, 20);
            this.mqttPORT.TabIndex = 14;
            // 
            // AlexaCompSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 235);
            this.Controls.Add(this.mqttPORT);
            this.Controls.Add(this.mqttSERVER);
            this.Controls.Add(this.mqttPASS);
            this.Controls.Add(this.mqttUSER);
            this.Controls.Add(this.AUTH);
            this.Controls.Add(this.PORT);
            this.Controls.Add(this.HOST);
            this.Controls.Add(this.UpdateSettingsButton);
            this.Controls.Add(this.MqttPortSettingLabel);
            this.Controls.Add(this.MqttServerSetttingLabel);
            this.Controls.Add(this.MqttPassSettingLabel);
            this.Controls.Add(this.MqttUserSettingLabel);
            this.Controls.Add(this.AuthSettingLabel);
            this.Controls.Add(this.PortSettingLabel);
            this.Controls.Add(this.HostSettingLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AlexaCompSettingsForm";
            this.Text = "AlexaComp Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label HostSettingLabel;
        private System.Windows.Forms.Label PortSettingLabel;
        private System.Windows.Forms.Label AuthSettingLabel;
        private System.Windows.Forms.Label MqttUserSettingLabel;
        private System.Windows.Forms.Label MqttPassSettingLabel;
        private System.Windows.Forms.Label MqttServerSetttingLabel;
        private System.Windows.Forms.Label MqttPortSettingLabel;
        private System.Windows.Forms.Button UpdateSettingsButton;
        private System.Windows.Forms.TextBox HOST;
        private System.Windows.Forms.TextBox PORT;
        private System.Windows.Forms.TextBox AUTH;
        private System.Windows.Forms.TextBox mqttUSER;
        private System.Windows.Forms.TextBox mqttPASS;
        private System.Windows.Forms.TextBox mqttSERVER;
        private System.Windows.Forms.TextBox mqttPORT;
    }
}