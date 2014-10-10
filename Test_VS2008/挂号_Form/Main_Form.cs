using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace 挂号_Form
{
    public partial class Main_Form : Form
    {
        public WebClient HTTPproc = new WebClient();

        public Main_Form()
        {
            InitializeComponent();
            ImageCode();
        }

        private void button_Login_Click(object sender, EventArgs e)
        {
            pictureBox_Code.Image = null;
        }

        #region 用户登录 Login(string strUsername, string strPassword, string strCode)
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="strUsername"></param>
        /// <param name="strPassword"></param>
        /// <param name="strCode"></param>
        /// <returns></returns>
        private bool Login(string strUsername, string strPassword, string strCode)
        {
            string strRequest = "http://guahao.qingdaonews.com/GhUser/login.html";
            string strParameter = "GhUser%5Buser_pass%5D=" + strUsername + "&GhUser%5Buser_password%5D=" + strPassword + "&code=" + strCode;
            string strContent = this.HTTPproc.OpenRead(strRequest, strParameter);
            if (strCode.IndexOf("window.history.go(-1)") > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 动态时间戳 TimeSpans()
        /// <summary>
        /// 动态时间戳
        /// </summary>
        /// <returns></returns>
        private long TimeSpans()
        {
            long epoch = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
            return epoch;
        }
        #endregion

        #region URLEncode UrlEncode(string str, string encode)
        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        private string UrlEncode(string str, string encode)
        {
            int factor = 0;
            if (encode == "UTF-8")
                factor = 3;
            if (encode == "GB2312")
                factor = 2;

            //不需要编码的字符
            string okChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.-";
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

        #region 获取验证码图片 ImageCode()
        /// <summary>
        /// 获取验证码图片
        /// </summary>
        private void ImageCode()
        {            
            long lTimeSpan = TimeSpans();
            string strRequest = "http://guahao.qingdaonews.com/ghUser/captcha/refresh/1.html?_=" + lTimeSpan;
            HTTPproc.RequestHeaders.Add("Referer:http://guahao.qingdaonews.com/GhUser/login.html");
            HTTPproc.RequestHeaders.Add("Accept:application/json, text/javascript, */*; q=0.01");
            string strContent = HTTPproc.OpenRead(strRequest);
            try
            {
                strContent = Regex.Match(strContent, @"\w{13,}").Value;
                //向指定网址请求返回数据流
                HTTPproc.RequestHeaders.Add("Referer:http://guahao.qingdaonews.com/GhUser/login.html");
                HTTPproc.RequestHeaders.Add("Accept:image/png,image/*;q=0.8,*/*;q=0.5");
                pictureBox_Code.Image = Image.FromStream(HTTPproc.DownloadData("http://guahao.qingdaonews.com/ghUser/captcha/v/" + strContent + ".html"));
                //HTTPproc.DownloadFile("http://guahao.qingdaonews.com/ghUser/captcha/v/" + strContent + ".html", @"1.png");
                //pictureBox_Code.Image = Image.FromFile(@"1.png");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
        }
        #endregion 获取验证码图片 ImageCode()

        private void pictureBox_Code_Click(object sender, EventArgs e)
        {
            ImageCode();            
        }
    }
}
