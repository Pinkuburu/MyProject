using System;
using System.Windows.Forms;

namespace ServerMonitor
{
    public partial class Form_ChangeUserPassword : Form
    {
        public string strUserName = null;
        public string strPassword = null;

        public Form_ChangeUserPassword()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.strUserName = textBox_UserName.Text.Trim();
            this.strPassword = textBox_Password.Text.Trim();
        }
    }
}
