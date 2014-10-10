using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Threading;


namespace sgTest_Helper
{
    class Program
    {
        static WebClient HTTPproc = new WebClient();
        public static string strURL = "192.168.1.54:85";
        public static string strUsers = "";
        static void Main(string[] args)
        {
            string strUserName = "sgtest73";
            string strPwd = "111111";
            int i = 0;
            string strName = null;
            string strValue = null;
            string strContent = null;

            Console.WriteLine(Login(strUserName, strPwd));
            Console.WriteLine(SendMSG("【登录成功】"));
            Console.WriteLine(SendMSG("【刷新排行榜】"));
            //Console.WriteLine(Tax());
            //Thread.Sleep(2000);
            //Console.WriteLine(Tax());

            for (i = 0; i < CheckArena().Count; i++)
            {
                strName = CheckArena()[i].ToString();
                strValue = CheckArena()[i + 1].ToString();
                i++;
                Thread.Sleep(2000);
                SendMSG(strName + " " + strValue);
                Console.WriteLine(strName + " " + strValue);
            }

            Console.ReadKey();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="strPwd"></param>
        /// <returns></returns>
        private static string Login(string strUserName, string strPwd)
        {
            string strRequestURL = "http://" + strURL + "/Login.aspx";
            string strParameter = "__EVENTTARGET=LoginButton&__EVENTARGUMENT=&__VIEWSTATE=%2FwEPDwULLTE1MjIyMjE1NzEPZBYCAgMPZBYCZg8PZBYCHgdvbmNsaWNrBSZ0aGlzLnZhbHVlPSIiO3RoaXMuc3R5bGUuY29sb3I9ImJsYWNrImRkZXzAIrLbNEqO0N6Xtq5oSSKFvS4%3D&__EVENTVALIDATION=%2FwEWBALL4%2FigAwLyj%2FOQAgK3jsrkBAL%2BjNCfD9T0OdqUmpB5KKkMO8VJLoarKc0K&tbUserName=" + strUserName + "&tbPassword=" + strPwd;
            string strContent = "";

            HTTPproc.Encoding = Encoding.UTF8;
            HTTPproc.RequestHeaders.Add("Referer:http://" + strURL + "/login.aspx");
            strContent = HTTPproc.OpenRead(strRequestURL,strParameter);
            strContent = Regex.Match(strContent, @"/AutoLogin\.aspx.*").Value.Replace("\r","");
            strRequestURL = "http://" + strURL + strContent;
            HTTPproc.RequestHeaders.Add("Referer:http://" + strURL + "/login.aspx");
            strContent = HTTPproc.OpenRead(strRequestURL);

            return strContent = "";
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="strMSG"></param>
        /// <returns></returns>
        private static string SendMSG(string strMSG)
        {
            string strRequestURL = "http://" + strURL + "/WebIM/Chatting.aspx?Chats=send&lastMessageId=1&Message=" + strMSG + "&typeId=0&TargetName=&TargetId=0&CampId=1&timeStamp=" + Timestamp();
            string strContent = "";
            strContent = HTTPproc.OpenRead(strRequestURL);
            return strContent;
        }

        /// <summary>
        /// 遍历排行榜
        /// </summary>
        /// <returns></returns>
        private static ArrayList CheckArena()
        {
            string strRequestURL = "http://" + strURL + "/Arena/ArenaInfo.aspx?type=1&Page=1";
            string strContent = HTTPproc.OpenRead(strRequestURL);

            ArrayList aryListArena = new ArrayList();
            try
            {
                Regex regexObj = new Regex(@"<td style=""width:233px;"">(\w{1,}|\w{0,}.\w{1,})</td>|<td style=""width:166px;"">\d{1,3}</td>");
                Match matchResult = regexObj.Match(strContent);
                while (matchResult.Success)
                {
                    aryListArena.Add(matchResult.Value.Replace("<td style=\"width:233px;\">", "").Replace("<td style=\"width:166px;\">","").Replace("</td>",""));
                    matchResult = matchResult.NextMatch();
                }
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            aryListArena.RemoveAt(0);
            return aryListArena;
        }

        /// <summary>
        /// 征收
        /// </summary>
        /// <returns></returns>
        private static string Tax()
        {
            string strRequestURL = "http://" + strURL + "/Municipal/MunicipalInfo.aspx?Type=4&timeStamp=" + Timestamp();
            string strContent = HTTPproc.OpenRead(strRequestURL);
            return strContent;
        }

        /// <summary>
        /// 查找用户
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        private static string FindUser(string strName)
        {
            string strUser = "";
            string strRequestURL = "http://" + strURL + "/Default.aspx";
            string strContent = HTTPproc.OpenRead(strRequestURL);
            strContent = Regex.Match(strContent, @"Nickname=""(\w{1,}|\w{0,}.\w{1,})""").Value;
            Console.WriteLine(strUsers + " " + strContent);
            if (strContent.IndexOf(strName) > 0)
            {
                strUser = strUsers;
            }
            return strUser;
        }

        #region 时间戳 Timestamp()
        /// <summary>
        /// 时间戳
        /// </summary>
        /// <returns></returns>
        private static long Timestamp()
        {
            long longTimestamp = 0;
            longTimestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
            return longTimestamp;
        }

        private static long Timestamp2()
        {
            long longTimestamp = 0;
            longTimestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            return longTimestamp;
        }
        #endregion 时间戳 Timestamp()
    }
}
