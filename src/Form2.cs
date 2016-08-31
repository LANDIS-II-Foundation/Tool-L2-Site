using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SiteVegCalc
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            this.Close();
            
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            this.Close();
        }
    }
}
