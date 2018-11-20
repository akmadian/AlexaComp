namespace AlexaComp.Forms {
    partial class AdvancedSettingsForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancedSettingsForm));
            this.label1 = new System.Windows.Forms.Label();
            this.submitRequestButton = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.PRIMARYCB = new System.Windows.Forms.ComboBox();
            this.SECONDARYCB = new System.Windows.Forms.ComboBox();
            this.TERTIARYCB = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Build Custom Request Object";
            // 
            // submitRequestButton
            // 
            this.submitRequestButton.Location = new System.Drawing.Point(108, 163);
            this.submitRequestButton.Name = "submitRequestButton";
            this.submitRequestButton.Size = new System.Drawing.Size(109, 23);
            this.submitRequestButton.TabIndex = 1;
            this.submitRequestButton.Text = "Submit Request";
            this.submitRequestButton.UseVisualStyleBackColor = true;
            this.submitRequestButton.Click += new System.EventHandler(this.submitRequestButton_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "LAUNCH",
            "COMPUTERCOMMAND",
            "GETCOMPSTAT",
            "AUDIOCOMMAND",
            "RGBCOMMAND"});
            this.comboBox1.Location = new System.Drawing.Point(96, 44);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(100, 21);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // PRIMARYCB
            // 
            this.PRIMARYCB.FormattingEnabled = true;
            this.PRIMARYCB.Location = new System.Drawing.Point(96, 71);
            this.PRIMARYCB.Name = "PRIMARYCB";
            this.PRIMARYCB.Size = new System.Drawing.Size(121, 21);
            this.PRIMARYCB.TabIndex = 3;
            // 
            // SECONDARYCB
            // 
            this.SECONDARYCB.FormattingEnabled = true;
            this.SECONDARYCB.Location = new System.Drawing.Point(96, 98);
            this.SECONDARYCB.Name = "SECONDARYCB";
            this.SECONDARYCB.Size = new System.Drawing.Size(121, 21);
            this.SECONDARYCB.TabIndex = 4;
            // 
            // TERTIARYCB
            // 
            this.TERTIARYCB.FormattingEnabled = true;
            this.TERTIARYCB.Location = new System.Drawing.Point(96, 125);
            this.TERTIARYCB.Name = "TERTIARYCB";
            this.TERTIARYCB.Size = new System.Drawing.Size(121, 21);
            this.TERTIARYCB.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "COMMAND";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "PRIMARY";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "SECONDARY";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 133);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "TERTIARY";
            // 
            // AdvancedSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TERTIARYCB);
            this.Controls.Add(this.SECONDARYCB);
            this.Controls.Add(this.PRIMARYCB);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.submitRequestButton);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AdvancedSettingsForm";
            this.Text = "Advanced Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button submitRequestButton;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox PRIMARYCB;
        private System.Windows.Forms.ComboBox SECONDARYCB;
        private System.Windows.Forms.ComboBox TERTIARYCB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}