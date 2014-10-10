using System.Windows.Forms;

namespace 模拟点击
{
    public partial class Form1 : Form
    {
        WebBrowserClass WBC = new WebBrowserClass();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            webBrowser1.Navigate("http://login.taobao.com/member/login.jhtml?f=top&redirectURL=http%3A%2F%2Fqz.jianghu.taobao.com%2Fhome%2Faward_exchange_home.htm%3Ftracelog%3Dqzindex001");
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            HtmlElement ckBox = webBrowser1.Document.GetElementById("J_SafeLoginCheck");
            if (ckBox.GetAttribute("checked") == "True")
            {
                WBC.WebExid(webBrowser1, "input", "J_SafeLoginCheck", "click");
            }
            WBC.WebWsername(webBrowser1, "input", "TPL_username", "diudiuqwe");
            WBC.WebWsername(webBrowser1, "input", "TPL_password", "qweqwe123");
            HtmlElement formLogin = webBrowser1.Document.Forms["J_StaticForm"];
            formLogin.InvokeMember("submit");
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            WBC.WebExid(webBrowser1, "a", "J_CoinGrantBtn", "click");
        }

        private void button4_Click(object sender, System.EventArgs e)
        {
            
        }
    }
}
