using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;

namespace DZ抢楼机X3
{
    public partial class Main_Form : Form
    {
        WebBrowserClass WBC = new WebBrowserClass();

        public Main_Form()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://ds.eefocus.com/module/forum/thread-1141-112-1.html");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string strHTML = webBrowser1.DocumentText;
            string strContent = null;
            try
            {
                strContent = Regex.Match(strHTML, @"共 \d{0,} 页").Value;
                int intCountPage = Convert.ToInt32(strContent.Replace("共 ", "").Replace(" 页", ""));
                webBrowser1.Navigate("http://ds.eefocus.com/module/forum/thread-1141-" + intCountPage + "-1.html");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            HtmlDocument document = webBrowser1.Document;
            document.Window.ScrollTo(0, 5000);
            HtmlElement ckBox = webBrowser1.Document.GetElementById("fastpostrefresh");
            if (ckBox.GetAttribute("checked") == "False")
            {
                WBC.WebExid(webBrowser1, "input", "fastpostrefresh", "click");
            }
            WBC.WebWsername(webBrowser1, "textarea", "message", "支持活动！开始抢楼！");
            WBC.WebExid(webBrowser1, "button", "fastpostsubmit", "click");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool blCheckBox = checkBox1.Checked;
            while (blCheckBox)
            {

                Thread.Sleep(5000);
            }
        }
    }
}
