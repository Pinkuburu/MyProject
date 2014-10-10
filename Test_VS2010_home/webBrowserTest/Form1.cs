using System.Windows.Forms;
using System.Text.RegularExpressions;
using System;
using System.Drawing;
using System.Collections;

namespace webBrowserTest
{
    public partial class Form1 : Form
    {
        public bool status = false;
        public Form1()
        {
            InitializeComponent();
            this.AcceptButton = button1;
            webBrowser1.Navigate("http://www.letao.com/letaozu/miaosha/");
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
        }

        void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser1.Document.Click += new HtmlElementEventHandler(Document_Click);
        }

        void Document_Click(object sender, HtmlElementEventArgs e)
        {
            if (webBrowser1.Document != null)
            {
                HtmlElement elem = webBrowser1.Document.GetElementFromPoint(e.ClientMousePosition);
                textBox_xy.Text = e.MousePosition.ToString();

                try
                {
                    textBox_pid.Text = Regex.Match(elem.InnerHtml, "pid=.*m").Value.Replace("pid=\"","").Replace("\" m","");
                    textBox_msid.Text = Regex.Match(elem.InnerHtml, "msid.*\"").Value.Replace("msid=\"","").Replace("\"","");
                }
                catch (ArgumentException ex)
                {
                    // Syntax error in the regular expression
                }
                textBox2.Text = webBrowser1.Document.Cookie;
                toolStripStatusLabel1.Text = Timestamp().ToString();
                //http://www.letao.com/images/v.aspx?1313818290185&mid=640
            }
        }        

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ArrayList arrSize = new ArrayList();
            WebClient HTTPproc = new WebClient();
            HTTPproc.Encoding = System.Text.Encoding.UTF8;
            HTTPproc.RequestHeaders.Add("Referer:http://www.letao.com/letaozu/miaosha/");
            HTTPproc.RequestHeaders.Add("Cookie:" + webBrowser1.Document.Cookie);
            
            //向指定网址请求返回数据流
            string strRequest = "http://www.letao.com/images/v.aspx?" + Timestamp().ToString() + "&mid=" + textBox_msid.Text;
            try
            {
                //加载中文验证码
                pictureBox1.Image = Image.FromStream(HTTPproc.GetStream(strRequest, ""));
                //加载和判定尺码
                strRequest = "http://www.letao.com/web_service/user_miaosha_ws.aspx?op=get_size_list";
                string strParameter = "data=[\"" + textBox_msid.Text + "\",\"" + textBox_pid.Text + "\"]";
                strParameter = UrlEncode(strParameter, "UTF-8");
                string strContent = HTTPproc.OpenRead(strRequest, strParameter);
                try
                {
                    Regex regexObj = new Regex(@">\d{1,}");
                    Match matchResult = regexObj.Match(strContent);
                    while (matchResult.Success)
                    {
                        arrSize.Add(matchResult.Value.Replace(">", ""));
                        matchResult = matchResult.NextMatch();
                    }
                    if (arrSize.IndexOf(textBox_size.Text) < 0)
                    {
                        textBox_size.Text = arrSize[0].ToString();
                    }
                }
                catch (ArgumentException ex)
                {
                    // Syntax error in the regular expression
                }
                toolStripStatusLabel1.Text = strContent; 
            }
            catch
            {
                DateTime dt = DateTime.Now;
                toolStripStatusLabel1.Text = "秒杀还没有开始图片不能加载 " + dt.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strRequest = null;
            string strParameter = null;
            string strContent = null;

            WebClient HTTPproc = new WebClient();
            if (this.status == false)
            {
                HTTPproc.RequestHeaders.Add("Referer:http://www.letao.com/letaozu/miaosha/");
            }
            else
            {
                HTTPproc.RequestHeaders.Add("Referer:http://www.letao.com/letaozu/pay/create_order.aspx?from=2&tid=" + textBox_msid.Text);
            }
            HTTPproc.RequestHeaders.Add("Cookie:" + webBrowser1.Document.Cookie);
            HTTPproc.Encoding = System.Text.Encoding.UTF8;

            if (this.status == false)
            {
                strRequest = "http://www.letao.com/web_service/user_miaosha_ws.aspx?op=add_miaosha_cart";
                strParameter = "data=[\"" + textBox_msid.Text + "\",\"" + textBox_pid.Text + "\",\"" + textBox_size.Text + "0\",\"" + textBox_code.Text + "\"]";
                strParameter = UrlEncode(strParameter, "UTF-8");
                strContent = HTTPproc.OpenRead(strRequest, strParameter);
                toolStripStatusLabel1.Text = strContent;

                //VALIDCODE_ERROR,ALERT,SUCCESS
                if (strContent.IndexOf("SUCCESS") > 0)
                {                    
                    //调用默认英文输入法
                    InputLanguage.CurrentInputLanguage = InputLanguage.InstalledInputLanguages[0];

                    this.status = true;
                    textBox_code.Text = "";
                    //请示英文验证码
                    strRequest = "http://www.letao.com/images/vorder.aspx?" + Timestamp().ToString() + "&mid=" + textBox_msid.Text;
                    try
                    {
                        HTTPproc = new WebClient();
                        HTTPproc.RequestHeaders.Add("Referer:http://www.letao.com/letaozu/pay/create_order.aspx?from=2&tid=" + textBox_msid.Text);
                        HTTPproc.RequestHeaders.Add("Cookie:" + webBrowser1.Document.Cookie);
                        pictureBox1.Image = Image.FromStream(HTTPproc.GetStream(strRequest, ""));
                    }
                    catch
                    {
                        DateTime dt = DateTime.Now;
                        toolStripStatusLabel1.Text = "秒杀还没有开始图片不能加载 " + dt.ToString();
                    }
                    //预览提交页debug用
                    webBrowser1.Navigate("http://www.letao.com/letaozu/pay/create_order.aspx?from=2&tid=" + textBox_msid.Text);
                }
            }
            else
            {
                strRequest = "http://www.letao.com/web_service/user_order_new_ws.aspx?op=create_customer_order";
                strParameter = "data=[{\"DataType\":\"System.DataTable\",\"TableName\":\"table1\",\"Columns\":[],\"Rows\":[]},{\"DataType\":\"System.DataTable\",\"TableName\":\"table1\",\"Columns\":[{\"ColumnName\":\"ADDCODE\",\"DataType\":\"System.String\"},{\"ColumnName\":\"TRANSFERID\",\"DataType\":\"System.String\"},{\"ColumnName\":\"COUPANCODE\",\"DataType\":\"System.String\"},{\"ColumnName\":\"PAYMENTTYPEID\",\"DataType\":\"System.String\"},{\"ColumnName\":\"NOSTOCKNOTIFY\",\"DataType\":\"System.String\"},{\"ColumnName\":\"DELIVERY\",\"DataType\":\"System.String\"}],\"Rows\":[{\"ADDCODE\":\"1704815\",\"TRANSFERID\":\"02\",\"COUPANCODE\":\"\",\"PAYMENTTYPEID\":\"20\",\"NOSTOCKNOTIFY\":\"0\",\"DELIVERY\":\"2\"}]},2," + textBox_msid.Text + ",\"" + textBox_code.Text + "\"]";
                strParameter = UrlEncode(strParameter, "UTF-8");
                strContent = HTTPproc.OpenRead(strRequest, strParameter);
                toolStripStatusLabel1.Text = strContent;
            }
        }

