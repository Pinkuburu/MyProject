using System;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace AutoVote
{
    class Program
    {        
        static RASDisplay ras = new RASDisplay();
        static int errorLevel = -1;
        static Random rnd = new Random();
        static int intTime = 0;
        static int intFirstCount_C = 0;
        static int intFirstCount_P = 0;
        static int intVIP_C = 0;
        static string strVIP = "100000009221125";
        static string strCode = null;
        static int intCodeID = 0;
        static int intLoad = -1;

        [DllImport("AntiVC.dll")]
        public static extern int LoadCdsFromFile(string FilePath, string Password);

        [DllImport("AntiVC.dll")]
        public static extern bool GetVcodeFromFile(int CdsFileIndex, string ImgURL, StringBuilder Vcode);

        static void Main(string[] args)
        {
            Console.WriteLine("加载库文件");
            intLoad = LoadCdsFromFile("zh.cds", "qweqwe123");

            if (intLoad != -1)
            {
                Console.WriteLine("加载库文件加载成功");
                //PostVote();
                while (true)
                {
                    errorLevel = Adsl_Disconnect();
                    if (errorLevel == 0)
                    {
                        errorLevel = -1;
                        errorLevel = Adsl_Connect();
                        if (errorLevel == 0)
                        {
                            string strIP = GetIP();
                            Console.WriteLine("新IP：" + strIP);
                            if (!CheckIPList(strIP))
                            {
                                WriteIPList(strIP);
                                PostVote();
                                //intTime = rnd.Next(180000, 300000);
                                //intTime = rnd.Next(60000, 120000);
                                Console.WriteLine(intTime);
                                Thread.Sleep(intTime);
                            }
                            else
                            {
                                Console.WriteLine("发现重复IP：" + strIP);
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("库文件加载失败");
            }
            
            Console.ReadKey();
        }

        #region IP重复处理
        private static void WriteIPList(string strIP)
        {
            using (StreamWriter sw = new StreamWriter("IP.txt", true, System.Text.Encoding.Default))
            {
                sw.WriteLine(strIP);
                sw.Close();
            }
        }

        private static bool CheckIPList(string strIP)
        {
            bool blTemp = false;
            using (StreamReader sr = new StreamReader("IP.txt", System.Text.Encoding.Default))
            {
                string strLine = sr.ReadLine();
                while (strLine != null)
                {
                    if (strLine.IndexOf(strIP) == 0)
                    {
                        blTemp = true;
                        break;
                    }
                    strLine = sr.ReadLine();
                }
                sr.Close();
                return blTemp;
            }
        }

        private static string GetIP()
        {
            WebClient HTTPproc1 = new WebClient();
            return HTTPproc1.OpenRead("http://ishow.xba.com.cn/ip.aspx");
        }
        #endregion IP重复处理

        #region UUWise
        private static void SetSoftInfo()
        {
            Wrapper.uu_setSoftInfo(90841, "87b47ee994af498986f0115e41cbf361");
        }

        private static int UULogin()
        {
            int res = Wrapper.uu_login("cupid0426", "qweqwe123");
            return res;
        }

        private static int GetOverage()
        {
            int score = Wrapper.uu_getScore("cupid0426", "qweqwe123");
            return score;
        }

        private static string DiscernCode()
        {
            StringBuilder result = new StringBuilder();
            int codeId = Wrapper.uu_recognizeByCodeTypeAndPath("Code.jpg", 1005, result);
            intCodeID = codeId;
            return result.ToString();
        }

        private static int ReportError()
        {
            int result = Wrapper.uu_reportError(intCodeID);
            return result;
        }
        #endregion UUWise

        private static void PostVote()
        {
            WebClient HTTPproc = new WebClient();
            HTTPproc.Encoding = Encoding.UTF8;
            Random rnd = new Random();

            Request:
            DateTime dt = DateTime.Now;
            //Console.WriteLine("哎，还继续投票 " + intCount);
            string strRequest = "http://xzzmzhr.cohl.com/ValidateCode.aspx?m=" + rnd.Next();
            HTTPproc.DownloadFile(strRequest, @"Code.jpg");//下载文件

            StringBuilder sb = new StringBuilder();

            if (GetVcodeFromFile(intLoad, @"Code.jpg", sb))
            {
                //strCode = DiscernCode();
                //Console.WriteLine("识别的验证码：" + strCode);
                Console.WriteLine("识别的验证码：" + sb.ToString());
                strCode = sb.ToString();
                if (strCode.Length == 5)
                {
                    strRequest = "http://xzzmzhr.cohl.com/ajax/SpecialAjax.aspx";
                    string strParameter = "type=specialvote&cid=100000009221125&sid=100000000129687&code=" + strCode;
                    string strContent = HTTPproc.OpenRead(strRequest, strParameter);

                    if (strContent.Trim() == "1")
                    {
                        Console.WriteLine("投票成功 " + dt);
                    }
                    else if (strContent.Trim() == "0")
                    {
                        Console.WriteLine("投票失败此IP可能已经被使用 " + dt);
                    }
                    else
                    {
                        //DateTime dta = DateTime.Now;
                        //File.Move("Code.jpg", strCode + dta + ".jpg");
                        ////=====================发现错码重新下载打码==========================
                        //Console.WriteLine("哎，还继续投票 " + intCount);
                        //strRequest = "http://xzzmzhr.cohl.com/ValidateCode.aspx?m=" + rnd.Next();
                        //HTTPproc.DownloadFile(strRequest, @"Code.jpg");//下载文件

                        //strCode = DiscernCode();
                        //Console.WriteLine("识别的验证码：" + strCode);

                        //strRequest = "http://xzzmzhr.cohl.com/ajax/SpecialAjax.aspx";
                        //strParameter = "type=specialvote&cid=100000009221125&sid=100000000129687&code=" + strCode;
                        //strContent = HTTPproc.OpenRead(strRequest, strParameter);
                        ////===================================================================

                        Console.WriteLine("如果出现这句话，请把内容截图发给作者");
                        Console.WriteLine(strContent);
                        goto Request;
                    }
                }
                else
                {
                    goto Request;
                }

                
            }
            else
            {
                Console.WriteLine("验证码识别失败");
            }

            //DateTime dt = DateTime.Now;
            //int i = 0;
            //string strStatus = "";
            //string strRequest = "http://xzzmzhr.cohl.com/SpecialInfo/Index.aspx";            
            //string strContent = HTTPproc.OpenRead(strRequest);

            //if (strContent.IndexOf("中海先锋-感动中海") > 0)
            //{
            //    Console.WriteLine("分析前五名列表 " + dt);
            //    ArrayList alresultList = new ArrayList();
            //    try
            //    {
            //        Regex regexObj = new Regex("SpecialValidate.*票");
            //        Match matchResult = regexObj.Match(strContent);
            //        while (matchResult.Success)
            //        {
            //            if (i < 5)
            //            {
            //                strTemp = matchResult.Value.Replace("SpecialValidate('", "");
            //                strTemp = Regex.Replace(strTemp, "'.*\"sticksnum\">", ",").Replace(" 票","");
            //                alresultList.Add(strTemp);

            //                matchResult = matchResult.NextMatch();
            //                i++;
            //            }
            //            else
            //            {
            //                break;
            //            }
            //        }
            //    }
            //    catch (ArgumentException ex)
            //    {
            //        // Syntax error in the regular expression
            //    }
            //    Console.WriteLine(alresultList.Count);
            //    if (alresultList.Count > 0)
            //    {
            //        string strFirst = alresultList[0].ToString();   //得到第一数据
            //        Console.WriteLine("得到第一数据 " + strFirst);
            //        string[] arrFirst = strFirst.Split(',');        //拆分数据
            //        string strFirstID = arrFirst[0].ToString();     //第一的投票ID
            //        Console.WriteLine("第一的投票ID " + strFirstID);
            //        string strFirstCount = arrFirst[1].ToString();  //第一的票数
            //        Console.WriteLine("第一的票数 " + strFirstCount);

            //        string strSecond = alresultList[1].ToString();   //得到第二数据
            //        Console.WriteLine("得到第二数据 " + strSecond);
            //        string[] arrSecond = strSecond.Split(',');        //拆分数据
            //        string strSecondID = arrSecond[0].ToString();     //第二的投票ID
            //        Console.WriteLine("第二的投票ID " + strSecondID);
            //        string strSecondCount = arrSecond[1].ToString();  //第二的票数
            //        Console.WriteLine("第二的票数 " + strSecondCount);

            //        string strVIPC = null;
            //        string strVIPCount = null;

            //        Console.WriteLine(alresultList.Count);
            //        for (int j = 0; j < 5; j++)
            //        {
            //            if (alresultList[j].ToString().IndexOf(strVIP) > -1)
            //            {
            //                strVIPC = alresultList[j].ToString();
            //                string[] arrVIP = strVIPC.Split(',');
            //                strVIPCount = arrVIP[1].ToString();

            //                Console.WriteLine(strVIPC + "  " + strVIPCount);
            //            }
            //        }

            //        int intFirst = Convert.ToInt32(strFirstCount);
            //        int intSecond = Convert.ToInt32(strSecondCount);
            //        int intVIP = Convert.ToInt32(strVIPCount);
            //        Console.WriteLine("VIP的票数 " + intVIP);
            //        int intCount = intVIP - intFirst;

            //        if (intCount == 0)
            //        {
            //            //intCount = intVIP - intSecond;
            //        }

            //        if (intCount < 50)
            //        {                        
            //        Request:
            //            Console.WriteLine("哎，还继续投票 " + intCount);
            //            strRequest = "http://xzzmzhr.cohl.com/ValidateCode.aspx?m=" + rnd.Next();
            //            HTTPproc.DownloadFile(strRequest, @"Code.jpg");//下载文件

            //            strCode = DiscernCode();
            //            Console.WriteLine("识别的验证码：" + strCode);

            //            strRequest = "http://xzzmzhr.cohl.com/ajax/SpecialAjax.aspx";
            //            string strParameter = "type=specialvote&cid=100000009221125&sid=100000000129687&code=" + strCode;
            //            strContent = HTTPproc.OpenRead(strRequest, strParameter);

            //            if (strContent.Trim() == "1")
            //            {
            //                Console.WriteLine("投票成功 " + dt);
            //            }
            //            else if (strContent.Trim() == "0")
            //            {
            //                Console.WriteLine("投票失败此IP可能已经被使用 " + dt);
            //            }
            //            else
            //            {
            //                Console.WriteLine("发现错误打码：" + ReportError());
            //                //DateTime dta = DateTime.Now;
            //                //File.Move("Code.jpg", strCode + dta + ".jpg");
            //                ////=====================发现错码重新下载打码==========================
            //                //Console.WriteLine("哎，还继续投票 " + intCount);
            //                //strRequest = "http://xzzmzhr.cohl.com/ValidateCode.aspx?m=" + rnd.Next();
            //                //HTTPproc.DownloadFile(strRequest, @"Code.jpg");//下载文件
                            
            //                //strCode = DiscernCode();
            //                //Console.WriteLine("识别的验证码：" + strCode);

            //                //strRequest = "http://xzzmzhr.cohl.com/ajax/SpecialAjax.aspx";
            //                //strParameter = "type=specialvote&cid=100000009221125&sid=100000000129687&code=" + strCode;
            //                //strContent = HTTPproc.OpenRead(strRequest, strParameter);
            //                ////===================================================================

            //                Console.WriteLine("如果出现这句话，请把内容截图发给作者");
            //                Console.WriteLine(strContent);
            //                goto Request;
            //            }
            //        }
            //        else
            //        {
            //            Console.WriteLine("哎，累死我了，终于不用投了，已经高于第二50票了");
            //        }
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("他们的服务器可能又挂了...");
            //}
        }

        private static int Adsl_Disconnect()
        {
            Console.WriteLine("断开连接");
            //ras.Disconnect();
            return CMD("rasdial /disconnect");
        }

        private static int Adsl_Connect()
        {
            Console.WriteLine("开始连接");
            //ras.Connect("ADSL");
            return CMD("rasdial ADSL a053203289293 123123");
            //return CMD("rasdial 宽带连接 190100050775 123456");
        }

        private static bool CheckFirst(string strVoteID)
        {
            if (strVoteID == strVIP)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region 命令控制台调用 CMD(string dosCommand)
        /// <summary>
        /// 命令控制台调用
        /// </summary>
        /// <param name="dosCommand"></param>
        /// <returns></returns>
        public static int CMD(string dosCommand)
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
                Console.WriteLine(output);
                errorLevel = process.ExitCode;//返回ERRORLEVEL
                process.WaitForExit();
            }
            return errorLevel;
        }
        #endregion 命令控制台调用 CMD(string dosCommand)
    }
}
