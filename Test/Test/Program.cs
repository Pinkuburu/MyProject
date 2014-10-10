using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Management;
using System.Net;
using System.Xml;
using System.Collections;
using System.Data;
using databaseitem;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Threading;

namespace Test
{
    public class ThreadLock   
    {
        Test.WebClient HTTPproc_1 = new WebClient();

        ArrayList al_Fushi = new ArrayList();
        ArrayList al_Jiaju = new ArrayList();
        ArrayList al_Meirong = new ArrayList();
        ArrayList al_Shipin = new ArrayList();
        ArrayList al_Dianqi = new ArrayList();

        private Thread threadOne;   
        private Thread threadTwo;
  
        private object objLock = new object();   
  
        public ThreadLock()   
        {
            threadOne = new Thread(new ThreadStart(ReadProduct));   
            threadOne.Name = "Thread_1";
            threadTwo = new Thread(new ThreadStart(ReadProduct));   
            threadTwo.Name = "Thread_2";
        }   
  
        public void Start()   
        {
            //设置HTTP请求默认编码
            HTTPproc_1.Encoding = System.Text.Encoding.Default;

            string resultString = null;
            string resultString_0 = null;
            string resultString_1 = null;
            string resultString_2 = null;
            string resultString_3 = null;
            string resultString_4 = null;
            int intPage = 0;
            int i = 0;
            int j = 0;
            int k = 0;

            //正则数据  --- id=788" target="_blank"><span class="title"><font title="长袖个性精梳棉T恤">长袖个性精梳棉T恤</font>
            Regex regexObj = new Regex(@"id=\d{1,4}.*</font>.*\r\n.*<br>");
            Regex regexObj_1 = new Regex("src=\"http://img.*.jpg");

            for (j = 1; j <= 5; j++)
            {
                resultString = HTTPproc_1.OpenRead("http://bo.tianxia.taobao.com/taoboyuan/seedstore_category.jhtml?pageNo=1&catId=" + j + "");
                try
                {
                    resultString_4 = resultString;
                    resultString = Regex.Match(resultString, @"共\d{1,2}页").Value.Replace("共", "").Replace("页", "");
                    intPage = Convert.ToInt32(resultString);
                }
                catch (ArgumentException ex)
                {
                    // Syntax error in the regular expression
                }

                for (k = 1; k <= intPage; k++)
                {
                    if (k > 1)
                    {
                        resultString = HTTPproc_1.OpenRead("http://bo.tianxia.taobao.com/taoboyuan/seedstore_category.jhtml?pageNo=" + k + "&catId=" + j + "");
                    }

                    if (k > 1)
                    {

                    }
                    else
                    {
                        resultString = resultString_4;
                    }

                    Match matchResults = regexObj.Match(resultString);
                    Match matchResults_1 = regexObj_1.Match(resultString);
                    while (matchResults.Success)
                    {
                        try
                        {
                            //正则ID数据  --- id=788" target="_blank"><span class="title"><font title="长袖个性精梳棉T恤">长袖个性精梳棉T恤</font>
                            resultString_0 = Regex.Match(matchResults.ToString(), @"id=\d{1,4}").Value.Replace("id=", "");
                            //正则名称数据  --- id=788" target="_blank"><span class="title"><font title="长袖个性精梳棉T恤">长袖个性精梳棉T恤</font>
                            //resultString_1 = Regex.Match(matchResults.ToString(), "<font.*</font>").Value;
                            //resultString_1 = Regex.Replace(resultString_1, "<font.*\">", "").Replace("</font>", "");
                            resultString_1 = Regex.Match(matchResults.ToString(), "<font.*\">").Value.Replace("<font title=\"", "").Replace("\">", "");
                            //正则通宝数据  --- 所需通宝:<span class="xuyao">40</span><br>
                            resultString_2 = Regex.Match(matchResults.ToString(), @"\d{1,3}</span>").Value.Replace("</span>", "");
                            //正则图片数据
                            resultString_3 = matchResults_1.ToString().Replace("src=\"", "");
                            //al_Product.Add(resultString_3);

                            if (j == 1)
                            {
                                al_Fushi.Add(resultString_3);
                            }
                            if (j == 2)
                            {
                                al_Jiaju.Add(resultString_3);
                            }
                            if (j == 3)
                            {
                                al_Meirong.Add(resultString_3);
                            }
                            if (j == 4)
                            {
                                al_Shipin.Add(resultString_3);
                            }
                            if (j == 5)
                            {
                                al_Dianqi.Add(resultString_3);
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            // Syntax error in the regular expression
                        }
                        matchResults = matchResults.NextMatch();
                        matchResults_1 = matchResults_1.NextMatch();
                    }
                }
            }

            threadOne.Start();   
            threadTwo.Start();
        }   
  
        //private void Run()   
        //{
        //    while (ticketList.Count > 0)//①   
        //    {
        //        lock (objLock)
        //        {
        //            if (ticketList.Count > 0)
        //            {
        //                string ticketNo = ticketList[0];//②   
        //                Console.WriteLine("{0}:售出一张票，票号：{1}", Thread.CurrentThread.Name, ticketNo);
        //                ticketList.RemoveAt(0);//③   
        //                Thread.Sleep(1);
        //            }
        //        }
        //    } 
        //}

        private void ReadProduct()
        {
            string resultString = null;

            //设置HTTP请求默认编码
            HTTPproc_1.Encoding = System.Text.Encoding.Default;

            #region 图片加载到本地目录

            DateTime StarTime = DateTime.Now;            
            Console.WriteLine(StarTime);

            while (al_Fushi.Count > 0)
            {
                lock (objLock)
                {
                    if (al_Fushi.Count > 0)
                    {
                        string strFushi = al_Fushi[0].ToString();//②

                        try
                        {
                            resultString = Regex.Match(strFushi, "/T.*.160x160.jpg").Value.Replace("/", "");
                            if (File.Exists(@"Image\\" + resultString + ""))
                            {
                                //HTTPproc_1.DownloadFile(al_Product[i].ToString(), @"Image\\" + resultString + "");
                                al_Fushi.RemoveAt(0);//③
                            }
                            else
                            {
                                HTTPproc_1.DownloadFile(strFushi, @"Image\\" + resultString + "");
                                Console.WriteLine("{0}:正在下载的图片链接:{1}", Thread.CurrentThread.Name, resultString);
                                al_Fushi.RemoveAt(0);//③   
                                Thread.Sleep(1);
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            // Syntax error in the regular expression
                        }
                    }
                }
            }

            while (al_Jiaju.Count > 0)
            {
                lock (objLock)
                {
                    if (al_Jiaju.Count > 0)
                    {
                        string strJiaju = al_Jiaju[0].ToString();//②

                        try
                        {
                            resultString = Regex.Match(strJiaju, "/T.*.160x160.jpg").Value.Replace("/", "");
                            if (File.Exists(@"Image\\" + resultString + ""))
                            {
                                //HTTPproc_1.DownloadFile(al_Product[i].ToString(), @"Image\\" + resultString + "");
                                al_Jiaju.RemoveAt(0);//③
                            }
                            else
                            {
                                HTTPproc_1.DownloadFile(strJiaju, @"Image\\" + resultString + "");
                                Console.WriteLine("{0}:正在下载的图片链接:{1}", Thread.CurrentThread.Name, resultString);
                                al_Jiaju.RemoveAt(0);//③   
                                Thread.Sleep(1);
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            // Syntax error in the regular expression
                        }
                    }
                }
            }

            while (al_Meirong.Count > 0)
            {
                lock (objLock)
                {
                    if (al_Meirong.Count > 0)
                    {
                        string strMeirong = al_Meirong[0].ToString();//②

                        try
                        {
                            resultString = Regex.Match(strMeirong, "/T.*.160x160.jpg").Value.Replace("/", "");
                            if (File.Exists(@"Image\\" + resultString + ""))
                            {
                                //HTTPproc_1.DownloadFile(al_Product[i].ToString(), @"Image\\" + resultString + "");
                                al_Meirong.RemoveAt(0);//③
                            }
                            else
                            {
                                HTTPproc_1.DownloadFile(strMeirong, @"Image\\" + resultString + "");
                                Console.WriteLine("{0}:正在下载的图片链接:{1}", Thread.CurrentThread.Name, resultString);
                                al_Meirong.RemoveAt(0);//③   
                                Thread.Sleep(1);
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            // Syntax error in the regular expression
                        }
                    }
                }
            }

            while (al_Shipin.Count > 0)
            {
                lock (objLock)
                {
                    if (al_Shipin.Count > 0)
                    {
                        string strShipin = al_Shipin[0].ToString();//②

                        try
                        {
                            resultString = Regex.Match(strShipin, "/T.*.160x160.jpg").Value.Replace("/", "");
                            if (File.Exists(@"Image\\" + resultString + ""))
                            {
                                //HTTPproc_1.DownloadFile(al_Product[i].ToString(), @"Image\\" + resultString + "");
                                al_Shipin.RemoveAt(0);//③   
                            }
                            else
                            {
                                HTTPproc_1.DownloadFile(strShipin, @"Image\\" + resultString + "");
                                Console.WriteLine("{0}:正在下载的图片链接:{1}", Thread.CurrentThread.Name, resultString);
                                al_Shipin.RemoveAt(0);//③   
                                Thread.Sleep(1);
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            // Syntax error in the regular expression
                        }                        
                    }
                }
            }

            while (al_Dianqi.Count > 0)
            {
                lock (objLock)
                {
                    if (al_Dianqi.Count > 0)
                    {
                        string strDianqi = al_Dianqi[0].ToString();//②

                        try
                        {
                            resultString = Regex.Match(strDianqi, "/T.*.160x160.jpg").Value.Replace("/", "");
                            if (File.Exists(@"Image\\" + resultString + ""))
                            {
                                //HTTPproc_1.DownloadFile(al_Product[i].ToString(), @"Image\\" + resultString + "");
                                al_Dianqi.RemoveAt(0);//③  
                            }
                            else
                            {
                                HTTPproc_1.DownloadFile(strDianqi, @"Image\\" + resultString + "");
                                Console.WriteLine("{0}:正在下载的图片链接:{1}", Thread.CurrentThread.Name, resultString);
                                al_Dianqi.RemoveAt(0);//③   
                                Thread.Sleep(1);
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            // Syntax error in the regular expression
                        }
                    }
                }
            }

            threadTwo.Abort();
            lock (objLock)
            {
                Thread.Sleep(1);
                DateTime EndTime = DateTime.Now;
                Console.WriteLine(EndTime);
                Console.WriteLine(EndTime - StarTime);
            }

            #endregion 图片加载到本地目录
            
        }
    }

    class Program
    {
        static void Main()
        {
            //DateTime dt = DateTime.Now;
            //Console.WriteLine(CheckDisk());
            //Console.WriteLine(ServerIP());

            #region 练手
            //FTPClient ftp = new FTPClient("121.205.90.73", "", "cupid", "qweqwe123", 21);
            //Console.WriteLine(ftp.Dir("/").ToString());
            //foreach (string st in ftp.Dir(""))
            //{
            //    Console.WriteLine(st);
            //}
            //DirectoryInfo dir1 = new DirectoryInfo(@"F:\qweqwe\MatchXML");
            
            //Console.WriteLine("full name id : {0}", dir1.FullName);
            //Console.WriteLine("attributes are : {0}", dir1.Attributes.ToString());
            //Console.WriteLine("asdfasdfasdf : {0}", dir1.CreationTime);
            //string[] dirs = Directory.GetDirectories(@"D:\BestXBA\MatchXML");
            //string[] dirs = Directory.GetDirectories(@"F:\qweqwe\MatchXML");
            //foreach (string dir in dirs)
            //{
            //    DirectoryInfo dir2 = new DirectoryInfo(dir);
            //    Console.WriteLine(dir + "      " + dir2.LastWriteTime);
            //    string[] dirs1 = Directory.GetDirectories(dir);
            //    foreach (string dir1 in dirs1)
            //    {
            //        DirectoryInfo dir3 = new DirectoryInfo(dir1);
            //        if (dir3.LastWriteTime < dt.AddDays(-5))
            //        {
            //            Console.WriteLine(dir1 + "      " + dir3.LastWriteTime);
            //            try
            //            {
            //                FileDirectoryUtility.DeleteDirectory(dir1);
            //            }
            //            catch (Exception ex)
            //            {
            //                Console.WriteLine(ex);
            //            }
            //        }
            //    }
            //}
            
            //Console.WriteLine(dt.AddDays(-5));
            //Console.ReadKey();
            //Console.WriteLine(GetFootBallTurn());
            //Console.WriteLine(ReadXmlTextValue(Weather("qingdao"), "Date"));
            //Console.WriteLine(Weather("青岛"));
            //Console.WriteLine(Encoding.Default.GetBytes(Weather("青岛")).Length);
            //Console.WriteLine(PostModel("http://webservice.jtjc.cn/service/weather.asmx/GetWeather", "City=青岛"));
            //Console.WriteLine(GetModel("http://webservice.jtjc.cn/service/weather.asmx/GetWeather?", "City=青岛"));
            //string str = System.AppDomain.CurrentDomain.BaseDirectory;
            //Console.WriteLine(GetFootBallTurn());
            //CreateTxt(GetFootBallTurn());
            //CreateTxt(GetBasketBallTurn());
            //Console.ReadLine();
            //string strA = readSendUser();
            //Console.WriteLine(readSendUser());
            //Console.ReadLine();

            //Test.WebClient HTTPproc = new Test.WebClient();
            //HTTPproc.Encoding = System.Text.Encoding.UTF8; 
            ////Console.WriteLine(HTTPproc.OpenRead("http://webservice.webxml.com.cn/WebServices/WeatherWebService.asmx/getWeatherbyCityName", "theCityName=青岛"));
            //string sResultContents = HTTPproc.OpenRead("http://webservice.webxml.com.cn/WebServices/WeatherWebService.asmx/getWeatherbyCityName", "theCityName=青岛");
            //XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.LoadXml(sResultContents);
            //string xmlContents = xmlDoc.ChildNodes[1].ChildNodes[6].InnerText.ToString();
            //xmlContents += xmlDoc.ChildNodes[1].ChildNodes[10].InnerText.ToString();
            //int intIndex = xmlContents.IndexOf("；气压：");
            //xmlContents = xmlContents.Substring(0, intIndex);
            //Console.WriteLine(xmlContents);
            //Console.ReadKey();
            //Console.Write(); //成功接收返回true,timeout 返回false
            #endregion 练手

            #region 武林英雄后台数据采集
            //string strContent = "";
            //string[] server = { "官方服务器", "测试服务器", "迅雷分区", "小i分区", "快车分区", "嘟牛分区", "游戏网分区", "yeswan分区", "VS竞技分区", "薇拉分区", "Game2分区", "Game5分区", "PPLive分区", "酷我分区", "中资源分区", "风行分区", "暴风分区", "U9U8分区", "同学网分区", "赛迪分区", "圈网分区", "热酷分区", "亿玛分区", "17173分区", "维斯分区", "游艺分区", "酷狗分区", "VeryCD分区", "uusee分区", "91玩分区", "浩方分区", "PLU分区", "天涯分区", "51分区", "多玩分区", "IS语音分区", "昆仑分区", "百度分区", "PPS分区", "新浪分区", "盛大分区", "360分区", "叶网分区", "天成分区", "TOM分区", "汇元分区", "联众分区", "星空分区", "彗星分区", "哇赛分区", "游戏基地分区", "XBA分区" };
            //string[] content = { };

            ////Console.WriteLine(server[0].ToString());
            //Test.WebClient HTTPproc = new Test.WebClient();
            //HTTPproc.Encoding = System.Text.Encoding.UTF8;
            ////设置Cookies
            //HTTPproc.CookieContainer.Add(new Uri("http://stat.hero.9wee.com"), new Cookie("CWSSESSID", "0b5cd1574d9223d92532febad7f6f75a"));
            //HTTPproc.CookieContainer.Add(new Uri("http://stat.hero.9wee.com"), new Cookie("hero_username", "xba"));
            //HTTPproc.CookieContainer.Add(new Uri("http://stat.hero.9wee.com"), new Cookie("hero_key", "5255fd486f4fe28fceb125d9c8ec3563"));
            //HTTPproc.CookieContainer.Add(new Uri("http://stat.hero.9wee.com"), new Cookie("gm_server_url", "UmMCawM0BjhQf1A4AWkBZFA5ByhcbAs0UD9QJQBXAE9RQFTiAYxc3ga3AYxVvQF2BG1QZQAwUGdSPFF1UWQBPVI6AmsDJAZxUDVQfwE7ATFQcAc0XHgLdVBnUGYAIQBuUW5UagEqXDsGPAEiVTwBKQ=="));
            ////查询传参
            //strContent = HTTPproc.GetHtml("http://stat.hero.9wee.com/gm/modules/stat_global.php?server_group=官方服务器&startday=2010-05-25&search=查 询");

            ////strContent = HTTPproc.OpenRead("http://stat.hero.9wee.com/gm/modules/stat_global.php?server_group=迅雷分区&startday=2010-05-25&search=查 询");
            //Console.WriteLine(strContent);
            //if (strContent.IndexOf("<ERROR>") == 1)
            //{
            //    Console.Write("Error");
            //}
            //int i = 1;

            //try
            //{
            //    Regex regexObj = new Regex("left\">.*</td>");
            //    Match matchResults = regexObj.Match(strContent);
            //    while (matchResults.Success)
            //    {
            //        if (i % 3 == 0)
            //        {
            //            Console.WriteLine(ReplaceWords(matchResults.ToString()));

            //        }
            //        else
            //        {
            //            //content[0].Insert(0, "dsaf");
            //            Console.Write(ReplaceWords(matchResults.ToString()) + "|");
            //        }
            //        matchResults = matchResults.NextMatch();
            //        i++;
            //    }
            //    Console.WriteLine();
            //}
            //catch (ArgumentException ex)
            //{
            //    // Syntax error in the regular expression
            //    Console.WriteLine(ex);
            //}


            ////try
            ////{
            ////    Regex regexObj = new Regex(@"\d{0,4}年\d{0,2}月\d{0,2}日\s{0,5}\d{0,2}:\d{0,2}\s*\d{0,}\s<A\sHREF=""/\S*"">");
            ////    Match matchResults = regexObj.Match(strContent);
            ////    while (matchResults.Success)
            ////    {
            ////        // matched text: matchResults.Value
            ////        // match start: matchResults.Index
            ////        // match length: matchResults.Length
            ////        string[] splitArray = null;
            ////        try
            ////        {
            ////            splitArray = Regex.Split(ReplaceWords(Convert.ToString(matchResults)), @"\s{1,10}");
            ////            Console.WriteLine(splitArray[0].ToString() + "    " + splitArray[1].ToString() + "    " + splitArray[2].ToString() + "    " + splitArray[3].ToString());
            ////            //Console.WriteLine(splitArray[1].ToString());
            ////            //Console.WriteLine(splitArray[2].ToString());
            ////            //Console.WriteLine(splitArray[3].ToString());
            ////        }
            ////        catch (ArgumentException ex)
            ////        {
            ////            // Syntax error in the regular expression
            ////            Console.WriteLine(ex);
            ////        }
            ////        matchResults = matchResults.NextMatch();                    
            ////    }
            ////    Console.WriteLine();
            ////}
            ////catch (ArgumentException ex)
            ////{
            ////    // Syntax error in the regular expression
            ////    Console.WriteLine(ex);
            ////}
            ////HTTPproc.DownloadFile("http://121.205.90.78:8085/TT_db/NewBTP_db_201005190700.BAK.rar", @"c:\NewBTP_db_201005190700.BAK.rar");

            //DataTable dtt = new DataTable("Table_AX");
            //dtt.Columns.Add("column0", System.Type.GetType("System.String"));
            //dtt.Columns.Add("column1", System.Type.GetType("System.String"));
            //dtt.Columns.Add("column2", System.Type.GetType("System.String"));

            //DataRow dr = dtt.NewRow();
            //dr["column0"] = "AX";
            //dr["column1"] = true;
            //dr["column2"] = "Cupid";
            //dtt.Rows.Add(dr);
            //dr = dtt.NewRow();
            //dr["column0"] = "AXX";
            //dr["column1"] = true;
            //dr["column2"] = "Cupid1";
            //dtt.Rows.Add(dr);


            //Test.ExcelEdit ExcelEdit = new ExcelEdit();
            //ExcelEdit.CreateExcel();
            //ExcelEdit.CreateWorkSheet("Test");
            //ExcelEdit.WriteData(dtt, 1, 1);
            //ExcelEdit.Save();
            //ExcelEdit.Close();
            #endregion 武林英雄后台数据采集

            //ThreadLock tl = new ThreadLock();
            //tl.Start();
            DateTime dt = DateTime.Now;
            DateTime dt1 = Convert.ToDateTime("1985-6-16");
            Console.WriteLine(dt.Year - dt1.Year);

            Console.ReadKey();
        }

        private static string ReplaceWords(string strContent)
        {
            strContent = strContent.Replace("left\">", "").Replace("</td>", "");
            return strContent;
        }

        #region 获服务器磁盘空间信息
        /// <summary>
        /// 获服务器磁盘空间信息
        /// </summary>
        /// <returns></returns>
        private static string CheckDisk()
        {
            string result = "";
            //检测磁盘空间
            DriveInfo[] MyDrives = DriveInfo.GetDrives();

            foreach (DriveInfo MyDrive in MyDrives)
            {
                if (MyDrive.DriveType == DriveType.Fixed)
                {
                    result += MyDrive.Name + "----" + MyDrive.VolumeLabel +"----" + MyDrive.TotalSize / 1024 / 1024 + "----" + MyDrive.TotalFreeSpace / 1024 / 1024 + "----" + MyDrive.TotalFreeSpace * 100 / MyDrive.TotalSize + "|";
                }
            }
            return result.Substring(0, result.Length - 1);
        }
        #endregion 获服务器磁盘空间信息

        #region 获取服务器IP信息
        /// <summary>
        /// 获取服务器IP信息
        /// </summary>
        /// <returns></returns>
        private static string ServerIP()
        {
            IPAddress[] ServerIP = Dns.GetHostByName(Dns.GetHostName()).AddressList;
            return ServerIP[0].ToString();
        }
        #endregion 获取服务器IP信息

        #region 获取某地区天气信息插件
        /// <summary>
        /// 获取某地区天气信息插件
        /// </summary>
        /// <returns></returns>
        public static string Weather(string strCity)
        {
            string strContents = "";

            Test.WebClient HTTPproc = new Test.WebClient();
            HTTPproc.Encoding = System.Text.Encoding.UTF8;
            //string sResultContents = HTTPproc.OpenRead("http://webservice.webxml.com.cn/WebServices/WeatherWebService.asmx/getWeatherbyCityName", "theCityName="+strCity+"");
            XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.LoadXml(sResultContents);
            if (xmlDoc.ChildNodes.Count > 0)
            {
                strContents = xmlDoc.ChildNodes[1].ChildNodes[6].InnerText.ToString()+"，";
                strContents += xmlDoc.ChildNodes[1].ChildNodes[7].InnerText.ToString() + "，";
                strContents += xmlDoc.ChildNodes[1].ChildNodes[10].InnerText.ToString();
                //int intIndex = strContents.IndexOf("；气压：");
                //strContents = strContents.Substring(0, intIndex);
                return strContents;
            }
            else
            {
                strContents = "哎，又出错了!";
                return strContents;
            }
        }
        #endregion

        #region Google在线翻译  GoogleTranslate(string word)
        /// <summary>
        /// Google在线翻译
        /// </summary>
        /// <param name="word">查询单词</param>
        /// <returns></returns>
        public static string GoogleTranslate(string word)
        {
            string url = @"http://translate.google.cn/translate_t?langpair=en|zh-CN&text=" + word + "#";
            WebRequest req = WebRequest.Create(url);
            WebResponse res = req.GetResponse();
            Stream s = res.GetResponseStream();
            StreamReader sr = new StreamReader(s, Encoding.Default);
            char[] cs = new char[1024];
            string str = sr.ReadToEnd();
            int i = str.IndexOf("<div id=result_box dir=\"ltr\">");
            int j = str.IndexOf("</div>", i + 29);
            string result = str.Substring(i + 29, j - i - 29);
            return result;
        }
        #endregion

        public static string PostModel(string strUrl, string strParm)
        {
            Encoding encode = System.Text.Encoding.Default;

            byte[] arrB = encode.GetBytes(strParm);
            string strBaseUrl = null;

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(strUrl);
            myReq.Method = "POST";
            myReq.ContentType = "application/x-www-form-urlencoded";
            myReq.ContentLength = arrB.Length;
            Stream outStream = myReq.GetRequestStream();
            outStream.Write(arrB, 0, arrB.Length);
            outStream.Close();
            WebResponse myResp = null;
            try
            {
                //接收HTTP做出的响应
                myResp = myReq.GetResponse();
            }
            catch (Exception e)
            {
                int ii = 0;
            }
            string str = null;
            try
            {
                Stream ReceiveStream = myResp.GetResponseStream();
                StreamReader readStream = new StreamReader(ReceiveStream, System.Text.Encoding.GetEncoding("UTF-8"));
                Char[] read = new Char[256];
                int count = readStream.Read(read, 0, 256);
                
                while (count > 0)
                {
                    str += new String(read, 0, count);
                    count = readStream.Read(read, 0, 256);
                }
                readStream.Close();
                myResp.Close();
                return str;
            }
            catch
            {
                return str = "false";
            }
        }

        private static string GetModel(string strUrl, string strParm)
        {
            string strRet = null;
            try
            {
                Encoding encode = System.Text.Encoding.Default;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl + strParm);
                request.Timeout = 2000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                System.IO.Stream resStream = response.GetResponseStream();
                
                StreamReader readStream = new StreamReader(resStream, System.Text.Encoding.GetEncoding("UTF-8"));

                Char[] read = new Char[256];
                int count = readStream.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strRet = strRet + str;
                    count = readStream.Read(read, 0, 256);
                }

                resStream.Close();
            }
            catch (Exception e)
            {
                strRet = "";
                Console.WriteLine(e);
            }
            return strRet;
        }

        #region 获取足球经理游戏轮次
        /// <summary>
        /// 获取足球经理游戏轮次
        /// </summary>
        /// <returns></returns>
        public static string GetFootBallTurn()
        {
            StringBuilder sb = new StringBuilder("");
            string strSQL = "Exec maitiam_football.dbo.GetGameRow ";

            int intTurnFB1 = 0, intTurnFB2 = 0, intTurnASFB1 = 0, intTurnDWFB1 = 0, intTurn17173FB1 = 0, intTurn17173FB2 = 0, intTurn17173FB3 = 0;
            int intTurnU91 = 0, intTurnCCID1 = 0, intTurnQDNEWSF1 = 0, intTurn51WANF1 = 0, intTurnMSNF1 = 0, intTurnNBAF1 = 0, intTurnFB3 = 0;
            int intTurnDWFB2 = 0, intTurnCSL1 = 0, intTurnCSL2 = 0, intTurnCSL3 = 0, intTurn17173FB4 = 0;

            int intDaysFB1 = 0, intDaysFB2 = 0, intDaysASFB1 = 0, intDaysDWFB1 = 0, intDays17173FB1 = 0, intDays17173FB2 = 0, intDays17173FB3 = 0;
            int intDaysU91 = 0, intDaysCCID1 = 0, intDaysQDNEWSF1 = 0, intDays51WANF1 = 0, intDaysMSNF1 = 0, intDaysNBAF1 = 0, intDaysFB3 = 0;
            int intDaysDWFB2 = 0, intDaysCSL1 = 0, intDaysCSL2 = 0, intDaysCSL3 = 0, intDays17173FB4 = 0;

            int intStatusFB1 = 0, intStatusFB2 = 0, intStatusASFB1 = 0, intStatusDWFB1 = 0, intStatus17173FB1 = 0, intStatus17173FB2 = 0, intStatus17173FB3 = 0;
            int intStatusU91 = 0, intStatusCCID1 = 0, intStatusQDNEWSF1 = 0, intStatus51WANF1 = 0, intStatusMSNF1 = 0, intStatusNBAF1 = 0, intStatusFB3 = 0;
            int intStatusDWFB2 = 0, intStatusCSL1 = 0, intStatusCSL2 = 0, intStatusCSL3 = 0, intStatus17173FB4 = 0;

            bool intFinishFB1, intFinishFB2, intFinishASFB1, intFinishDWFB1, intFinish17173FB1, intFinish17173FB2, intFinish17173FB3;
            bool intFinishU91, intFinishCCID1, intFinishQDNEWSF1, intFinish51WANF1, intFinishMSNF1, intFinishNBAF1, intFinishFB3;
            bool intFinishDWFB2, intFinishCSL1, intFinishCSL2, intFinishCSL3, intFinish17173FB4;

            DataRow dr;

            sb.Append("FB1\n");
            try
            {
                string strcon1 = DBConnection.Get30GamePhoneConnString(1, "XBAF");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "XBAF"), CommandType.StoredProcedure, "GetGameRow");
                intTurnFB1 = (byte)dr["Turn"];
                intStatusFB1 = (byte)dr["Status"];
                intDaysFB1 = (byte)dr["Days"];
                intFinishFB1 = (bool)dr["IsFinish"];

                if (intTurnFB1 == 23)
                {
                    sb.Append("FB1第" + intDaysFB1 + "天\n");
                    sb.Append("FB1第 [" + intTurnFB1 + "]轮\n");
                    sb.Append("FB1游戏状态 " + intStatusFB1 + "\n");
                    sb.Append("FB1夜间更新状态 " + intFinishFB1 + "\n\n\n");
                }
                else
                {
                    sb.Append("FB1第 " + intDaysFB1 + "天\n");
                    sb.Append("FB1第 " + intTurnFB1 + "轮\n");
                    sb.Append("FB1游戏状态 " + intStatusFB1 + "\n");
                    sb.Append("FB1夜间更新状态 " + intFinishFB1 + "\n\n\n");
                }

            }
            catch
            {
                intTurnFB1 = 0;
                intStatusFB1 = 0;
                intDaysFB1 = 0;
                intFinishFB1 = false;
            }

            sb.Append("FB2\n");
            try
            {
                string strcon1 = DBConnection.Get30GamePhoneConnString(2, "XBAF");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "XBAF"), CommandType.StoredProcedure, "GetGameRow");
                intTurnFB2 = (byte)dr["Turn"];
                intStatusFB2 = (byte)dr["Status"];
                intDaysFB2 = (byte)dr["Days"];
                intFinishFB2 = (bool)dr["IsFinish"];

                if (intTurnFB2 == 23)
                {
                    sb.Append("FB2第" + intDaysFB2 + "天\n");
                    sb.Append("FB2第 [" + intTurnFB2 + "]轮\n");
                    sb.Append("FB2游戏状态 " + intStatusFB2 + "\n");
                    sb.Append("FB2夜间更新状态 " + intFinishFB2 + "\n\n\n");
                }
                else
                {
                    sb.Append("FB2第 " + intDaysFB2 + "天\n");
                    sb.Append("FB2第 " + intTurnFB2 + "轮\n");
                    sb.Append("FB2游戏状态 " + intStatusFB2 + "\n");
                    sb.Append("FB2夜间更新状态 " + intFinishFB2 + "\n\n\n");
                }

            }
            catch
            {
                intTurnFB2 = 0;
                intStatusFB2 = 0;
                intDaysFB2 = 0;
                intFinishFB2 = false;
            }

            sb.Append("FB3\n");
            try
            {
                string strcon1 = DBConnection.Get30GamePhoneConnString(3, "XBAF");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "XBAF"), CommandType.StoredProcedure, "GetGameRow");
                intTurnFB3 = (byte)dr["Turn"];
                intStatusFB3 = (byte)dr["Status"];
                intDaysFB3 = (byte)dr["Days"];
                intFinishFB3 = (bool)dr["IsFinish"];

