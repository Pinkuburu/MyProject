using System;
using System.Text.RegularExpressions;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace 点名时间筹资监控
{
    class Program
    {
        static WebClient HTTPproc = new WebClient();
        static void Main(string[] args)
        {
            HTTPproc.Encoding = Encoding.UTF8;
            string strPosts = null;
            string strSubscribers = null;
            string strStatus = null;
            string strGZ = null;
            string strContent = null;
            string strURL = "http://www.demohour.com";
            string strID = "334049";
            string strRequest = "http://www.demohour.com/1146169?filter=created";
            bool blStatus = true;
            int i = 0;
            while (blStatus)
            {
                strContent = HTTPproc.OpenRead(strRequest);

                try
                {
                    strPosts = Regex.Match(strContent, "(?<=话题：.*).*(?=</a>)").Value;
                }
                catch (ArgumentException ex)
                {
                    // Syntax error in the regular expression
                }

                try
                {
                    strSubscribers = Regex.Match(strContent, "(?<=关注：.*).*(?=</a>)").Value;
                }
                catch (ArgumentException ex)
                {
                    // Syntax error in the regular expression
                }

                try
                {
                    strGZ = Regex.Match(strContent, "(?<=widthb\"><strong><span>.*).*(?=</span></strong>浏览人数</p>)").Value;
                }
                catch (ArgumentException ex)
                {
                    // Syntax error in the regular expression
                }


                if (strContent.IndexOf("预热中") > 0)
                {
                    strStatus = "预热中";
                }
                else
                {
                    strStatus = "筹资中";

                    bool blMsg = false;
                    blMsg = SendMessage("cupid0616", "项目开始众筹 " + DateTime.Now);
                    if (blMsg)
                    {
                        Console.WriteLine("消息发送成功");
                    }
                    else
                    {
                        Console.WriteLine("消息发送失败");
                    }

                    strContent = HTTPproc.OpenRead("http://www.demohour.com/projects/" + strID + "/pledges");
                    string resultString = "";
                    try
                    {
                        resultString = Regex.Match(strContent, @"/pledges/\d{0,}").Value;
                    }
                    catch (ArgumentException ex)
                    {
                        // Syntax error in the regular expression
                    }

                    if (resultString != "")
                    {
                        CMD("C:\\Windows\\explorer " + strURL + resultString);
                        //CMD("E:\\FireFox\\firefox " + strURL + resultString);

                        CMD("\"E:\\网络软件\\Mozilla Firefox\\firefox\" " + strURL + resultString);
                        CMD("\"C:\\Program Files\\Google\\Chrome\\Application\\chrome\" " + strURL + resultString);
                        CMD("E:\\网络软件\\opera\\opera " + strURL + resultString);
                    }

                    
                    //CMD("C:\\Windows\\explorer http://www.demohour.com/projects/334049/pledges");
                    //CMD("\"E:\\网络软件\\Mozilla Firefox\\firefox\" http://www.demohour.com/projects/334049/pledges");
                    //CMD("\"C:\\Program Files\\Google\\Chrome\\Application\\chrome\" http://www.demohour.com/projects/334049");
                    //CMD("E:\\网络软件\\opera\\opera " + strURL + resultString);
                    blStatus = false;
                }

                Console.WriteLine("话题：{0} 关注：{1} 浏览人数：{2} 状态：{3}  {4}", strPosts, strSubscribers, strGZ, strStatus, DateTime.Now.ToString());
                
                Thread.Sleep(2000);

                i++;
                if (i > 100)
                {
                    Console.Clear();
                    i = 0;
                }
            }
            
            Console.ReadKey();
        }

        #region 命令控制台调用 CMD(string dosCommand)
        /// <summary>
        /// 命令控制台调用
        /// </summary>
        /// <param name="dosCommand"></param>
        /// <returns></returns>
        public static void CMD(string dosCommand)
        {
            int errorLevel = -1;
            //string pathToScannerProgram = Path.Combine(virusCheckFolder, "scan.exe");
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + dosCommand;
            startInfo.RedirectStandardOutput = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.Start();
                //process.StandardOutput.ReadToEnd();
                string output = process.StandardOutput.ReadToEnd();
                //Console.WriteLine(output);
                //errorLevel = process.ExitCode;//返回ERRORLEVEL
                process.WaitForExit();
            }
            //return errorLevel;
        }
        #endregion 命令控制台调用 CMD(string dosCommand)

        #region 推送消息 SendMessage(string strUserName, string strContent)
        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="strUserName">用户名</param>
        /// <param name="strContent">消息内容</param>
        private static bool SendMessage(string strUserName, string strMessage)
        {
            WebClient HTTPproc = new WebClient();
            HTTPproc.Encoding = Encoding.UTF8;
            string strRequest = "http://1290.me/!push-msgadd";
            string strResponse = "domains=" + strUserName + "&permitcode=&nickname=Robot&sendtime=&video=&act=&code=&size=&extension=&mime=&md5=&type=0&upload=&uinfo=&q=&uploadname=&content=" + UrlEncode(strMessage, "UTF-8") + "&onlyme=&f=2&size=&yunstore=&imgtype=&width=&height=&cover=&zip=&json=&image=";
            string strContent = HTTPproc.OpenRead(strRequest, strResponse);//{"op":"r","result":"success","message":"naK8ZH"}
            if (strContent.IndexOf("success") > 0)
            {
                return true;
            }
            else
            {
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
