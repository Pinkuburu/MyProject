using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Taobao.Top.Api;
using Taobao.Top.Api.Domain;
using Taobao.Top.Api.Request;
using System.Security.Cryptography;
using System.Threading;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {            
            //string strk = "";
            //Login("cupid0426@163.com", "677521++", "http://home.renren.com/Home.do", out strk);
            //strk = Encoding.GetEncoding("UTF-8");
            //Console.WriteLine(DBConnection.GetDataCenterConnString(1));
            //Console.WriteLine(ServerName());
            //Console.WriteLine(CheckDisk());
            //Console.WriteLine(ServerIP());
            //http://stat.hero.9wee.com/gm/modules/stat_global.php?server_group=%E5%AE%98%E6%96%B9%E6%9C%8D%E5%8A%A1%E5%99%A8&startday=2010-05-25&search=%E6%9F%A5+%E8%AF%A2
            //DateTime dt = DateTime.Now;
            //DateTime dtTime = Convert.ToDateTime("2010-05-24");
            //Console.WriteLine(strTime.AddDays(-5).ToShortDateString());

            //TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12029810", "3052030e84f8f7b2094f6900dec7bdb5");

            ///// 得到单个用户信息(taobao.user.get)
            //DynamicTopRequest req = new DynamicTopRequest("taobao.user.get");
            //req.AddTextParameter("fields", "user_id,nick,sex,buyer_credit,seller_credit,location.country,created,last_visit,location.zip,birthday,type,has_more_pic,item_img_num,item_img_size,prop_img_num,prop_img_size,auto_repost,promoted_type,status,alipay_bind,consumer_protection");
            //req.AddTextParameter("nick", "cupid0426");
            //req.AddTextParameter("alipay_no", "cupid0426");
            //string rsp = client.GetResponse(req);
            //Console.WriteLine(rsp);
            //Console.ReadKey();
            //Console.WriteLine(qqPwdEncrypt.Encrypt("qweqwe123","!5I0"));

            //data={"fid":"700859715"}&keyName=8a57faea3ff0c2cd0&requestSig=1a6ddecf35addd3f4d8b648511f0bcff
            DateTime dt = DateTime.Now;
            Console.WriteLine(dt.ToShortDateString());
            Console.WriteLine(dt.AddDays(3).ToShortDateString());
            Console.WriteLine(Sig("0ce11638979c40550cbee086b95850f8", "8054b3e8ece415448"));
            for (int i = 0; i < 1000000; i++)
            {
                Console.WriteLine(GetAllTime(1290999859- 1620 - Timestamp()));
                Thread.Sleep(1000);
            }

            Console.ReadKey();
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

        #region 时间格式化 GetAllTime(long time)
        /// <summary>
        /// 时间格式化
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetAllTime(long time)
        {
            string hh, mm, ss;//, fff;

            //long f = time % 100; // 毫秒   
            long s = time;// / 100; // 转化为秒
            long m = (s / 60)%60;     // 分
            long h = s / 3600;     // 时
            s = s % 60;     // 秒 

            //毫秒格式00
            //if (f < 10)
            //{
            //    fff = "0" + f.ToString();
            //}
            //else
            //{
            //    fff = f.ToString();
            //}

            //秒格式00
            if (s < 10)
            {
                ss = "0" + s.ToString();
            }
            else
            {
                ss = s.ToString();
            }

            //分格式00
            if (m < 10)
            {
                mm = "0" + m.ToString();
            }
            else
            {
                mm = m.ToString();
            }

            //时格式00
            if (h < 10)
            {
                hh = "0" + h.ToString();
            }
            else
            {
                hh = h.ToString();
            }

            //返回 hh:mm:ss.ff            
            return hh + ":" + mm + ":" + ss;// +"." + fff;
        }
        #endregion 时间格式化 GetAllTime(long time)

        #region 得到Sig值 Sig(string strKey, string strKeyName)
        /// <summary>
        /// 得到Sig值
        /// </summary>
        /// <param name="strKey">Key</param>
        /// <param name="strKeyName">KeyName</param>
        /// <returns></returns>
        private static string Sig(string strKey, string strKeyName)
        {
            int intInc = 0;
            int intkey = 0;
            string strKeyMD5_2 = null;
            string strSig = null;

            #region 返回KeyName的值
            switch (strKeyName)
            {
                case "97ba558178f22af9":
                    intInc = 1;
                    break;
                case "8a57faa3ff0c2cd0":
                    intInc = 2;
                    break;
                case "b05395426617a666":
                    intInc = 3;
                    break;
                case "8054b38ece415448":
                    intInc = 4;
                    break;
                case "5a0815d2500be4c3":
                    intInc = 5;
                    break;
                case "cb47e040c444bb13":
                    intInc = 6;
                    break;
                case "4f0b466d4e838204":
                    intInc = 7;
                    break;
                case "9bb033dd03a03a21":
                    intInc = 8;
                    break;
                case "a548d6aefbeb32e0":
                    intInc = 9;
                    break;
                case "c156d1c03531d5f6":
                    intInc = 10;
                    break;
            }
            #endregion 返回KeyName的值

            strKeyMD5_2 = UserMd5(UserMd5(strKey));
            strKeyMD5_2 = strKeyMD5_2.Substring(1, 6);
            strKeyMD5_2 = UserMd5(strKeyMD5_2);
            strKeyMD5_2 = strKeyMD5_2.Substring(0, 6);
            intkey = Int32.Parse(strKeyMD5_2, System.Globalization.NumberStyles.AllowHexSpecifier);
            intkey = intkey + intInc;
            strSig = UserMd5(Convert.ToString(intkey));

            return strSig;
        }
        #endregion 得到Sig值 Sig(string strKey, string strKeyName)

        #region MD5加密算法 UserMd5(string str)
        /// <summary>
        /// MD5加密算法
        /// </summary>
        /// <param name="str">输入字符串</param>
        /// <returns>输出32位小写MD5加密值</returns>
        static string UserMd5(string str)
        {
            string cl = str;
            string pwd = "";
            MD5 md5 = MD5.Create();//实例化一个md5对像  
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　  
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得  
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
                pwd = pwd + s[i].ToString("x2");
            }
            return pwd;
        }
        #endregion

        #region QQ密码加密算法
        /// <summary>
        /// QQ密码加密算法
        /// </summary>
        public static class qqPwdEncrypt
        {
            /// <summary>
            /// 计算网页上QQ登录时密码加密后的结果
            /// </summary>
            /// <param name="pwd" />QQ密码</param>
            /// <param name="verifyCode" />验证码</param>
            /// <returns></returns>
            public static String Encrypt(string pwd, string verifyCode)
            {
                return (md5(md5_3(pwd).ToUpper() + verifyCode.ToUpper())).ToUpper();
            }
            /// <summary>
            /// 计算字符串的三次MD5
            /// </summary>
            /// <param name="s" /></param>
            /// <returns></returns>
            private static String md5_3(String s)
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);

                bytes = md5.ComputeHash(bytes);
                bytes = md5.ComputeHash(bytes);
                bytes = md5.ComputeHash(bytes);

                md5.Clear();

                string ret = "";
                for (int i = 0; i < bytes.Length; i++)
                {
                    ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
                }

                return ret.PadLeft(32, '0');
            }
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
        }
        #endregion QQ密码加密算法

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
                    result += MyDrive.Name + "----" + MyDrive.VolumeLabel + "----" + MyDrive.TotalSize / 1024 / 1024 + "----" + MyDrive.TotalFreeSpace / 1024 / 1024 + "----" + MyDrive.TotalFreeSpace * 100 / MyDrive.TotalSize + "|";
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
        /// <param name="strCity"></param>
        /// <returns></returns>
        public static string Weather()//获取某地区天气信息string strCity
        {
            //string strMobileData = strCity;
            string strWeather = "";
            string strUrl = "http://www.freeshua.com/work.php?t=2&shop_url=1&order_status=0";
            HttpWebRequest oRequest = (HttpWebRequest)WebRequest.Create(strUrl);
            HttpWebResponse oResponse = (HttpWebResponse)oRequest.GetResponse();
            StreamReader sr = new StreamReader(oResponse.GetResponseStream(), System.Text.Encoding.GetEncoding("gb2312")); //如果是显示出来的乱码，改utf-8。
            string sResultContents = sr.ReadToEnd();//网页的HTML就在变量sResultContents 中
            oResponse.Close();
            StringBuilder abc = new StringBuilder("");
            try
            {
                int start = sResultContents.IndexOf("<div class=\"renwunr\">");
                int end = sResultContents.IndexOf("条记录", start + 1);
                sResultContents = sResultContents.Substring(start, end - start);

                Regex re = new Regex(@"saveUserInfo\(\d,\d,\d{1,10},event\)", RegexOptions.IgnoreCase);
                MatchCollection mc = re.Matches(sResultContents);

                

                foreach (string i in mc)
                {
                    abc.Append(i.ToString());
                }

                //sResultContents = Convert.ToString(Regex.Matches(sResultContents, @"saveUserInfo\(\d,\d,\d{1,10},event\)"));

            //    sResultContents = sResultContents.Replace("<table class=\"ts std\"><tr><td><div style=\"float:left\" class=med><FONT color=#cc0033>", "");
            //    sResultContents = sResultContents.Replace("</FONT></div><div style=\"float:left\">&nbsp;-&nbsp;", "\\n温度：");

            //    int start_sub = sResultContents.IndexOf("<a href=\"http://www.google");
            //    int end_sub = sResultContents.IndexOf("iGoogle</a>");
            //    sResultContents = sResultContents.Remove(start_sub, end_sub - start_sub);

            //    sResultContents = sResultContents.Replace("iGoogle</a></div><tr><td><div style=\"padding:5px;float:left\"><div style=\"font-size:140%\"><b>", "");
            //    sResultContents = sResultContents.Replace("</b></div><div>", "\\n");
            //    sResultContents = sResultContents.Replace("<b>", "");
            //    sResultContents = sResultContents.Replace("</b>", "");
            //    sResultContents = sResultContents.Replace("</div></div><div align=center style=\"padding:5px;float:left\">", "\\n");

            //    start_sub = sResultContents.IndexOf("<br><img style=\"border:1px solid");
            //    end_sub = sResultContents.IndexOf("\" title=\"");
            //    sResultContents = sResultContents.Remove(start_sub, end_sub - start_sub + 9);

            //    start_sub = sResultContents.IndexOf("\" width=40 height=40");
            //    sResultContents = sResultContents.Remove(start_sub);

            //    sResultContents = sResultContents.Replace("<br>", "，");
            //    sResultContents = sResultContents.Replace("，湿度：", "\\n湿度：");
            }
            catch
            {
                sResultContents = "查无此地区或输入有误！";
            }
            strWeather = sResultContents;
            return abc.ToString();
        }
        #endregion

        #region 生成特定文件格式的TXT文档
        /// <summary>
        /// 生成特定文件格式的TXT文档
        /// </summary>
        /// <param name="strResultContents"></param>
        public static void CreateTxt(string strSMSContent)//生成特定文件格式的TXT文档
        {
            Random rnbNum = new Random();
            string strNum = Convert.ToString(rnbNum.Next(000000, 999999));

            string filename = "13406802804_" + strNum + "";
            FileStream fs = new FileStream(@".\" + filename + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("UTF-8"));//通过指定字符编码方式可以实现对汉字的支持，否则在用记事本打开查看会出现乱码
            sw.Flush();
            sw.BaseStream.Seek(0, SeekOrigin.Begin);
            sw.WriteLine(strSMSContent);
            sw.Flush();
            sw.Close();
        }
        #endregion

        public static void Login(string UserName, string UserPwd, string LoginUrl, out string Txt)
        {
            try
            {
                //定义Cookie容器
                CookieContainer CookieArray = new CookieContainer();

                //创建Http请求
                HttpWebRequest LoginHttpWebRequest = (HttpWebRequest)WebRequest.Create(LoginUrl);

                //登录数据
                string LoginData = "email=" + UserName + "&password=" + UserPwd;
                //数据被传输类型
                LoginHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                //数据长度
                LoginHttpWebRequest.ContentLength = LoginData.Length;
                //数据传输方法 get或post
                LoginHttpWebRequest.Method = "POST";
                //设置HttpWebRequest的CookieContainer为刚才建立的那个CookieArray  
                LoginHttpWebRequest.CookieContainer = CookieArray;
                //获取登录数据流
                Stream myRequestStream = LoginHttpWebRequest.GetRequestStream();
                //StreamWriter
                StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
                //把数据写入HttpWebRequest的Request流  
                myStreamWriter.Write(LoginData);

                //关闭打开对象     
                myStreamWriter.Close();

                myRequestStream.Close();

                //新建一个HttpWebResponse     
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)LoginHttpWebRequest.GetResponse();

                //获取一个包含url的Cookie集合的CookieCollection     
                myHttpWebResponse.Cookies = CookieArray.GetCookies(LoginHttpWebRequest.RequestUri);

                WebHeaderCollection a = myHttpWebResponse.Headers;

                Stream myResponseStream = myHttpWebResponse.GetResponseStream();

                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("gb2312"));

                Txt = myStreamReader.ReadToEnd();

                //把数据从HttpWebResponse的Response流中读出     
                myStreamReader.Close();

                myResponseStream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static UInt32 UnixStamp()
        {
            TimeSpan ts = DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return Convert.ToUInt32(ts.TotalSeconds);
        }
    }
}
