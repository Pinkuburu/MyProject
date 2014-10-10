using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace Product_Count
{
    class Program
    {
        static void Main(string[] args)
        {
            string strKeywords = "";
            string strRequest = "http://s.taobao.com/search?q=";
            string strContent = "";
            string strWrite = "";
            string strSSS = "";

            Product_Count.WebClient HTTPproc = new WebClient();


            if (args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    try
                    {
                        HTTPproc.Encoding = System.Text.Encoding.Default;
                        strContent = HTTPproc.OpenRead(strRequest + UrlEncode(args[i].Trim()));

                        try
                        {
                            Regex regexObj_0 = new Regex(@"http://ju\.atpanel\.com/\?url=http://item\.taobao\.com/item\.htm\?id=\d*&.*title=.*"">");
                            Regex regexObj_1 = new Regex(@"<li class=""sale"">����ɽ�\d*��");
                            Regex regexObj_2 = new Regex(@"<li class=""price""><em>\d*.\d*");

                            Match matchResults_0 = regexObj_0.Match(strContent);
                            Match matchResults_1 = regexObj_1.Match(strContent);
                            Match matchResults_2 = regexObj_2.Match(strContent);

                            strWrite = "=========== " + args[i] + " ===========\r\n";
                            while (matchResults_0.Success)
                            {
                                // matched text: matchResults.Value
                                // match start: matchResults.Index
                                // match length: matchResults.Length

                                strWrite += matchResults_0.Value.ToString().Replace("http://ju.atpanel.com/?url=", "").Replace("&ad_id=&am_id=&cm_id=&pm_id=1500206164949e8479ce\" target=\"_blank\" class=\"EventCanSelect\" title=\"", "\r\n").Replace("\">", "");
                                strWrite += matchResults_1.Value.ToString().Replace("<li class=\"sale\">", "\r\n");//Regex.Match(strContent, @"<li class=""sale"">����ɽ�\d*��").Value.Replace("<li class=\"sale\">", "\r\n");
                                strWrite += matchResults_2.Value.ToString().Replace("<li class=\"price\"><em>", "\r\n");//Regex.Match(strContent, @"<li class=""price""><em>\d*.\d*").Value.Replace("<li class=\"price\"><em>", "\r\n");

                                matchResults_0 = matchResults_0.NextMatch();
                                matchResults_1 = matchResults_1.NextMatch();
                                matchResults_2 = matchResults_2.NextMatch();
                            }
                            strWrite += "\r\n============================";
                            Console.WriteLine(strWrite);                            
                        }
                        catch (ArgumentException ex)
                        {
                            // Syntax error in the regular expression
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    strSSS += strWrite + "\r\n";
                }
                CreateTxt(strSSS);
            }
            else
            {
                Console.Write("�����������ؼ��֣��м���'|'�ָ���");
                strKeywords = Console.ReadLine();

                string[] arrKeywords = strKeywords.Split('|');

                for (int i = 0; i < arrKeywords.Length; i++)
                {
                    try
                    {
                        HTTPproc.Encoding = System.Text.Encoding.Default;
                        strContent = HTTPproc.OpenRead(strRequest + UrlEncode(arrKeywords[i].Trim()));

                        try
                        {
                            Regex regexObj_0 = new Regex(@"http://ju\.atpanel\.com/\?url=http://item\.taobao\.com/item\.htm\?id=\d*&.*title=.*"">");
                            Regex regexObj_1 = new Regex(@"<li class=""sale"">����ɽ�\d*��");
                            Regex regexObj_2 = new Regex(@"<li class=""price""><em>\d*.\d*");

                            Match matchResults_0 = regexObj_0.Match(strContent);
                            Match matchResults_1 = regexObj_1.Match(strContent);
                            Match matchResults_2 = regexObj_2.Match(strContent);

                            strWrite = "=========== " + arrKeywords[i] + " ===========\r\n";
                            while (matchResults_0.Success)
                            {
                                // matched text: matchResults.Value
                                // match start: matchResults.Index
                                // match length: matchResults.Length

                                strWrite += matchResults_0.Value.ToString().Replace("http://ju.atpanel.com/?url=", "").Replace("&ad_id=&am_id=&cm_id=&pm_id=1500206164949e8479ce\" target=\"_blank\" class=\"EventCanSelect\" title=\"", "\r\n").Replace("\">", "");
                                strWrite += matchResults_1.Value.ToString().Replace("<li class=\"sale\">", "\r\n");//Regex.Match(strContent, @"<li class=""sale"">����ɽ�\d*��").Value.Replace("<li class=\"sale\">", "\r\n");
                                strWrite += matchResults_2.Value.ToString().Replace("<li class=\"price\"><em>", "\r\n");//Regex.Match(strContent, @"<li class=""price""><em>\d*.\d*").Value.Replace("<li class=\"price\"><em>", "\r\n");

                                matchResults_0 = matchResults_0.NextMatch();
                                matchResults_1 = matchResults_1.NextMatch();
                                matchResults_2 = matchResults_2.NextMatch();
                            }
                            strWrite += "\r\n============================";
                            Console.WriteLine(strWrite);
                        }
                        catch (ArgumentException ex)
                        {
                            // Syntax error in the regular expression
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
                CreateTxt(strWrite);
            }
            //Console.ReadKey();
        }

        #region Url���� UrlEncode(string url)
        /// <summary>
        /// Url����
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string UrlEncode(string url)
        {
            byte[] bs = Encoding.GetEncoding("GB2312").GetBytes(url);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bs.Length; i++)
            {
                if (bs[i] < 128)
                {
                    sb.Append((char)bs[i]);
                }
                else
                {
                    sb.Append("%" + bs[i++].ToString("x").PadLeft(2, '0'));
                    sb.Append("%" + bs[i].ToString("x").PadLeft(2, '0'));
                }
            } return sb.ToString();
        }
        #endregion

        #region �����ض��ļ���ʽ��TXT�ĵ�
        /// <summary>
        /// �����ض��ļ���ʽ��TXT�ĵ�
        /// </summary>
        public static void CreateTxt(string strContent)//�����ض��ļ���ʽ��TXT�ĵ�
        {
            DateTime dt = DateTime.Now;
            FileStream fs = new FileStream(@"Log" + dt.ToString("yyyy-MM-dd_HHmmss") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("UTF-8"));//ͨ��ָ���ַ����뷽ʽ����ʵ�ֶԺ��ֵ�֧�֣��������ü��±��򿪲鿴���������

            sw.Flush();
            sw.BaseStream.Seek(0, SeekOrigin.Begin);
            sw.WriteLine(strContent);
            sw.Flush();
            sw.Close();
        }
        #endregion
    }
}
