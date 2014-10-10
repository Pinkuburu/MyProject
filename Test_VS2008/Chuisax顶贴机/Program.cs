using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Text.RegularExpressions;

namespace Chuisax顶贴机
{
    class Program
    {
        static WebClient HTTPproc = new WebClient();
        static ArrayList alThread = new ArrayList();
        static string _formhash = "";
        static string strRequest = "";
        static string strParameter = "";
        static string strContent = "";
        static string strThreadID = "";
        static string strBBSVerify = "";
        static string strThreadVerify = "";
        static string strFID = "";
        static string strTID = "";
        static int intSite = 10;

        static void Main(string[] args)
        {
            strFID = "23";
            strTID = "48435";
            LoginChuiSax("test001", "qweqwe123");
            ReadForum(strFID);            
            strBBSVerify = ReadBBSVerify();
            strThreadVerify = ReadThreadVerify(strTID);
            PostThread();
            //Console.WriteLine(ReadBBSVerify());
            //Console.WriteLine(ReadThreadVerify(strThreadID));
            Console.ReadKey();
        }

        #region 抓取论坛BBSVerify ReadBBSVerify();
        /// <summary>
        /// 抓取论坛BBSVerify
        /// </summary>
        /// <returns></returns>
        private static string ReadBBSVerify()
        {
            string strBBSverify = "";
            string strContent;

            string strURL = "http://www.chuisax.com/index.php";
            try
            {
                strContent = HTTPproc.OpenRead(strURL);
            }
            catch
            {
                Thread.Sleep(3000);
            	strContent = HTTPproc.OpenRead(strURL);
            }            

            string resultString = null;
            try
            {
                resultString = Regex.Match(strContent, "//var verifyhash.*;").Value;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("抓取BBS验证码出错");
                //Thread.Sleep(60000);
            }
            
            //var verifyhash = 'd349afbd';
            strBBSverify = resultString.Replace("//var verifyhash = '", "").Replace("';","");
            Console.WriteLine("得到BBSVerify:" + strBBSverify);

            return strBBSverify;
        }
        #endregion 抓取论坛BBSVerify ReadBBSVerify();

        #region 抓取帖子ThreadVerify ReadThreadVerify(string TID)
        /// <summary>
        /// 抓取帖子ThreadVerify
        /// </summary>
        /// <param name="strURL"></param>
        /// <returns></returns>
        private static string ReadThreadVerify(string TID)
        {
            string strThreadVerify = "";
            string strContent;
            string strURL = "read-htm-tid-" + TID + ".html";
            try
            {
                strContent = HTTPproc.OpenRead("http://www.chuisax.com/" + strURL);
            }
            catch
            {
                Thread.Sleep(3000);
                strContent = HTTPproc.OpenRead("http://www.chuisax.com/" + strURL);
            }

            string resultString = null;
            try
            {
                resultString = Regex.Match(strContent, "_hexie.value.*;").Value;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("抓取帖子验证码出错");
                //Thread.Sleep(60000);
            }

            //_hexie.value = 'd2628bec';
            strThreadVerify = resultString.Replace("_hexie.value = '", "").Replace("';", "");
            Console.WriteLine("得到ThreadVerify:" + strThreadVerify);

            return strThreadVerify;
        }
        #endregion 抓取帖子ThreadVerify ReadThreadVerify(string TID)

        #region ChuiSax登录 LoginChuiSax(string strUserName, string strPassword)
        /// <summary>
        /// ChuiSax登录
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="strPassword"></param>
        private static void LoginChuiSax(string strUserName, string strPassword)
        {
            string strRequest = "http://www.chuisax.com/Login.php";
            string strParameter = "forward=&jumpurl=http%3A%2F%2Fwww.chuisax.com%2Findex.php&step=2&lgt=0&pwuser=" + strUserName + "&pwpwd=" + strPassword + "&hideid=0&submit=";

            HTTPproc.RequestHeaders.Add("Referer:http://www.chuisax.com/Login.php");
            try
            {
                HTTPproc.OpenRead(strRequest, strParameter);
                Console.WriteLine("登录成功");
            }
            catch
            {
                Thread.Sleep(3000);
                HTTPproc.OpenRead(strRequest, strParameter);
            }
        }
        #endregion ChuiSax登录 LoginChuiSax(string strUserName, string strPassword)

