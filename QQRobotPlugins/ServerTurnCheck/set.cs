using System;
using System.Windows.Forms;
using QQRobot.Skin;

namespace ServerTurnCheck
{
    public partial class set : BasicForm
    {
        public set()
        {
            InitializeComponent();
            qQtextBox_BasketBall.Text = ServerTurnCheck.sData.bb_Data.Trim();
            qQtextBox_FootBall.Text = ServerTurnCheck.sData.fb_Data.Trim();
        }

        private void Button_Setup_Click(object sender, EventArgs e)
        {
            ServerTurnCheck.sData.bb_Data = qQtextBox_BasketBall.Text.Trim();
            ServerTurnCheck.sData.fb_Data = qQtextBox_FootBall.Text.Trim();

            this.DialogResult = DialogResult.OK;
        }
    }
}