        #region URL编码 UrlEncode(string str, string encode)
        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        private static string UrlEncode(string str, string encode)
        {
            int factor = 0;
            if (encode == "UTF-8")
                factor = 3;
            if (encode == "GB2312")
                factor = 2;
            //不需要编码的字符

            string okChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.=";
            System.Text.Encoder encoder = System.Text.Encoding.GetEncoding(encode).GetEncoder();
            char[] c1 = str.ToCharArray();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //一个字符一个字符的编码

            for (int i = 0; i < c1.Length; i++)
            {
                //不需要编码

                if (okChar.IndexOf(c1[i]) > -1)
                    sb.Append(c1[i]);
                else
                {
                    byte[] c2 = new byte[factor];
                    int charUsed, byteUsed; bool completed;

                    encoder.Convert(c1, i, 1, c2, 0, factor, true, out charUsed, out byteUsed, out completed);

                    foreach (byte b in c2)
                    {
                        if (b != 0)
                            sb.AppendFormat("%{0:X}", b);
                    }
                }
            }
            return sb.ToString().Trim();
        }
        #endregion

        #region 时间戳 Timestamp()
        /// <summary>
        /// 时间戳
        /// </summary>
        /// <returns></returns>
        private long Timestamp()
        {
            long longTimestamp = 0;
            longTimestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            return longTimestamp;
        }
        #endregion 时间戳 Timestamp()        

        private void textBox_code_Click(object sender, EventArgs e)
        {
            string[] languagename = new string[] { "拼音", "五笔" };
            for (int i = 0; i < languagename.Length; i++)
            {
                foreach (InputLanguage lang in InputLanguage.InstalledInputLanguages)
                {
                    if (lang.LayoutName.IndexOf(languagename[i]) >= 0)
                    {
                        InputLanguage.CurrentInputLanguage = lang;
                        break;
                    }
                }
            }
        }

        private void textBox_code_TextChanged(object sender, EventArgs e)
        {
            if (textBox_code.Text.Length == 4)
            {
                System.Windows.Forms.SendKeys.Send("{ENTER}");
            }
        }
    }
}
