using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace FxCommand
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime dt = DateTime.Now;
            string strContent = args[1].ToString() + "--" + args[2].ToString();
            if (dt.DayOfWeek.ToString() == "Saturday" || dt.DayOfWeek.ToString() == "Sunday")
            {
                SendSMS("13780645905", strContent);
            }
            else
            {
                if (dt.Hour > 9 && dt.Hour < 18)
                {
                    try
                    {
                        SendQQMsg("279043930", strContent);
                        Thread.Sleep(2000);
                        SendQQMsg("182536608", strContent);
                    }
                    catch
                    {
                        SendSMS("13780645905", strContent);
                    }

                }
                else
                {
                    SendSMS("13780645905", strContent);
                }
            }            
        }

        #region 短信发送 SendSMS(string strToMobile, string strMsg)
        /// <summary>
        /// 短信发送
        /// </summary>
        /// <param name="strToMobile">收短信手机号</param>
        /// <param name="strMsg">短信内容</param>
        /// <returns></returns>
        public static string SendSMS(string strToMobile, string strMsg)
        {
            //C:\fetion>fetion --mobile=13691102424 --pwd=UTx/t4YJrvqz8mnO --exit-on-verifycode=1 --to=13573866764 --msg-utf8=TEST
            string strResult = null;
            try
            {
                string strExePath = @"C:\fetion\fetion.exe";
                //strExePath = Path.GetFullPath(strExePath);
                ProcessStartInfo procInfo = new ProcessStartInfo(strExePath, string.Format("--mobile=13691102424 --pwd=UTx/t4YJrvqz8mnO --exit-on-verifycode=1 --to={0} --msg-gb={1}", strToMobile, strMsg));
                // 隐藏EXE运行的窗口
                procInfo.WindowStyle = ProcessWindowStyle.Hidden;
                // exe运行
                Process procBatch = Process.Start(procInfo);
                // 取得EXE运行后的返回值，返回值只能是整型
                strResult = "短信发送成功~~";
            }
            catch
            {
                strResult = "短信发送失败~~";
            }
            return strResult;
        }
        #endregion 短信发送 SendSMS(string strToMobile, string strMsg)

        #region 发送QQ消息 SendQQMsg(string strQQ, string strMsg)
        /// <summary>
        /// 发送QQ消息
        /// </summary>
        /// <param name="strQQ"></param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public static string SendQQMsg(string strQQ, string strMsg)
        {
            string strContent = null;
            WebClient HTTPproc = new WebClient();
            //Http://61.147.125.204:8848/Api?Key=Cupid&utf=1&SendType=SendMessage&ID=182536608&Message=%e6%b5%8b%e8%af%95%e6%b6%88%e6%81%af&Time=5000  //HttpUtility.UrlEncode(Rich_Message.Text)
            strContent = HTTPproc.OpenRead("Http://61.147.125.204:8848/Api?Key=Cupid&utf=1&SendType=SendMessage&ID=" + strQQ + "&Message=" + UrlEncode(strMsg, "UTF-8") + "&Time=1000");
            return strContent;
        }
        #endregion 发送QQ消息 SendQQMsg(string strQQ, string strMsg)

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
        #endregion
    }
}
