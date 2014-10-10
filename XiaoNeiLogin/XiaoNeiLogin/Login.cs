using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace XiaoNeiLogin
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            label_Login_NickName.Visible = false;
            label_Login_UserID.Visible = false;
            pictureBox_Login.Visible = false;
            MaximizeBox = false;
            MinimizeBox = false;
            this.AcceptButton = btn_Login;
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            if (textBox_Login_UserName.Text.Trim() != "" && textBox_Login_Password.Text.Trim() != "")
            {
                textBox_Login_Password.Visible = false;
                textBox_Login_UserName.Visible = false;
                label1.Visible = false;
                label2.Visible = false;
                label_Login_NickName.Visible = true;
                label_Login_UserID.Visible = true;
                pictureBox_Login.Visible = true;
                label_Login_UserID.Text = "校内ID：";
                label_Login_NickName.Text = "昵称：";

                XiaoNeiLogin.WebClient HTTPproc = new WebClient();
                //设置HTTP请求默认编码
                HTTPproc.Encoding = System.Text.Encoding.UTF8;
                string strRedirectURL = HTTPproc.OpenRead("http://www.renren.com/PLogin.do", "email=" + UrlEncode(textBox_Login_UserName.Text.Trim(), "UTF-8") + "&password=" + UrlEncode(textBox_Login_Password.Text.Trim(), "UTF-8") + "&origURL=http%3A%2F%2Fwww.renren.com%2FSysHome.do&domain=renren.com");
                try
                {
                    HTTPproc.OpenRead(strRedirectURL.Replace("The URL has moved <a href=\"", "").Replace("\">here</a>", ""));
                    ReadUserInfo(HTTPproc.OpenRead("http://www.renren.com/Home.do"));
                }
                catch
                {
                    MessageBox.Show("用户名或密码出错","系统消息");
                }
                
            }
            else
            {
                MessageBox.Show("信息填写有误请检查！","系统消息");
            }
        }

        #region 读取用户信息 ReadUserInfo(string strContent)
        /// <summary>
        /// 读取用户信息
        /// </summary>
        /// <param name="strContent"></param>
        private void ReadUserInfo(string strContent)
        {
            string resultString = null;
            try
            {
                resultString = Regex.Match(strContent, "XN.user.tinyPic = '.*'").Value;
                pictureBox_Login.ImageLocation = resultString.Replace("XN.user.tinyPic = '","").Replace("'","");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }

            try
            {
                resultString = Regex.Match(strContent, "XN.user.id = '.*'").Value;
                label_Login_UserID.Text += resultString.Replace("XN.user.id = '", "").Replace("'", "");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }

            try
            {
                resultString = Regex.Match(strContent, "<title>.*</title>").Value;
                label_Login_NickName.Text += resultString.Replace("<title>人人网 校内 - ", "").Replace("</title>", "");
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
        }
        #endregion 读取用户信息 ReadUserInfo(string strContent)

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

            string okChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.";
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
        #endregion URL编码 UrlEncode(string str, string encode)
    }
}