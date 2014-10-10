using System;
using System.Windows.Forms;

namespace Happyisland
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
                if (radioButton_Login_Form_RR.Checked)
                {
                    Main_Form.strSwitch = "renren";
                    Main_Form.Login_Handle();
                }
                else
                {
                    Main_Form.strSwitch = "taobao";
                    Main_Form.TB_Login_Handle();
                }                
                if (Main_Form.ShowDialog() == DialogResult.Cancel)
                {
                    Application.Exit();
                }
            }
        }
    }
}