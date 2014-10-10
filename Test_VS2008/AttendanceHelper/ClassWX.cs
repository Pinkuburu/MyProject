using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace AttendanceHelper
{
    public class ClassWX
    {
        WebClient HTTPproc = new WebClient();
        public static string strToken;

        #region 微信公众平台登陆 Login(string strUserName, string strPassword)
        /// <summary>
        /// 微信公众平台登陆
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="strPassword"></param>
        /// <returns></returns>
        public bool Login(string strUserName, string strPassword)
        {
            WriteLog("开始登录");
            strPassword = to32MD5(strPassword);
            string strRequest = "http://mp.weixin.qq.com/cgi-bin/login?lang=zh_CN";
            string strParameter = "username=" + strUserName + "&pwd=" + strPassword + "&imgcode=&f=json";
            HTTPproc.RequestHeaders.Set("Referer", "http://mp.weixin.qq.com/cgi-bin/loginpage?lang=zh_CN&t=wxm2-login");
            string strContent = HTTPproc.OpenRead(strRequest, strParameter);
            string resultString = null;
            try
            {
                resultString = Regex.Match(strContent, "(?<=\"ErrCode\":).*").Value.Trim();
                strToken = Regex.Match(strContent, @"(?<=token=)\d{0,13}").Value;
                WriteLog("得到Token文件：" + strToken);
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }

            if (resultString == "0")
            {
                WriteLog("登录成功");
                WriteLog("创建Cookie文件");
                WriteTxt(HTTPproc.Cookie);
                WriteLog("Cookie文件创建完成");
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion 微信公众平台登陆 Login(string strUserName, string strPassword)

        #region 平台发消息 SendMSG(string strFakeid, string strMessage)
        /// <summary>
        /// 平台发消息
        /// </summary>
        /// <param name="strFakeid"></param>
        /// <param name="strMessage"></param>
        /// <returns></returns>
        public bool SendMSG(string strFakeid, string strMessage)
        {
            WriteLog("开始发消息");

            string strCookies = null;
            //WriteLog("开始判定是否有Cookie文件");
            //if (!File.Exists(@"Cookie.txt"))
            //{
            //    WriteLog("没有Cookie文件，开始转入登录流程");
            //    bool blLogin = false;
            //    blLogin = Login("182536608@qq.com", "loveemma++");
            //    if (blLogin)
            //    {
            //        WriteLog("登录成功，读取Cookie文件");
            //        strCookies = ReadTxt();
            //    }
            //}
            //else
            //{
            //    WriteLog("已有Cookie文件，正在读取");
            //    strCookies = ReadTxt();
            //}
            Login("182536608@qq.com", "loveemma++");
            strCookies = ReadTxt();

            strMessage = UrlEncode(strMessage, "UTF-8");
            string strRequest = "http://mp.weixin.qq.com/cgi-bin/singlesend";
            string strParameter = "type=1&content=" + strMessage + "&tofakeid=" + strFakeid + "&imgcode=&token=" + strToken + "&lang=zh_CN&random=0.6902240682940994&t=ajax-response";
            HTTPproc.RequestHeaders.Set("Cookie", strCookies);
            HTTPproc.RequestHeaders.Set("Referer", "http://mp.weixin.qq.com/cgi-bin/singlesendpage?tofakeid=" + strFakeid + "&t=message/send&action=index&token=" + strToken + "&lang=zh_CN");

            string strContent = HTTPproc.OpenRead(strRequest, strParameter);
            if (strContent.IndexOf("ok") > 0)
            {
                WriteLog("消息发送成功");
                WriteLog(strContent);
                return true;
            }
            else
            {
                if (File.Exists(@"Cookie.txt"))
                {
                    File.Delete(@"Cookie.txt");
                }
                WriteLog("Cookie文件过期");
                WriteLog(strContent);
                return false;
            }
        }
        #endregion 平台发消息 SendMSG(string strFakeid, string strMessage)

        #region URL编码 UrlEncode(string str, string encode)
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
            string okChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.%";
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

        #region MD5加密 to32MD5(string str)
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string to32MD5(string str)
        {
            MD5 md5 = MD5.Create();
            string md5str = "";
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            for (int i = 0; i < s.Length; i++)
            {
                md5str = md5str + s[i].ToString("x");
            }
            return md5str;
        }
        #endregion MD5加密 to32MD5(string str)

        #region 创建Cookie文件 WriteTxt(string strContent)
        /// <summary>
        /// 创建Cookie文件
        /// </summary>
        /// <param name="strContent"></param>
        private void WriteTxt(string strContent)
        {
            FileStream fs = new FileStream(@"Cookie.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.Write(strContent);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }
        #endregion 创建Cookie文件 WriteTxt(string strContent)

        #region 读取Cookie文件 ReadTxt()
        /// <summary>
        /// 读取Cookie文件
        /// </summary>
        /// <returns></returns>
        private string ReadTxt()
        {
            StreamReader objReader = new StreamReader(@"Cookie.txt");
            string sLine = "";
            while (objReader.Peek() >= 0)
            {
                sLine = objReader.ReadLine();
            }
            objReader.Close();
            return sLine;
        }
        #endregion 读取Cookie文件 ReadTxt()

        #region 写日志 WriteLog(string strContent)
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="strContent"></param>
        public void WriteLog(string strContent)
        {
            Log.WriteLog(LogFile.Trace, strContent);
        }
        #endregion 写日志 WriteLog(string strContent)
    }
}
