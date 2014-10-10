using System;
using System.Windows.Forms;
using System.Threading;

namespace 兄弟助手
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strContent = null;

            SDGAME sd = new SDGAME();

            strContent = sd.Login("cupid0426@163.com", "qweqwe123");

            MessageBox.Show(strContent);
        }
    }
}
