using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SiteVegCalc
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                tbFileName.Text = openFileDialog1.FileName;
                
            }
           
        

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            cbLoad.Checked = true;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            cbLoad.Checked = false;
            this.Close();
        }
    }
}
