using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;

namespace 淘宝投票
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string strRequest = "http://survey.news.ifeng.com/poll.php?surveyId=14370&q_28566[]=118251";

            WebClient HTTPproc = new WebClient();
            HTTPproc.Encoding = System.Text.Encoding.UTF8;
            int i = 0;
            while (true)
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strRequest);
                req.Method = "GET";
                using (WebResponse wr = req.GetResponse())
                {
                    //在这里对接收到的页面内容进行处理  
                }
                i++;
                Console.WriteLine(i);
            }
        }
    }
}
