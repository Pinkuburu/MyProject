using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DemoHour_Login
{
    class Program
    {
        public static string strToken = "";//唯一Token                                           

        static void Main(string[] args)
        {
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = "http://www.demohour.com/login",//URL     必需项    
                Method = "get",//URL     可选项 默认为Get   
                Cookie = "",//字符串Cookie     可选项   
                Referer ="",//来源URL     可选项   
                Postdata = "",//Post数据     可选项GET时不需要写   
                Timeout = 100000,//连接超时时间     可选项默认为100000    
                ReadWriteTimeout = 30000,//写入Post数据超时时间     可选项默认为30000   
                UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)",//用户的浏览器类型，版本，操作系统     可选项有默认值   
                ContentType = "text/html",//返回类型    可选项有默认值   
                Allowautoredirect = false,//是否根据301跳转     可选项    
                Expect100Continue = true,
                ResultType = ResultType.String
            };
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            string cookie = result.Cookie;
            string request_method = "request_method=" + HttpCookieHelper.GetCookieValue("request_method", cookie) + ";";
            string _demohour_session = "_demohour_session=" + HttpCookieHelper.GetCookieValue("_demohour_session", cookie) + ";";
            cookie = request_method + _demohour_session;
            Console.WriteLine(cookie);

            try
            {
                strToken = Regex.Match(html, "(?<=content=\".*).*(?=\" name=\"csrf-token)").Value;//抓取登录Token
            }
            catch (ArgumentException ex)
            {
                strToken = "";
            }
            Console.WriteLine(strToken);

            http = new HttpHelper();
            item = new HttpItem()
            {
                URL = "http://www.demohour.com/session?_action=new",//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写   
                Cookie = cookie,//字符串Cookie     可选项   
                Referer = "http://www.demohour.com/login",//来源URL     可选项   
                Postdata = "utf8=%E2%9C%93&authenticity_token=" + UrlEncode(strToken, "UTF-8") + "&email=" + UrlEncode("cupid0426@163.com", "UTF-8") + "&password=" + UrlEncode("qweqwe123", "UTF-8"),//Post数据     可选项GET时不需要写   
                Timeout = 100000,//连接超时时间     可选项默认为100000    
                ReadWriteTimeout = 30000,//写入Post数据超时时间     可选项默认为30000   
                UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)",//用户的浏览器类型，版本，操作系统     可选项有默认值   
                ContentType = "text/html",//返回类型    可选项有默认值   
                Allowautoredirect = false,//是否根据301跳转     可选项   
                ResultType = ResultType.String
            };
            result = http.GetHtml(item);
            html = result.Html;
            cookie = result.Cookie;

            Console.ReadKey();
        }

        #region URLEncode UrlEncode(string str, string encode)
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
            string okChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.-";
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
