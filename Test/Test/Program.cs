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
            //����HTTP����Ĭ�ϱ���
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

            //��������  --- id=788" target="_blank"><span class="title"><font title="������Ծ�����T��">������Ծ�����T��</font>
            Regex regexObj = new Regex(@"id=\d{1,4}.*</font>.*\r\n.*<br>");
            Regex regexObj_1 = new Regex("src=\"http://img.*.jpg");

            for (j = 1; j <= 5; j++)
            {
                resultString = HTTPproc_1.OpenRead("http://bo.tianxia.taobao.com/taoboyuan/seedstore_category.jhtml?pageNo=1&catId=" + j + "");
                try
                {
                    resultString_4 = resultString;
                    resultString = Regex.Match(resultString, @"��\d{1,2}ҳ").Value.Replace("��", "").Replace("ҳ", "");
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
                            //����ID����  --- id=788" target="_blank"><span class="title"><font title="������Ծ�����T��">������Ծ�����T��</font>
                            resultString_0 = Regex.Match(matchResults.ToString(), @"id=\d{1,4}").Value.Replace("id=", "");
                            //������������  --- id=788" target="_blank"><span class="title"><font title="������Ծ�����T��">������Ծ�����T��</font>
                            //resultString_1 = Regex.Match(matchResults.ToString(), "<font.*</font>").Value;
                            //resultString_1 = Regex.Replace(resultString_1, "<font.*\">", "").Replace("</font>", "");
                            resultString_1 = Regex.Match(matchResults.ToString(), "<font.*\">").Value.Replace("<font title=\"", "").Replace("\">", "");
                            //����ͨ������  --- ����ͨ��:<span class="xuyao">40</span><br>
                            resultString_2 = Regex.Match(matchResults.ToString(), @"\d{1,3}</span>").Value.Replace("</span>", "");
                            //����ͼƬ����
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
        //    while (ticketList.Count > 0)//��   
        //    {
        //        lock (objLock)
        //        {
        //            if (ticketList.Count > 0)
        //            {
        //                string ticketNo = ticketList[0];//��   
        //                Console.WriteLine("{0}:�۳�һ��Ʊ��Ʊ�ţ�{1}", Thread.CurrentThread.Name, ticketNo);
        //                ticketList.RemoveAt(0);//��   
        //                Thread.Sleep(1);
        //            }
        //        }
        //    } 
        //}

        private void ReadProduct()
        {
            string resultString = null;

            //����HTTP����Ĭ�ϱ���
            HTTPproc_1.Encoding = System.Text.Encoding.Default;

            #region ͼƬ���ص�����Ŀ¼

            DateTime StarTime = DateTime.Now;            
            Console.WriteLine(StarTime);

            while (al_Fushi.Count > 0)
            {
                lock (objLock)
                {
                    if (al_Fushi.Count > 0)
                    {
                        string strFushi = al_Fushi[0].ToString();//��

                        try
                        {
                            resultString = Regex.Match(strFushi, "/T.*.160x160.jpg").Value.Replace("/", "");
                            if (File.Exists(@"Image\\" + resultString + ""))
                            {
                                //HTTPproc_1.DownloadFile(al_Product[i].ToString(), @"Image\\" + resultString + "");
                                al_Fushi.RemoveAt(0);//��
                            }
                            else
                            {
                                HTTPproc_1.DownloadFile(strFushi, @"Image\\" + resultString + "");
                                Console.WriteLine("{0}:�������ص�ͼƬ����:{1}", Thread.CurrentThread.Name, resultString);
                                al_Fushi.RemoveAt(0);//��   
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
                        string strJiaju = al_Jiaju[0].ToString();//��

                        try
                        {
                            resultString = Regex.Match(strJiaju, "/T.*.160x160.jpg").Value.Replace("/", "");
                            if (File.Exists(@"Image\\" + resultString + ""))
                            {
                                //HTTPproc_1.DownloadFile(al_Product[i].ToString(), @"Image\\" + resultString + "");
                                al_Jiaju.RemoveAt(0);//��
                            }
                            else
                            {
                                HTTPproc_1.DownloadFile(strJiaju, @"Image\\" + resultString + "");
                                Console.WriteLine("{0}:�������ص�ͼƬ����:{1}", Thread.CurrentThread.Name, resultString);
                                al_Jiaju.RemoveAt(0);//��   
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
                        string strMeirong = al_Meirong[0].ToString();//��

                        try
                        {
                            resultString = Regex.Match(strMeirong, "/T.*.160x160.jpg").Value.Replace("/", "");
                            if (File.Exists(@"Image\\" + resultString + ""))
                            {
                                //HTTPproc_1.DownloadFile(al_Product[i].ToString(), @"Image\\" + resultString + "");
                                al_Meirong.RemoveAt(0);//��
                            }
                            else
                            {
                                HTTPproc_1.DownloadFile(strMeirong, @"Image\\" + resultString + "");
                                Console.WriteLine("{0}:�������ص�ͼƬ����:{1}", Thread.CurrentThread.Name, resultString);
                                al_Meirong.RemoveAt(0);//��   
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
                        string strShipin = al_Shipin[0].ToString();//��

                        try
                        {
                            resultString = Regex.Match(strShipin, "/T.*.160x160.jpg").Value.Replace("/", "");
                            if (File.Exists(@"Image\\" + resultString + ""))
                            {
                                //HTTPproc_1.DownloadFile(al_Product[i].ToString(), @"Image\\" + resultString + "");
                                al_Shipin.RemoveAt(0);//��   
                            }
                            else
                            {
                                HTTPproc_1.DownloadFile(strShipin, @"Image\\" + resultString + "");
                                Console.WriteLine("{0}:�������ص�ͼƬ����:{1}", Thread.CurrentThread.Name, resultString);
                                al_Shipin.RemoveAt(0);//��   
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
                        string strDianqi = al_Dianqi[0].ToString();//��

                        try
                        {
                            resultString = Regex.Match(strDianqi, "/T.*.160x160.jpg").Value.Replace("/", "");
                            if (File.Exists(@"Image\\" + resultString + ""))
                            {
                                //HTTPproc_1.DownloadFile(al_Product[i].ToString(), @"Image\\" + resultString + "");
                                al_Dianqi.RemoveAt(0);//��  
                            }
                            else
                            {
                                HTTPproc_1.DownloadFile(strDianqi, @"Image\\" + resultString + "");
                                Console.WriteLine("{0}:�������ص�ͼƬ����:{1}", Thread.CurrentThread.Name, resultString);
                                al_Dianqi.RemoveAt(0);//��   
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

            #endregion ͼƬ���ص�����Ŀ¼
            
        }
    }

    class Program
    {
        static void Main()
        {
            //DateTime dt = DateTime.Now;
            //Console.WriteLine(CheckDisk());
            //Console.WriteLine(ServerIP());

            #region ����
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
            //Console.WriteLine(Weather("�ൺ"));
            //Console.WriteLine(Encoding.Default.GetBytes(Weather("�ൺ")).Length);
            //Console.WriteLine(PostModel("http://webservice.jtjc.cn/service/weather.asmx/GetWeather", "City=�ൺ"));
            //Console.WriteLine(GetModel("http://webservice.jtjc.cn/service/weather.asmx/GetWeather?", "City=�ൺ"));
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
            ////Console.WriteLine(HTTPproc.OpenRead("http://webservice.webxml.com.cn/WebServices/WeatherWebService.asmx/getWeatherbyCityName", "theCityName=�ൺ"));
            //string sResultContents = HTTPproc.OpenRead("http://webservice.webxml.com.cn/WebServices/WeatherWebService.asmx/getWeatherbyCityName", "theCityName=�ൺ");
            //XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.LoadXml(sResultContents);
            //string xmlContents = xmlDoc.ChildNodes[1].ChildNodes[6].InnerText.ToString();
            //xmlContents += xmlDoc.ChildNodes[1].ChildNodes[10].InnerText.ToString();
            //int intIndex = xmlContents.IndexOf("����ѹ��");
            //xmlContents = xmlContents.Substring(0, intIndex);
            //Console.WriteLine(xmlContents);
            //Console.ReadKey();
            //Console.Write(); //�ɹ����շ���true,timeout ����false
            #endregion ����

            #region ����Ӣ�ۺ�̨���ݲɼ�
            //string strContent = "";
            //string[] server = { "�ٷ�������", "���Է�����", "Ѹ�׷���", "Сi����", "�쳵����", "�ţ����", "��Ϸ������", "yeswan����", "VS��������", "ޱ������", "Game2����", "Game5����", "PPLive����", "���ҷ���", "����Դ����", "���з���", "�������", "U9U8����", "ͬѧ������", "���Ϸ���", "Ȧ������", "�ȿ����", "�������", "17173����", "ά˹����", "���շ���", "�ṷ����", "VeryCD����", "uusee����", "91�����", "�Ʒ�����", "PLU����", "���ķ���", "51����", "�������", "IS��������", "���ط���", "�ٶȷ���", "PPS����", "���˷���", "ʢ�����", "360����", "Ҷ������", "��ɷ���", "TOM����", "��Ԫ����", "���ڷ���", "�ǿշ���", "���Ƿ���", "��������", "��Ϸ���ط���", "XBA����" };
            //string[] content = { };

            ////Console.WriteLine(server[0].ToString());
            //Test.WebClient HTTPproc = new Test.WebClient();
            //HTTPproc.Encoding = System.Text.Encoding.UTF8;
            ////����Cookies
            //HTTPproc.CookieContainer.Add(new Uri("http://stat.hero.9wee.com"), new Cookie("CWSSESSID", "0b5cd1574d9223d92532febad7f6f75a"));
            //HTTPproc.CookieContainer.Add(new Uri("http://stat.hero.9wee.com"), new Cookie("hero_username", "xba"));
            //HTTPproc.CookieContainer.Add(new Uri("http://stat.hero.9wee.com"), new Cookie("hero_key", "5255fd486f4fe28fceb125d9c8ec3563"));
            //HTTPproc.CookieContainer.Add(new Uri("http://stat.hero.9wee.com"), new Cookie("gm_server_url", "UmMCawM0BjhQf1A4AWkBZFA5ByhcbAs0UD9QJQBXAE9RQFTiAYxc3ga3AYxVvQF2BG1QZQAwUGdSPFF1UWQBPVI6AmsDJAZxUDVQfwE7ATFQcAc0XHgLdVBnUGYAIQBuUW5UagEqXDsGPAEiVTwBKQ=="));
            ////��ѯ����
            //strContent = HTTPproc.GetHtml("http://stat.hero.9wee.com/gm/modules/stat_global.php?server_group=�ٷ�������&startday=2010-05-25&search=�� ѯ");

            ////strContent = HTTPproc.OpenRead("http://stat.hero.9wee.com/gm/modules/stat_global.php?server_group=Ѹ�׷���&startday=2010-05-25&search=�� ѯ");
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
            ////    Regex regexObj = new Regex(@"\d{0,4}��\d{0,2}��\d{0,2}��\s{0,5}\d{0,2}:\d{0,2}\s*\d{0,}\s<A\sHREF=""/\S*"">");
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
            #endregion ����Ӣ�ۺ�̨���ݲɼ�

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

        #region ����������̿ռ���Ϣ
        /// <summary>
        /// ����������̿ռ���Ϣ
        /// </summary>
        /// <returns></returns>
        private static string CheckDisk()
        {
            string result = "";
            //�����̿ռ�
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
        #endregion ����������̿ռ���Ϣ

        #region ��ȡ������IP��Ϣ
        /// <summary>
        /// ��ȡ������IP��Ϣ
        /// </summary>
        /// <returns></returns>
        private static string ServerIP()
        {
            IPAddress[] ServerIP = Dns.GetHostByName(Dns.GetHostName()).AddressList;
            return ServerIP[0].ToString();
        }
        #endregion ��ȡ������IP��Ϣ

        #region ��ȡĳ����������Ϣ���
        /// <summary>
        /// ��ȡĳ����������Ϣ���
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
                strContents = xmlDoc.ChildNodes[1].ChildNodes[6].InnerText.ToString()+"��";
                strContents += xmlDoc.ChildNodes[1].ChildNodes[7].InnerText.ToString() + "��";
                strContents += xmlDoc.ChildNodes[1].ChildNodes[10].InnerText.ToString();
                //int intIndex = strContents.IndexOf("����ѹ��");
                //strContents = strContents.Substring(0, intIndex);
                return strContents;
            }
            else
            {
                strContents = "�����ֳ�����!";
                return strContents;
            }
        }
        #endregion

        #region Google���߷���  GoogleTranslate(string word)
        /// <summary>
        /// Google���߷���
        /// </summary>
        /// <param name="word">��ѯ����</param>
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
                //����HTTP��������Ӧ
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

        #region ��ȡ��������Ϸ�ִ�
        /// <summary>
        /// ��ȡ��������Ϸ�ִ�
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
                    sb.Append("FB1��" + intDaysFB1 + "��\n");
                    sb.Append("FB1�� [" + intTurnFB1 + "]��\n");
                    sb.Append("FB1��Ϸ״̬ " + intStatusFB1 + "\n");
                    sb.Append("FB1ҹ�����״̬ " + intFinishFB1 + "\n\n\n");
                }
                else
                {
                    sb.Append("FB1�� " + intDaysFB1 + "��\n");
                    sb.Append("FB1�� " + intTurnFB1 + "��\n");
                    sb.Append("FB1��Ϸ״̬ " + intStatusFB1 + "\n");
                    sb.Append("FB1ҹ�����״̬ " + intFinishFB1 + "\n\n\n");
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
                    sb.Append("FB2��" + intDaysFB2 + "��\n");
                    sb.Append("FB2�� [" + intTurnFB2 + "]��\n");
                    sb.Append("FB2��Ϸ״̬ " + intStatusFB2 + "\n");
                    sb.Append("FB2ҹ�����״̬ " + intFinishFB2 + "\n\n\n");
                }
                else
                {
                    sb.Append("FB2�� " + intDaysFB2 + "��\n");
                    sb.Append("FB2�� " + intTurnFB2 + "��\n");
                    sb.Append("FB2��Ϸ״̬ " + intStatusFB2 + "\n");
                    sb.Append("FB2ҹ�����״̬ " + intFinishFB2 + "\n\n\n");
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
                    sb.Append("FB3��" + intDaysFB3 + "��\n");
                    sb.Append("FB3�� [" + intTurnFB3 + "]��\n");
                    sb.Append("FB3��Ϸ״̬ " + intStatusFB3 + "\n");
                    sb.Append("FB3ҹ�����״̬ " + intFinishFB3 + "\n\n\n");
                }
                else
                {
                    sb.Append("FB3�� " + intDaysFB3 + "��\n");
                    sb.Append("FB3�� " + intTurnFB3 + "��\n");
                    sb.Append("FB3��Ϸ״̬ " + intStatusFB3 + "\n");
                    sb.Append("FB3ҹ�����״̬ " + intFinishFB3 + "\n\n\n");
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
                    sb.Append("ASFB1��" + intDaysASFB1 + "��\n");
                    sb.Append("ASFB1�� [" + intTurnASFB1 + "]��\n");
                    sb.Append("ASFB1��Ϸ״̬ " + intStatusASFB1 + "\n");
                    sb.Append("ASFB1ҹ�����״̬ " + intFinishASFB1 + "\n\n\n");
                }
                else
                {
                    sb.Append("ASFB1�� " + intDaysASFB1 + "��\n");
                    sb.Append("ASFB1�� " + intTurnASFB1 + "��\n");
                    sb.Append("ASFB1��Ϸ״̬ " + intStatusASFB1 + "\n");
                    sb.Append("ASFB1ҹ�����״̬ " + intFinishASFB1 + "\n\n\n");
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
                    sb.Append("DWFB1��" + intDaysDWFB1 + "��\n");
                    sb.Append("DWFB1�� [" + intTurnDWFB1 + "]��\n");
                    sb.Append("DWFB1��Ϸ״̬ " + intStatusDWFB1 + "\n");
                    sb.Append("DWFB1ҹ�����״̬ " + intFinishDWFB1 + "\n\n\n");
                }
                else
                {
                    sb.Append("DWFB1�� " + intDaysDWFB1 + "��\n");
                    sb.Append("DWFB1�� " + intTurnDWFB1 + "��\n");
                    sb.Append("DWFB1��Ϸ״̬ " + intStatusDWFB1 + "\n");
                    sb.Append("DWFB1ҹ�����״̬ " + intFinishDWFB1 + "\n\n\n");
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
                    sb.Append("DWFB2��" + intDaysDWFB2 + "��\n");
                    sb.Append("DWFB2�� [" + intTurnDWFB2 + "]��\n");
                    sb.Append("DWFB2��Ϸ״̬ " + intStatusDWFB2 + "\n");
                    sb.Append("DWFB2ҹ�����״̬ " + intFinishDWFB2 + "\n\n\n");
                }
                else
                {
                    sb.Append("DWFB2�� " + intDaysDWFB2 + "��\n");
                    sb.Append("DWFB2�� " + intTurnDWFB2 + "��\n");
                    sb.Append("DWFB2��Ϸ״̬ " + intStatusDWFB2 + "\n");
                    sb.Append("DWFB2ҹ�����״̬ " + intFinishDWFB2 + "\n\n\n");
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
                    sb.Append("17173FB1��" + intDays17173FB1 + "��\n");
                    sb.Append("17173FB1�� [" + intTurn17173FB1 + "]��\n");
                    sb.Append("17173FB1��Ϸ״̬ " + intStatus17173FB1 + "\n");
                    sb.Append("17173FB1ҹ�����״̬ " + intFinish17173FB1 + "\n\n\n");
                }
                else
                {
                    sb.Append("17173FB1�� " + intDays17173FB1 + "��\n");
                    sb.Append("17173FB1�� " + intTurn17173FB1 + "��\n");
                    sb.Append("17173FB1��Ϸ״̬ " + intStatus17173FB1 + "\n");
                    sb.Append("17173FB1ҹ�����״̬ " + intFinish17173FB1 + "\n\n\n");
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
                    sb.Append("17173FB2��" + intDays17173FB2 + "��\n");
                    sb.Append("17173FB2�� [" + intTurn17173FB2 + "]��\n");
                    sb.Append("17173FB2��Ϸ״̬ " + intStatus17173FB2 + "\n");
                    sb.Append("17173FB2ҹ�����״̬ " + intFinish17173FB2 + "\n\n\n");
                }
                else
                {
                    sb.Append("17173FB2�� " + intDays17173FB2 + "��\n");
                    sb.Append("17173FB2�� " + intTurn17173FB2 + "��\n");
                    sb.Append("17173FB2��Ϸ״̬ " + intStatus17173FB2 + "\n");
                    sb.Append("17173FB2ҹ�����״̬ " + intFinish17173FB2 + "\n\n\n");
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
                    sb.Append("17173FB3��" + intDays17173FB3 + "��\n");
                    sb.Append("17173FB3�� [" + intTurn17173FB3 + "]��\n");
                    sb.Append("17173FB3��Ϸ״̬ " + intStatus17173FB3 + "\n");
                    sb.Append("17173FB3ҹ�����״̬ " + intFinish17173FB3 + "\n\n\n");
                }
                else
                {
                    sb.Append("17173FB3�� " + intDays17173FB3 + "��\n");
                    sb.Append("17173FB3�� " + intTurn17173FB3 + "��\n");
                    sb.Append("17173FB3��Ϸ״̬ " + intStatus17173FB3 + "\n");
                    sb.Append("17173FB3ҹ�����״̬ " + intFinish17173FB3 + "\n\n\n");
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
                    sb.Append("17173FB4��" + intDays17173FB4 + "��\n");
                    sb.Append("17173FB4�� [" + intTurn17173FB4 + "]��\n");
                    sb.Append("17173FB4��Ϸ״̬ " + intStatus17173FB4 + "\n");
                    sb.Append("17173FB4ҹ�����״̬ " + intFinish17173FB4 + "\n\n\n");
                }
                else
                {
                    sb.Append("17173FB4�� " + intDays17173FB4 + "��\n");
                    sb.Append("17173FB4�� " + intTurn17173FB4 + "��\n");
                    sb.Append("17173FB4��Ϸ״̬ " + intStatus17173FB4 + "\n");
                    sb.Append("17173FB4ҹ�����״̬ " + intFinish17173FB4 + "\n\n\n");
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
                    sb.Append("51WANF1��" + intDays51WANF1 + "��\n");
                    sb.Append("51WANF1�� [" + intTurn51WANF1 + "]��\n");
                    sb.Append("51WANF1��Ϸ״̬ " + intStatus51WANF1 + "\n");
                    sb.Append("51WANF1ҹ�����״̬ " + intFinish51WANF1 + "\n\n\n");
                }
                else
                {
                    sb.Append("51WANF1�� " + intDays51WANF1 + "��\n");
                    sb.Append("51WANF1�� " + intTurn51WANF1 + "��\n");
                    sb.Append("51WANF1��Ϸ״̬ " + intStatus51WANF1 + "\n");
                    sb.Append("51WANF1ҹ�����״̬ " + intFinish51WANF1 + "\n\n\n");
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
                    sb.Append("NBAF1��" + intDaysNBAF1 + "��\n");
                    sb.Append("NBAF1�� [" + intTurnNBAF1 + "]��\n");
                    sb.Append("NBAF1��Ϸ״̬ " + intStatusNBAF1 + "\n");
                    sb.Append("NBAF1ҹ�����״̬ " + intFinishNBAF1 + "\n\n\n");
                }
                else
                {
                    sb.Append("NBAF1�� " + intDaysNBAF1 + "��\n");
                    sb.Append("NBAF1�� " + intTurnNBAF1 + "��\n");
                    sb.Append("NBAF1��Ϸ״̬ " + intStatusNBAF1 + "\n");
                    sb.Append("NBAF1ҹ�����״̬ " + intFinishNBAF1 + "\n\n\n");
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
                    sb.Append("CSL1��" + intDaysCSL1 + "��\n");
                    sb.Append("CSL1�� [" + intTurnCSL1 + "]��\n");
                    sb.Append("CSL1��Ϸ״̬ " + intStatusCSL1 + "\n");
                    sb.Append("CSL1ҹ�����״̬ " + intFinishCSL1 + "\n\n\n");
                }
                else
                {
                    sb.Append("CSL1�� " + intDaysCSL1 + "��\n");
                    sb.Append("CSL1�� " + intTurnCSL1 + "��\n");
                    sb.Append("CSL1��Ϸ״̬ " + intStatusCSL1 + "\n");
                    sb.Append("CSL1ҹ�����״̬ " + intFinishCSL1 + "\n\n\n");
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
                    sb.Append("CSL2��" + intDaysCSL2 + "��\n");
                    sb.Append("CSL2�� [" + intTurnCSL2 + "]��\n");
                    sb.Append("CSL2��Ϸ״̬ " + intStatusCSL2 + "\n");
                    sb.Append("CSL2ҹ�����״̬ " + intFinishCSL2 + "\n\n\n");
                }
                else
                {
                    sb.Append("CSL2�� " + intDaysCSL2 + "��\n");
                    sb.Append("CSL2�� " + intTurnCSL2 + "��\n");
                    sb.Append("CSL2��Ϸ״̬ " + intStatusCSL2 + "\n");
                    sb.Append("CSL2ҹ�����״̬ " + intFinishCSL2 + "\n\n\n");
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
                    sb.Append("CSL3��" + intDaysCSL3 + "��\n");
                    sb.Append("CSL3�� [" + intTurnCSL3 + "]��\n");
                    sb.Append("CSL3��Ϸ״̬ " + intStatusCSL3 + "\n");
                    sb.Append("CSL3ҹ�����״̬ " + intFinishCSL3 + "\n\n\n");
                }
                else
                {
                    sb.Append("CSL3�� " + intDaysCSL3 + "��\n");
                    sb.Append("CSL3�� " + intTurnCSL3 + "��\n");
                    sb.Append("CSL3��Ϸ״̬ " + intStatusCSL3 + "\n");
                    sb.Append("CSL3ҹ�����״̬ " + intFinishCSL3 + "\n\n\n");
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

        #region ��ȡ��������Ϸ�ִ�
        /// <summary>
        /// ��ȡ��������Ϸ�ִ�
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

            sb.Append("������\n");
            try
            {
                string strcon1 = DBConnection.Get30GamePhoneConnString(1, "XBA");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(1, "XBA"), CommandType.Text, strSQL);
                intTurnN1 = (int)dr["Turn"];
                intStatusN1 = (byte)dr["Status"];
                intOldTurnN1 = (int)dr["OldTurn"];

                if (intTurnN1 == 27)
                {
                    sb.Append("���������� ");
                    sb.Append(intOldTurnN1 + "\n");
                    sb.Append("���������� ");
                    sb.Append("[" + intTurnN1 + "]\n");
                    sb.Append("��������Ϸ״̬ ");
                    sb.Append(intStatusN1 + "\n\n\n");
                }
                else
                {
                    sb.Append("���������� ");
                    sb.Append(intOldTurnN1 + "\n");
                    sb.Append("���������� ");
                    sb.Append(intTurnN1 + "\n");
                    sb.Append("��������Ϸ״̬ ");
                    sb.Append(intStatusN1 + "\n\n\n");
                }

            }
            catch
            {
                intTurnN1 = 0;
                intStatusN1 = 0;
                intOldTurnN1 = 0;
            }

            sb.Append("�Ϸ���\n");
            try
            {
                string strcon1 = DBConnection.Get30GamePhoneConnString(1, "XBA");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(2, "XBA"), CommandType.Text, strSQL);
                intTurnS1 = (int)dr["Turn"];
                intStatusS1 = (byte)dr["Status"];
                intOldTurnS1 = (int)dr["OldTurn"];

                if (intTurnS1 == 27)
                {
                    sb.Append("�Ϸ������� ");
                    sb.Append(intOldTurnS1 + "\n");
                    sb.Append("�Ϸ������� ");
                    sb.Append("[" + intTurnS1 + "]\n");
                    sb.Append("�Ϸ�����Ϸ״̬ ");
                    sb.Append(intStatusS1 + "\n\n\n");
                }
                else
                {
                    sb.Append("�Ϸ������� ");
                    sb.Append(intOldTurnS1 + "\n");
                    sb.Append("�Ϸ������� ");
                    sb.Append(intTurnS1 + "\n");
                    sb.Append("�Ϸ�����Ϸ״̬ ");
                    sb.Append(intStatusS1 + "\n\n\n");
                }
            }
            catch
            {
                intTurnS1 = 0;
                intStatusS1 = 0;
                intOldTurnS1 = 0;
            }

            sb.Append("�����\n");
            try
            {
                string strcon = DBConnection.Get30GamePhoneConnString(5, "XBA");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(5, "XBA"), CommandType.Text, strSQL);
                intTurnS3 = (int)dr["Turn"];
                intStatusS3 = (byte)dr["Status"];
                intOldTurnS3 = (int)dr["OldTurn"];

                if (intTurnS3 == 27)
                {
                    sb.Append("��������� ");
                    sb.Append(intOldTurnS3 + "\n");
                    sb.Append("��������� ");
                    sb.Append("[" + intTurnS3 + "]\n");
                    sb.Append("�������Ϸ״̬ ");
                    sb.Append(intStatusS3 + "\n\n\n");
                }
                else
                {
                    sb.Append("��������� ");
                    sb.Append(intOldTurnS3 + "\n");
                    sb.Append("��������� ");
                    sb.Append(intTurnS3 + "\n");
                    sb.Append("�������Ϸ״̬ ");
                    sb.Append(intStatusS3 + "\n\n\n");
                }

            }
            catch
            {
                intTurnS3 = 0;
                intStatusS3 = 0;
                intOldTurnS3 = 0;
            }

            sb.Append("�汱��\n");
            try
            {
                string strcon = DBConnection.Get30GamePhoneConnString(6, "XBA");
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(6, "XBA"), CommandType.Text, strSQL);
                intTurnS4 = (int)dr["Turn"];
                intStatusS4 = (byte)dr["Status"];
                intOldTurnS4 = (int)dr["OldTurn"];

                if (intTurnS4 == 27)
                {
                    sb.Append("�汱������ ");
                    sb.Append(intOldTurnS4 + "\n");
                    sb.Append("�汱������ ");
                    sb.Append("[" + intTurnS4 + "]\n");
                    sb.Append("�汱����Ϸ״̬ ");
                    sb.Append(intStatusS4 + "\n\n\n");
                }
                else
                {
                    sb.Append("�汱������ ");
                    sb.Append(intOldTurnS4 + "\n");
                    sb.Append("�汱������ ");
                    sb.Append(intTurnS4 + "\n");
                    sb.Append("�汱����Ϸ״̬ ");
                    sb.Append(intStatusS4 + "\n\n\n");
                }

            }
            catch
            {
                intTurnS4 = 0;
                intStatusS4 = 0;
                intOldTurnS4 = 0;
            }

            sb.Append("���˷�\n");
            try
            {
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(7, "XBA"), CommandType.Text, strSQL);
                intTurnXBA7 = (int)dr["Turn"];
                intStatusXBA7 = (byte)dr["Status"];
                intOldTurnXBA7 = (int)dr["OldTurn"];

                if (intTurnXBA7 == 27)
                {
                    sb.Append("���˷����� ");
                    sb.Append(intOldTurnXBA7 + "\n");
                    sb.Append("���˷����� ");
                    sb.Append("[" + intTurnXBA7 + "]\n");
                    sb.Append("���˷���Ϸ״̬ ");
                    sb.Append(intStatusXBA7 + "\n\n\n");
                }
                else
                {
                    sb.Append("���˷����� ");
                    sb.Append(intOldTurnXBA7 + "\n");
                    sb.Append("���˷����� ");
                    sb.Append(intTurnXBA7 + "\n");
                    sb.Append("���˷���Ϸ״̬ ");
                    sb.Append(intStatusXBA7 + "\n\n\n");
                }

            }
            catch
            {
                intTurnXBA7 = 0;
                intStatusXBA7 = 0;
                intOldTurnXBA7 = 0;
            }

            sb.Append("���Ƿ�\n");
            try
            {
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(50, "XBA"), CommandType.Text, strSQL);
                intTurnHC1 = (int)dr["Turn"];
                intStatusHC1 = (byte)dr["Status"];
                intOldTurnHC1 = (int)dr["OldTurn"];

                if (intTurnHC1 == 27)
                {
                    sb.Append("���Ƿ����� ");
                    sb.Append(intOldTurnHC1 + "\n");
                    sb.Append("���Ƿ����� ");
                    sb.Append("[" + intTurnHC1 + "]\n");
                    sb.Append("���Ƿ���Ϸ״̬ ");
                    sb.Append(intStatusHC1 + "\n\n\n");
                }
                else
                {
                    sb.Append("���Ƿ����� ");
                    sb.Append(intOldTurnHC1 + "\n");
                    sb.Append("���Ƿ����� ");
                    sb.Append(intTurnHC1 + "\n");
                    sb.Append("���Ƿ���Ϸ״̬ ");
                    sb.Append(intStatusHC1 + "\n\n\n");
                }

            }
            catch
            {
                intTurnHC1 = 0;
                intStatusHC1 = 0;
                intOldTurnHC1 = 0;
            }

            sb.Append("���Ϸ�\n");
            try
            {
                dr = SqlHelper.ExecuteDataRow(DBConnection.Get30GamePhoneConnString(8, "XBA"), CommandType.Text, strSQL);
                intTurnXBA8 = (int)dr["Turn"];
                intStatusXBA8 = (byte)dr["Status"];
                intOldTurnXBA8 = (int)dr["OldTurn"];

                if (intTurnXBA8 == 27)
                {
                    sb.Append("���Ϸ����� ");
                    sb.Append(intOldTurnXBA8 + "\n");
                    sb.Append("���Ϸ����� ");
                    sb.Append("[" + intTurnXBA8 + "]\n");
                    sb.Append("���Ϸ���Ϸ״̬ ");
                    sb.Append(intStatusXBA8 + "\n\n\n");
                }
                else
                {
                    sb.Append("���Ϸ����� ");
                    sb.Append(intOldTurnXBA8 + "\n");
                    sb.Append("���Ϸ����� ");
                    sb.Append(intTurnXBA8 + "\n");
                    sb.Append("���Ϸ���Ϸ״̬ ");
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
                    sb.Append("CGA1���� ");
                    sb.Append(intOldTurnCGA1 + "\n");
                    sb.Append("CGA1���� ");
                    sb.Append("[" + intTurnCGA1 + "]\n");
                    sb.Append("CGA1��Ϸ״̬ ");
                    sb.Append(intStatusCGA1 + "\n\n\n");
                }
                else
                {
                    sb.Append("CGA1���� ");
                    sb.Append(intOldTurnCGA1 + "\n");
                    sb.Append("CGA1���� ");
                    sb.Append(intTurnCGA1 + "\n");
                    sb.Append("CGA1��Ϸ״̬ ");
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
                    sb.Append("CGA3���� ");
                    sb.Append(intOldTurnCGA3 + "\n");
                    sb.Append("CGA3���� ");
                    sb.Append("[" + intTurnCGA3 + "]\n");
                    sb.Append("CGA3��Ϸ״̬ ");
                    sb.Append(intStatusCGA3 + "\n\n\n");
                }
                else
                {
                    sb.Append("CGA3���� ");
                    sb.Append(intOldTurnCGA3 + "\n");
                    sb.Append("CGA3���� ");
                    sb.Append(intTurnCGA3 + "\n");
                    sb.Append("CGA3��Ϸ״̬ ");
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
                    sb.Append("171731���� ");
                    sb.Append(intOldTurn171731 + "\n");
                    sb.Append("171731���� ");
                    sb.Append("[" + intTurn171731 + "]\n");
                    sb.Append("171731��Ϸ״̬ ");
                    sb.Append(intStatus171731 + "\n\n\n");
                }
                else
                {
                    sb.Append("171731���� ");
                    sb.Append(intOldTurn171731 + "\n");
                    sb.Append("171731���� ");
                    sb.Append(intTurn171731 + "\n");
                    sb.Append("171731��Ϸ״̬ ");
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
                    sb.Append("171733���� ");
                    sb.Append(intOldTurn171733 + "\n");
                    sb.Append("171733���� ");
                    sb.Append("[" + intTurn171733 + "]\n");
                    sb.Append("171733��Ϸ״̬ ");
                    sb.Append(intStatus171733 + "\n\n\n");
                }
                else
                {
                    sb.Append("171733���� ");
                    sb.Append(intOldTurn171733 + "\n");
                    sb.Append("171733���� ");
                    sb.Append(intTurn171733 + "\n");
                    sb.Append("171733��Ϸ״̬ ");
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
                    sb.Append("171735���� ");
                    sb.Append(intOldTurn171735 + "\n");
                    sb.Append("171735���� ");
                    sb.Append("[" + intTurn171735 + "]\n");
                    sb.Append("171735��Ϸ״̬ ");
                    sb.Append(intStatus171735 + "\n\n\n");
                }
                else
                {
                    sb.Append("171735���� ");
                    sb.Append(intOldTurn171735 + "\n");
                    sb.Append("171735���� ");
                    sb.Append(intTurn171735 + "\n");
                    sb.Append("171735��Ϸ״̬ ");
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
                    sb.Append("171736���� ");
                    sb.Append(intOldTurn171736 + "\n");
                    sb.Append("171736���� ");
                    sb.Append("[" + intTurn171736 + "]\n");
                    sb.Append("171736��Ϸ״̬ ");
                    sb.Append(intStatus171736 + "\n\n\n");
                }
                else
                {
                    sb.Append("171736���� ");
                    sb.Append(intOldTurn171736 + "\n");
                    sb.Append("171736���� ");
                    sb.Append(intTurn171736 + "\n");
                    sb.Append("171736��Ϸ״̬ ");
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
                    sb.Append("TW1���� ");
                    sb.Append("[" + intTurnTW1 + "]\n");
                    sb.Append("TW1��Ϸ״̬ ");
                    sb.Append(intStatusTW1 + "\n\n\n");
                }
                else
                {
                    sb.Append("TW1���� ");
                    sb.Append(intTurnTW1 + "\n");
                    sb.Append("TW1��Ϸ״̬ ");
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
                    sb.Append("TW2���� ");
                    sb.Append("[" + intTurnTW2 + "]\n");
                    sb.Append("TW2��Ϸ״̬ ");
                    sb.Append(intStatusTW2 + "\n\n\n");
                }
                else
                {
                    sb.Append("TW2���� ");
                    sb.Append(intTurnTW2 + "\n");
                    sb.Append("TW2��Ϸ״̬ ");
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
                    sb.Append("51WAN3���� ");
                    sb.Append(intOldTurn51Wan3 + "\n");
                    sb.Append("51WAN3���� ");
                    sb.Append("[" + intTurn51Wan3 + "]\n");
                    sb.Append("51WAN3��Ϸ״̬ ");
                    sb.Append(intStatus51Wan3 + "\n\n\n");
                }
                else
                {
                    sb.Append("51WAN3���� ");
                    sb.Append(intOldTurn51Wan3 + "\n");
                    sb.Append("51WAN3���� ");
                    sb.Append(intTurn51Wan3 + "\n");
                    sb.Append("51WAN3��Ϸ״̬ ");
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
                    sb.Append("51Wan4���� ");
                    sb.Append(intOldTurn51Wan4 + "\n");
                    sb.Append("51Wan4���� ");
                    sb.Append("[" + intTurn51Wan4 + "]\n");
                    sb.Append("51Wan4��Ϸ״̬ ");
                    sb.Append(intStatus51Wan4 + "\n\n\n");
                }
                else
                {
                    sb.Append("51Wan4���� ");
                    sb.Append(intOldTurn51Wan4 + "\n");
                    sb.Append("51Wan4���� ");
                    sb.Append(intTurn51Wan4 + "\n");
                    sb.Append("51Wan4��Ϸ״̬ ");
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
                    sb.Append("DuoWan1���� ");
                    sb.Append(intOldTurnDW1 + "\n");
                    sb.Append("DuoWan1���� ");
                    sb.Append("[" + intTurnDW1 + "]\n");
                    sb.Append("DuoWan1��Ϸ״̬ ");
                    sb.Append(intStatusDW1 + "\n\n\n");
                }
                else
                {
                    sb.Append("DuoWan1���� ");
                    sb.Append(intOldTurnDW1 + "\n");
                    sb.Append("DuoWan1���� ");
                    sb.Append(intTurnDW1 + "\n");
                    sb.Append("DuoWan1��Ϸ״̬ ");
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
                    sb.Append("NBA1���� ");
                    sb.Append(intOldTurnNBA1 + "\n");
                    sb.Append("NBA1���� ");
                    sb.Append("[" + intTurnNBA1 + "]\n");
                    sb.Append("NBA1��Ϸ״̬ ");
                    sb.Append(intStatusNBA1 + "\n\n\n");
                }
                else
                {
                    sb.Append("NBA1���� ");
                    sb.Append(intOldTurnNBA1 + "\n");
                    sb.Append("NBA1���� ");
                    sb.Append(intTurnNBA1 + "\n");
                    sb.Append("NBA1��Ϸ״̬ ");
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
                    sb.Append("NBA2���� ");
                    sb.Append(intOldTurnNBA2 + "\n");
                    sb.Append("NBA2���� ");
                    sb.Append("[" + intTurnNBA2 + "]\n");
                    sb.Append("NBA2��Ϸ״̬ ");
                    sb.Append(intStatusNBA2 + "\n\n\n");
                }
                else
                {
                    sb.Append("NBA2���� ");
                    sb.Append(intOldTurnNBA2 + "\n");
                    sb.Append("NBA2���� ");
                    sb.Append(intTurnNBA2 + "\n");
                    sb.Append("NBA2��Ϸ״̬ ");
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
                    sb.Append("NBA5���� ");
                    sb.Append(intOldTurnNBA5 + "\n");
                    sb.Append("NBA5���� ");
                    sb.Append("[" + intTurnNBA5 + "]\n");
                    sb.Append("NBA5��Ϸ״̬ ");
                    sb.Append(intStatusNBA5 + "\n\n\n");
                }
                else
                {
                    sb.Append("NBA5���� ");
                    sb.Append(intOldTurnNBA5 + "\n");
                    sb.Append("NBA5���� ");
                    sb.Append(intTurnNBA5 + "\n");
                    sb.Append("NBA5��Ϸ״̬ ");
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
                    sb.Append("NBA6���� ");
                    sb.Append(intOldTurnNBA6 + "\n");
                    sb.Append("NBA6���� ");
                    sb.Append("[" + intTurnNBA6 + "]\n");
                    sb.Append("NBA6��Ϸ״̬ ");
                    sb.Append(intStatusNBA6 + "\n\n\n");
                }
                else
                {
                    sb.Append("NBA6���� ");
                    sb.Append(intOldTurnNBA6 + "\n");
                    sb.Append("NBA6���� ");
                    sb.Append(intTurnNBA6 + "\n");
                    sb.Append("NBA6��Ϸ״̬ ");
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
                    sb.Append("SINA1���� ");
                    sb.Append(intOldTurnSINA1 + "\n");
                    sb.Append("SINA1���� ");
                    sb.Append("[" + intTurnSINA1 + "]\n");
                    sb.Append("SINA1��Ϸ״̬ ");
                    sb.Append(intStatusSINA1 + "\n\n\n");
                }
                else
                {
                    sb.Append("SINA1���� ");
                    sb.Append(intOldTurnSINA1 + "\n");
                    sb.Append("SINA1���� ");
                    sb.Append(intTurnSINA1 + "\n");
                    sb.Append("SINA1��Ϸ״̬ ");
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
                    sb.Append("SINA3���� ");
                    sb.Append(intOldTurnSINA3 + "\n");
                    sb.Append("SINA3���� ");
                    sb.Append("[" + intTurnSINA3 + "]\n");
                    sb.Append("SINA3��Ϸ״̬ ");
                    sb.Append(intStatusSINA3 + "\n\n\n");
                }
                else
                {
                    sb.Append("SINA3���� ");
                    sb.Append(intOldTurnSINA3 + "\n");
                    sb.Append("SINA3���� ");
                    sb.Append(intTurnSINA3 + "\n");
                    sb.Append("SINA3��Ϸ״̬ ");
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
                    sb.Append("XiaoI���� ");
                    sb.Append(intOldTurnXiaoI + "\n");
                    sb.Append("XiaoI���� ");
                    sb.Append("[" + intTurnXiaoI + "]\n");
                    sb.Append("XiaoI��Ϸ״̬ ");
                    sb.Append(intStatusXiaoI + "\n\n\n");
                }
                else
                {
                    sb.Append("XiaoI���� ");
                    sb.Append(intOldTurnXiaoI + "\n");
                    sb.Append("XiaoI���� ");
                    sb.Append(intTurnXiaoI + "\n");
                    sb.Append("XiaoI��Ϸ״̬ ");
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
                    sb.Append("AS1���� ");
                    sb.Append(intOldTurnAS1 + "\n");
                    sb.Append("AS1���� ");
                    sb.Append("[" + intTurnAS1 + "]\n");
                    sb.Append("AS1��Ϸ״̬ ");
                    sb.Append(intStatusAS1 + "\n\n\n");
                }
                else
                {
                    sb.Append("AS1���� ");
                    sb.Append(intOldTurnAS1 + "\n");
                    sb.Append("AS1���� ");
                    sb.Append(intTurnAS1 + "\n");
                    sb.Append("AS1��Ϸ״̬ ");
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
                    sb.Append("CGC1���� ");
                    sb.Append(intOldTurnCGC1 + "\n");
                    sb.Append("CGC1���� ");
                    sb.Append("[" + intTurnCGC1 + "]\n");
                    sb.Append("CGC1��Ϸ״̬ ");
                    sb.Append(intStatusCGC1 + "\n\n\n");
                }
                else
                {
                    sb.Append("CGC1���� ");
                    sb.Append(intOldTurnCGC1 + "\n");
                    sb.Append("CGC1���� ");
                    sb.Append(intTurnCGC1 + "\n");
                    sb.Append("CGC1��Ϸ״̬ ");
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
                    sb.Append("TOM1���� ");
                    sb.Append(intOldTurnTOM1 + "\n");
                    sb.Append("TOM1���� ");
                    sb.Append("[" + intTurnTOM1 + "]\n");
                    sb.Append("TOM1��Ϸ״̬ ");
                    sb.Append(intStatusTOM1 + "\n\n\n");
                }
                else
                {
                    sb.Append("TOM1���� ");
                    sb.Append(intOldTurnTOM1 + "\n");
                    sb.Append("TOM1���� ");
                    sb.Append(intTurnTOM1 + "\n");
                    sb.Append("TOM1��Ϸ״̬ ");
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
                    sb.Append("KX1���� ");
                    sb.Append(intOldTurnKX1 + "\n");
                    sb.Append("KX1���� ");
                    sb.Append("[" + intTurnKX1 + "]\n");
                    sb.Append("KX1��Ϸ״̬ ");
                    sb.Append(intStatusKX1 + "\n\n\n");
                }
                else
                {
                    sb.Append("KX1���� ");
                    sb.Append(intOldTurnKX1 + "\n");
                    sb.Append("KX1���� ");
                    sb.Append(intTurnKX1 + "\n");
                    sb.Append("KX1��Ϸ״̬ ");
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
                    sb.Append("SOHU1���� ");
                    sb.Append(intOldTurnSOHU1 + "\n");
                    sb.Append("SOHU1���� ");
                    sb.Append("[" + intTurnSOHU1 + "]\n");
                    sb.Append("SOHU1��Ϸ״̬ ");
                    sb.Append(intStatusSOHU1 + "\n\n\n");
                }
                else
                {
                    sb.Append("SOHU1���� ");
                    sb.Append(intOldTurnSOHU1 + "\n");
                    sb.Append("SOHU1���� ");
                    sb.Append(intTurnSOHU1 + "\n");
                    sb.Append("SOHU1��Ϸ״̬ ");
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
                    sb.Append("5S1���� ");
                    sb.Append(intOldTurn5S1 + "\n");
                    sb.Append("5S1���� ");
                    sb.Append("[" + intTurn5S1 + "]\n");
                    sb.Append("5S1��Ϸ״̬ ");
                    sb.Append(intStatus5S1 + "\n\n\n");
                }
                else
                {
                    sb.Append("5S1���� ");
                    sb.Append(intOldTurn5S1 + "\n");
                    sb.Append("5S1���� ");
                    sb.Append(intTurn5S1 + "\n");
                    sb.Append("5S1��Ϸ״̬ ");
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

        #region �����ض��ļ���ʽ��TXT�ĵ�
        /// <summary>
        /// �����ض��ļ���ʽ��TXT�ĵ�
        /// </summary>
        /// <param name="strSID">�û�SID</param>
        /// <param name="strSMSContent">��Ϣ����</param>
        /// <param name="type">1\SMS 2\PC</param>
        public static void CreateTxt(string strTXTContent)//�����ض��ļ���ʽ��TXT�ĵ�
        {
            Random rnbNum = new Random();
            string strPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string strNum = Convert.ToString(rnbNum.Next(000000, 999999));
            string filename = "BB_" + DateTime.Now.ToShortDateString() + "";
            FileStream fs = new FileStream(@strPath + filename + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("UTF-8"));//ͨ��ָ���ַ����뷽ʽ����ʵ�ֶԺ��ֵ�֧�֣��������ü��±��򿪲鿴���������

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

            //Base64���룬ע��UTF8
            code = Base64_decode(code);
            byte[] bytes = Convert.FromBase64String(code);
            decode = System.Text.Encoding.UTF8.GetString(bytes);
            //��ôȥ����������ݣ�sip:78516401@fetion.com.cn;p=187
            //�����õı��취���������������Ķ����ַ�ȥ����Ȼ�����󣳸��ַ������˭�и��õİ취�������
            //decode = Convert.ToString(Regex.Match(decode, @"\d{4,10}@fetion\.com\.cn"));
            //decode = decode.Substring(0, decode.Length - 3);
            return decode;
        }

        #region ��������λ�����㲢����
        /// <summary>
        /// ��������λ�����㲢����
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
            //�ٷ���
            DateTime dtimeFinishTimeBF1, dtimeFinishTimeNF2, dtimeFinishTimeHJ3, dtimeFinishTimeHR4, dtimeFinishTimeLN7, dtimeFinishTimeKRT8;
            //�Ʒ���
            DateTime dtimeFinishTimeCGA3, dtimeFinishTimeCGA4;
            //17173��
            DateTime dtimeFinishTime171731, dtimeFinishTime171733, dtimeFinishTime171736;
            //51WAN��
            DateTime dtimeFinishTime51WAN4;
            //������
            DateTime dtimeFinishTimeDW1;
            //�´���
            DateTime /*dtimeFinishTimeNBA1,*/ dtimeFinishTimeNBA2, dtimeFinishTimeNBA5;
            //������
            DateTime dtimeFinishTimeSINA1, dtimeFinishTimeSINA3;
            //СI��
            //DateTime dtimeFinishTimeXiaoI1;
            //������
            DateTime dtimeFinishTimeAS1;
            //������
            DateTime dtimeFinishTimeCGC1;
            //TOM��
            DateTime dtimeFinishTimeTOM1;
            //������
            //DateTime dtimeFinishTimeKX1;
            //�Ѻ���
            DateTime dtimeFinishTimeSOHU1;
            //5S��
            //DateTime dtimeFinishTime5S1;
            //̨����
            //DateTime dtimeFinishTimeTW1, dtimeFinishTimeTW2;
            //PPS��
            DateTime dtimeFinishTimePPS1, dtimeFinishTimePPS2;
            //YL��
            DateTime dtimeFinishTimeYL1;
            //TTB��
            DateTime dtimeFinishTimeTTB1;
            //SINAB��
            DateTime dtimeFinishTimeSINAB1;

            StringBuilder strBuild = new StringBuilder("");

            DateTime dt = DateTime.Now;

            ArrayList arrResult = new ArrayList();
            DataRow dr;

            //�������
            int intDays = 0;


            try
            {
                //������
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
                arrResult.Add("���������ӳ�ʱ��");
            }

            try
            {
                //�Ϸ���
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
                arrResult.Add("�Ϸ������ӳ�ʱ��");
            }

            try
            {
                //�����
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
                arrResult.Add("��������ӳ�ʱ��");
            }

            try
            {
                //���˷�
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
                arrResult.Add("���˷����ӳ�ʱ��");
            }

            try
            {
                //�������˷�
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
                arrResult.Add("�������˷����ӳ�ʱ��");
            }

            try
            {
                //���Ϸ�
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
                arrResult.Add("���Ϸ����ӳ�ʱ��");
            }

            try
            {
                //�Ʒ�����
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
                arrResult.Add("�Ʒ��������ӳ�ʱ��");
            }

            try
            {
                //�Ʒ��ķ�
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
                arrResult.Add("�Ʒ��ķ����ӳ�ʱ��");
            }

            try
            {
                //17173-1��
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
                arrResult.Add("17173-1�����ӳ�ʱ��");
            }

            try
            {
                //17173-3��
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
                arrResult.Add("17173-3�����ӳ�ʱ��");
            }

            try
            {
                //17173-6��
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
                arrResult.Add("17173-6�����ӳ�ʱ��");
            }

            try
            {
                //51WAN4��
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
                arrResult.Add("51WAN4�����ӳ�ʱ��");
            }

            try
            {
                //DW1��
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
                arrResult.Add("DW1�����ӳ�ʱ��");
            }

            #region ===================== ��09������� ====================== NBA1
            //try
            //{
            //    //NBA1��
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
            //    arrResult.Add("NBA1�����ӳ�ʱ��");
            //}
            #endregion =========================================================

            try
            {
                //NBA2��
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
                arrResult.Add("NBA2�����ӳ�ʱ��");
            }

            try
            {
                //NBA5��
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
                arrResult.Add("NBA5�����ӳ�ʱ��");
            }

            try
            {
                //SINA1��
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
                arrResult.Add("SINA1�����ӳ�ʱ��");
            }

            try
            {
                //SINA3��
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
                arrResult.Add("SINA3�����ӳ�ʱ��");
            }

            #region ===================== ��09������� ====================== XiaoI
            //try
            //{
            //    //XiaoI1��
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
            //    arrResult.Add("XiaoI1�����ӳ�ʱ��");
            //}
            #endregion =========================================================

            try
            {
                //AS1��
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
                arrResult.Add("AS1�����ӳ�ʱ��");
            }

            try
            {
                //CGC1��
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
                arrResult.Add("CGC1�����ӳ�ʱ��");
            }

            try
            {
                //TOM1��
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
                arrResult.Add("TOM1�����ӳ�ʱ��");
            }

            #region ===================== ��09������� ====================== KX1
            //try
            //{
            //    //KX1��
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
            //    arrResult.Add("KX1�����ӳ�ʱ��");
            //}
            #endregion =========================================================

            try
            {
                //SOHU1��
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
                arrResult.Add("SOHU1�����ӳ�ʱ��");
            }

            #region ===================== ��09������� ====================== 5S1
            //try
            //{
            //    //5S1��
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
            //    arrResult.Add("5S1�����ӳ�ʱ��");
            //}
            #endregion =========================================================

            #region ===================== ��09������� ====================== TW1 TW2
            //try
            //{
            //    //TW1��
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
            //    arrResult.Add("TW1�����ӳ�ʱ��");
            //}

            //try
            //{
            //    //TW2��
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
            //    arrResult.Add("TW2�����ӳ�ʱ��");
            //}
            #endregion =========================================================

            try
            {
                //PPS1��
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
                arrResult.Add("PPS1�����ӳ�ʱ��");
            }

            try
            {
                //PPS2��
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
                arrResult.Add("PPS2�����ӳ�ʱ��");
            }

            //try
            //{
            //    //YL1��
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
            //    arrResult.Add("YL1�����ӳ�ʱ��");
            //}

            try
            {
                //TTB1��
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
                arrResult.Add("TTB1�����ӳ�ʱ��");
            }

            try
            {
                //SINAB1��
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
                arrResult.Add("SINAB1�����ӳ�ʱ��");
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
            //�ٷ���
            DateTime dtimeFinishTimeFB1, dtimeFinishTimeFB2, dtimeFinishTimeFB3, dtimeFinishTimeFB4;
            //������
            DateTime dtimeFinishTimeAS1;
            //������
            DateTime dtimeFinishTimeDW1, dtimeFinishTimeDW2;
            //17173��
            DateTime dtimeFinishTime171731, dtimeFinishTime171732, dtimeFinishTime171733, dtimeFinishTime171734, dtimeFinishTime171735, dtimeFinishTime171736;
            //51WAN��
            DateTime dtimeFinishTime51WAN1;
            //�´���
            DateTime dtimeFinishTimeNBA1;
            //�г���
            DateTime dtimeFinishTimeCSL1, dtimeFinishTimeCSL2, dtimeFinishTimeCSL3, dtimeFinishTimeCSL4, dtimeFinishTimeCSL5;
            //��̳��
            DateTime dtimeFinishTimeTT1, dtimeFinishTimeTT2;
            //21CN��
            DateTime dtimeFinishTime21CN1;
            //ESL��
            DateTime dtimeFinishTimeESL1;
            //PPSF��
            DateTime dtimeFinishTimePPSF1;

            StringBuilder strBuild = new StringBuilder("");

            DateTime dt = DateTime.Now;

            ArrayList arrResult = new ArrayList();
            DataRow dr;

            //�������
            byte byteDays = 0;

            try
            {
                //FB1��
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
                arrResult.Add("FB-1�����ӳ�ʱ��");
            }

            try
            {
                //FB3��
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
                arrResult.Add("FB-3�����ӳ�ʱ��");
            }

            try
            {
                //FB4��
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
                arrResult.Add("FB-4�����ӳ�ʱ��");
            }

            try
            {
                //ASF1��
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
                arrResult.Add("AS-F1�����ӳ�ʱ��");
            }

            try
            {
                //DWF1��
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
                arrResult.Add("DW-F1�����ӳ�ʱ��");
            }

            try
            {
                //DWF2��
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
                arrResult.Add("DW-F2�����ӳ�ʱ��");
            }

            try
            {
                //17173F1��
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
                arrResult.Add("17173-F1�����ӳ�ʱ��");
            }

            try
            {
                //17173F4��
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
                arrResult.Add("17173-F4�����ӳ�ʱ��");
            }

            try
            {
                //17173F6��
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
                arrResult.Add("17173-F6�����ӳ�ʱ��");
            }

            try
            {
                //51WANF1��
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
                arrResult.Add("51WAN-F1�����ӳ�ʱ��");
            }

            try
            {
                //NBAF1��
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
                arrResult.Add("NBA-F1�����ӳ�ʱ��");
            }

            try
            {
                //CSLF1��
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
                arrResult.Add("CSL-F1�����ӳ�ʱ��");
            }

            try
            {
                //CSLF2��
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
                arrResult.Add("CSL-F2�����ӳ�ʱ��");
            }

            try
            {
                //CSLF3��
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
                arrResult.Add("CSL-F3�����ӳ�ʱ��");
            }

            try
            {
                //CSLF4��
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
                arrResult.Add("CSL-F4�����ӳ�ʱ��");
            }

            try
            {
                //CSLF5��
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
                arrResult.Add("CSL-F5�����ӳ�ʱ��");
            }

            try
            {
                //TT1��
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
                arrResult.Add("TT-F1�����ӳ�ʱ��");
            }

            try
            {
                //TT2��
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
                arrResult.Add("TT-F2�����ӳ�ʱ��");
            }

            try
            {
                //21CN1��
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
                arrResult.Add("21CN-F1�����ӳ�ʱ��");
            }

            try
            {
                //ESL1��
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
                arrResult.Add("ESL-F1�����ӳ�ʱ��");
            }

            try
            {
                //PPSF1��
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
                arrResult.Add("PPSF-F1�����ӳ�ʱ��");
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
