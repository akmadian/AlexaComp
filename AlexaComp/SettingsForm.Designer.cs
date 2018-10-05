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
            this.UpdateSettingsButton = new System.Windows.Forms.Button();
            this.HOST = new System.Windows.Forms.TextBox();
            this.runOnStartCheck = new System.Windows.Forms.CheckBox();
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
            // UpdateSettingsButton
            // 
            this.UpdateSettingsButton.Location = new System.Drawing.Point(179, 64);
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
            // runOnStartCheck
            // 
            this.runOnStartCheck.AutoSize = true;
            this.runOnStartCheck.Location = new System.Drawing.Point(32, 41);
            this.runOnStartCheck.Name = "runOnStartCheck";
            this.runOnStartCheck.Size = new System.Drawing.Size(156, 17);
            this.runOnStartCheck.TabIndex = 15;
            this.runOnStartCheck.Text = "Run AlexaComp On Startup";
            this.runOnStartCheck.UseVisualStyleBackColor = true;
            this.runOnStartCheck.CheckedChanged += new System.EventHandler(this.runOnStartCheck_CheckedChanged);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 97);
            this.Controls.Add(this.runOnStartCheck);
            this.Controls.Add(this.HOST);
            this.Controls.Add(this.UpdateSettingsButton);
            this.Controls.Add(this.HostSettingLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.Text = "AlexaComp Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label HostSettingLabel;
        private System.Windows.Forms.Button UpdateSettingsButton;
        private System.Windows.Forms.TextBox HOST;
        private System.Windows.Forms.CheckBox runOnStartCheck;
    }
}