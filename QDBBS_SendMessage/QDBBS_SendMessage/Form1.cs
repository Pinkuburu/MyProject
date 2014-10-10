using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace QDBBS_SendMessage
{
    public partial class Form1 : Form
    {
        #region 窗体
        public Form1()
        {
            InitializeComponent();
            //toolStripStatusLabel1.Text = "";
            //listView1.Visible = false;
            //ListViewItem li = new ListViewItem();
            //li.SubItems.Clear();
            //Form2 f2 = new Form2();
            //f2.Show();  
            //f2.listView1.Items.Add(li);

            //f2.listView1.Items[0].SubItems[0].Text = "qwewqeqwe";
            //f2.listView1.Items[0].SubItems[1].Text = "qwewqeqwe";
            this.AcceptButton = button1;

            MaximizeBox = false;
            MinimizeBox = false;

            string filePath = Path.GetFullPath(@"Option.ini");
            INIClass iniClass = new INIClass(filePath);
            if (iniClass.ExistINIFile() == true)
            {
                textUserName.Text = iniClass.IniReadValue("UserInfo", "UserName");
                checkBox1.Checked = Convert.ToBoolean(iniClass.IniReadValue("UserInfo", "SaveUser").ToString());
            }
        }
        #endregion

        #region 登录
        private void button1_Click(object sender, EventArgs e)
        {
            string filePath = Path.GetFullPath(@"Option.ini");
            INIClass iniClass = new INIClass(filePath);

            if (checkBox1.Checked == true)
            {                                
                iniClass.IniWriteValue("UserInfo", "UserName", textUserName.Text.ToString().Trim());
                iniClass.IniWriteValue("UserInfo", "SaveUser", "true");
            }
            else
            {
                iniClass.IniWriteValue("UserInfo", "UserName", "");
                iniClass.IniWriteValue("UserInfo", "SaveUser", "false");
            }

            Form2 f2 = new Form2();
            this.Hide();          
            f2.LoadParameter(this.textUserName.Text.ToString().Trim(), this.textPassword.Text.ToString().Trim(), "http://club.qingdaonews.com/sublogin_new.php");
            if (f2.ShowDialog() == DialogResult.Cancel)
            {
                Application.Exit();
            }
        }
        #endregion
    }
}