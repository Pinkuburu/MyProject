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
            HTTPproc = new WebClient();
            LoginBBS("cupid0426", "qweqwe123");
            while (true)
            {
                ReadForum("http://bbs.qdit.com/forum-9-1.html");
                PostThread("9", "827418");
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
            string strIdhash = null;
            string strLoginhash = null;
            string strFormhash = null;
            string strParameter = null;
            string strImgcode = null;
            string strTemp = null;
            string strReferer = null;

            //Step 1.得到加密串
            string strRequest = "http://bbs.qdit.com/member.php?mod=logging&action=login";
            strReferer = strRequest;
            string strContent = HTTPproc.OpenRead(strRequest);

            //得到加密串
            string resultString = null;
            try
            {
                resultString = Regex.Match(strContent, @"seccode_\w{8,10}").Value;
                strIdhash = resultString.Replace("seccode_", "");
                strLoginhash = Regex.Match(strContent, @"main_messaqge_\w{5,8}").Value.Replace("main_messaqge_", "");
                strFormhash = Regex.Match(strContent, @"""formhash"" value=""\w+").Value.Replace("\"formhash\" value=\"","");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("没有找到加密串");
            }

            //Step 2.行到验证码路径
            HTTPproc.RequestHeaders.Add("Referer:" + strReferer);
            strRequest = "http://bbs.qdit.com/misc.php?mod=seccode&action=update&idhash=" + strIdhash + "&inajax=1&ajaxtarget=" + resultString;
            strContent = HTTPproc.OpenRead(strRequest);

            resultString = null;
            try
            {
                resultString = Regex.Match(strContent, @"update=\d{2,6}").Value.Replace("update=", "");
                strTemp = resultString;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("没有找到时间串");
            }

            //Step 3.下载验证码
            strRequest = "http://bbs.qdit.com/misc.php?mod=seccode&update=" + strTemp + "&idhash=" + strIdhash;
            HTTPproc.RequestHeaders.Add("Referer:" + strReferer);
            HTTPproc.DownloadFile(strRequest, @"1.png");

            Console.Write("请输入验证码：");
            strImgcode = Console.ReadLine();

            //Step 4.登录
            strRequest = "http://bbs.qdit.com/member.php?mod=logging&action=login&loginsubmit=yes&loginhash=LF6D7&inajax=1";
            strPassword = md5(strPassword);
            strParameter = "formhash=" + strFormhash + "&referer=http%3A%2F%2Fbbs.qdit.com%2F.%2F&username=" + strUserName + "&password=" + strPassword + "&questionid=0&answer=&sechash=" + strIdhash + "&seccodeverify=" + strImgcode;
            HTTPproc.OpenRead(strRequest, strParameter);

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

        #region 回帖 PostThread()
        /// <summary>
        /// 回帖
        /// </summary>
        private static void PostThread(string fid, string tid)
        {
            DateTime dt = DateTime.Now;
            if (alThread.IndexOf("http://bbs.qdit.com/thread-" + tid + "-1-1.html") > 4 || alThread.IndexOf("http://bbs.qdit.com/thread-" + tid + "-1-1.html") < 0)
            {
                strRequest = "http://bbs.qdit.com/forum.php?mod=post&action=reply&fid=" + fid + "&tid=" + tid + "&extra=page%3D1&replysubmit=yes&infloat=yes&handlekey=fastpost&inajax=1";
                strParameter = "message=%E9%A1%B6%E8%B5%B7.......&posttime=" + Timestamp() + "&formhash=" + _formhash + "&subject=";
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
        #endregion 回帖 PostThread()

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

        #region 计算字符串的一次MD5 md5(String s)
        /// <summary>
        /// 计算字符串的一次MD5
        /// </summary>
        /// <param name="s" /></param>
        /// <returns></returns>
        private static String md5(String s)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);

            bytes = md5.ComputeHash(bytes);

            md5.Clear();

            string ret = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
            }

            return ret.PadLeft(32, '0');
        }
        #endregion 计算字符串的一次MD5 md5(String s)
    }
}
