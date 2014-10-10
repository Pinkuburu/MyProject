using System;
using System.Windows.Forms;
using System.IO;

namespace ServerMonitor
{
    public partial class Config : Form
    {
        IniFile ini = new IniFile(@"ServerConfig.ini");
        //1vu(enex01
        public Config()
        {
            InitializeComponent();
            this.AcceptButton = button_Set;

            if (File.Exists(@"ServerConfig.ini"))
            {
                string strPassword = ini.GetString("ConnectionServer", "Password", "");

                textBox_UserName.Text = ini.GetString("ConnectionServer", "UserName", "");
                textBox_Password.Text = Cryptography.DESDecrypt(strPassword, "xmb*8g4f");
                textBox_ServerAddress.Text = ini.GetString("ConnectionServer", "ServerAddress", "");
                textBox_ServerName.Text = ini.GetString("ServerConfig", "ServerName", "");
            }
        }

        private void button_Set_Click(object sender, EventArgs e)
        {
            string strUserName = textBox_UserName.Text.Trim().ToString();
            string strPassword = textBox_Password.Text.Trim().ToString();
            string strServerAddress = textBox_ServerAddress.Text.Trim().ToString();
            string strServerName = textBox_ServerName.Text.Trim().ToString();

            if (strUserName != "" && strPassword != "" && strServerAddress != "" && strServerName != "")
            {
                ini.WriteValue("ConnectionServer", "UserName", strUserName);
                ini.WriteValue("ConnectionServer", "Password", Cryptography.DESEncrypt(strPassword, "xmb*8g4f"));
                ini.WriteValue("ConnectionServer", "ServerAddress", strServerAddress);
                ini.WriteValue("ServerConfig", "ServerName", strServerName);
                this.Close();
            }
            else
            {
                if (strUserName == "")
                {
                    MessageBox.Show("登录名不能为空");
                    return;
                }
                if (strPassword == "")
                {
                    MessageBox.Show("密码不能为空");
                    return;
                }
                if (strServerAddress == "")
                {
                    MessageBox.Show("服务器地址不能为空");
                    return;
                }
                if (strServerName == "")
                {
                    MessageBox.Show("服务器名称不能为空");
                    return;
                }
            }
        }
    }
}
