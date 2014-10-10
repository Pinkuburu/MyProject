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
    public partial class Form1 : Form
    {
        public WebClient HTTPproc = new WebClient();

        public Form1()
        {
            InitializeComponent();
            ImageCode();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ImageCode();
        }

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
                pictureBox1.Image = Image.FromStream(HTTPproc.GetStream("http://guahao.qingdaonews.com/ghUser/captcha/v/" + strContent + ".html", ""));
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
        }
        #endregion 获取验证码图片 ImageCode()

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
    }
}
