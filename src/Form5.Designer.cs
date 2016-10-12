namespace L2_Site_Budworm
{
    partial class Form5
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
            this.tbFileName = new System.Windows.Forms.TextBox();
            this.loadButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.cbLoad = new System.Windows.Forms.CheckBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // tbFileName
            // 
            this.tbFileName.Location = new System.Drawing.Point(4, 38);
            this.tbFileName.Name = "tbFileName";
            this.tbFileName.Size = new System.Drawing.Size(203, 20);
            this.tbFileName.TabIndex = 4;
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(217, 33);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(71, 30);
            this.loadButton.TabIndex = 3;
            this.loadButton.Text = "Browse";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Choose File Name";
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(119, 64);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(64, 30);
            this.buttonLoad.TabIndex = 6;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbLoad
            // 
            this.cbLoad.AutoSize = true;
            this.cbLoad.Location = new System.Drawing.Point(217, 81);
            this.cbLoad.Name = "cbLoad";
            this.cbLoad.Size = new System.Drawing.Size(15, 14);
            this.cbLoad.TabIndex = 7;
            this.cbLoad.UseVisualStyleBackColor = true;
            this.cbLoad.Visible = false;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(40, 64);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(64, 30);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "\"txt files (*.txt)|*.txt|All files (*.*)|*.*\" ";
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 107);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.cbLoad);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbFileName);
            this.Controls.Add(this.loadButton);
            this.Name = "Form5";
            this.Text = "Load Species Parameters";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox tbFileName;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonLoad;
        public System.Windows.Forms.CheckBox cbLoad;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}