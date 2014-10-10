using System.Windows.Forms;
using zoyobar.shared.panzer.web;
using zoyobar.shared.panzer.web.ib;
using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using System.Threading;

namespace TaoBao_TryHelper
{
    public partial class Form_Main : Form
    {
        Thread thTest;
        public Form_Main()
        {
            InitializeComponent();
        }
        
        private void Form_Main_Load(object sender, EventArgs e)
        {
            IEBrowser ie = new IEBrowser(webBrowser_Main);
            ie.Navigate("https://login.taobao.com/member/login.jhtml?f=top&redirectURL=http%3A%2F%2Ftry.taobao.com%2F");
            ie = new IEBrowser(webBrowser_Main);
            ie.IsNewWindowEnabled = false;
        }

        private void btn_OpenLink_Click(object sender, System.EventArgs e)
        {
            IEBrowser ie = new IEBrowser(this.webBrowser_Main);
            string strURL = textBox_Url.Text.Trim().ToString();
            if (strURL != "")
            {
                ie.Navigate(strURL);
            }
            else
            {
                MessageBox.Show("没有要打开的链接");
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {            
            string strAnswer = FindAnswer(webBrowser_Main);
            if (strAnswer != "")
            {
                //J_AnswerInput
                //try-detail-buy
                IEBrowser ie = new IEBrowser(webBrowser_Main);

                ie.InstallJQuery(Properties.Resources.jquery_1_6_4_min);
                if (ie.IsJQueryInstalled)
                {
                    ie.ExecuteJQuery(JQuery.Create("'#J_AnswerInput'").Val("'" + strAnswer + "'"));
                    //点击免费申请按钮
                    ie.ExecuteJQuery(JQuery.Create("'a.try-detail-buy'").Attr("'id'", "'trybuy'"));
                    ie.ExecuteScript("document.getElementById('trybuy').click();");

                    //thTest = new Thread(new ThreadStart(aaa));
                    //thTest.Start();
                }
            }
        }

        private void aaa()
        {
            MessageBox.Show("aaaa");
            //int intb = 0;
            //int inta = webBrowser_Main.Document.All.Count;
            //bool bl =true;
            //while(bl)
            //{
            //    intb = webBrowser_Main.Document.All.Count;
            //    if (intb > inta)
            //    {
            //        MessageBox.Show(intb.ToString());
            //        break;
            //    }
            //}
            
            //IEBrowser ie = new IEBrowser(webBrowser_Main);

            //bool bl = true;
            //string strContent = "";
            //int intb = 0;
            //int inta = ie.Document.All.Count;
            //while (bl)
            //{
            //    intb = ie.Document.All.Count;
            //    //if (strContent.IndexOf("提交申请") > 0)
            //    if (intb > inta)
            //    {
            //        //try-btn try-btn-submit
            //        ie.ExecuteJQuery(JQuery.Create("'a:contains(提交申请)'").Attr("'id'", "'trybuysubmit'"));
            //        ie.ExecuteScript("document.getElementById('trybuysubmit').click();");
            //        bl = false;
            //    }
            //}
            //thTest.Abort();

            //IEBrowser ie;
            //while (bl)
            //{
            //    ie = new IEBrowser(webBrowser_Main);
            //    //ie.InstallScript(
            //    //"function clickLink(text) {" +
            //    //"       links = document.getElementsByTagName('a');" +
            //    //"       for(var index = 0; index < links.length; index++)" +
            //    //"       {" +
            //    //"               if(links[index].innerText == text)" +
            //    //"               {" +
            //    //"                       links[index].click();" +
            //    //"                       break;" +
            //    //"               }" +
            //    //"       }" +
            //    //"}"
            //    //);

            //    if (ie.ExecuteJQuery<int>(JQuery.Create("'div.try-stdmod-footer'", false).Length()) == 1)
            //    {
            //        // 调用 javascript 函数 clickLink, 模拟点击退出链接.
            //        //ie.InvokeScript("clickLink", new object[] { "提交申请" });

            //        ie.ExecuteJQuery(JQuery.Create("'a:contains(提交申请)'").Attr("'id'", "'trybuysubmit'"));
            //        ie.ExecuteScript("document.getElementById('trybuysubmit').click();");

            //        ie.Dispose();
            //        bl = false;
            //        thTest.Abort();
            //    }
            //    Thread.Sleep(100);
            //}            
        }



        private string FindAnswer(WebBrowser wb)
        {
            WebClient HTTPproc = new WebClient();
            string strURL = "";
            string strAnswer = "";
            string strQuestion = "";
            string strContent = WebBrowserEncode(wb, "gb2312");

            //查找问题链接
            try
            {
                strURL = Regex.Match(strContent, "(?<=是 ？<a href=\").*(?=\" target=\"_blank\")").Value;
            }
            catch (ArgumentException ex)
            {
                strURL = "";
            }

            //查找问题内容
            try
            {
                strQuestion = Regex.Match(strContent, "(?<=属性描述.*<em>).*(?=</em>)").Value;
            }
            catch (ArgumentException ex)
            {
                strQuestion = "";
            }

            if (strURL != "")
            {
                //设置HTTP请求默认编码
                HTTPproc.Encoding = System.Text.Encoding.Default;
                strContent = HTTPproc.OpenRead(strURL);
                bool blWhile = true;

                while (blWhile)
                {
                    if (HTTPproc.StatusCode == 302)
                    {
                        strContent = HTTPproc.OpenRead(HTTPproc.ResponseHeaders["Location"]);
                    }
                    else
                    {
                        blWhile = false;
                    }
                }

                try
                {
                    strAnswer = Regex.Match(strContent, "(?<=" + strQuestion + ":&nbsp;.*).*(?=</li>)").Value;
                    if (strAnswer == "")
                    {
                        strAnswer = Regex.Match(strContent, "(?<=" + strQuestion + ".*&nbsp;.*).*(?=</td>)").Value;
                    }
                    strAnswer = System.Web.HttpUtility.HtmlDecode(strAnswer);
                }
                catch
                {
                    strAnswer = "";
                }
            }
            else
            {
                if (strContent.IndexOf("试用品申请成功后需提交") > 0)
                {
                    strAnswer = "试用报告";
                }
                else
                {
                    MessageBox.Show("可能还没开始");
                }
            }
            return strAnswer;
        }

        #region HTML编码 WebBrowserEncode(WebBrowser wb,string strEncode)
        /// <summary>
        /// HTML编码
        /// </summary>
        /// <param name="wb">WebBrowser</param>
        /// <param name="strEncode">网页编码类型</param>
        /// <returns>编码后的HTML内容</returns>
        private string WebBrowserEncode(WebBrowser wb, string strEncode)
        {
            StreamReader getReader = new StreamReader(wb.DocumentStream, Encoding.GetEncoding(strEncode));
            return getReader.ReadToEnd();
        }
        #endregion
    }
}
