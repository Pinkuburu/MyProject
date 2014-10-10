using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ServerMonitor
{
    public partial class Form_Download : Form
    {
        public string strUrl;
        public string strPath;
        public string strUnRAR;

        public Form_Download()
        {
            InitializeComponent();
        }

        private void button_Download_Click(object sender, EventArgs e)
        {
            this.strUrl = textBox_Url.Text.Trim();
            this.strPath = textBox_Path.Text.Trim();
            this.strUnRAR = checkBox_unrar.Checked.ToString();
        }
    }
}
