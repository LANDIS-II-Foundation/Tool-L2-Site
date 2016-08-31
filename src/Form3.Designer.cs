namespace SiteVegCalcV2_3
{
    partial class Form3
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.loadButton = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.tbFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.doneButton = new System.Windows.Forms.Button();
            this.cbRef = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(61, 6);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(152, 38);
            this.loadButton.TabIndex = 0;
            this.loadButton.Text = "Load Text File";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Items.AddRange(new object[] {
            "Biomass",
            "PctShade",
            "ShadeClass",
            "Num Cohorts",
            "Cohort ANPP",
            "Cohort Biomass",
            "Dead Wood"});
            this.listBox1.Location = new System.Drawing.Point(61, 109);
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox1.Size = new System.Drawing.Size(151, 95);
            this.listBox1.TabIndex = 1;
            // 
            // tbFileName
            // 
            this.tbFileName.Location = new System.Drawing.Point(63, 64);
            this.tbFileName.Name = "tbFileName";
            this.tbFileName.Size = new System.Drawing.Size(150, 20);
            this.tbFileName.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(61, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Choose graphs to add point to:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(95, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Reference File";
            // 
            // doneButton
            // 
            this.doneButton.Location = new System.Drawing.Point(60, 210);
            this.doneButton.Name = "doneButton";
            this.doneButton.Size = new System.Drawing.Size(152, 38);
            this.doneButton.TabIndex = 5;
            this.doneButton.Text = "Done";
            this.doneButton.UseVisualStyleBackColor = true;
            this.doneButton.Click += new System.EventHandler(this.doneButton_Click);
            // 
            // cbRef
            // 
            this.cbRef.AutoSize = true;
            this.cbRef.Location = new System.Drawing.Point(227, 234);
            this.cbRef.Name = "cbRef";
            this.cbRef.Size = new System.Drawing.Size(15, 14);
            this.cbRef.TabIndex = 6;
            this.cbRef.UseVisualStyleBackColor = true;
            this.cbRef.Visible = false;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 259);
            this.Controls.Add(this.cbRef);
            this.Controls.Add(this.doneButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbFileName);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.loadButton);
            this.Name = "Form3";
            this.Text = "Add Reference Data";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button doneButton;
        public System.Windows.Forms.CheckBox cbRef;
        public System.Windows.Forms.ListBox listBox1;
        public System.Windows.Forms.TextBox tbFileName;
    }
}