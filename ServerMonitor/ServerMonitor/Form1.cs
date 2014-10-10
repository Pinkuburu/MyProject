using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ServerMonitor
{
    public partial class Form1 : Form
    {
        public string strServer = null;
        public string strUserName = null;
        public string strPassword = null;

        INIClass ini = new INIClass(@"config.ini");

        public Form1()
        {
            InitializeComponent();
            ini.ReadConfig("ConnectionServer", "Server", ref this.strServer);
            ini.ReadConfig("ConnectionServer", "UserName", ref this.strUserName);
            ini.ReadConfig("ConnectionServer", "Password", ref this.strPassword);
        }

        private void toolStripButton_Login_Click(object sender, EventArgs e)
        {
            XMPPClass objXmpp = new XMPPClass(this.strUserName, this.strPassword, this.strServer);
            toolStripButton_Login.Enabled = false;
        }
    }
}
