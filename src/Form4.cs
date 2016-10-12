using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace L2_Site_Budworm
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                tbFileName.Text = saveFileDialog1.FileName;
                
            }
           
        

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            cbSave.Checked = true;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            cbSave.Checked = false;
            this.Close();
        }
    }
}
