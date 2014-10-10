using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace DemoHour_Message
{
    class Program
    {
        static WebClient HTTPproc;
        public static string strToken = "";///唯一Token

        static void Main(string[] args)
        {
            string strUserInfo = "cupid0426@163.com,qweqwe123";
            
            bool isLogin = false;
            HTTPproc = new WebClient();
            HTTPproc.Encoding = Encoding.UTF8;
            string[] arrUser = strUserInfo.Split(',');//将用户名和密码拆分成数组
            isLogin = Login(arrUser[0].ToString(), arrUser[1].ToString());//用户名密码登录

            string strRequest = "http://nf-2.demohour.com/notifications/new";
            string strContent = "";
            int intComments = 0;
            int intMessages = 0;
            int intNotifications = 0;
            int intPosts = 0;

            int intCommentsTemp = 0;
            int intMessagesTemp = 0;
            int intNotificationsTemp = 0;
            int intPostsTemp = 0;

            while (isLogin)
            {
                strContent = HTTPproc.OpenRead(strRequest);

                try
                {
                    strContent = Regex.Match(strContent, @"\{.*\}").Value.Replace("\"", "").Replace("{", "").Replace("}", "");
                    string[] arrClass = strContent.Split(',');
                    foreach (string strClass in arrClass)
                    {
                        string[] arrContent = strClass.Split(':');
                        if (arrContent[0] == "new_comments_count")
                        {
                            intComments = Convert.ToInt32(arrContent[1]);
                        }

                        if (arrContent[0] == "new_messages_count")
                        {
                            intMessages = Convert.ToInt32(arrContent[1]);
                        }

                        if (arrContent[0] == "new_notifications_count")
                        {
                            intNotifications = Convert.ToInt32(arrContent[1]);
                        }

                        if (arrContent[0] == "new_posts_count")
                        {
                            intPosts = Convert.ToInt32(arrContent[1]);
                        }
                    }
                }
                catch (ArgumentException ex)
                {
                    // Syntax error in the regular expression
                }

                Console.WriteLine("评论：" + intComments + " 私信：" + intMessages + " 通知：" + intNotifications + " 帖子：" + intPosts + "    " + DateTime.Now);

                //if (intComments > 0 && intCommentsTemp == 0)
                //{
                //    SendMessage("有新评论：" + intComments);
                //}
                //intCommentsTemp = intComments;

                if (intMessages > 0 && intMessagesTemp == 0)
                {
                    SendMessage("有新私信：" + intMessages);
                }
                intMessagesTemp = intMessages;

                //if (intNotifications > 0 && intNotificationsTemp == 0)
                //{
                //    SendMessage("有新通知：" + intNotifications);
                //}
                //intNotificationsTemp = intNotifications;

                //if (intPosts > 0 && intPostsTemp == 0)
                //{
                //    SendMessage("有新帖子：" + intPosts);
                //}
                //intPostsTemp = intPosts;

                Thread.Sleep(5000);
            }
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

            strRequest = "http://www.demohour.com/session?_action=new";
            strParameter = "utf8=%E2%9C%93&authenticity_token=" + UrlEncode(strToken, "UTF-8") + "&email=" + UrlEncode(strUserName, "UTF-8") + "&password=" + UrlEncode(strPassword, "UTF-8");
            HTTPproc.RequestHeaders.Add("Referer:http://www.demohour.com/login");
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
