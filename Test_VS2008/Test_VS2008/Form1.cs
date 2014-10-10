using System.Windows.Forms;
using System;

namespace Test_VS2008
{
    public partial class Form1 : Form
    {
        int i = 0;
        public Form1()
        {
            InitializeComponent();
            this.timer1.Enabled = false;
            this.timer1.Interval = 1000;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            webBrowser1.Navigate("http://welnuo.taobao.com/");
            this.timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            label1.Text = i.ToString();
            if (i%180 == 0)
            {
                start();
            }
            i++;
        }

        private void start()
        {
            int j = 0;
            Random rnd = new Random();
            j = rnd.Next(1, 6);
            switch(j)
            {
                case 1:
                    webBrowser1.Navigate("http://welnuo.taobao.com/");
                    break;
                case 2:
                    webBrowser1.Navigate("http://item.taobao.com/item.htm?id=8560784838");
                    break;
                case 3:
                    webBrowser1.Navigate("http://item.taobao.com/item.htm?id=8503245018");
                    break;
                case 4:
                    webBrowser1.Navigate("http://item.taobao.com/item.htm?id=8484959430");
                    break;
                case 5:
                    webBrowser1.Navigate("http://item.taobao.com/item.htm?id=8560466338");
                    break;
                case 6:
                    webBrowser1.Navigate("http://item.taobao.com/item.htm?id=8568311494");
                    break;
            }            
        }        
    }
}
