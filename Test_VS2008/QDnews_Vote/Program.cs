using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QDnews_Vote
{
    class Program
    {
        public static string strCookie = "";

        static void Main(string[] args)
        {
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = "http://vote.qingdaonews.com/baby/201405/huishan/index.php?p1=1&p2=3&jdfwkey=qnfh33",//URL     必需项
                Method = "GET",//URL     可选项 默认为Get
                Timeout = 15000,//连接超时时间     可选项默认为100000
                ReadWriteTimeout = 30000,//写入Post数据超时时间     可选项默认为30000
                IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",//用户的浏览器类型，版本，操作系统     可选项有默认值
                Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值
                ContentType = "text/html",//返回类型    可选项有默认值
            };
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            string cookie = result.Cookie;
            Console.WriteLine(html);
            Console.WriteLine(cookie);
            Console.WriteLine(HttpCookieHelper.GetCookieList(cookie)[0]);
            Console.ReadKey();
        }
    }
}
