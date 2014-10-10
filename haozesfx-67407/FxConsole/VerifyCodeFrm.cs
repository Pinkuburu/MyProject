using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace FxConsole
{
    public partial class VerifyCodeFrm : Form
    {
        public VerifyCodeFrm()
        {
            InitializeComponent();
        }

        public VerifyCodeFrm(string reason, byte[] picCode)
        {
            InitializeComponent();
            this.lblTip.Text = reason;

            using (MemoryStream stream = new MemoryStream(picCode, true))
            {
                Bitmap bmp = new Bitmap(stream);
                this.pictureBox1.Image = bmp;
            }
        }

        public string VerfyCode
        {
            get;
            set;
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            this.VerfyCode = this.txtPicNum.Text.Trim();
            this.DialogResult = DialogResult.OK;
        }
    }
}
