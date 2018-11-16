namespace AlexaComp
{
    partial class AlexaCompGUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlexaCompGUI));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.RepoLink = new System.Windows.Forms.LinkLabel();
            this.openLogFileButton = new System.Windows.Forms.Button();
            this.LogAllSensorsButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.runOnStartupCheck = new System.Windows.Forms.CheckBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.Overview = new System.Windows.Forms.TabPage();
            this.Settings = new System.Windows.Forms.TabPage();
            this.RGBSystemLabel = new System.Windows.Forms.Label();
            this.RBGDropdown = new System.Windows.Forms.ComboBox();
            this.ProgramList = new System.Windows.Forms.TabPage();
            this.errorLabel = new System.Windows.Forms.Label();
            this.dataListView = new System.Windows.Forms.ListView();
            this.addToListButton = new System.Windows.Forms.Button();
            this.programPathTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.programNameTextBox = new System.Windows.Forms.TextBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.RGBSubmitButton = new System.Windows.Forms.Button();
            this.RBox = new System.Windows.Forms.TextBox();
            this.GBox = new System.Windows.Forms.TextBox();
            this.BBox = new System.Windows.Forms.TextBox();
            this.SpeedBox = new System.Windows.Forms.TextBox();
            this.GranularityBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.Overview.SuspendLayout();
            this.Settings.SuspendLayout();
            this.ProgramList.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.BalloonTipText = "AlexaComp has been minimized to system tray.";
            this.notifyIcon.BalloonTipTitle = "AlexaComp";
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "AlexaComp";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Roboto", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 13);
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
            this.RepoLink.Location = new System.Drawing.Point(14, 413);
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
            this.openLogFileButton.Location = new System.Drawing.Point(18, 336);
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
            this.LogAllSensorsButton.Location = new System.Drawing.Point(18, 367);
            this.LogAllSensorsButton.Name = "LogAllSensorsButton";
            this.LogAllSensorsButton.Size = new System.Drawing.Size(173, 25);
            this.LogAllSensorsButton.TabIndex = 17;
            this.LogAllSensorsButton.Text = "Log All Sensors";
            this.LogAllSensorsButton.UseVisualStyleBackColor = true;
            this.LogAllSensorsButton.Click += new System.EventHandler(this.LogAllSensorsButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // runOnStartupCheck
            // 
            this.runOnStartupCheck.AutoSize = true;
            this.runOnStartupCheck.Location = new System.Drawing.Point(18, 19);
            this.runOnStartupCheck.Name = "runOnStartupCheck";
            this.runOnStartupCheck.Size = new System.Drawing.Size(156, 17);
            this.runOnStartupCheck.TabIndex = 15;
            this.runOnStartupCheck.Text = "Run AlexaComp On Startup";
            this.runOnStartupCheck.UseVisualStyleBackColor = true;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.Overview);
            this.tabControl.Controls.Add(this.Settings);
            this.tabControl.Controls.Add(this.ProgramList);
            this.tabControl.Location = new System.Drawing.Point(1, -1);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(657, 470);
            this.tabControl.TabIndex = 18;
            // 
            // Overview
            // 
            this.Overview.Controls.Add(this.label1);
            this.Overview.Controls.Add(this.RepoLink);
            this.Overview.Location = new System.Drawing.Point(4, 22);
            this.Overview.Name = "Overview";
            this.Overview.Padding = new System.Windows.Forms.Padding(3);
            this.Overview.Size = new System.Drawing.Size(649, 444);
            this.Overview.TabIndex = 0;
            this.Overview.Text = "Overview";
            this.Overview.UseVisualStyleBackColor = true;
            // 
            // Settings
            // 
            this.Settings.Controls.Add(this.label8);
            this.Settings.Controls.Add(this.label7);
            this.Settings.Controls.Add(this.label6);
            this.Settings.Controls.Add(this.label5);
            this.Settings.Controls.Add(this.label4);
            this.Settings.Controls.Add(this.GranularityBox);
            this.Settings.Controls.Add(this.SpeedBox);
            this.Settings.Controls.Add(this.BBox);
            this.Settings.Controls.Add(this.GBox);
            this.Settings.Controls.Add(this.RBox);
            this.Settings.Controls.Add(this.RGBSubmitButton);
            this.Settings.Controls.Add(this.RGBSystemLabel);
            this.Settings.Controls.Add(this.openLogFileButton);
            this.Settings.Controls.Add(this.LogAllSensorsButton);
            this.Settings.Controls.Add(this.RBGDropdown);
            this.Settings.Controls.Add(this.runOnStartupCheck);
            this.Settings.Location = new System.Drawing.Point(4, 22);
            this.Settings.Name = "Settings";
            this.Settings.Padding = new System.Windows.Forms.Padding(3);
            this.Settings.Size = new System.Drawing.Size(649, 444);
            this.Settings.TabIndex = 1;
            this.Settings.Text = "Settings";
            this.Settings.UseVisualStyleBackColor = true;
            // 
            // RGBSystemLabel
            // 
            this.RGBSystemLabel.AutoSize = true;
            this.RGBSystemLabel.Location = new System.Drawing.Point(18, 66);
            this.RGBSystemLabel.Name = "RGBSystemLabel";
            this.RGBSystemLabel.Size = new System.Drawing.Size(85, 13);
            this.RGBSystemLabel.TabIndex = 17;
            this.RGBSystemLabel.Text = "Test RGB Effect";
            this.RGBSystemLabel.Click += new System.EventHandler(this.RGBSystemLabel_Click);
            // 
            // RBGDropdown
            // 
            this.RBGDropdown.FormattingEnabled = true;
            this.RBGDropdown.Items.AddRange(new object[] {
            "Rainbow Fading",
            "Static Color",
            "Alternating",
            "Error Effect",
            "Pulsing Effect",
            "Breathing Color"});
            this.RBGDropdown.Location = new System.Drawing.Point(18, 85);
            this.RBGDropdown.Name = "RBGDropdown";
            this.RBGDropdown.Size = new System.Drawing.Size(121, 21);
            this.RBGDropdown.TabIndex = 16;
            // 
            // ProgramList
            // 
            this.ProgramList.Controls.Add(this.errorLabel);
            this.ProgramList.Controls.Add(this.dataListView);
            this.ProgramList.Controls.Add(this.addToListButton);
            this.ProgramList.Controls.Add(this.programPathTextBox);
            this.ProgramList.Controls.Add(this.label3);
            this.ProgramList.Controls.Add(this.label2);
            this.ProgramList.Controls.Add(this.programNameTextBox);
            this.ProgramList.Location = new System.Drawing.Point(4, 22);
            this.ProgramList.Name = "ProgramList";
            this.ProgramList.Size = new System.Drawing.Size(649, 444);
            this.ProgramList.TabIndex = 2;
            this.ProgramList.Text = "ProgramList";
            this.ProgramList.UseVisualStyleBackColor = true;
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.errorLabel.Location = new System.Drawing.Point(150, 372);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(125, 18);
            this.errorLabel.TabIndex = 8;
            this.errorLabel.Text = "Default Label Text";
            this.errorLabel.Visible = false;
            this.errorLabel.Click += new System.EventHandler(this.errorLabel_Click);
            // 
            // dataListView
            // 
            this.dataListView.Location = new System.Drawing.Point(3, 3);
            this.dataListView.Name = "dataListView";
            this.dataListView.Size = new System.Drawing.Size(643, 302);
            this.dataListView.TabIndex = 7;
            this.dataListView.UseCompatibleStateImageBehavior = false;
            // 
            // addToListButton
            // 
            this.addToListButton.Location = new System.Drawing.Point(490, 372);
            this.addToListButton.Name = "addToListButton";
            this.addToListButton.Size = new System.Drawing.Size(150, 23);
            this.addToListButton.TabIndex = 0;
            this.addToListButton.Text = "Add Program to List";
            this.addToListButton.UseVisualStyleBackColor = true;
            this.addToListButton.Click += new System.EventHandler(this.addToListButton_Click_1);
            // 
            // programPathTextBox
            // 
            this.programPathTextBox.Location = new System.Drawing.Point(153, 337);
            this.programPathTextBox.Name = "programPathTextBox";
            this.programPathTextBox.Size = new System.Drawing.Size(493, 20);
            this.programPathTextBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 340);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Program Path (Absolute) -";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 314);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Program Name -";
            // 
            // programNameTextBox
            // 
            this.programNameTextBox.Location = new System.Drawing.Point(153, 311);
            this.programNameTextBox.Name = "programNameTextBox";
            this.programNameTextBox.Size = new System.Drawing.Size(493, 20);
            this.programNameTextBox.TabIndex = 6;
            // 
            // RGBSubmitButton
            // 
            this.RGBSubmitButton.Location = new System.Drawing.Point(86, 246);
            this.RGBSubmitButton.Name = "RGBSubmitButton";
            this.RGBSubmitButton.Size = new System.Drawing.Size(75, 23);
            this.RGBSubmitButton.TabIndex = 18;
            this.RGBSubmitButton.Text = "Submit";
            this.RGBSubmitButton.UseVisualStyleBackColor = true;
            this.RGBSubmitButton.Click += new System.EventHandler(this.RGBSubmitButton_Click);
            // 
            // RBox
            // 
            this.RBox.Location = new System.Drawing.Point(61, 112);
            this.RBox.Name = "RBox";
            this.RBox.Size = new System.Drawing.Size(100, 20);
            this.RBox.TabIndex = 19;
            // 
            // GBox
            // 
            this.GBox.Location = new System.Drawing.Point(61, 139);
            this.GBox.Name = "GBox";
            this.GBox.Size = new System.Drawing.Size(100, 20);
            this.GBox.TabIndex = 20;
            // 
            // BBox
            // 
            this.BBox.Location = new System.Drawing.Point(61, 166);
            this.BBox.Name = "BBox";
            this.BBox.Size = new System.Drawing.Size(100, 20);
            this.BBox.TabIndex = 21;
            // 
            // SpeedBox
            // 
            this.SpeedBox.Location = new System.Drawing.Point(61, 193);
            this.SpeedBox.Name = "SpeedBox";
            this.SpeedBox.Size = new System.Drawing.Size(100, 20);
            this.SpeedBox.TabIndex = 22;
            // 
            // GranularityBox
            // 
            this.GranularityBox.Location = new System.Drawing.Point(81, 219);
            this.GranularityBox.Name = "GranularityBox";
            this.GranularityBox.Size = new System.Drawing.Size(100, 20);
            this.GranularityBox.TabIndex = 23;
            this.GranularityBox.TextChanged += new System.EventHandler(this.textBox5_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "R";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 145);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "G";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 172);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 26;
            this.label6.Text = "B";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 199);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 27;
            this.label7.Text = "Speed";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 226);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "Granularity";
            // 
            // AlexaCompGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(657, 469);
            this.Controls.Add(this.tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(400, 508);
            this.Name = "AlexaCompGUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AlexaComp Configuration";
            this.tabControl.ResumeLayout(false);
            this.Overview.ResumeLayout(false);
            this.Overview.PerformLayout();
            this.Settings.ResumeLayout(false);
            this.Settings.PerformLayout();
            this.ProgramList.ResumeLayout(false);
            this.ProgramList.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel RepoLink;
        private System.Windows.Forms.Button openLogFileButton;
        private System.Windows.Forms.Button LogAllSensorsButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox runOnStartupCheck;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage Overview;
        private System.Windows.Forms.TabPage Settings;
        private System.Windows.Forms.Label RGBSystemLabel;
        private System.Windows.Forms.ComboBox RBGDropdown;
        private System.Windows.Forms.TabPage ProgramList;
        private System.Windows.Forms.ListView dataListView;
        private System.Windows.Forms.TextBox programNameTextBox;
        private System.Windows.Forms.TextBox programPathTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button addToListButton;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox GranularityBox;
        private System.Windows.Forms.TextBox SpeedBox;
        private System.Windows.Forms.TextBox BBox;
        private System.Windows.Forms.TextBox GBox;
        private System.Windows.Forms.TextBox RBox;
        private System.Windows.Forms.Button RGBSubmitButton;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
    }
}

