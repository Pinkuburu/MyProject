using System;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace BuyTicket
{
    public partial class LoginCode_Form : Form
    {
        //定义HTTP请求方法
        WebClient httpImgCode = new WebClient();
        //验证码
        public string strCode = "";
        //Cookie
        public string strCookie = "";

        public LoginCode_Form()
        {
            InitializeComponent();
            this.AcceptButton = button_Login;
        }

        private void button_Login_Click(object sender, EventArgs e)
        {
            bool blCode = false;
            this.strCode = textBox_Code.Text.Trim().ToString();
            blCode = CheckCode(this.strCode);
            if (blCode)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.textBox_Code.Text = "";
                ImageCode();
            }
        }

        private void LoginCode_Form_Load(object sender, EventArgs e)
        {
            ImageCode();
        }

        #region 获取验证码图片 ImageCode()
        /// <summary>
        /// 获取验证码图片
        /// </summary>
        private void ImageCode()
        {
            string strRequest = "http://kyfw.12306.cn/otn/passcodeNew/getPassCodeNew?module=login&rand=sjrand";
            //向指定网址请求返回数据流
            this.pictureBox_Code.Image = Image.FromStream(httpImgCode.GetStream(strRequest, ""));
            //将得到的Cookie存储到全局变量strImageCookie
            string strImageCookie = httpImgCode.Cookie.ToString();
            this.strCookie = strImageCookie;
            //try
            //{
            //    this.strCookie = Regex.Match(strImageCookie, @"JSESSIONID=\w{16,32}").Value;
            //}
            //catch (ArgumentException ex)
            //{
            //    // Syntax error in the regular expression
            //}
        }
        #endregion 获取验证码图片 ImageCode()

        private void pictureBox_Code_Click(object sender, EventArgs e)
        {
            ImageCode();
        }

        private bool CheckCode(string strCode)
        {
            string T_Code = "";

            if (strCode != "")
            {
                string strRequest = "http://kyfw.12306.cn/otn/passcodeNew/checkRandCodeAnsyn";
                string strParameter = "randCode=" + this.strCode + "&rand=sjrand";
                string strContent = httpImgCode.OpenRead(strRequest, strParameter);
                JObject jo = JObject.Parse(strContent);
                T_Code = jo["data"].ToString();
                if (T_Code == "Y")
                {
                    return true;
                }
            }
            return false;
        }

        private void textBox_Code_TextChanged(object sender, EventArgs e)
        {
            int intLength = textBox_Code.Text.Trim().Length;

            if (intLength == 4)
            {
                bool blCode = false;
                this.strCode = textBox_Code.Text.Trim().ToString();
                blCode = CheckCode(this.strCode);
                if (blCode)
                {
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    this.textBox_Code.Text = "";
                    ImageCode();
                }
            }            
        }
    }
}
