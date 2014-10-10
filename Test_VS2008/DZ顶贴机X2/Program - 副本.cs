using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading;

namespace DZ顶贴机X2
{
    class Program
    {
        static WebClient HTTPproc = new WebClient();
        static ArrayList alThread = new ArrayList();
        static string _formhash = "";
        static string strRequest = "";
        static string strParameter = "";
        static string strContent = "";

        static void Main(string[] args)
        {
            while (true)
            {
                HTTPproc = new WebClient();
                LoginBBS("论坛帐号", "论坛密码");
                ReadForum("http://bbs.qdit.com/forum-35-1.html");
                PostThread();
                Thread.Sleep(30000);
            }
        }

        #region QDIT登录 LoginBBS(string strUserName,string strPassword)
        /// <summary>
        /// QDIT登录
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="strPassword"></param>
        private static void LoginBBS(string strUserName,string strPassword)
        {
            string strRequest = "http://bbs.qdit.com/member.php?mod=logging&action=login&loginsubmit=yes&infloat=yes&lssubmit=yes&inajax=1";
            string strParameter = "username=" + strUserName + "&password=" + strPassword + "&quickforward=yes&handlekey=ls";

            HTTPproc.RequestHeaders.Add("Referer:http://bbs.qdit.com/");
            try
            {
                HTTPproc.OpenRead(strRequest, strParameter);
            }
            catch
            {
                Thread.Sleep(3000);
                HTTPproc.OpenRead(strRequest, strParameter);
            }
        }
        #endregion QDIT登录 LoginBBS(string strUserName,string strPassword)

        #region 读取论坛栏目内容 ReadForum(string strForumLink)
        /// <summary>
        /// 读取论坛栏目内容
        /// </summary>
        /// <param name="strForumLink"></param>
        private static void ReadForum(string strForumLink)
        {
            string strContent = null;

            try
            {
                strContent = HTTPproc.OpenRead(strForumLink);
            }
            catch (System.Exception ex)
            {
                Thread.Sleep(3000);
                strContent = HTTPproc.OpenRead(strForumLink);
            }
            
            try
            {
                _formhash = Regex.Match(strContent, "formhash=.*>退出").Value;
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }

            _formhash = _formhash.Replace("formhash=", "").Replace("\">退出", ""); //formhash处理

            try
            {
                Regex regexObj = new Regex(@"http://bbs\.qdit\.com/thread-\d{1,}-\d-\d\.html"" onclick");
                Match matchResult = regexObj.Match(strContent);
                while (matchResult.Success)
                {
                    alThread.Add(matchResult.Value.Replace("\" onclick", ""));
                    matchResult = matchResult.NextMatch();
                }
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
        }
        #endregion 读取论坛栏目内容 ReadForum(string strForumLink)

        /// <summary>
        /// 回帖
        /// </summary>
        private static void PostThread()
        {
            DateTime dt = DateTime.Now;
            if (alThread.IndexOf("http://bbs.qdit.com/thread-623848-1-1.html") > 2 || alThread.IndexOf("http://bbs.qdit.com/thread-623848-1-1.html") < 0)
            {
                strRequest = "http://bbs.qdit.com/forum.php?mod=post&action=reply&fid=35&tid=623848&extra=page%3D1&replysubmit=yes&infloat=yes&handlekey=fastpost&inajax=1";
                strParameter = "message=QDIT%C8%AB%D7%D4%B6%AF%B6%A5%CC%F9%BB%FA" + dt.ToString(" yyyy-MM-dd hh:mm:ss") + "&posttime=" + Timestamp() + "&formhash=" + _formhash + "&subject=";
                try
                {
                    strContent = HTTPproc.OpenRead(strRequest, strParameter);
                }
                catch
                {
                    Thread.Sleep(3000);
                    strContent = HTTPproc.OpenRead(strRequest, strParameter);
                }
                Console.WriteLine(strContent);
            }
            alThread.Clear();
        }

        #region 时间戳 Timestamp()
        /// <summary>
        /// 时间戳
        /// </summary>
        /// <returns></returns>
        private static long Timestamp()
        {
            long longTimestamp = 0;
            longTimestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            return longTimestamp;
        }
        #endregion 时间戳 Timestamp()
    }
}