                if (intTurnFB3 == 23)
                {
                    sb.Append("FB3第" + intDaysFB3 + "天\n");
                    sb.Append("FB3第 [" + intTurnFB3 + "]轮\n");
                    sb.Append("FB3游戏状态 " + intStatusFB3 + "\n");
                    sb.Append("FB3夜间更新状态 " + intFinishFB3 + "\n\n\n");
                }
                else
                {
                    sb.Append("FB3第 " + intDaysFB3 + "天\n");
                    sb.Append("FB3第 " + intTurnFB3 + "轮\n");
                    sb.Append("FB3游戏状态 " + intStatusFB3 + "\n");
                    sb.Append("FB3夜间更新状态 " + intFinishFB3 + "\n\n\n");
                }

            }
            catch
            {
                intTurnFB3 = 0;
                intStatusFB3 = 0;
                intDaysFB3 = 0;
                intFinishFB3 = false;
            }

            sb.Append("ASFB1\n");
            try
            {
                string strcon1 = DBConnection.Get30GamePhoneConnString(1, "ASF");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "ASF"), CommandType.StoredProcedure, "GetGameRow");
                intTurnASFB1 = (byte)dr["Turn"];
                intStatusASFB1 = (byte)dr["Status"];
                intDaysASFB1 = (byte)dr["Days"];
                intFinishASFB1 = (bool)dr["IsFinish"];

                if (intTurnASFB1 == 23)
                {
                    sb.Append("ASFB1第" + intDaysASFB1 + "天\n");
                    sb.Append("ASFB1第 [" + intTurnASFB1 + "]轮\n");
                    sb.Append("ASFB1游戏状态 " + intStatusASFB1 + "\n");
                    sb.Append("ASFB1夜间更新状态 " + intFinishASFB1 + "\n\n\n");
                }
                else
                {
                    sb.Append("ASFB1第 " + intDaysASFB1 + "天\n");
                    sb.Append("ASFB1第 " + intTurnASFB1 + "轮\n");
                    sb.Append("ASFB1游戏状态 " + intStatusASFB1 + "\n");
                    sb.Append("ASFB1夜间更新状态 " + intFinishASFB1 + "\n\n\n");
                }

            }
            catch
            {
                intTurnASFB1 = 0;
                intStatusASFB1 = 0;
                intDaysASFB1 = 0;
                intFinishASFB1 = false;
            }

            sb.Append("DWFB1\n");
            try
            {
                string strcon1 = DBConnection.Get30GamePhoneConnString(1, "DWF");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "DWF"), CommandType.StoredProcedure, "GetGameRow");
                intTurnDWFB1 = (byte)dr["Turn"];
                intStatusDWFB1 = (byte)dr["Status"];
                intDaysDWFB1 = (byte)dr["Days"];
                intFinishDWFB1 = (bool)dr["IsFinish"];

                if (intTurnDWFB1 == 23)
                {
                    sb.Append("DWFB1第" + intDaysDWFB1 + "天\n");
                    sb.Append("DWFB1第 [" + intTurnDWFB1 + "]轮\n");
                    sb.Append("DWFB1游戏状态 " + intStatusDWFB1 + "\n");
                    sb.Append("DWFB1夜间更新状态 " + intFinishDWFB1 + "\n\n\n");
                }
                else
                {
                    sb.Append("DWFB1第 " + intDaysDWFB1 + "天\n");
                    sb.Append("DWFB1第 " + intTurnDWFB1 + "轮\n");
                    sb.Append("DWFB1游戏状态 " + intStatusDWFB1 + "\n");
                    sb.Append("DWFB1夜间更新状态 " + intFinishDWFB1 + "\n\n\n");
                }

            }
            catch
            {
                intTurnDWFB1 = 0;
                intStatusDWFB1 = 0;
                intDaysDWFB1 = 0;
                intFinishDWFB1 = false;
            }

            sb.Append("DWFB2\n");
            try
            {
                string strcon1 = DBConnection.Get30GamePhoneConnString(2, "DWF");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "DWF"), CommandType.StoredProcedure, "GetGameRow");
                intTurnDWFB2 = (byte)dr["Turn"];
                intStatusDWFB2 = (byte)dr["Status"];
                intDaysDWFB2 = (byte)dr["Days"];
                intFinishDWFB2 = (bool)dr["IsFinish"];

                if (intTurnDWFB2 == 23)
                {
                    sb.Append("DWFB2第" + intDaysDWFB2 + "天\n");
                    sb.Append("DWFB2第 [" + intTurnDWFB2 + "]轮\n");
                    sb.Append("DWFB2游戏状态 " + intStatusDWFB2 + "\n");
                    sb.Append("DWFB2夜间更新状态 " + intFinishDWFB2 + "\n\n\n");
                }
                else
                {
                    sb.Append("DWFB2第 " + intDaysDWFB2 + "天\n");
                    sb.Append("DWFB2第 " + intTurnDWFB2 + "轮\n");
                    sb.Append("DWFB2游戏状态 " + intStatusDWFB2 + "\n");
                    sb.Append("DWFB2夜间更新状态 " + intFinishDWFB2 + "\n\n\n");
                }

            }
            catch
            {
                intTurnDWFB2 = 0;
                intStatusDWFB2 = 0;
                intDaysDWFB2 = 0;
                intFinishDWFB2 = false;
            }

            sb.Append("17173FB1\n");
            try
            {
                string strcon1 = DBConnection.Get30GamePhoneConnString(1, "17173F");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "17173F"), CommandType.StoredProcedure, "GetGameRow");
                intTurn17173FB1 = (byte)dr["Turn"];
                intStatus17173FB1 = (byte)dr["Status"];
                intDays17173FB1 = (byte)dr["Days"];
                intFinish17173FB1 = (bool)dr["IsFinish"];

                if (intTurn17173FB1 == 23)
                {
                    sb.Append("17173FB1第" + intDays17173FB1 + "天\n");
                    sb.Append("17173FB1第 [" + intTurn17173FB1 + "]轮\n");
                    sb.Append("17173FB1游戏状态 " + intStatus17173FB1 + "\n");
                    sb.Append("17173FB1夜间更新状态 " + intFinish17173FB1 + "\n\n\n");
                }
                else
                {
                    sb.Append("17173FB1第 " + intDays17173FB1 + "天\n");
                    sb.Append("17173FB1第 " + intTurn17173FB1 + "轮\n");
                    sb.Append("17173FB1游戏状态 " + intStatus17173FB1 + "\n");
                    sb.Append("17173FB1夜间更新状态 " + intFinish17173FB1 + "\n\n\n");
                }

            }
            catch
            {
                intTurn17173FB1 = 0;
                intStatus17173FB1 = 0;
                intDays17173FB1 = 0;
                intFinish17173FB1 = false;
            }

            sb.Append("17173FB2\n");
            try
            {
                string strcon1 = DBConnection.Get30GamePhoneConnString(2, "17173F");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "17173F"), CommandType.StoredProcedure, "GetGameRow");
                intTurn17173FB2 = (byte)dr["Turn"];
                intStatus17173FB2 = (byte)dr["Status"];
                intDays17173FB2 = (byte)dr["Days"];
                intFinish17173FB2 = (bool)dr["IsFinish"];

                if (intTurn17173FB2 == 23)
                {
                    sb.Append("17173FB2第" + intDays17173FB2 + "天\n");
                    sb.Append("17173FB2第 [" + intTurn17173FB2 + "]轮\n");
                    sb.Append("17173FB2游戏状态 " + intStatus17173FB2 + "\n");
                    sb.Append("17173FB2夜间更新状态 " + intFinish17173FB2 + "\n\n\n");
                }
                else
                {
                    sb.Append("17173FB2第 " + intDays17173FB2 + "天\n");
                    sb.Append("17173FB2第 " + intTurn17173FB2 + "轮\n");
                    sb.Append("17173FB2游戏状态 " + intStatus17173FB2 + "\n");
                    sb.Append("17173FB2夜间更新状态 " + intFinish17173FB2 + "\n\n\n");
                }

            }
            catch
            {
                intTurn17173FB2 = 0;
                intStatus17173FB2 = 0;
                intDays17173FB2 = 0;
                intFinish17173FB2 = false;
            }

            sb.Append("17173FB3\n");
            try
            {
                string strcon1 = DBConnection.Get30GamePhoneConnString(3, "17173F");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "17173F"), CommandType.StoredProcedure, "GetGameRow");
                intTurn17173FB3 = (byte)dr["Turn"];
                intStatus17173FB3 = (byte)dr["Status"];
                intDays17173FB3 = (byte)dr["Days"];
                intFinish17173FB3 = (bool)dr["IsFinish"];

                if (intTurn17173FB3 == 23)
                {
                    sb.Append("17173FB3第" + intDays17173FB3 + "天\n");
                    sb.Append("17173FB3第 [" + intTurn17173FB3 + "]轮\n");
                    sb.Append("17173FB3游戏状态 " + intStatus17173FB3 + "\n");
                    sb.Append("17173FB3夜间更新状态 " + intFinish17173FB3 + "\n\n\n");
                }
                else
                {
                    sb.Append("17173FB3第 " + intDays17173FB3 + "天\n");
                    sb.Append("17173FB3第 " + intTurn17173FB3 + "轮\n");
                    sb.Append("17173FB3游戏状态 " + intStatus17173FB3 + "\n");
                    sb.Append("17173FB3夜间更新状态 " + intFinish17173FB3 + "\n\n\n");
                }

            }
            catch
            {
                intTurn17173FB3 = 0;
                intStatus17173FB3 = 0;
                intDays17173FB3 = 0;
                intFinish17173FB3 = false;
            }

            sb.Append("17173FB4\n");
            try
            {
                string strcon1 = DBConnection.Get30GamePhoneConnString(4, "17173F");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(4, "17173F"), CommandType.StoredProcedure, "GetGameRow");
                intTurn17173FB4 = (byte)dr["Turn"];
                intStatus17173FB4 = (byte)dr["Status"];
                intDays17173FB4 = (byte)dr["Days"];
                intFinish17173FB4 = (bool)dr["IsFinish"];

                if (intTurn17173FB4 == 23)
                {
                    sb.Append("17173FB4第" + intDays17173FB4 + "天\n");
                    sb.Append("17173FB4第 [" + intTurn17173FB4 + "]轮\n");
                    sb.Append("17173FB4游戏状态 " + intStatus17173FB4 + "\n");
                    sb.Append("17173FB4夜间更新状态 " + intFinish17173FB4 + "\n\n\n");
                }
                else
                {
                    sb.Append("17173FB4第 " + intDays17173FB4 + "天\n");
                    sb.Append("17173FB4第 " + intTurn17173FB4 + "轮\n");
                    sb.Append("17173FB4游戏状态 " + intStatus17173FB4 + "\n");
                    sb.Append("17173FB4夜间更新状态 " + intFinish17173FB4 + "\n\n\n");
                }

            }
            catch
            {
                intTurn17173FB4 = 0;
                intStatus17173FB4 = 0;
                intDays17173FB4 = 0;
                intFinish17173FB4 = false;
            }

            sb.Append("51WANF1\n");
            try
            {
                string strcon1 = DBConnection.Get30GamePhoneConnString(1, "51WANF");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "51WANF"), CommandType.StoredProcedure, "GetGameRow");
                intTurn51WANF1 = (byte)dr["Turn"];
                intStatus51WANF1 = (byte)dr["Status"];
                intDays51WANF1 = (byte)dr["Days"];
                intFinish51WANF1 = (bool)dr["IsFinish"];

                if (intTurn51WANF1 == 23)
                {
                    sb.Append("51WANF1第" + intDays51WANF1 + "天\n");
                    sb.Append("51WANF1第 [" + intTurn51WANF1 + "]轮\n");
                    sb.Append("51WANF1游戏状态 " + intStatus51WANF1 + "\n");
                    sb.Append("51WANF1夜间更新状态 " + intFinish51WANF1 + "\n\n\n");
                }
                else
                {
                    sb.Append("51WANF1第 " + intDays51WANF1 + "天\n");
                    sb.Append("51WANF1第 " + intTurn51WANF1 + "轮\n");
                    sb.Append("51WANF1游戏状态 " + intStatus51WANF1 + "\n");
                    sb.Append("51WANF1夜间更新状态 " + intFinish51WANF1 + "\n\n\n");
                }

            }
            catch
            {
                intTurn51WANF1 = 0;
                intStatus51WANF1 = 0;
                intDays51WANF1 = 0;
                intFinish51WANF1 = false;
            }

            sb.Append("NBAF1\n");
            try
            {
                string strcon1 = DBConnection.Get30GamePhoneConnString(1, "NBAF");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "NBAF"), CommandType.StoredProcedure, "GetGameRow");
                intTurnNBAF1 = (byte)dr["Turn"];
                intStatusNBAF1 = (byte)dr["Status"];
                intDaysNBAF1 = (byte)dr["Days"];
                intFinishNBAF1 = (bool)dr["IsFinish"];

                if (intTurnNBAF1 == 23)
                {
                    sb.Append("NBAF1第" + intDaysNBAF1 + "天\n");
                    sb.Append("NBAF1第 [" + intTurnNBAF1 + "]轮\n");
                    sb.Append("NBAF1游戏状态 " + intStatusNBAF1 + "\n");
                    sb.Append("NBAF1夜间更新状态 " + intFinishNBAF1 + "\n\n\n");
                }
                else
                {
                    sb.Append("NBAF1第 " + intDaysNBAF1 + "天\n");
                    sb.Append("NBAF1第 " + intTurnNBAF1 + "轮\n");
                    sb.Append("NBAF1游戏状态 " + intStatusNBAF1 + "\n");
                    sb.Append("NBAF1夜间更新状态 " + intFinishNBAF1 + "\n\n\n");
                }

            }
            catch
            {
                intTurnNBAF1 = 0;
                intStatusNBAF1 = 0;
                intDaysNBAF1 = 0;
                intFinishNBAF1 = false;
            }

            sb.Append("CSL1\n");
            try
            {
                string strcon1 = DBConnection.Get30GamePhoneConnString(1, "SINAF");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "SINAF"), CommandType.StoredProcedure, "GetGameRow");
                intTurnCSL1 = (byte)dr["Turn"];
                intStatusCSL1 = (byte)dr["Status"];
                intDaysCSL1 = (byte)dr["Days"];
                intFinishCSL1 = (bool)dr["IsFinish"];

                if (intTurnCSL1 == 23)
                {
                    sb.Append("CSL1第" + intDaysCSL1 + "天\n");
                    sb.Append("CSL1第 [" + intTurnCSL1 + "]轮\n");
                    sb.Append("CSL1游戏状态 " + intStatusCSL1 + "\n");
                    sb.Append("CSL1夜间更新状态 " + intFinishCSL1 + "\n\n\n");
                }
                else
                {
                    sb.Append("CSL1第 " + intDaysCSL1 + "天\n");
                    sb.Append("CSL1第 " + intTurnCSL1 + "轮\n");
                    sb.Append("CSL1游戏状态 " + intStatusCSL1 + "\n");
                    sb.Append("CSL1夜间更新状态 " + intFinishCSL1 + "\n\n\n");
                }

            }
            catch
            {
                intTurnCSL1 = 0;
                intStatusCSL1 = 0;
                intDaysCSL1 = 0;
                intFinishCSL1 = false;
            }

            sb.Append("CSL2\n");
            try
            {
                string strcon1 = DBConnection.Get30GamePhoneConnString(2, "SINAF");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "SINAF"), CommandType.StoredProcedure, "GetGameRow");
                intTurnCSL2 = (byte)dr["Turn"];
                intStatusCSL2 = (byte)dr["Status"];
                intDaysCSL2 = (byte)dr["Days"];
                intFinishCSL2 = (bool)dr["IsFinish"];

                if (intTurnCSL2 == 23)
                {
                    sb.Append("CSL2第" + intDaysCSL2 + "天\n");
                    sb.Append("CSL2第 [" + intTurnCSL2 + "]轮\n");
                    sb.Append("CSL2游戏状态 " + intStatusCSL2 + "\n");
                    sb.Append("CSL2夜间更新状态 " + intFinishCSL2 + "\n\n\n");
                }
                else
                {
                    sb.Append("CSL2第 " + intDaysCSL2 + "天\n");
                    sb.Append("CSL2第 " + intTurnCSL2 + "轮\n");
                    sb.Append("CSL2游戏状态 " + intStatusCSL2 + "\n");
                    sb.Append("CSL2夜间更新状态 " + intFinishCSL2 + "\n\n\n");
                }

            }
            catch
            {
                intTurnCSL2 = 0;
                intStatusCSL2 = 0;
                intDaysCSL2 = 0;
                intFinishCSL2 = false;
            }

            sb.Append("CSL3\n");
            try
            {
                string strcon1 = DBConnection.Get30GamePhoneConnString(3, "SINAF");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "SINAF"), CommandType.StoredProcedure, "GetGameRow");
                intTurnCSL3 = (byte)dr["Turn"];
                intStatusCSL3 = (byte)dr["Status"];
                intDaysCSL3 = (byte)dr["Days"];
                intFinishCSL3 = (bool)dr["IsFinish"];

                if (intTurnCSL3 == 23)
                {
                    sb.Append("CSL3第" + intDaysCSL3 + "天\n");
                    sb.Append("CSL3第 [" + intTurnCSL3 + "]轮\n");
                    sb.Append("CSL3游戏状态 " + intStatusCSL3 + "\n");
                    sb.Append("CSL3夜间更新状态 " + intFinishCSL3 + "\n\n\n");
                }
                else
                {
                    sb.Append("CSL3第 " + intDaysCSL3 + "天\n");
                    sb.Append("CSL3第 " + intTurnCSL3 + "轮\n");
                    sb.Append("CSL3游戏状态 " + intStatusCSL3 + "\n");
                    sb.Append("CSL3夜间更新状态 " + intFinishCSL3 + "\n\n\n");
                }

            }
            catch
            {
                intTurnCSL3 = 0;
                intStatusCSL3 = 0;
                intDaysCSL3 = 0;
                intFinishCSL3 = false;
            }
            return sb.ToString();
        }
        #endregion

        #region 获取篮球经理游戏轮次
        /// <summary>
        /// 获取篮球经理游戏轮次
        /// </summary>
        /// <returns></returns>
        public static string GetBasketBallTurn()
        {
            StringBuilder sb = new StringBuilder("");

            string strSQL = "Exec NewBTP.dbo.GetGameRow ";
            int intTurnN1, intTurnS1, intTurnS2, intTurnD1, intTurnVIP1, intTurnCGA1, intTurnCGA2, intTurnCGA3, intTurnCGA4, intTurnHC1, intTurnAS1, intTurnCGC1, intTurnXL1, intTurnCCID1, intTurn5S1;
            int intStatusN1, intStatusS1, intStatusS2, intStatusD1, intStatusVIP1, intStatusCGA1, intStatusCGA2, intStatusCGA3, intStatusCGA4, intStatusHC1, intStatusAS1, intStatusCGC1, intStatusXL1, intStatusCCID1, intStatus5S1;

            int intTurn171731, intTurn171732, intTurn171733, intTurn171734, intTurn171735, intTurnTW1, intTurnTW2, intTurnNBA1, intTurnNBA2, intTurnNBA3, intTurnNBA4, intTurnNBA5, intTurnXiaoI;
            int intStatus171731, intStatus171732, intStatus171733, intStatus171734, intStatusTW1, intStatusTW2, intStatusNBA1, intStatusNBA2, intStatusNBA3, intStatusNBA4, intStatusNBA5, intStatusXiaoI;

            int intTurnMSN1, intTurnTOM1, intTurnKX1, intTurnNBA6, intTurnSOHU1, intTurn171736, intTurnXBA7, intTurnXBA8;
            int intStatusMSN1, intStatusTOM1, intStatusKX1, intStatusNBA6, intStatusSOHU1, intStatus171736, intStatusXBA7, intStatusXBA8;

            int intTurnDuNiu1, intStatusDuNiu1, intTurnDW1, intStatusDW1, intTurnS3, intStatusS3, intTurnS4, intStatusS4, intTurnSINA1, intStatusSINA1, intTurnSINA2, intStatusSINA2;
            int intTurn51Wan1, intStatus51Wan1, intTurn51Wan2, intStatus51Wan2, intTurn51Wan3, intStatus51Wan3, intTurn51Wan4, intStatus51Wan4, intTurnSINA3, intStatusSINA3, intStatus171735;

            int intOldTurnN1, intOldTurnS1, intOldTurnS2, intOldTurnS3, intOldTurnS4, intOldTurnD1, intOldTurnCGA1, intOldTurnCGA2, intOldTurnCGA3, intOldTurnCGA4, intOldTurn171731, intOldTurn171732, intOldTurn171733;
            int intOldTurnTW1, intOldTurn51Wan1, intOldTurn51Wan2, intOldTurn51Wan3, intOldTurn51Wan4, intOldTurnDuNiu1, intOldTurnDW1, intOldTurnTW2, intOldTurnHC1, intOldTurnXL1, intOldTurnVIP1, intOldTurnCCID1;

            int intOldTurnNBA1, intOldTurnNBA2, intOldTurnNBA3, intOldTurnNBA4, intOldTurnNBA5, intOldTurnSINA1, intOldTurnSINA2, intOldTurnXiaoI, intOldTurnAS1, intOldTurnCGC1, intOldTurn171734;
            int intOldTurnSINA3, intOldTurn171735, intOldTurnMSN1, intOldTurnTOM1, intOldTurnKX1, intOldTurnNBA6, intOldTurnSOHU1, intOldTurn171736, intOldTurnXBA7, intOldTurnXBA8, intOldTurn5S1;

            DataRow dr;

            sb.Append("北方服\n");
            try
            {
                string strcon1 = DBConnection.Get30GamePhoneConnString(1, "XBA");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "XBA"), CommandType.Text, strSQL);
                intTurnN1 = (int)dr["Turn"];
                intStatusN1 = (byte)dr["Status"];
                intOldTurnN1 = (int)dr["OldTurn"];

                if (intTurnN1 == 27)
                {
                    sb.Append("北方服上轮 ");
                    sb.Append(intOldTurnN1 + "\n");
                    sb.Append("北方服本轮 ");
                    sb.Append("[" + intTurnN1 + "]\n");
                    sb.Append("北方服游戏状态 ");
                    sb.Append(intStatusN1 + "\n\n\n");
                }
                else
                {
                    sb.Append("北方服上轮 ");
                    sb.Append(intOldTurnN1 + "\n");
                    sb.Append("北方服本轮 ");
                    sb.Append(intTurnN1 + "\n");
                    sb.Append("北方服游戏状态 ");
                    sb.Append(intStatusN1 + "\n\n\n");
                }

            }
            catch
            {
                intTurnN1 = 0;
                intStatusN1 = 0;
                intOldTurnN1 = 0;
            }

            sb.Append("南方服\n");
            try
            {
                string strcon1 = DBConnection.Get30GamePhoneConnString(1, "XBA");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "XBA"), CommandType.Text, strSQL);
                intTurnS1 = (int)dr["Turn"];
                intStatusS1 = (byte)dr["Status"];
                intOldTurnS1 = (int)dr["OldTurn"];

                if (intTurnS1 == 27)
                {
                    sb.Append("南方服上轮 ");
                    sb.Append(intOldTurnS1 + "\n");
                    sb.Append("南方服本轮 ");
                    sb.Append("[" + intTurnS1 + "]\n");
                    sb.Append("南方服游戏状态 ");
                    sb.Append(intStatusS1 + "\n\n\n");
                }
                else
                {
                    sb.Append("南方服上轮 ");
                    sb.Append(intOldTurnS1 + "\n");
                    sb.Append("南方服本轮 ");
                    sb.Append(intTurnS1 + "\n");
                    sb.Append("南方服游戏状态 ");
                    sb.Append(intStatusS1 + "\n\n\n");
                }
            }
            catch
            {
                intTurnS1 = 0;
                intStatusS1 = 0;
                intOldTurnS1 = 0;
            }

            sb.Append("火箭服\n");
            try
            {
                string strcon = DBConnection.Get30GamePhoneConnString(5, "XBA");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(5, "XBA"), CommandType.Text, strSQL);
                intTurnS3 = (int)dr["Turn"];
                intStatusS3 = (byte)dr["Status"];
                intOldTurnS3 = (int)dr["OldTurn"];

                if (intTurnS3 == 27)
                {
                    sb.Append("火箭服上轮 ");
                    sb.Append(intOldTurnS3 + "\n");
                    sb.Append("火箭服本轮 ");
                    sb.Append("[" + intTurnS3 + "]\n");
                    sb.Append("火箭服游戏状态 ");
                    sb.Append(intStatusS3 + "\n\n\n");
                }
                else
                {
                    sb.Append("火箭服上轮 ");
                    sb.Append(intOldTurnS3 + "\n");
                    sb.Append("火箭服本轮 ");
                    sb.Append(intTurnS3 + "\n");
                    sb.Append("火箭服游戏状态 ");
                    sb.Append(intStatusS3 + "\n\n\n");
                }

            }
            catch
            {
                intTurnS3 = 0;
                intStatusS3 = 0;
                intOldTurnS3 = 0;
            }

            sb.Append("湘北服\n");
            try
            {
                string strcon = DBConnection.Get30GamePhoneConnString(6, "XBA");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(6, "XBA"), CommandType.Text, strSQL);
                intTurnS4 = (int)dr["Turn"];
                intStatusS4 = (byte)dr["Status"];
                intOldTurnS4 = (int)dr["OldTurn"];

                if (intTurnS4 == 27)
                {
                    sb.Append("湘北服上轮 ");
                    sb.Append(intOldTurnS4 + "\n");
                    sb.Append("湘北服本轮 ");
                    sb.Append("[" + intTurnS4 + "]\n");
                    sb.Append("湘北服游戏状态 ");
                    sb.Append(intStatusS4 + "\n\n\n");
                }
                else
                {
                    sb.Append("湘北服上轮 ");
                    sb.Append(intOldTurnS4 + "\n");
                    sb.Append("湘北服本轮 ");
                    sb.Append(intTurnS4 + "\n");
                    sb.Append("湘北服游戏状态 ");
                    sb.Append(intStatusS4 + "\n\n\n");
                }

            }
            catch
            {
                intTurnS4 = 0;
                intStatusS4 = 0;
                intOldTurnS4 = 0;
            }

            sb.Append("湖人服\n");
            try
            {
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(7, "XBA"), CommandType.Text, strSQL);
                intTurnXBA7 = (int)dr["Turn"];
                intStatusXBA7 = (byte)dr["Status"];
                intOldTurnXBA7 = (int)dr["OldTurn"];

                if (intTurnXBA7 == 27)
                {
                    sb.Append("湖人服上轮 ");
                    sb.Append(intOldTurnXBA7 + "\n");
                    sb.Append("湖人服本轮 ");
                    sb.Append("[" + intTurnXBA7 + "]\n");
                    sb.Append("湖人服游戏状态 ");
                    sb.Append(intStatusXBA7 + "\n\n\n");
                }
                else
                {
                    sb.Append("湖人服上轮 ");
                    sb.Append(intOldTurnXBA7 + "\n");
                    sb.Append("湖人服本轮 ");
                    sb.Append(intTurnXBA7 + "\n");
                    sb.Append("湖人服游戏状态 ");
                    sb.Append(intStatusXBA7 + "\n\n\n");
                }

            }
            catch
            {
                intTurnXBA7 = 0;
                intStatusXBA7 = 0;
                intOldTurnXBA7 = 0;
            }

            sb.Append("欢城服\n");
            try
            {
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(50, "XBA"), CommandType.Text, strSQL);
                intTurnHC1 = (int)dr["Turn"];
                intStatusHC1 = (byte)dr["Status"];
                intOldTurnHC1 = (int)dr["OldTurn"];

                if (intTurnHC1 == 27)
                {
                    sb.Append("欢城服上轮 ");
                    sb.Append(intOldTurnHC1 + "\n");
                    sb.Append("欢城服本轮 ");
                    sb.Append("[" + intTurnHC1 + "]\n");
                    sb.Append("欢城服游戏状态 ");
                    sb.Append(intStatusHC1 + "\n\n\n");
                }
                else
                {
                    sb.Append("欢城服上轮 ");
                    sb.Append(intOldTurnHC1 + "\n");
                    sb.Append("欢城服本轮 ");
                    sb.Append(intTurnHC1 + "\n");
                    sb.Append("欢城服游戏状态 ");
                    sb.Append(intStatusHC1 + "\n\n\n");
                }

            }
            catch
            {
                intTurnHC1 = 0;
                intStatusHC1 = 0;
                intOldTurnHC1 = 0;
            }

            sb.Append("岭南服\n");
            try
            {
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(8, "XBA"), CommandType.Text, strSQL);
                intTurnXBA8 = (int)dr["Turn"];
                intStatusXBA8 = (byte)dr["Status"];
                intOldTurnXBA8 = (int)dr["OldTurn"];

                if (intTurnXBA8 == 27)
                {
                    sb.Append("岭南服上轮 ");
                    sb.Append(intOldTurnXBA8 + "\n");
                    sb.Append("岭南服本轮 ");
                    sb.Append("[" + intTurnXBA8 + "]\n");
                    sb.Append("岭南服游戏状态 ");
                    sb.Append(intStatusXBA8 + "\n\n\n");
                }
                else
                {
                    sb.Append("岭南服上轮 ");
                    sb.Append(intOldTurnXBA8 + "\n");
                    sb.Append("岭南服本轮 ");
                    sb.Append(intTurnXBA8 + "\n");
                    sb.Append("岭南服游戏状态 ");
                    sb.Append(intStatusXBA8 + "\n\n\n");
                }

            }
            catch
            {
                intTurnXBA8 = 0;
                intStatusXBA8 = 0;
                intOldTurnXBA8 = 0;
            }

            sb.Append("CGA1\n");
            try
            {
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "CGA"), CommandType.Text, strSQL);
                intTurnCGA1 = (int)dr["Turn"];
                intStatusCGA1 = (byte)dr["Status"];
                intOldTurnCGA1 = (int)dr["OldTurn"];

                if (intTurnCGA1 == 27)
                {
                    sb.Append("CGA1上轮 ");
                    sb.Append(intOldTurnCGA1 + "\n");
                    sb.Append("CGA1本轮 ");
                    sb.Append("[" + intTurnCGA1 + "]\n");
                    sb.Append("CGA1游戏状态 ");
                    sb.Append(intStatusCGA1 + "\n\n\n");
                }
                else
                {
                    sb.Append("CGA1上轮 ");
                    sb.Append(intOldTurnCGA1 + "\n");
                    sb.Append("CGA1本轮 ");
                    sb.Append(intTurnCGA1 + "\n");
                    sb.Append("CGA1游戏状态 ");
                    sb.Append(intStatusCGA1 + "\n\n\n");
                }

            }
            catch
            {
                intTurnCGA1 = 0;
                intStatusCGA1 = 0;
                intOldTurnCGA1 = 0;
            }

            sb.Append("CGA3\n");
            try
            {
                string strcon = DBConnection.Get30GamePhoneConnString(3, "CGA");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "CGA"), CommandType.Text, strSQL);
                intTurnCGA3 = (int)dr["Turn"];
                intStatusCGA3 = (byte)dr["Status"];
                intOldTurnCGA3 = (int)dr["OldTurn"];

                if (intTurnCGA3 == 27)
                {
                    sb.Append("CGA3上轮 ");
                    sb.Append(intOldTurnCGA3 + "\n");
                    sb.Append("CGA3本轮 ");
                    sb.Append("[" + intTurnCGA3 + "]\n");
                    sb.Append("CGA3游戏状态 ");
                    sb.Append(intStatusCGA3 + "\n\n\n");
                }
                else
                {
                    sb.Append("CGA3上轮 ");
                    sb.Append(intOldTurnCGA3 + "\n");
                    sb.Append("CGA3本轮 ");
                    sb.Append(intTurnCGA3 + "\n");
                    sb.Append("CGA3游戏状态 ");
                    sb.Append(intStatusCGA3 + "\n\n\n");
                }

            }
            catch
            {
                intTurnCGA3 = 0;
                intStatusCGA3 = 0;
                intOldTurnCGA3 = 0;
            }

            sb.Append("171731\n");
            try
            {
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "17173"), CommandType.Text, strSQL);
                intTurn171731 = (int)dr["Turn"];
                intStatus171731 = (byte)dr["Status"];
                intOldTurn171731 = (int)dr["OldTurn"];

                if (intTurn171731 == 27)
                {
                    sb.Append("171731上轮 ");
                    sb.Append(intOldTurn171731 + "\n");
                    sb.Append("171731本轮 ");
                    sb.Append("[" + intTurn171731 + "]\n");
                    sb.Append("171731游戏状态 ");
                    sb.Append(intStatus171731 + "\n\n\n");
                }
                else
                {
                    sb.Append("171731上轮 ");
                    sb.Append(intOldTurn171731 + "\n");
                    sb.Append("171731本轮 ");
                    sb.Append(intTurn171731 + "\n");
                    sb.Append("171731游戏状态 ");
                    sb.Append(intStatus171731 + "\n\n\n");
                }

            }
            catch
            {
                intTurn171731 = 0;
                intStatus171731 = 0;
                intOldTurn171731 = 0;
            }

            sb.Append("171733\n");
            try
            {
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "17173"), CommandType.Text, strSQL);
                intTurn171733 = (int)dr["Turn"];
                intStatus171733 = (byte)dr["Status"];
                intOldTurn171733 = (int)dr["OldTurn"];

                if (intTurn171733 == 27)
                {
                    sb.Append("171733上轮 ");
                    sb.Append(intOldTurn171733 + "\n");
                    sb.Append("171733本轮 ");
                    sb.Append("[" + intTurn171733 + "]\n");
                    sb.Append("171733游戏状态 ");
                    sb.Append(intStatus171733 + "\n\n\n");
                }
                else
                {
                    sb.Append("171733上轮 ");
                    sb.Append(intOldTurn171733 + "\n");
                    sb.Append("171733本轮 ");
                    sb.Append(intTurn171733 + "\n");
                    sb.Append("171733游戏状态 ");
                    sb.Append(intStatus171733 + "\n\n\n");
                }

            }
            catch
            {
                intTurn171733 = 0;
                intStatus171733 = 0;
                intOldTurn171733 = 0;
            }

            sb.Append("171735\n");
            try
            {
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(5, "17173"), CommandType.Text, strSQL);
                intTurn171735 = (int)dr["Turn"];
                intStatus171735 = (byte)dr["Status"];
                intOldTurn171735 = (int)dr["OldTurn"];

                if (intTurn171735 == 27)
                {
                    sb.Append("171735上轮 ");
                    sb.Append(intOldTurn171735 + "\n");
                    sb.Append("171735本轮 ");
                    sb.Append("[" + intTurn171735 + "]\n");
                    sb.Append("171735游戏状态 ");
                    sb.Append(intStatus171735 + "\n\n\n");
                }
                else
                {
                    sb.Append("171735上轮 ");
                    sb.Append(intOldTurn171735 + "\n");
                    sb.Append("171735本轮 ");
                    sb.Append(intTurn171735 + "\n");
                    sb.Append("171735游戏状态 ");
                    sb.Append(intStatus171735 + "\n\n\n");
                }

            }
            catch
            {
                intTurn171735 = 0;
                intStatus171735 = 0;
                intOldTurn171735 = 0;
            }

            sb.Append("171736\n");
            try
            {
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(6, "17173"), CommandType.Text, strSQL);
                intTurn171736 = (int)dr["Turn"];
                intStatus171736 = (byte)dr["Status"];
                intOldTurn171736 = (int)dr["OldTurn"];

                if (intTurn171736 == 27)
                {
                    sb.Append("171736上轮 ");
                    sb.Append(intOldTurn171736 + "\n");
                    sb.Append("171736本轮 ");
                    sb.Append("[" + intTurn171736 + "]\n");
                    sb.Append("171736游戏状态 ");
                    sb.Append(intStatus171736 + "\n\n\n");
                }
                else
                {
                    sb.Append("171736上轮 ");
                    sb.Append(intOldTurn171736 + "\n");
                    sb.Append("171736本轮 ");
                    sb.Append(intTurn171736 + "\n");
                    sb.Append("171736游戏状态 ");
                    sb.Append(intStatus171736 + "\n\n\n");
                }

            }
            catch
            {
                intTurn171736 = 0;
                intStatus171736 = 0;
                intOldTurn171736 = 0;
            }

            sb.Append("TW1\n");
            try
            {
                string strcon = DBConnection.Get30GamePhoneConnString(1, "TW");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "TW"), CommandType.Text, strSQL);
                intTurnTW1 = (int)dr["Turn"];
                intStatusTW1 = (byte)dr["Status"];
                //intOldTurnTW1 = (int)dr["OldTurn"];

                if (intTurnTW1 == 27)
                {
                    sb.Append("TW1本轮 ");
                    sb.Append("[" + intTurnTW1 + "]\n");
                    sb.Append("TW1游戏状态 ");
                    sb.Append(intStatusTW1 + "\n\n\n");
                }
                else
                {
                    sb.Append("TW1本轮 ");
                    sb.Append(intTurnTW1 + "\n");
                    sb.Append("TW1游戏状态 ");
                    sb.Append(intStatusTW1 + "\n\n\n");
                }
            }
            catch
            {
                intTurnTW1 = 0;
                intStatusTW1 = 0;
                //intOldTurnTW1 = 0;
            }

            sb.Append("TW2\n");
            try
            {
                string strcon = DBConnection.Get30GamePhoneConnString(2, "TW");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "TW"), CommandType.Text, strSQL);
                intTurnTW2 = (int)dr["Turn"];
                intStatusTW2 = (byte)dr["Status"];
                //intOldTurnTW2 = (int)dr["OldTurn"];

                if (intTurnTW2 == 27)
                {
                    sb.Append("TW2本轮 ");
                    sb.Append("[" + intTurnTW2 + "]\n");
                    sb.Append("TW2游戏状态 ");
                    sb.Append(intStatusTW2 + "\n\n\n");
                }
                else
                {
                    sb.Append("TW2本轮 ");
                    sb.Append(intTurnTW2 + "\n");
                    sb.Append("TW2游戏状态 ");
                    sb.Append(intStatusTW2 + "\n\n\n");
                }

            }
            catch
            {
                intTurnTW2 = 0;
                intStatusTW2 = 0;
                //intOldTurnTW1 = 0;
            }

            sb.Append("51WAN3\n");
            try
            {
                string strcon = DBConnection.Get30GamePhoneConnString(3, "51WAN");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "51WAN"), CommandType.Text, strSQL);
                intTurn51Wan3 = (int)dr["Turn"];
                intStatus51Wan3 = (byte)dr["Status"];
                intOldTurn51Wan3 = (int)dr["OldTurn"];

                if (intTurn51Wan3 == 27)
                {
                    sb.Append("51WAN3上轮 ");
                    sb.Append(intOldTurn51Wan3 + "\n");
                    sb.Append("51WAN3本轮 ");
                    sb.Append("[" + intTurn51Wan3 + "]\n");
                    sb.Append("51WAN3游戏状态 ");
                    sb.Append(intStatus51Wan3 + "\n\n\n");
                }
                else
                {
                    sb.Append("51WAN3上轮 ");
                    sb.Append(intOldTurn51Wan3 + "\n");
                    sb.Append("51WAN3本轮 ");
                    sb.Append(intTurn51Wan3 + "\n");
                    sb.Append("51WAN3游戏状态 ");
                    sb.Append(intStatus51Wan3 + "\n\n\n");
                }

            }
            catch
            {
                intTurn51Wan3 = 0;
                intStatus51Wan3 = 0;
                intOldTurn51Wan3 = 0;
            }

            sb.Append("51Wan4\n");
            try
            {
                string strcon = DBConnection.Get30GamePhoneConnString(4, "51WAN");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(4, "51WAN"), CommandType.Text, strSQL);
                intTurn51Wan4 = (int)dr["Turn"];
                intStatus51Wan4 = (byte)dr["Status"];
                intOldTurn51Wan4 = (int)dr["OldTurn"];

                if (intTurn51Wan4 == 27)
                {
                    sb.Append("51Wan4上轮 ");
                    sb.Append(intOldTurn51Wan4 + "\n");
                    sb.Append("51Wan4本轮 ");
                    sb.Append("[" + intTurn51Wan4 + "]\n");
                    sb.Append("51Wan4游戏状态 ");
                    sb.Append(intStatus51Wan4 + "\n\n\n");
                }
                else
                {
                    sb.Append("51Wan4上轮 ");
                    sb.Append(intOldTurn51Wan4 + "\n");
                    sb.Append("51Wan4本轮 ");
                    sb.Append(intTurn51Wan4 + "\n");
                    sb.Append("51Wan4游戏状态 ");
                    sb.Append(intStatus51Wan4 + "\n\n\n");
                }

            }
            catch
            {
                intTurn51Wan4 = 0;
                intStatus51Wan4 = 0;
                intOldTurn51Wan4 = 0;
            }

            sb.Append("DuoWan1\n");
            try
            {
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "DW"), CommandType.Text, strSQL);
                intTurnDW1 = (int)dr["Turn"];
                intStatusDW1 = (byte)dr["Status"];
                intOldTurnDW1 = (int)dr["OldTurn"];

                if (intTurnDW1 == 27)
                {
                    sb.Append("DuoWan1上轮 ");
                    sb.Append(intOldTurnDW1 + "\n");
                    sb.Append("DuoWan1本轮 ");
                    sb.Append("[" + intTurnDW1 + "]\n");
                    sb.Append("DuoWan1游戏状态 ");
                    sb.Append(intStatusDW1 + "\n\n\n");
                }
                else
                {
                    sb.Append("DuoWan1上轮 ");
                    sb.Append(intOldTurnDW1 + "\n");
                    sb.Append("DuoWan1本轮 ");
                    sb.Append(intTurnDW1 + "\n");
                    sb.Append("DuoWan1游戏状态 ");
                    sb.Append(intStatusDW1 + "\n\n\n");
                }

            }
            catch
            {
                intTurnDW1 = 0;
                intStatusDW1 = 0;
                intOldTurnDW1 = 0;
            }

            sb.Append("NBA1\n");
            try
            {
                string strcon = DBConnection.Get30GamePhoneConnString(1, "NBA");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "NBA"), CommandType.Text, strSQL);
                intTurnNBA1 = (int)dr["Turn"];
                intStatusNBA1 = (byte)dr["Status"];
                intOldTurnNBA1 = (int)dr["OldTurn"];

                if (intTurnNBA1 == 27)
                {
                    sb.Append("NBA1上轮 ");
                    sb.Append(intOldTurnNBA1 + "\n");
                    sb.Append("NBA1本轮 ");
                    sb.Append("[" + intTurnNBA1 + "]\n");
                    sb.Append("NBA1游戏状态 ");
                    sb.Append(intStatusNBA1 + "\n\n\n");
                }
                else
                {
                    sb.Append("NBA1上轮 ");
                    sb.Append(intOldTurnNBA1 + "\n");
                    sb.Append("NBA1本轮 ");
                    sb.Append(intTurnNBA1 + "\n");
                    sb.Append("NBA1游戏状态 ");
                    sb.Append(intStatusNBA1 + "\n\n\n");
                }

            }
            catch
            {
                intTurnNBA1 = 0;
                intStatusNBA1 = 0;
                intOldTurnNBA1 = 0;
            }

            sb.Append("NBA2\n");
            try
            {
                string strcon = DBConnection.Get30GamePhoneConnString(2, "NBA");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "NBA"), CommandType.Text, strSQL);
                intTurnNBA2 = (int)dr["Turn"];
                intStatusNBA2 = (byte)dr["Status"];
                intOldTurnNBA2 = (int)dr["OldTurn"];

                if (intTurnNBA2 == 27)
                {
                    sb.Append("NBA2上轮 ");
                    sb.Append(intOldTurnNBA2 + "\n");
                    sb.Append("NBA2本轮 ");
                    sb.Append("[" + intTurnNBA2 + "]\n");
                    sb.Append("NBA2游戏状态 ");
                    sb.Append(intStatusNBA2 + "\n\n\n");
                }
                else
                {
                    sb.Append("NBA2上轮 ");
                    sb.Append(intOldTurnNBA2 + "\n");
                    sb.Append("NBA2本轮 ");
                    sb.Append(intTurnNBA2 + "\n");
                    sb.Append("NBA2游戏状态 ");
                    sb.Append(intStatusNBA2 + "\n\n\n");
                }

            }
            catch
            {
                intTurnNBA2 = 0;
                intStatusNBA2 = 0;
                intOldTurnNBA2 = 0;
            }

            sb.Append("NBA5\n");
            try
            {
                string strcon = DBConnection.Get30GamePhoneConnString(5, "NBA");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(5, "NBA"), CommandType.Text, strSQL);
                intTurnNBA5 = (int)dr["Turn"];
                intStatusNBA5 = (byte)dr["Status"];
                intOldTurnNBA5 = (int)dr["OldTurn"];

                if (intTurnNBA5 == 27)
                {
                    sb.Append("NBA5上轮 ");
                    sb.Append(intOldTurnNBA5 + "\n");
                    sb.Append("NBA5本轮 ");
                    sb.Append("[" + intTurnNBA5 + "]\n");
                    sb.Append("NBA5游戏状态 ");
                    sb.Append(intStatusNBA5 + "\n\n\n");
                }
                else
                {
                    sb.Append("NBA5上轮 ");
                    sb.Append(intOldTurnNBA5 + "\n");
                    sb.Append("NBA5本轮 ");
                    sb.Append(intTurnNBA5 + "\n");
                    sb.Append("NBA5游戏状态 ");
                    sb.Append(intStatusNBA5 + "\n\n\n");
                }

            }
            catch
            {
                intTurnNBA5 = 0;
                intStatusNBA5 = 0;
                intOldTurnNBA5 = 0;
            }

            sb.Append("NBA6\n");
            try
            {
                string strcon = DBConnection.Get30GamePhoneConnString(6, "NBA");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(6, "NBA"), CommandType.Text, strSQL);
                intTurnNBA6 = (int)dr["Turn"];
                intStatusNBA6 = (byte)dr["Status"];
                intOldTurnNBA6 = (int)dr["OldTurn"];

                if (intTurnNBA6 == 27)
                {
                    sb.Append("NBA6上轮 ");
                    sb.Append(intOldTurnNBA6 + "\n");
                    sb.Append("NBA6本轮 ");
                    sb.Append("[" + intTurnNBA6 + "]\n");
                    sb.Append("NBA6游戏状态 ");
                    sb.Append(intStatusNBA6 + "\n\n\n");
                }
                else
                {
                    sb.Append("NBA6上轮 ");
                    sb.Append(intOldTurnNBA6 + "\n");
                    sb.Append("NBA6本轮 ");
                    sb.Append(intTurnNBA6 + "\n");
                    sb.Append("NBA6游戏状态 ");
                    sb.Append(intStatusNBA6 + "\n\n\n");
                }

            }
            catch
            {
                intTurnNBA6 = 0;
                intStatusNBA6 = 0;
                intOldTurnNBA6 = 0;
            }

            sb.Append("SINA1\n");
            try
            {
                string strcon = DBConnection.Get30GamePhoneConnString(1, "SINA");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "SINA"), CommandType.Text, strSQL);
                intTurnSINA1 = (int)dr["Turn"];
                intStatusSINA1 = (byte)dr["Status"];
                intOldTurnSINA1 = (int)dr["OldTurn"];

                if (intTurnSINA1 == 27)
                {
                    sb.Append("SINA1上轮 ");
                    sb.Append(intOldTurnSINA1 + "\n");
                    sb.Append("SINA1本轮 ");
                    sb.Append("[" + intTurnSINA1 + "]\n");
                    sb.Append("SINA1游戏状态 ");
                    sb.Append(intStatusSINA1 + "\n\n\n");
                }
                else
                {
                    sb.Append("SINA1上轮 ");
                    sb.Append(intOldTurnSINA1 + "\n");
                    sb.Append("SINA1本轮 ");
                    sb.Append(intTurnSINA1 + "\n");
                    sb.Append("SINA1游戏状态 ");
                    sb.Append(intStatusSINA1 + "\n\n\n");
                }

            }
            catch
            {
                intTurnSINA1 = 0;
                intStatusSINA1 = 0;
                intOldTurnSINA1 = 0;
            }

            sb.Append("SINA3\n");
            try
            {
                string strcon = DBConnection.Get30GamePhoneConnString(3, "SINA");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "SINA"), CommandType.Text, strSQL);
                intTurnSINA3 = (int)dr["Turn"];
                intStatusSINA3 = (byte)dr["Status"];
                intOldTurnSINA3 = (int)dr["OldTurn"];

                if (intTurnSINA3 == 27)
                {
                    sb.Append("SINA3上轮 ");
                    sb.Append(intOldTurnSINA3 + "\n");
                    sb.Append("SINA3本轮 ");
                    sb.Append("[" + intTurnSINA3 + "]\n");
                    sb.Append("SINA3游戏状态 ");
                    sb.Append(intStatusSINA3 + "\n\n\n");
                }
                else
                {
                    sb.Append("SINA3上轮 ");
                    sb.Append(intOldTurnSINA3 + "\n");
                    sb.Append("SINA3本轮 ");
                    sb.Append(intTurnSINA3 + "\n");
                    sb.Append("SINA3游戏状态 ");
                    sb.Append(intStatusSINA3 + "\n\n\n");
                }

            }
            catch
            {
                intTurnSINA3 = 0;
                intStatusSINA3 = 0;
                intOldTurnSINA3 = 0;
            }

            sb.Append("XiaoI\n");
            try
            {
                string strcon = DBConnection.Get30GamePhoneConnString(1, "MSN");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "MSN"), CommandType.Text, strSQL);
                intTurnXiaoI = (int)dr["Turn"];
                intStatusXiaoI = (byte)dr["Status"];
                intOldTurnXiaoI = (int)dr["OldTurn"];

                if (intTurnXiaoI == 27)
                {
                    sb.Append("XiaoI上轮 ");
                    sb.Append(intOldTurnXiaoI + "\n");
                    sb.Append("XiaoI本轮 ");
                    sb.Append("[" + intTurnXiaoI + "]\n");
                    sb.Append("XiaoI游戏状态 ");
                    sb.Append(intStatusXiaoI + "\n\n\n");
                }
                else
                {
                    sb.Append("XiaoI上轮 ");
                    sb.Append(intOldTurnXiaoI + "\n");
                    sb.Append("XiaoI本轮 ");
                    sb.Append(intTurnXiaoI + "\n");
                    sb.Append("XiaoI游戏状态 ");
                    sb.Append(intStatusXiaoI + "\n\n\n");
                }

            }
            catch
            {
                intTurnXiaoI = 0;
                intStatusXiaoI = 0;
                intOldTurnXiaoI = 0;
            }

            sb.Append("AS1\n");
            try
            {
                string strcon = DBConnection.Get30GamePhoneConnString(1, "AS");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "AS"), CommandType.Text, strSQL);
                intTurnAS1 = (int)dr["Turn"];
                intStatusAS1 = (byte)dr["Status"];
                intOldTurnAS1 = (int)dr["OldTurn"];

                if (intTurnAS1 == 27)
                {
                    sb.Append("AS1上轮 ");
                    sb.Append(intOldTurnAS1 + "\n");
                    sb.Append("AS1本轮 ");
                    sb.Append("[" + intTurnAS1 + "]\n");
                    sb.Append("AS1游戏状态 ");
                    sb.Append(intStatusAS1 + "\n\n\n");
                }
                else
                {
                    sb.Append("AS1上轮 ");
                    sb.Append(intOldTurnAS1 + "\n");
                    sb.Append("AS1本轮 ");
                    sb.Append(intTurnAS1 + "\n");
                    sb.Append("AS1游戏状态 ");
                    sb.Append(intStatusAS1 + "\n\n\n");
                }

            }
            catch
            {
                intTurnAS1 = 0;
                intStatusAS1 = 0;
                intOldTurnAS1 = 0;
            }

            sb.Append("CGC1\n");
            try
            {
                string strcon = DBConnection.Get30GamePhoneConnString(1, "CGC");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "CGC"), CommandType.Text, strSQL);
                intTurnCGC1 = (int)dr["Turn"];
                intStatusCGC1 = (byte)dr["Status"];
                intOldTurnCGC1 = (int)dr["OldTurn"];

                if (intTurnCGC1 == 27)
                {
                    sb.Append("CGC1上轮 ");
                    sb.Append(intOldTurnCGC1 + "\n");
                    sb.Append("CGC1本轮 ");
                    sb.Append("[" + intTurnCGC1 + "]\n");
                    sb.Append("CGC1游戏状态 ");
                    sb.Append(intStatusCGC1 + "\n\n\n");
                }
                else
                {
                    sb.Append("CGC1上轮 ");
                    sb.Append(intOldTurnCGC1 + "\n");
                    sb.Append("CGC1本轮 ");
                    sb.Append(intTurnCGC1 + "\n");
                    sb.Append("CGC1游戏状态 ");
                    sb.Append(intStatusCGC1 + "\n\n\n");
                }

            }
            catch
            {
                intTurnCGC1 = 0;
                intStatusCGC1 = 0;
                intOldTurnCGC1 = 0;
            }

            sb.Append("TOM1\n");
            try
            {
                string strcon = DBConnection.Get30GamePhoneConnString(1, "TOM");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "TOM"), CommandType.Text, strSQL);
                intTurnTOM1 = (int)dr["Turn"];
                intStatusTOM1 = (byte)dr["Status"];
                intOldTurnTOM1 = (int)dr["OldTurn"];

                if (intTurnTOM1 == 27)
                {
                    sb.Append("TOM1上轮 ");
                    sb.Append(intOldTurnTOM1 + "\n");
                    sb.Append("TOM1本轮 ");
                    sb.Append("[" + intTurnTOM1 + "]\n");
                    sb.Append("TOM1游戏状态 ");
                    sb.Append(intStatusTOM1 + "\n\n\n");
                }
                else
                {
                    sb.Append("TOM1上轮 ");
                    sb.Append(intOldTurnTOM1 + "\n");
                    sb.Append("TOM1本轮 ");
                    sb.Append(intTurnTOM1 + "\n");
                    sb.Append("TOM1游戏状态 ");
                    sb.Append(intStatusTOM1 + "\n\n\n");
                }

            }
            catch
            {
                intTurnTOM1 = 0;
                intStatusTOM1 = 0;
                intOldTurnTOM1 = 0;
            }

            sb.Append("KX1\n");
            try
            {
                string strcon = DBConnection.Get30GamePhoneConnString(1, "KX");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "KX"), CommandType.Text, strSQL);
                intTurnKX1 = (int)dr["Turn"];
                intStatusKX1 = (byte)dr["Status"];
                intOldTurnKX1 = (int)dr["OldTurn"];

                if (intTurnKX1 == 27)
                {
                    sb.Append("KX1上轮 ");
                    sb.Append(intOldTurnKX1 + "\n");
                    sb.Append("KX1本轮 ");
                    sb.Append("[" + intTurnKX1 + "]\n");
                    sb.Append("KX1游戏状态 ");
                    sb.Append(intStatusKX1 + "\n\n\n");
                }
                else
                {
                    sb.Append("KX1上轮 ");
                    sb.Append(intOldTurnKX1 + "\n");
                    sb.Append("KX1本轮 ");
                    sb.Append(intTurnKX1 + "\n");
                    sb.Append("KX1游戏状态 ");
                    sb.Append(intStatusKX1 + "\n\n\n");
                }

            }
            catch
            {
                intTurnKX1 = 0;
                intStatusKX1 = 0;
                intOldTurnKX1 = 0;
            }

            sb.Append("SOHU1\n");
            try
            {
                string strcon = DBConnection.Get30GamePhoneConnString(51, "SOHU");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(51, "SOHU"), CommandType.Text, strSQL);
                intTurnSOHU1 = (int)dr["Turn"];
                intStatusSOHU1 = (byte)dr["Status"];
                intOldTurnSOHU1 = (int)dr["OldTurn"];

                if (intTurnSOHU1 == 27)
                {
                    sb.Append("SOHU1上轮 ");
                    sb.Append(intOldTurnSOHU1 + "\n");
                    sb.Append("SOHU1本轮 ");
                    sb.Append("[" + intTurnSOHU1 + "]\n");
                    sb.Append("SOHU1游戏状态 ");
                    sb.Append(intStatusSOHU1 + "\n\n\n");
                }
                else
                {
                    sb.Append("SOHU1上轮 ");
                    sb.Append(intOldTurnSOHU1 + "\n");
                    sb.Append("SOHU1本轮 ");
                    sb.Append(intTurnSOHU1 + "\n");
                    sb.Append("SOHU1游戏状态 ");
                    sb.Append(intStatusSOHU1 + "\n\n\n");
                }

            }
            catch
            {
                intTurnSOHU1 = 0;
                intStatusSOHU1 = 0;
                intOldTurnSOHU1 = 0;
            }

            sb.Append("5S\n");
            try
            {
                string strcon = DBConnection.Get30GamePhoneConnString(1, "5S");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "5S"), CommandType.Text, strSQL);
                intTurn5S1 = (int)dr["Turn"];
                intStatus5S1 = (byte)dr["Status"];
                intOldTurn5S1 = (int)dr["OldTurn"];

                if (intTurn5S1 == 27)
                {
                    sb.Append("5S1上轮 ");
                    sb.Append(intOldTurn5S1 + "\n");
                    sb.Append("5S1本轮 ");
                    sb.Append("[" + intTurn5S1 + "]\n");
                    sb.Append("5S1游戏状态 ");
                    sb.Append(intStatus5S1 + "\n\n\n");
                }
                else
                {
                    sb.Append("5S1上轮 ");
                    sb.Append(intOldTurn5S1 + "\n");
                    sb.Append("5S1本轮 ");
                    sb.Append(intTurn5S1 + "\n");
                    sb.Append("5S1游戏状态 ");
                    sb.Append(intStatus5S1 + "\n\n\n");
                }

            }
            catch
            {
                intTurn5S1 = 0;
                intStatus5S1 = 0;
                intOldTurn5S1 = 0;
            }
            return sb.ToString();
        }
        #endregion

        #region 生成特定文件格式的TXT文档
        /// <summary>
        /// 生成特定文件格式的TXT文档
        /// </summary>
        /// <param name="strSID">用户SID</param>
        /// <param name="strSMSContent">消息内容</param>
        /// <param name="type">1\SMS 2\PC</param>
        public static void CreateTxt(string strTXTContent)//生成特定文件格式的TXT文档
        {
            Random rnbNum = new Random();
            string strPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string strNum = Convert.ToString(rnbNum.Next(000000, 999999));
            string filename = "BB_" + DateTime.Now.ToShortDateString() + "";
            FileStream fs = new FileStream(@strPath + filename + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("UTF-8"));//通过指定字符编码方式可以实现对汉字的支持，否则在用记事本打开查看会出现乱码

            sw.Flush();
            sw.BaseStream.Seek(0, SeekOrigin.Begin);
            sw.WriteLine(strTXTContent);
            sw.Flush();
            sw.Close();
        }
        #endregion

        public static string readSendUser()
        {
            string code = "a230cd9bbf875bb28892249e64c45a01";
            string decode = "";

            //Base64解码，注意UTF8
            code = Base64_decode(code);
            byte[] bytes = Convert.FromBase64String(code);
            decode = System.Text.Encoding.UTF8.GetString(bytes);
            //怎么去除多余的内容？sip:78516401@fetion.com.cn;p=187
            //菜鸟用的笨办法，先用正则把里面的多余字符去除，然后把最后３个字符清除．谁有更好的办法，请救了
            //decode = Convert.ToString(Regex.Match(decode, @"\d{4,10}@fetion\.com\.cn"));
            //decode = decode.Substring(0, decode.Length - 3);
            return decode;
        }

        #region 处理数据位数不足并补齐
        /// <summary>
        /// 处理数据位数不足并补齐
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static string Base64_decode(string code)
        {
            while (code.Length % 4 > 0)
            {
                code = code + "=";
            }
            return code;
        }
        #endregion

        public static string GetBasketBallUpdateNightCheck()
        {
            //官方区
            DateTime dtimeFinishTimeBF1, dtimeFinishTimeNF2, dtimeFinishTimeHJ3, dtimeFinishTimeHR4, dtimeFinishTimeLN7, dtimeFinishTimeKRT8;
            //浩方区
            DateTime dtimeFinishTimeCGA3, dtimeFinishTimeCGA4;
            //17173区
            DateTime dtimeFinishTime171731, dtimeFinishTime171733, dtimeFinishTime171736;
            //51WAN区
            DateTime dtimeFinishTime51WAN4;
            //多玩区
            DateTime dtimeFinishTimeDW1;
            //新传区
            DateTime /*dtimeFinishTimeNBA1,*/ dtimeFinishTimeNBA2, dtimeFinishTimeNBA5;
            //新浪区
            DateTime dtimeFinishTimeSINA1, dtimeFinishTimeSINA3;
            //小I区
            //DateTime dtimeFinishTimeXiaoI1;
            //傲世区
            DateTime dtimeFinishTimeAS1;
            //中游区
            DateTime dtimeFinishTimeCGC1;
            //TOM区
            DateTime dtimeFinishTimeTOM1;
            //看下区
            //DateTime dtimeFinishTimeKX1;
            //搜狐区
            DateTime dtimeFinishTimeSOHU1;
            //5S区
            //DateTime dtimeFinishTime5S1;
            //台湾区
            //DateTime dtimeFinishTimeTW1, dtimeFinishTimeTW2;
            //PPS区
            DateTime dtimeFinishTimePPS1, dtimeFinishTimePPS2;
            //YL区
            DateTime dtimeFinishTimeYL1;
            //TTB区
            DateTime dtimeFinishTimeTTB1;
            //SINAB区
            DateTime dtimeFinishTimeSINAB1;

            StringBuilder strBuild = new StringBuilder("");

            DateTime dt = DateTime.Now;

            ArrayList arrResult = new ArrayList();
            DataRow dr;

            //获得天数
            int intDays = 0;


            try
            {
                //北方服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeBF1 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeBF1 < dt.Date && intDays == 1)
                {
                    arrResult.Add("BF");
                }
                //arrResult.Add("1");
            }
            catch
            {
                arrResult.Add("北方服连接超时！");
            }

            try
            {
                //南方服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeNF2 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeNF2 < dt.Date && intDays == 1)
                {
                    arrResult.Add("NF");
                }
                //arrResult.Add("2");
            }
            catch
            {
                arrResult.Add("南方服连接超时！");
            }

            try
            {
                //火箭服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(5, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeHJ3 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeHJ3 < dt.Date && intDays == 1)
                {
                    arrResult.Add("HJ");
                }
                //arrResult.Add("3");
            }
            catch
            {
                arrResult.Add("火箭服连接超时！");
            }

            try
            {
                //湖人服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(7, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeHR4 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeHR4 < dt.Date && intDays == 1)
                {
                    arrResult.Add("HR");
                }
                //arrResult.Add("4");
            }
            catch
            {
                arrResult.Add("湖人服连接超时！");
            }

            try
            {
                //凯尔特人服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(10, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeKRT8 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeKRT8 < dt.Date && intDays == 1)
                {
                    arrResult.Add("KRT");
                }
                //arrResult.Add("6");
            }
            catch
            {
                arrResult.Add("凯尔特人服连接超时！");
            }

            try
            {
                //陵南服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(8, "XBA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeLN7 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeLN7 < dt.Date && intDays == 1)
                {
                    arrResult.Add("LN");
                }
                //arrResult.Add("7");
            }
            catch
            {
                arrResult.Add("陵南服连接超时！");
            }

            try
            {
                //浩方三服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "CGA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeCGA3 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeCGA3 < dt.Date && intDays == 1)
                {
                    arrResult.Add("CGA3");
                }
                //arrResult.Add("8");
            }
            catch
            {
                arrResult.Add("浩方三服连接超时！");
            }

            try
            {
                //浩方四服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "CGA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeCGA4 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeCGA4 < dt.Date && intDays == 1)
                {
                    arrResult.Add("CGA4");
                }
                //arrResult.Add("9");
            }
            catch
            {
                arrResult.Add("浩方四服连接超时！");
            }

            try
            {
                //17173-1服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "17173"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTime171731 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTime171731 < dt.Date && intDays == 1)
                {
                    arrResult.Add("17173-1");
                }
                //arrResult.Add("10");
            }
            catch
            {
                arrResult.Add("17173-1服连接超时！");
            }

            try
            {
                //17173-3服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "17173"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTime171733 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTime171733 < dt.Date && intDays == 1)
                {
                    arrResult.Add("17173-3");
                }
                //arrResult.Add("11");
            }
            catch
            {
                arrResult.Add("17173-3服连接超时！");
            }

            try
            {
                //17173-6服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(6, "17173"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTime171736 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTime171736 < dt.Date && intDays == 1)
                {
                    arrResult.Add("17173-6");
                }
                //arrResult.Add("13");
            }
            catch
            {
                arrResult.Add("17173-6服连接超时！");
            }

            try
            {
                //51WAN4服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(4, "51WAN"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTime51WAN4 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTime51WAN4 < dt.Date && intDays == 1)
                {
                    arrResult.Add("51WAN4");
                }
                //arrResult.Add("15");
            }
            catch
            {
                arrResult.Add("51WAN4服连接超时！");
            }

            try
            {
                //DW1服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "DW"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeDW1 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeDW1 < dt.Date && intDays == 1)
                {
                    arrResult.Add("DW1");
                }
                //arrResult.Add("16");
            }
            catch
            {
                arrResult.Add("DW1服连接超时！");
            }

            #region ===================== 非09版服务器 ====================== NBA1
            //try
            //{
            //    //NBA1服
            //    dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(6, "NBA"), CommandType.StoredProcedure, "GetGameRow");
            //    dtimeFinishTimeNBA1 = (DateTime)dr["FinishTime"];
            //    intDays = (int)dr["Days"];
            //
            //    if (dtimeFinishTimeNBA1 < dt.Date && intDays == 1)
            //    {
            //        arrResult.Add("NBA1");
            //    }
            //    //arrResult.Add("17");
            //}
            //catch
            //{
            //    arrResult.Add("NBA1服连接超时！");
            //}
            #endregion =========================================================

            try
            {
                //NBA2服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "NBA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeNBA2 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeNBA2 < dt.Date && intDays == 1)
                {
                    arrResult.Add("NBA2");
                }
                //arrResult.Add("18");
            }
            catch
            {
                arrResult.Add("NBA2服连接超时！");
            }

            try
            {
                //NBA5服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(5, "NBA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeNBA5 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeNBA5 < dt.Date && intDays == 1)
                {
                    arrResult.Add("NBA5");
                }
                //arrResult.Add("19");
            }
            catch
            {
                arrResult.Add("NBA5服连接超时！");
            }

            try
            {
                //SINA1服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "SINA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeSINA1 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeSINA1 < dt.Date && intDays == 1)
                {
                    arrResult.Add("SINA1");
                }
                //arrResult.Add("21");
            }
            catch
            {
                arrResult.Add("SINA1服连接超时！");
            }

            try
            {
                //SINA3服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "SINA"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeSINA3 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeSINA3 < dt.Date && intDays == 1)
                {
                    arrResult.Add("SINA3");
                }
                //arrResult.Add("22");
            }
            catch
            {
                arrResult.Add("SINA3服连接超时！");
            }

            #region ===================== 非09版服务器 ====================== XiaoI
            //try
            //{
            //    //XiaoI1服
            //    dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "MSN"), CommandType.StoredProcedure, "GetGameRow");
            //    dtimeFinishTimeXiaoI1 = (DateTime)dr["FinishTime"];
            //    intDays = (int)dr["Days"];
            //
            //    if (dtimeFinishTimeXiaoI1 < dt.Date && intDays == 1)
            //    {
            //        arrResult.Add("XiaoI1");
            //    }
            //    //arrResult.Add("23");
            //}
            //catch
            //{
            //    arrResult.Add("XiaoI1服连接超时！");
            //}
            #endregion =========================================================

            try
            {
                //AS1服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "AS"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeAS1 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeAS1 < dt.Date && intDays == 1)
                {
                    arrResult.Add("AS1");
                }
                //arrResult.Add("24");
            }
            catch
            {
                arrResult.Add("AS1服连接超时！");
            }

            try
            {
                //CGC1服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "CGC"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeCGC1 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeCGC1 < dt.Date && intDays == 1)
                {
                    arrResult.Add("CGC1");
                }
                //arrResult.Add("25");
            }
            catch
            {
                arrResult.Add("CGC1服连接超时！");
            }

            try
            {
                //TOM1服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "TOM"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeTOM1 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeTOM1 < dt.Date && intDays == 1)
                {
                    arrResult.Add("TOM1");
                }
                //arrResult.Add("27");
            }
            catch
            {
                arrResult.Add("TOM1服连接超时！");
            }

            #region ===================== 非09版服务器 ====================== KX1
            //try
            //{
            //    //KX1服
            //    dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "KX"), CommandType.StoredProcedure, "GetGameRow");
            //    dtimeFinishTimeKX1 = (DateTime)dr["FinishTime"];
            //    intDays = (int)dr["Days"];
            //
            //    if (dtimeFinishTimeKX1 < dt.Date)
            //    {
            //        arrResult.Add("KX1");
            //    }
            //    //arrResult.Add("28");
            //}
            //catch
            //{
            //    arrResult.Add("KX1服连接超时！");
            //}
            #endregion =========================================================

            try
            {
                //SOHU1服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(51, "17173"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeSOHU1 = (DateTime)dr["FinishTime"];
                if (dtimeFinishTimeSOHU1 < dt.Date && intDays == 1)
                {
                    arrResult.Add("SOHU1");
                }
                //arrResult.Add("29");
            }
            catch
            {
                arrResult.Add("SOHU1服连接超时！");
            }

            #region ===================== 非09版服务器 ====================== 5S1
            //try
            //{
            //    //5S1服
            //    dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "5S"), CommandType.StoredProcedure, "GetGameRow");
            //    dtimeFinishTime5S1 = (DateTime)dr["FinishTime"];
            //    intDays = (int)dr["Days"];
            //
            //    if (dtimeFinishTime5S1 < dt.Date && intDays == 1)
            //    {
            //        arrResult.Add("5S1");
            //    }
            //    //arrResult.Add("30");
            //}
            //catch
            //{
            //    arrResult.Add("5S1服连接超时！");
            //}
            #endregion =========================================================

            #region ===================== 非09版服务器 ====================== TW1 TW2
            //try
            //{
            //    //TW1服
            //    dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "TW"), CommandType.StoredProcedure, "GetGameRow");
            //    dtimeFinishTimeTW1 = (DateTime)dr["FinishTime"];
            //    intDays = (int)dr["Days"];
            //
            //    if (dtimeFinishTimeTW1 < dt.Date && intDays == 1)
            //    {
            //        arrResult.Add("TW1");
            //    }
            //    //arrResult.Add("31");
            //}
            //catch
            //{
            //    arrResult.Add("TW1服连接超时！");
            //}

            //try
            //{
            //    //TW2服
            //    dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "TW"), CommandType.StoredProcedure, "GetGameRow");
            //    dtimeFinishTimeTW2 = (DateTime)dr["FinishTime"];
            //    intDays = (int)dr["Days"];
            //
            //    if (dtimeFinishTimeTW2 < dt.Date && intDays == 1)
            //    {
            //        arrResult.Add("TW2");
            //    }
            //    //arrResult.Add("32");
            //}
            //catch
            //{
            //    arrResult.Add("TW2服连接超时！");
            //}
            #endregion =========================================================

            try
            {
                //PPS1服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "PPS"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimePPS1 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimePPS1 < dt.Date && intDays == 1)
                {
                    arrResult.Add("PPS1");
                }
                //arrResult.Add("32");
            }
            catch
            {
                arrResult.Add("PPS1服连接超时！");
            }

            try
            {
                //PPS2服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "PPS"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimePPS2 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimePPS2 < dt.Date && intDays == 1)
                {
                    arrResult.Add("PPS2");
                }
                //arrResult.Add("32");
            }
            catch
            {
                arrResult.Add("PPS2服连接超时！");
            }

            //try
            //{
            //    //YL1服
            //    dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "YL"), CommandType.StoredProcedure, "GetGameRow");
            //    dtimeFinishTimeYL1 = (DateTime)dr["FinishTime"];
            //    intDays = (int)dr["Days"];

            //    if (dtimeFinishTimeYL1 < dt.Date && intDays == 1)
            //    {
            //        arrResult.Add("YL1");
            //    }
            //    //arrResult.Add("32");
            //}
            //catch
            //{
            //    arrResult.Add("YL1服连接超时！");
            //}

            try
            {
                //TTB1服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "TTB"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeTTB1 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeTTB1 < dt.Date && intDays == 1)
                {
                    arrResult.Add("TTB1");
                }
                //arrResult.Add("32");
            }
            catch
            {
                arrResult.Add("TTB1服连接超时！");
            }

            try
            {
                //SINAB1服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "SINAB"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeSINAB1 = (DateTime)dr["FinishTime"];
                intDays = (int)dr["Days"];

                if (dtimeFinishTimeSINAB1 < dt.Date && intDays == 1)
                {
                    arrResult.Add("SINAB1");
                }
                //arrResult.Add("32");
            }
            catch
            {
                arrResult.Add("SINAB1服连接超时！");
            }

            foreach (string i in arrResult)
            {
                strBuild.Append(i.ToString() + "\\n");
                //Console.WriteLine(i);
            }

            if (strBuild.Length > 0)
            {
                strBuild.Append(string.Format("{0:G}", dt));
                return strBuild.ToString();
            }
            return strBuild.Append(false).ToString();
        }

        public static string GetFootBallUpdateNightCheck()
        {
            //官方区
            DateTime dtimeFinishTimeFB1, dtimeFinishTimeFB2, dtimeFinishTimeFB3, dtimeFinishTimeFB4;
            //傲世区
            DateTime dtimeFinishTimeAS1;
            //多玩区
            DateTime dtimeFinishTimeDW1, dtimeFinishTimeDW2;
            //17173区
            DateTime dtimeFinishTime171731, dtimeFinishTime171732, dtimeFinishTime171733, dtimeFinishTime171734, dtimeFinishTime171735, dtimeFinishTime171736;
            //51WAN区
            DateTime dtimeFinishTime51WAN1;
            //新传区
            DateTime dtimeFinishTimeNBA1;
            //中超区
            DateTime dtimeFinishTimeCSL1, dtimeFinishTimeCSL2, dtimeFinishTimeCSL3, dtimeFinishTimeCSL4, dtimeFinishTimeCSL5;
            //体坛区
            DateTime dtimeFinishTimeTT1, dtimeFinishTimeTT2;
            //21CN区
            DateTime dtimeFinishTime21CN1;
            //ESL区
            DateTime dtimeFinishTimeESL1;
            //PPSF区
            DateTime dtimeFinishTimePPSF1;

            StringBuilder strBuild = new StringBuilder("");

            DateTime dt = DateTime.Now;

            ArrayList arrResult = new ArrayList();
            DataRow dr;

            //获得天数
            byte byteDays = 0;

            try
            {
                //FB1服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "XBAF"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeFB1 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeFB1 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("FB-1");
                }
            }
            catch
            {
                arrResult.Add("FB-1服连接超时！");
            }

            try
            {
                //FB3服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "XBAF"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeFB3 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeFB3 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("FB-3");
                }
            }
            catch
            {
                arrResult.Add("FB-3服连接超时！");
            }

            try
            {
                //FB4服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(4, "XBAF"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeFB4 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeFB4 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("FB-4");
                }
            }
            catch
            {
                arrResult.Add("FB-4服连接超时！");
            }

            try
            {
                //ASF1服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "ASF"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeAS1 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeAS1 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("AS-F1");
                }
            }
            catch
            {
                arrResult.Add("AS-F1服连接超时！");
            }

            try
            {
                //DWF1服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "DWF"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeDW1 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeDW1 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("DW-F1");
                }
            }
            catch
            {
                arrResult.Add("DW-F1服连接超时！");
            }

            try
            {
                //DWF2服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "DWF"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeDW2 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeDW2 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("DW-F2");
                }
            }
            catch
            {
                arrResult.Add("DW-F2服连接超时！");
            }

            try
            {
                //17173F1服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "17173F"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTime171731 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTime171731 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("17173-F1");
                }
            }
            catch
            {
                arrResult.Add("17173-F1服连接超时！");
            }

            try
            {
                //17173F4服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(4, "17173F"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTime171734 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTime171734 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("17173-F4");
                }
            }
            catch
            {
                arrResult.Add("17173-F4服连接超时！");
            }

            try
            {
                //17173F6服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(6, "17173F"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTime171736 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTime171736 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("17173-F6");
                }
            }
            catch
            {
                arrResult.Add("17173-F6服连接超时！");
            }

            try
            {
                //51WANF1服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "51WANF"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTime51WAN1 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTime51WAN1 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("51WAN-F1");
                }
            }
            catch
            {
                arrResult.Add("51WAN-F1服连接超时！");
            }

            try
            {
                //NBAF1服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "NBAF"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeNBA1 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeNBA1 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("NBA-F1");
                }
            }
            catch
            {
                arrResult.Add("NBA-F1服连接超时！");
            }

            try
            {
                //CSLF1服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "SINAF"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeCSL1 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeCSL1 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("CSL-F1");
                }
            }
            catch
            {
                arrResult.Add("CSL-F1服连接超时！");
            }

            try
            {
                //CSLF2服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "SINAF"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeCSL2 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeCSL2 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("CSL-F2");
                }
            }
            catch
            {
                arrResult.Add("CSL-F2服连接超时！");
            }

            try
            {
                //CSLF3服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(3, "SINAF"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeCSL3 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeCSL3 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("CSL-F3");
                }
            }
            catch
            {
                arrResult.Add("CSL-F3服连接超时！");
            }

            try
            {
                //CSLF4服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(4, "SINAF"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeCSL4 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeCSL4 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("CSL-F4");
                }
            }
            catch
            {
                arrResult.Add("CSL-F4服连接超时！");
            }

            try
            {
                //CSLF5服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(5, "SINAF"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeCSL5 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeCSL5 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("CSL-F5");
                }
            }
            catch
            {
                arrResult.Add("CSL-F5服连接超时！");
            }

            try
            {
                //TT1服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "TT"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeTT1 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeTT1 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("TT-F1");
                }
            }
            catch
            {
                arrResult.Add("TT-F1服连接超时！");
            }

            try
            {
                //TT2服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "TT"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeTT2 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeTT2 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("TT-F2");
                }
            }
            catch
            {
                arrResult.Add("TT-F2服连接超时！");
            }

            try
            {
                //21CN1服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "21CN"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTime21CN1 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTime21CN1 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("21CN-F1");
                }
            }
            catch
            {
                arrResult.Add("21CN-F1服连接超时！");
            }

            try
            {
                //ESL1服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "ESL"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimeESL1 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimeESL1 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("ESL-F1");
                }
            }
            catch
            {
                arrResult.Add("ESL-F1服连接超时！");
            }

            try
            {
                //PPSF1服
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "PPSF"), CommandType.StoredProcedure, "GetGameRow");
                dtimeFinishTimePPSF1 = (DateTime)dr["FinishTime"];
                byteDays = (byte)dr["Days"];

                if (dtimeFinishTimePPSF1 < dt.Date && byteDays == 1)
                {
                    arrResult.Add("PPSF-F1");
                }
            }
            catch
            {
                arrResult.Add("PPSF-F1服连接超时！");
            }

            foreach (string i in arrResult)
            {
                strBuild.Append(i.ToString() + "\\n");
                //Console.WriteLine(i);
            }

            //Console.ReadLine();
            if (strBuild.Length > 0)
            {
                strBuild.Append(string.Format("{0:G}", dt));
                return strBuild.ToString();
            }
            return strBuild.Append(false).ToString();
        }
    }
}
