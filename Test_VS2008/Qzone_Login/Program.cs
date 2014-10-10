using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Globalization;

namespace Qzone_Login
{
    class Program
    {
        static WebClient HTTPproc = new WebClient();
        static void Main(string[] args)
        {
            HTTPproc.Encoding = Encoding.UTF8;

            string strRequest = "http://ui.ptlogin2.qq.com/cgi-bin/login?daid=5&pt_qzone_sig=1&hide_title_bar=1&low_login=0&qlogin_auto_login=1&no_verifyimg=1&link_target=blank&appid=549000912&style=12&target=self&s_url=http%3A//qzs.qq.com/qzone/v5/loginsucc.html?para=izone&pt_qr_app=%CA%D6%BB%FAQQ%BF%D5%BC%E4&pt_qr_link=http%3A//z.qzone.com/download.html&self_regurl=http%3A//qzs.qq.com/qzone/v6/reg/index.html&pt_qr_help_link=http%3A//z.qzone.com/download.html";
            string strContent = "";

            strContent = HTTPproc.OpenRead(strRequest);

            string strLOGIN_SIG = null;
            string strVERIFYCODE = null;
            string strUIN = null;
            try
            {
                strLOGIN_SIG = Regex.Match(strContent, "(?<=login_sig:\".*).*(?=\",client)").Value;
            }
            catch (ArgumentException ex)
            {
                strLOGIN_SIG = null;
            }

            if (strLOGIN_SIG != null)
            {
                strRequest = "http://check.ptlogin2.qq.com/check?regmaster=&uin=182536608&appid=549000912&js_ver=10060&js_type=1&login_sig=" + strLOGIN_SIG + "&u1=http%3A%2F%2Fqzs.qq.com%2Fqzone%2Fv5%2Floginsucc.html%3Fpara%3Dizone&r=0.37843011078722644";
                strContent = HTTPproc.OpenRead(strRequest);
                string[] array = strContent.Replace("ptui_checkVC(", "").Replace("'", "").Replace(")","").Split(",".ToCharArray());
                strVERIFYCODE = array[1];
                strUIN = array[2];

                if (strVERIFYCODE != null)
                {
                    strRequest = "http://ptlogin2.qq.com/login?u=182536608&p=" + Md5("loveemma++", strUIN, strVERIFYCODE) + "&verifycode=" + strVERIFYCODE + "&aid=549000912&u1=http%3A%2F%2Fqzs.qq.com%2Fqzone%2Fv5%2Floginsucc.html%3Fpara%3Dizone&h=1&ptredirect=0&ptlang=2052&daid=5&from_ui=1&dumy=&low_login_enable=0&regmaster=&fp=loginerroralert&action=4-19-1386739317733&mibao_css=&t=1&g=1&js_ver=10060&js_type=1&login_sig=" + strLOGIN_SIG + "&pt_rsa=0&pt_qzone_sig=1";
                    strContent = HTTPproc.OpenRead(strRequest);
                }

            }



            Console.WriteLine(strContent);
            Console.WriteLine(strVERIFYCODE);


            Console.ReadKey();
        }

        /// <summary>
        /// 连接两个字节数组
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        private static byte[] JoinBytes(byte[] b1, byte[] b2)
        {
            var b3 = new byte[b1.Length + b2.Length];
            Array.Copy(b1, b3, b1.Length);
            Array.Copy(b2, 0, b3, 16, b2.Length);
            return b3;
        }

        /// <summary>
        /// 将字符串加密为字节数组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static byte[] Md5ToArray(string input)
        {
            return MD5.Create().ComputeHash(Encoding.Default.GetBytes(input));
        }

        /// <summary>
        /// 加密字符串，并转换十六进制表示的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string Md5(string input)
        {
            var buffer = MD5.Create().ComputeHash(Encoding.Default.GetBytes(input));
            var builder = new StringBuilder();
            for (var i = 0; i < buffer.Length; i++)
            {
                builder.Append(buffer[i].ToString("X2"));
            }
            return builder.ToString();
        }

        /// <summary>
        /// 对一个字节数组加密，并转换十六进制表示的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string Md5(byte[] input)
        {
            var buffer = MD5.Create().ComputeHash(input);
            var builder = new StringBuilder();
            for (var i = 0; i < buffer.Length; i++)
            {
                builder.Append(buffer[i].ToString("X2"));
            }
            return builder.ToString();
        }

        /// <summary>
        /// 对密码进行加密
        /// </summary>
        /// <param name="password">QQ密码</param>
        /// <param name="vcode">第一次检验用户状态时获取的key</param>
        /// <param name="verifyCode">验证码</param>
        /// <returns></returns>
        internal static string Md5(string password, string uin, string yzm)
        {
            var b1 = Md5ToArray(password);
            byte[] uinBytes = ToBytes(uin);
            var s1 = Md5(JoinBytes(b1, uinBytes));
            return Md5(s1 + yzm);
        }
        /// <summary>
        /// 转换为字节数组表示
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static byte[] ToBytes(string str)
        {
            var bytes = new byte[8];
            for (var i = 0; i < 8; i++)
            {
                bytes[i] = byte.Parse(str.Substring((i * 4) + 2, 2), NumberStyles.HexNumber);
            }
            return bytes;
        }
    }
}
