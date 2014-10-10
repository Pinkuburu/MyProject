using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace island
{
    public partial class Login_Form : Form
    {
        public Login_Form()
        {
            InitializeComponent();
            this.AcceptButton = btn_Login;
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            if (textBox_LoginForm_UserName.Text.Trim() == "" || textBox_Login_Form_Password.Text.Trim() == "")
            {
                MessageBox.Show("用户名或密码填写有误", "系统提示");
            }
            else
            {
                this.Hide();    //隐藏登录窗口
                Main_Form Main_Form = new Main_Form();//调用主操作窗口
                Main_Form.strUserName = textBox_LoginForm_UserName.Text.Trim();
                Main_Form.strPassword = textBox_Login_Form_Password.Text.Trim();
                //Main_Form.Login_Handle();
                Main_Form.TB_Login_Handle();
                if (Main_Form.ShowDialog() == DialogResult.Cancel)
                {
                    Application.Exit();
                }
            }
        }
    }
}