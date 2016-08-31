using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;

namespace SiteVegCalcV2_3
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                tbFileName.Text = openFileDialog1.FileName;
                cbRef.Checked = true;
            }
           
        }

   
        private void doneButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
