using QQRobot.Skin;
using System.Windows.Forms;

namespace SendMailPlugins
{
    public partial class set : BasicForm
    {
        public set()
        {
            InitializeComponent();
            alTextBox_Mail.Text = SendMailClass.sData.mail.Trim();
            alTextBox_Pwd.Text = SendMailClass.sData.pwd.Trim();
        }

        private void basicButton_Setup_Click(object sender, System.EventArgs e)
        {
            SendMailClass.sData.mail = alTextBox_Mail.Text.Trim();
            SendMailClass.sData.pwd = alTextBox_Pwd.Text.Trim();
            this.DialogResult = DialogResult.OK;
        }
    }
}
