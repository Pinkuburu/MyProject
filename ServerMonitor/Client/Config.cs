using System;
using System.Windows.Forms;
using System.IO;

namespace Client
{
    public partial class Config : Form
    {
        IniFile ini = new IniFile(Application.StartupPath + "\\Client.ini");
        Library lib = new Library();
        SysInfoClass SIC = new SysInfoClass();
        //1vu(enex01
        public Config()
        {
            InitializeComponent();
            this.AcceptButton = button_Set;

            if (File.Exists(@"Client.ini"))
            {
                string strPassword = lib.ReadINI("ConnectionServer", "Password");

                textBox_UserName.Text = lib.ReadINI("ConnectionServer", "UserName");
                textBox_Password.Text = lib.DESDecrypt(strPassword);
                textBox_ServerAddress.Text = lib.ReadINI("ConnectionServer", "ServerAddress");
                textBox_ServerName.Text = lib.ReadINI("ClientConfig", "ServerName");
                textBox_MonitorServer.Text = lib.ReadINI("ClientConfig", "MonitorServer");
                //checkBox_AutoRun.Checked = Convert.ToBoolean(lib.ReadINI("ClientConfig", "AutoRun"));
            }
            else
            {
                textBox_UserName.Text = SIC.ServerIP();
            }
        }

        private void button_Set_Click(object sender, EventArgs e)
        {
            string strUserName = textBox_UserName.Text.Trim().ToString();
            string strPassword = textBox_Password.Text.Trim().ToString();
            string strServerAddress = textBox_ServerAddress.Text.Trim().ToString();
            string strServerName = textBox_ServerName.Text.Trim().ToString();
            string strMonitorServer = textBox_MonitorServer.Text.Trim().ToString();
            //string strAutoRun = checkBox_AutoRun.Checked.ToString();

            if (strUserName != "" && strPassword != "" && strServerAddress != "" && strServerName != "")
            {
                //if (checkBox_AutoRun.Checked)
                //{
                //    lib.AutoRun(Application.ExecutablePath, true);
                //}
                //else
                //{
                //    lib.AutoRun(Application.ExecutablePath, false);
                //}

                lib.SaveINI("ConnectionServer", "UserName", strUserName);
                lib.SaveINI("ConnectionServer", "Password", lib.DESEncrypt(strPassword));
                lib.SaveINI("ConnectionServer", "ServerAddress", strServerAddress);
                lib.SaveINI("ClientConfig", "ServerName", strServerName);
                lib.SaveINI("ClientConfig", "MonitorServer", strMonitorServer);
                lib.SaveINI("ClientConfig", "Debug", "0");
                //lib.SaveINI("ClientConfig", "AutoRun", strAutoRun);
                lib.SaveINI("Update", "Url", "http://ishow.xba.com.cn/");
                lib.SaveINI("Update", "Version", "0");
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
