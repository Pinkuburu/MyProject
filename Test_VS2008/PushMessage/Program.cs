using System;
using System.Text;
using System.Text.RegularExpressions;

namespace PushMessage
{
    class Program
    {
        static WebClient HTTPproc = new WebClient();
        static void Main(string[] args)
        {
            HTTPproc.Encoding = Encoding.UTF8;
            //string strContent = SendMessage("cupid0616", "这是一段测试消息，哈哈哈哈~~~");
            string strContent = HTTPproc.OpenRead("http://www.weather.com.cn/data/sk/101120201.html");
            string[] arrInfo = {};
            try
            {
                arrInfo = Regex.Match(strContent, @"\{.*").Value.Replace("\"", "").Replace("{weatherinfo:{", "").Replace("}}", "").Split(',');
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            string aaa = null;
            for (int i = 0; i < arrInfo.Length; i++)
            {
                if (i == 0 || i == 2 || i == 3 || i == 4 || i == 5)
                {
                    try
                    {
                        strContent = Regex.Match(arrInfo[i].ToString(), "(?<=:).*").Value;

                        if (i == 0)
                        {
                            aaa = strContent;
                        }
                        else
                        {
                            aaa = aaa + "，" + strContent;
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        // Syntax error in the regular expression
                    }
                }
            }
            Console.WriteLine(aaa);
            SendMessage(aaa);
        }

        #region MSG SendMessage(string strContent)
        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="strUserName">用户名</param>
        /// <param name="strContent">消息内容</param>
        private static bool SendMessage(string strMessage)
        {
            WebClient HTTPmsg = new WebClient();
            HTTPmsg.Encoding = Encoding.UTF8;
            string strRequest = "http://qpush.me/pusher/push_site/";
            string strResponse = "name=cupid&code=733922&sig=&cache=false&msg%5Btext%5D=" + strMessage;
            string strContent = HTTPmsg.OpenRead(strRequest, strResponse);//{"op":"r","result":"success","message":"naK8ZH"}
            Console.WriteLine(strContent);
            if (strContent == "\"d8cc10e903918f74\"")
            {
                return true;
            }
            else
            {
                Console.WriteLine("消息发送失败");
                return false;
            }
        }
        #endregion

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
