namespace AlexaComp
{
    partial class ProgramListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgramListForm));
            this.programPathTextBox = new System.Windows.Forms.TextBox();
            this.addToListButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.programNameTextBox = new System.Windows.Forms.TextBox();
            this.dataListView = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // programPathTextBox
            // 
            this.programPathTextBox.Location = new System.Drawing.Point(157, 347);
            this.programPathTextBox.Name = "programPathTextBox";
            this.programPathTextBox.Size = new System.Drawing.Size(493, 20);
            this.programPathTextBox.TabIndex = 3;
            this.programPathTextBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // addToListButton
            // 
            this.addToListButton.Location = new System.Drawing.Point(477, 373);
            this.addToListButton.Name = "addToListButton";
            this.addToListButton.Size = new System.Drawing.Size(150, 23);
            this.addToListButton.TabIndex = 0;
            this.addToListButton.Text = "Add Program to List";
            this.addToListButton.UseVisualStyleBackColor = true;
            this.addToListButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 350);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Program Path (Absolute) -";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 325);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Program Name -";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // programNameTextBox
            // 
            this.programNameTextBox.Location = new System.Drawing.Point(157, 322);
            this.programNameTextBox.Name = "programNameTextBox";
            this.programNameTextBox.Size = new System.Drawing.Size(493, 20);
            this.programNameTextBox.TabIndex = 6;
            this.programNameTextBox.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // dataListView
            // 
            this.dataListView.Location = new System.Drawing.Point(12, 12);
            this.dataListView.Name = "dataListView";
            this.dataListView.Size = new System.Drawing.Size(643, 302);
            this.dataListView.TabIndex = 7;
            this.dataListView.UseCompatibleStateImageBehavior = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 407);
            this.Controls.Add(this.dataListView);
            this.Controls.Add(this.programNameTextBox);
            this.Controls.Add(this.programPathTextBox);
            this.Controls.Add(this.addToListButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(682, 446);
            this.MinimumSize = new System.Drawing.Size(682, 446);
            this.Name = "Form1";
            this.Text = "AlexaComp Program List Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox programPathTextBox;
        private System.Windows.Forms.Button addToListButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox programNameTextBox;
        private System.Windows.Forms.ListView dataListView;
    }
}