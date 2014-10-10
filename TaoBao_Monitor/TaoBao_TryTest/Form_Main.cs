using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace TaoBao_TryTest
{
    public partial class Form_Main : Form
    {
        public string strToken = null;
        public string strID = null;
        public bool blLoop = true;
        public int intCount = 0;

        public Form_Main()
        {
            InitializeComponent();
            this.strID = "3910716";
            webBrowser_Main.Navigate("http://login.taobao.com/member/login.jhtml?from=buy&style=mini&redirect_url=http%3A%2F%2Fbuy.taobao.com%2Fauction%2Fbuy_now.jhtml");
            webBrowser_Main.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser_Main_DocumentCompleted);
        }

        void webBrowser_Main_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //System.IO.StreamReader getReader = new System.IO.StreamReader(this.webBrowser_Main.DocumentStream, System.Text.Encoding.Default);
            //string gethtml = getReader.ReadToEnd();
            //http://i.taobao.com/my_taobao.htm
            if (e.Url.ToString().IndexOf("error_key=100") > 0)
            {
                this.intCount++;
                string aaa = webBrowser_Main.Document.Cookie;
                WebBrowser_Debug(aaa);
                if (this.intCount == 1)
                {

                }
            }
        }

        #region Web调试信息 WebBrowser_Debug(string strContent)
        /// <summary>
        /// Web调试信息
        /// </summary>
        /// <param name="strContent">显示内容</param>
        private void WebBrowser_Debug(string strContent)
        {
            textBox_Debug.Text += DateTime.Now + " | DocumentCompleted | " + strContent + "\r\n";
            textBox_Debug.SelectionStart = textBox_Debug.Text.Length;
            textBox_Debug.ScrollToCaret();
            if (textBox_Debug.Text.Length > 3000)
            {
                textBox_Debug.Text = "";
            }
        }
        #endregion Web调试信息 WebBrowser_Debug(string strContent)

        #region 淘宝登录 TB_Login(string strUserName, string strPassword)
        /// <summary>
        /// 淘宝登录
        /// </summary>
        /// <param name="strUserName">帐号</param>
        /// <param name="strPassword">密码</param>
        private void TB_Login(string strUserName, string strPassword)
        {
            byte[] PostBytes = null;
            //http://buy.taobao.com/auction/buy_now.jhtml
            string cPostData = "TPL_username=" + UrlEncode(strUserName) + "&TPL_password=" + UrlEncode(strPassword) + "&action=Authenticator&event_submit_do_login=anything&TPL_redirect_url=http://try.taobao.com/item/add_try_request.htm?item_id=" + this.strID + "&from=tb&fc=2&style=default&css_style=&tid=XOR_1_000000000000000000000000000000_63584451347B0E700A71020D&support=000001&CtrlVersion=1%2C0%2C0%2C7&loginType=3&minititle=&minipara=&pstrong=2&longLogin=-1&llnick=&sign=&need_sign=&isIgnore=&popid=&callback=&guf=&not_duplite_str=&need_user_id=&poy=&gvfdcname=10&from_encoding=";
            PostBytes = Encoding.UTF8.GetBytes(cPostData);
            webBrowser_Main.Navigate("http://login.taobao.com/member/login.jhtml", "_self", PostBytes, "Content-Type:application/x-www-form-urlencoded\r\n");
            webBrowser_Main.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser_Main_DocumentCompleted);
        }
        #endregion 淘宝登录 TB_Login(string strUserName, string strPassword)

        #region Url编码 UrlEncode(string url)
        /// <summary>
        /// Url编码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string UrlEncode(string url)
        {
            byte[] bs = Encoding.GetEncoding("GB2312").GetBytes(url);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bs.Length; i++)
            {
                if (bs[i] < 128)
                {
                    sb.Append((char)bs[i]);
                }
                else
                {
                    sb.Append("%" + bs[i++].ToString("x").PadLeft(2, '0'));
                    sb.Append("%" + bs[i].ToString("x").PadLeft(2, '0'));
                }
            } return sb.ToString();
        }
        #endregion
        
    }
}
