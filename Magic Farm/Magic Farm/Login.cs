using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Magic_Farm
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            this.AcceptButton = button_Login;
        }

        private void button_Login_Click(object sender, EventArgs e)
        {
            if (textBox_UserName.Text.Trim() == "" || textBox_Password.Text.Trim() == "")
            {
                MessageBox.Show("�û�����������д����", "ϵͳ��ʾ");
            }
            else
            {
                this.Hide();    //���ص�¼����
                Main Main_Form = new Main();//��������������
                Main_Form.strUserNames = textBox_UserName.Text.Trim();
                Main_Form.strPasswords = textBox_Password.Text.Trim();

                Main_Form.TB_Login_Handle();
                if (Main_Form.ShowDialog() == DialogResult.Cancel)
                {
                    Application.Exit();
                    //this.Close();
                }
            }
        }
    }
}