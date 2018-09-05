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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlexaComp_Config_Window));
            this.addToListButton = new System.Windows.Forms.Button();
            this.programNameTextBox = new System.Windows.Forms.TextBox();
            this.programPathTextBox = new System.Windows.Forms.TextBox();
            this.programNameLable = new System.Windows.Forms.Label();
            this.programPathLabel = new System.Windows.Forms.Label();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.dataListView = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // addToListButton
            // 
            this.addToListButton.Location = new System.Drawing.Point(599, 378);
            this.addToListButton.Name = "addToListButton";
            this.addToListButton.Size = new System.Drawing.Size(150, 23);
            this.addToListButton.TabIndex = 0;
            this.addToListButton.Text = "Add Program to List";
            this.addToListButton.UseVisualStyleBackColor = true;
            this.addToListButton.Click += new System.EventHandler(this.addToListButton_Click);
            // 
            // programNameTextBox
            // 
            this.programNameTextBox.Location = new System.Drawing.Point(256, 326);
            this.programNameTextBox.Name = "programNameTextBox";
            this.programNameTextBox.Size = new System.Drawing.Size(493, 20);
            this.programNameTextBox.TabIndex = 2;
            this.programNameTextBox.TextChanged += new System.EventHandler(this.programNameTextBox_TextChanged);
            // 
            // programPathTextBox
            // 
            this.programPathTextBox.Location = new System.Drawing.Point(256, 352);
            this.programPathTextBox.Name = "programPathTextBox";
            this.programPathTextBox.Size = new System.Drawing.Size(493, 20);
            this.programPathTextBox.TabIndex = 3;
            this.programPathTextBox.TextChanged += new System.EventHandler(this.programPathTextBox_TextChanged);
            // 
            // programNameLable
            // 
            this.programNameLable.AutoSize = true;
            this.programNameLable.Location = new System.Drawing.Point(123, 329);
            this.programNameLable.Name = "programNameLable";
            this.programNameLable.Size = new System.Drawing.Size(83, 13);
            this.programNameLable.TabIndex = 4;
            this.programNameLable.Text = "Program Name -";
            this.programNameLable.Click += new System.EventHandler(this.label1_Click);
            // 
            // programPathLabel
            // 
            this.programPathLabel.AutoSize = true;
            this.programPathLabel.Location = new System.Drawing.Point(123, 355);
            this.programPathLabel.Name = "programPathLabel";
            this.programPathLabel.Size = new System.Drawing.Size(127, 13);
            this.programPathLabel.TabIndex = 5;
            this.programPathLabel.Text = "Program Path (Absolute) -";
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Location = new System.Drawing.Point(701, 13);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(21, 252);
            this.vScrollBar1.TabIndex = 7;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // dataListView
            // 
            this.dataListView.Location = new System.Drawing.Point(13, 13);
            this.dataListView.Name = "dataListView";
            this.dataListView.Size = new System.Drawing.Size(685, 252);
            this.dataListView.TabIndex = 8;
            this.dataListView.UseCompatibleStateImageBehavior = false;
            this.dataListView.SelectedIndexChanged += new System.EventHandler(this.dataListView_SelectedIndexChanged);
            // 
            // AlexaComp_Config_Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataListView);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.programPathLabel);
            this.Controls.Add(this.programNameLable);
            this.Controls.Add(this.programPathTextBox);
            this.Controls.Add(this.programNameTextBox);
            this.Controls.Add(this.addToListButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AlexaComp_Config_Window";
            this.Text = "AlexaComp Configuration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button addToListButton;
        private System.Windows.Forms.TextBox programNameTextBox;
        private System.Windows.Forms.TextBox programPathTextBox;
        private System.Windows.Forms.Label programNameLable;
        private System.Windows.Forms.Label programPathLabel;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.ListView dataListView;
    }
}

