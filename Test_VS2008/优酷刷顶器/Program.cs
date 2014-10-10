using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;

namespace 优酷刷顶器
{
    class Program
    {
        static WebClient HTTPproc = new WebClient();

        static void Main(string[] args)
        {
            string strRequest = null;
            string strParameter = null;
            string strContent = null;
            string strVid = null;
            string strVid2 = null;
            string strVid3 = null;
            string strCount = null;

            if (args.Length > 1)
            {
                int i = 0;
                strVid = args[0].ToString();
                strVid2 = args[1].ToString();
                strVid3 = args[2].ToString();
                strCount = args[3].ToString();

                if (strCount != null)
                {
                    while (i < Convert.ToInt32(strCount))
                    {
                        if (strVid != "0")
                        {
                            strRequest = "http://stat.youku.com/player/addPlayerStaticReport";
                            strParameter = "videoid=" + strVid + "&starttime=0&ikuflag=n&speed=609006&time=1312967115&sid=131296711572717279977&segid=1&code=3001";
                            try
                            {
                                strContent = HTTPproc.OpenRead(strRequest, strParameter);
                                Console.WriteLine("视频ID:" + strVid + " 第 " + i + " 次");
                            }
                            catch
                            {
                                Console.WriteLine("视频ID:" + strVid + " 第 " + i + " 次失败");
                                Thread.Sleep(2000);
                                i--;
                            }
                        }

                        if (strVid2 != "0")
                        {
                            strRequest = "http://v.youku.com/QVideo/~ajax/updown?__rt=1&__ro=";
                            strParameter = "__ap={\"videoId\": \"" + strVid2 + "\", \"type\": \"up\"}";
                            try
                            {
                                strContent = PostModel(strRequest, UrlEncode(strParameter, "UTF-8"));
                                Console.WriteLine("视频顶第 " + i + " 次");
                            }
                            catch
                            {
                                Console.WriteLine("视频顶第 " + i + " 次失败");
                                Thread.Sleep(2000);
                                i--;
                            }
                            //Console.WriteLine(strContent);
                        }

                        if (strVid3 != "0")
                        {
                            strRequest = "http://v.youku.com/QVideo/~ajax/updown?__rt=1&__ro=";
                            strParameter = "__ap={\"videoId\": \"" + strVid3 + "\", \"type\": \"down\"}";
                            try
                            {
                                strContent = PostModel(strRequest, UrlEncode(strParameter, "UTF-8"));
                                Console.WriteLine("视频踩第 " + i + " 次");
                            }
                            catch
                            {
                                Console.WriteLine("视频踩第 " + i + " 次失败");
                                Thread.Sleep(2000);
                                i--;
                            }
                            //Console.WriteLine(strContent);
                        }
                        i++;
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    Console.WriteLine("没有输入数量");
                }
                
            }
            else
            {
                Console.WriteLine("缺少视频ID");
                Console.ReadKey();
            }
        }

        #region POST方法 PostModel(string strUrl, string strParm)
        /// <summary>
        /// POST方法
        /// </summary>
        /// <param name="strUrl"></param>
        /// <param name="strParm"></param>
        /// <returns></returns>
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
            Stream ReceiveStream = myResp.GetResponseStream();
            StreamReader readStream = new StreamReader(ReceiveStream, encode);
            Char[] read = new Char[256];
            int count = readStream.Read(read, 0, 256);
            string str = null;
            while (count > 0)
            {
                str += new String(read, 0, count);
                count = readStream.Read(read, 0, 256);
            }
            readStream.Close();
            myResp.Close();
            return str;
        }
        #endregion POST方法 PostModel(string strUrl, string strParm)

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

            string okChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._=&";
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
