using System;
using System.Text.RegularExpressions;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace DemoHour_QuickBuy
{
    class Program
    {
        static WebClient HTTPproc;
        public static string strToken = "";///唯一Token
        public static string strBuyLink = "";

        static void Main(string[] args)
        {
            bool blLoop = true;
            bool blIsBuy = false;
            string strLinkID = "340000";
            string[] arrUserInfo = { "cupid0426@163.com,qweqwe123" };//{ "cupid0426@163.com,qweqwe123", "emma-20@163.com,qweqwe123", "lancelot11@126.com,k123456789", "182536608@qq.com,qweqwe123" };
            int i = 0;
            while (blLoop)
            {
                blIsBuy = IsOpen(strLinkID);

                if (blIsBuy)
                {
                    blLoop = false;                    
                }
                else
                {
                    i++;
                    if (i > 100)
                    {
                        Console.Clear();
                        i = 0;
                    }
                    Thread.Sleep(500);
                }
            }

            if (!blLoop)
            {
                bool isLogin = false;
                bool isBuy = false;

                foreach (string s in arrUserInfo)
                {
                    HTTPproc = new WebClient();
                    HTTPproc.Encoding = Encoding.UTF8;
                    string[] arrUser = s.Split(',');//将用户名和密码拆分成数组
                    isLogin = Login(arrUser[0].ToString(), arrUser[1].ToString());//用户名密码登录
                    if (isLogin)//判定是否登录成功
                    {
                        for (int j = 0; j < 2;j++ )
                        {
                            isBuy = Buy(strBuyLink);
                            if (isBuy)
                            {
                                Console.WriteLine(j);
                                SendMessage(arrUser[0].ToString() + " Buy success " + DateTime.Now);
                            }
                        }                        
                    }
                }                
            }            

            Console.ReadKey();
        }

        #region Login Login(string strUserName, string strPassword)
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="strUserName">UserName</param>
        /// <param name="strPassword">Password</param>
        /// <returns>true:false</returns>
        private static bool Login(string strUserName, string strPassword)
        {
            string strRequest = "http://www.demohour.com/login";
            string strContent = "";
            string strParameter = "";
            Console.WriteLine("Login..." + strUserName);
            strContent = HTTPproc.OpenRead(strRequest);

            try
            {
                strToken = Regex.Match(strContent, "(?<=content=\".*).*(?=\" name=\"csrf-token)").Value;//抓取登录Token
            }
            catch (ArgumentException ex)
            {
                strToken = "";
            }

            strRequest = "http://www.demohour.com/session";
            strParameter = "utf8=%E2%9C%93&authenticity_token=" + UrlEncode(strToken, "UTF-8") + "&email=" + UrlEncode(strUserName, "UTF-8") + "&password=" + UrlEncode(strPassword, "UTF-8") + "&auto_login=0&auto_login=1";
            strContent = HTTPproc.OpenRead(strRequest, strParameter);
            if (HTTPproc.StatusCode == 302)
            {
                Console.WriteLine("Login Success.");
                return true;
            }
            Console.WriteLine("Login Fail.");
            return false;
        }
        #endregion

        #region URLEncode UrlEncode(string str, string encode)
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

        #region AnalyzePage IsOpen(string strLinkID)
        /// <summary>
        /// AnalyzePage
        /// </summary>
        /// <param name="strLinkID">LinkID</param>
        /// <returns>true:false</returns>
        private static bool IsOpen(string strLinkID)
        {
            WebClient HTTP_IsOpen = new WebClient();
            HTTP_IsOpen.Encoding = Encoding.UTF8;

            string strSubscribers = "";//关注
            string strStatus = "";//当前状态

            string strRequest = "http://www.demohour.com/projects/" + strLinkID;
            string strContent = HTTP_IsOpen.OpenRead(strRequest);

            try
            {
                strSubscribers = Regex.Match(strContent, "(?<=查看所有支持者\"><i>).*(?=</i>)").Value;
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }

            try
            {
                strBuyLink = Regex.Match(strContent, @"/pledges/\d{0,}").Value;
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }

            if (strBuyLink == "")
            {
                strStatus = "预热中";
                Console.WriteLine("关注：{0} 状态：{1}  {2}", strSubscribers, strStatus, DateTime.Now.ToString());
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region Buy Buy(string strBuyID)
        /// <summary>
        /// Buy
        /// </summary>
        /// <param name="strBuyID">BuyID</param>
        private static bool Buy(string strBuyID)
        {
            string strRequest = "http://www.demohour.com" + strBuyID;
            string strContent = HTTPproc.OpenRead(strRequest);
            string strAddressID = "";
            string strParameter = "";
            string strCheckout = "";
            string strPay = "";

            Console.WriteLine("Into buying stage...");
            if (HTTPproc.StatusCode == 302)
            {
                strRequest = HTTPproc.ResponseHeaders["Location"].ToString();
                strContent = HTTPproc.OpenRead(strRequest);

                try
                {
                    strAddressID = Regex.Match(strContent, @"(?<=<li id=""address_.*)\d{1,}(?="">)").Value;
                }
                catch (ArgumentException ex)
                {
                    // Syntax error in the regular expression
                }

                Console.WriteLine("Start buying submitted...");
                strCheckout = strRequest + "/checkout";
                strParameter = "utf8=%E2%9C%93&authenticity_token=" + UrlEncode(strToken, "UTF-8") + "&address_id=" + strAddressID + "&comment=";
                strContent = HTTPproc.OpenRead(strCheckout, strParameter);
                if (HTTPproc.StatusCode == 302)
                {
                    strPay = strRequest + "/pay";
                    strParameter = "utf8=%E2%9C%93&authenticity_token=" + UrlEncode(strToken, "UTF-8") + "&balance=1";
                    strContent = HTTPproc.OpenRead(strPay, strParameter);
                    if (HTTPproc.StatusCode == 302)
                    {
                        string strSponsored = HTTPproc.ResponseHeaders["Location"].ToString();
                        if (strSponsored.IndexOf("sponsored") > 0)
                        {
                            Console.WriteLine("Buy success...");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("There have been some errors...");
                            return false;
                        }                        
                    }
                    else
                    {
                        Console.WriteLine("There have been some errors...");
                        return false;
                    }                    
                }
            }
            return false;
        }
        #endregion

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
    }
}
