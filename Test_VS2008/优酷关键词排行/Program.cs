using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace 优酷关键词排行
{
    class Program
    {
        static WebClient HTTPproc = new WebClient();
        static void Main(string[] args)
        {
            string strRequest = null;
            string strContent = null;

            int i = 0;

            HTTPproc.Encoding = System.Text.Encoding.UTF8;
            strRequest = "http://tip.so.youku.com/search_keys?k=%E8%90%A8%E5%85%8B%E6%96%AF%20%E5%A8%81%E5%B0%94%E8%AF%BA&type=video";

            while (i < 2000)
            {
                strContent = HTTPproc.OpenRead(strRequest);
                Console.WriteLine(strContent);
                i++;
                Thread.Sleep(1000);
            }
        }
    }
}
