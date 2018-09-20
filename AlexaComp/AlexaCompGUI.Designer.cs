namespace AlexaComp
{
    partial class AlexaComp_Config_Window
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlexaComp_Config_Window));
            this.EditConfigButton = new System.Windows.Forms.Button();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.editProgramListButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.RepoLink = new System.Windows.Forms.LinkLabel();
            this.openLogFileButton = new System.Windows.Forms.Button();
            this.LogAllSensorsButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // EditConfigButton
            // 
            this.EditConfigButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EditConfigButton.Location = new System.Drawing.Point(209, 46);
            this.EditConfigButton.Name = "EditConfigButton";
            this.EditConfigButton.Size = new System.Drawing.Size(173, 25);
            this.EditConfigButton.TabIndex = 10;
            this.EditConfigButton.Text = "Edit Config";
            this.EditConfigButton.UseVisualStyleBackColor = true;
            this.EditConfigButton.Click += new System.EventHandler(this.EditConfigButton_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.BalloonTipText = "AlexaComp has been minimized to system tray.";
            this.notifyIcon.BalloonTipTitle = "AlexaComp";
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "AlexaComp";
            // 
            // editProgramListButton
            // 
            this.editProgramListButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.editProgramListButton.Location = new System.Drawing.Point(209, 77);
            this.editProgramListButton.Name = "editProgramListButton";
            this.editProgramListButton.Size = new System.Drawing.Size(173, 25);
            this.editProgramListButton.TabIndex = 12;
            this.editProgramListButton.Text = "Edit Program List";
            this.editProgramListButton.UseVisualStyleBackColor = true;
            this.editProgramListButton.Click += new System.EventHandler(this.editProgramListButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(-42, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(225, 236);
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Roboto", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(17, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 38);
            this.label1.TabIndex = 14;
            this.label1.Text = "AlexaComp";
            // 
            // RepoLink
            // 
            this.RepoLink.AutoSize = true;
            this.RepoLink.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.RepoLink.LinkArea = new System.Windows.Forms.LinkArea(41, 57);
            this.RepoLink.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(230)))));
            this.RepoLink.Location = new System.Drawing.Point(12, 393);
            this.RepoLink.Name = "RepoLink";
            this.RepoLink.Size = new System.Drawing.Size(294, 17);
            this.RepoLink.TabIndex = 15;
            this.RepoLink.TabStop = true;
            this.RepoLink.Text = "This Alexa skill is open source, see the Github Repo here.";
            this.RepoLink.UseCompatibleTextRendering = true;
            this.RepoLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.RepoLink_LinkClicked);
            // 
            // openLogFileButton
            // 
            this.openLogFileButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.openLogFileButton.Location = new System.Drawing.Point(209, 108);
            this.openLogFileButton.Name = "openLogFileButton";
            this.openLogFileButton.Size = new System.Drawing.Size(173, 25);
            this.openLogFileButton.TabIndex = 16;
            this.openLogFileButton.Text = "Open Log File";
            this.openLogFileButton.UseVisualStyleBackColor = true;
            this.openLogFileButton.Click += new System.EventHandler(this.openLogFileButton_Click);
            // 
            // LogAllSensorsButton
            // 
            this.LogAllSensorsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LogAllSensorsButton.Location = new System.Drawing.Point(209, 139);
            this.LogAllSensorsButton.Name = "LogAllSensorsButton";
            this.LogAllSensorsButton.Size = new System.Drawing.Size(173, 25);
            this.LogAllSensorsButton.TabIndex = 17;
            this.LogAllSensorsButton.Text = "Log All Sensors";
            this.LogAllSensorsButton.UseVisualStyleBackColor = true;
            this.LogAllSensorsButton.Click += new System.EventHandler(this.LogAllSensorsButton_Click);
            // 
            // AlexaComp_Config_Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(404, 419);
            this.Controls.Add(this.LogAllSensorsButton);
            this.Controls.Add(this.openLogFileButton);
            this.Controls.Add(this.RepoLink);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.editProgramListButton);
            this.Controls.Add(this.EditConfigButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(420, 458);
            this.Name = "AlexaComp_Config_Window";
            this.Text = "AlexaComp Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button EditConfigButton;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Button editProgramListButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel RepoLink;
        private System.Windows.Forms.Button openLogFileButton;
        private System.Windows.Forms.Button LogAllSensorsButton;
    }
}