        #region 读取论坛栏目内容 ReadForum(string strForumLink)
        /// <summary>
        /// 读取论坛栏目内容
        /// </summary>
        /// <param name="strForumLink"></param>
        private static void ReadForum(string FID)
        {
            string strContent = null;
            string strForumLink = "http://www.chuisax.com/thread-htm-fid-" + FID + ".html";
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
                Regex regexObj = new Regex(@"read-htm-tid-\d{0,8}\.html"" name=""readlink""");
                Match matchResult = regexObj.Match(strContent);
                while (matchResult.Success)
                {
                    alThread.Add(matchResult.Value.Replace("\" name=\"readlink\"", ""));
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
            if (alThread.IndexOf(strThreadID) > intSite || alThread.IndexOf(strThreadID) < 0)
            {
                strRequest = "http://www.chuisax.com/post.php?fid=" + strFID + "&nowtime=" + Timestamp() + "&verify=" + strBBSVerify;
                
                long ssss = Timestamp();

                strParameter = "-----------------------------" + ssss + "\n"
                             + "Content-Disposition: form-data; name=\"atc_usesign\"\n"
                             + "\n"
                             + "1\n"
                             + "-----------------------------" + ssss + "\n"
                             + "Content-Disposition: form-data; name=\"replytouser\"\n"
                             + "\n"
                             + "\n"
                             + "-----------------------------" + ssss + "\n"
                             + "Content-Disposition: form-data; name=\"atc_convert\"\n"
                             + "\n"
                             + "1\n"
                             + "-----------------------------" + ssss + "\n"
                             + "Content-Disposition: form-data; name=\"atc_autourl\"\n"
                             + "\n"
                             + "1\n"
                             + "-----------------------------" + ssss + "\n"
                             + "Content-Disposition: form-data; name=\"step\"\n"
                             + "\n"
                             + "2\n"
                             + "-----------------------------" + ssss + "\n"
                             + "Content-Disposition: form-data; name=\"type\"\n"
                             + "\n"
                             + "ajax_addfloor\n"
                             + "-----------------------------" + ssss + "\n"
                             + "Content-Disposition: form-data; name=\"action\"\n"
                             + "\n"
                             + "reply\n"
                             + "-----------------------------" + ssss + "\n"
                             + "Content-Disposition: form-data; name=\"fid\"\n"
                             + "\n"
                             + strFID + "\n"
                             + "-----------------------------" + ssss + "\n"
                             + "Content-Disposition: form-data; name=\"cyid\"\n"
                             + "\n"
                             + "\n"
                             + "-----------------------------" + ssss + "\n"
                             + "Content-Disposition: form-data; name=\"tid\"\n"
                             + "\n"
                             + strTID + "\n"
                             + "-----------------------------" + ssss + "\n"
                             + "Content-Disposition: form-data; name=\"stylepath\"\n"
                             + "\n"
                             + "wind\n"
                             + "-----------------------------" + ssss + "\n"
                             + "Content-Disposition: form-data; name=\"ajax\"\n"
                             + "\n"
                             + "1\n"
                             + "-----------------------------" + ssss + "\n"
                             + "Content-Disposition: form-data; name=\"verify\"\n"
                             + "\n"
                             + strBBSVerify + "\n"
                             + "-----------------------------" + ssss + "\n"
                             + "Content-Disposition: form-data; name=\"_hexie\"\n"
                             + "\n"
                             + strThreadVerify + "\n"
                             + "-----------------------------" + ssss + "\n"
                             + "Content-Disposition: form-data; name=\"iscontinue\"\n"
                             + "\n"
                             + "0\n"
                             + "-----------------------------" + ssss + "\n"
                             + "Content-Disposition: form-data; name=\"isformchecked\"\n"
                             + "\n"
                             + "1\n"
                             + "-----------------------------" + ssss + "\n"
                             + "Content-Disposition: form-data; name=\"atc_title\"\n"
                             + "\n"
                             + "\n"
                             + "-----------------------------" + ssss + "\n"
                             + "Content-Disposition: form-data; name=\"atc_content\"\n"
                             + "\n"
                             + "ChuiSax全自动顶贴机" + dt.ToString(" yyyy-MM-dd HH:mm:ss") + "\n"
                             + "-----------------------------" + ssss + "--\n";

                try
                {
                    HTTPproc.RequestHeaders.Add("Content-Type: multipart/form-data; boundary=---------------------------" + ssss);
                    strContent = HTTPproc.OpenRead(strRequest, strParameter);
                }
                catch
                {
                    Thread.Sleep(3000);
                    HTTPproc.RequestHeaders.Add("Content-Type: multipart/form-data; boundary=---------------------------" + ssss);
                    strContent = HTTPproc.OpenRead(strRequest, strParameter);
                }
                //Console.WriteLine(strContent);
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
