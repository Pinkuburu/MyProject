using System.Windows.Forms;
using System.Text.RegularExpressions;
using System;
using System.Text;
using System.Threading;
using System.IO;

namespace TaoBao_TryWebBrowser
{
    public partial class Form_Main : Form
    {
        WebClient HTTPproc = new WebClient();
        WebBrowser wbFindAnswer;
        public string strQuestion = "";

        public Form_Main()
        {
            InitializeComponent();
            webBrowser_Main.Navigate("https://login.taobao.com/member/login.jhtml?f=top&redirectURL=http%3A%2F%2Ftry.taobao.com%2F");
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            webBrowser_Main.Navigate("http://try.taobao.com");
        }

        private void button_OpenLink_Click(object sender, System.EventArgs e)
        {
            string strLink = textBox_Link.Text.ToString();

            if(strLink != "")
            {
                webBrowser_Main.Navigate(strLink);
            }            
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            FindAnswer(webBrowser_Main);
            //string strURL = webBrowser_Main.Url.ToString();
            //string strContent = WebBrowserEncode(webBrowser_Main, "utf-8");

            
            //if (strContent.IndexOf("活动未开始") < 0)
            //{
            //    //MessageBox.Show("活动已经开始");
            //    //开始找答案
            //    string straaaa = FindAnswer();
            //}
            //else
            //{
            //    MessageBox.Show("活动还没开始");
            //}        
        }

        #region aaaa
        //private string FindQuestion()
        //{
        //    string strContent = WebBrowserEncode(webBrowser_Main, "utf-8");

        //    try
        //    {
        //        strContent = Regex.Match(strContent, "(?<=属性描述.*<em>).*(?=</em>)").Value;
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        // Syntax error in the regular expression
        //    }

        //    return strContent;            
        //}

        //private string FindURL()
        //{
        //    string strContent = WebBrowserEncode(webBrowser_Main, "gb2312");

        //    try
        //    {
        //        strContent = Regex.Match(strContent, "(?<=是 ？<a href=\").*(?=\" target=\"_blank\")").Value;
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        strContent = "试用报告";
        //    }

        //    return strContent;
        //}

        //private string FindAnswer()
        //{
        //    string strURL = FindURL();
        //    string strQuestion = FindQuestion();
        //    //设置HTTP请求默认编码
        //    HTTPproc.Encoding = System.Text.Encoding.Default;
        //    string strContent = HTTPproc.OpenRead(strURL);
        //    //string strContent = webBrowser_Answer.Navigate(strURL);
        //    bool blWhile = true;
            
        //    while (blWhile)
        //    {
        //        if (HTTPproc.StatusCode == 302)
        //        {
        //            strContent = HTTPproc.OpenRead(HTTPproc.ResponseHeaders["Location"]);
        //        }
        //        else
        //        {
        //            blWhile = false;
        //        }
        //    }

        //    string resultString = null;
        //    try
        //    {
        //        resultString = Regex.Match(strContent, "(?<=" + strQuestion + ":&nbsp;.*).*(?=</li>)").Value;
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        resultString = "试用报告";
        //    }
        //    resultString = System.Web.HttpUtility.HtmlDecode(resultString);
        //    webBrowser_Main.Document.GetElementById("J_AnswerInput").InnerText = resultString;
        //    webBrowser_Main.Document.GetElementById("J_Answer").Children[2].InvokeMember("click");

        //    return resultString;
        //}
        #endregion aaaa

        private void FindAnswer(WebBrowser wb)
        {
            string strURL = "";
            string strAnswer = "";
            string strContent = WebBrowserEncode(webBrowser_Main, "gb2312");

            try
            {
                strURL = Regex.Match(strContent, "(?<=是 ？<a href=\").*(?=\" target=\"_blank\")").Value;
            }
            catch (ArgumentException ex)
            {
                strURL = "";
            }

            try
            {
                this.strQuestion = Regex.Match(strContent, "(?<=属性描述.*<em>).*(?=</em>)").Value;
            }
            catch (ArgumentException ex)
            {
                this.strQuestion = "试用报告";
            }

            if (strContent.IndexOf("试用品申请成功后需提交") > 0)
            {
                this.strQuestion = "试用报告";
            }

            if (strURL != "" || this.strQuestion == "试用报告")
            {
                if (this.strQuestion != "试用报告")
                {
                    wbFindAnswer = new WebBrowser();
                    wbFindAnswer.Navigate(strURL);
                    wbFindAnswer.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(wbFindAnswer_DocumentCompleted);
                    
                }
                else
                {
                    this.strQuestion = System.Web.HttpUtility.HtmlDecode(this.strQuestion);
                    wb.Document.GetElementById("J_AnswerInput").InnerText = this.strQuestion;
                    wb.Document.GetElementById("J_Answer").Children[2].InvokeMember("click");
                }                
            }
            else
            {
                MessageBox.Show("strURL 没有找到");
            }
        }

        void wbFindAnswer_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string strAnswer = "";
            string strContent = WebBrowserEncode(wbFindAnswer, "gb2312");
            try
            {
                strAnswer = Regex.Match(strContent, "(?<=" + strQuestion + ":&nbsp;.*).*(?=</li>)").Value;
                if (strAnswer == "")
                {
                    strAnswer = Regex.Match(strContent, "(?<=" + strQuestion + ".*&nbsp;.*).*(?=</td>)").Value;
                }                
            }
            catch 
            {
                strAnswer = "";
            }

            if (strAnswer != "")
            {
                strAnswer = System.Web.HttpUtility.HtmlDecode(strAnswer);
                //读取ua值
                //var param1Value = webBrowser_Main.Document.InvokeScript("eval", new String[] { "ua" }).ToString();
                //MessageBox.Show(param1Value);
                webBrowser_Main.Document.GetElementById("J_AnswerInput").InnerText = strAnswer;
                webBrowser_Main.Document.GetElementById("J_Answer").Children[2].InvokeMember("click");
                webBrowser_Main.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser_Main_DocumentCompleted);
            }
            else
            {
                MessageBox.Show("strAnswer 没有找到");
            }
            listBox1.Items.Insert(0, "wbFindAnswer:" + wbFindAnswer.ReadyState);
            wbFindAnswer.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(wbFindAnswer_DocumentCompleted);
        }

        void webBrowser_Main_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            listBox1.Items.Insert(0, "webBrowser:" + webBrowser_Main.ReadyState);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FindButtonClick("a", "提交申请");
        }

        #region 查找指定元素并点击 FindButtonClick(string strTagName, string strInnerText)
        /// <summary>
        /// 查找指定元素并点击
        /// </summary>
        /// <param name="strTagName">元素名称</param>
        /// <param name="strInnerText">查找内容</param>
        private void FindButtonClick(string strTagName, string strInnerText)
        {
            string strContent = "";
            int intCount = webBrowser_Main.Document.GetElementsByTagName(strTagName).Count;
            for (int i = 0; i < intCount; i++)
            {
                string strText = webBrowser_Main.Document.GetElementsByTagName(strTagName)[i].InnerText;
                if (strText == strInnerText)
                {
                    webBrowser_Main.Document.GetElementsByTagName(strTagName)[i].InvokeMember("click");
                }
            }
        }
        #endregion

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