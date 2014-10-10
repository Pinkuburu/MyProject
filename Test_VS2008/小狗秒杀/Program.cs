using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Threading;

namespace 小狗秒杀
{
    class Program
    {
        static ArrayList al_Tid = new ArrayList();
        static WebClient HTTPproc = new WebClient();

        static void Main(string[] args)
        {
            string[] arrLinks = { "http://detail.tmall.com/item.htm?id=10361850957", "http://item.taobao.com/item.htm?id=10793059779", "http://item.taobao.com/item.htm?id=7910579201", "http://item.taobao.com/item.htm?id=12452217495", "http://item.taobao.com/item.htm?id=3566133469", "http://item.taobao.com/item.htm?id=10361850957", "http://item.taobao.com/item.htm?id=5151124125", "http://item.taobao.com/item.htm?id=9786825377", "http://item.taobao.com/item.htm?id=3566133469", "http://item.taobao.com/item.htm?id=3559016425" };

            foreach (string strLink in arrLinks)
            {
                Find_MS(strLink);
            }            
            Console.ReadKey();
        }

        private static void Find_MS(string strURL)
        {
            string strLink = null;
            string strContent = null;

            strContent = HTTPproc.OpenRead(strURL);

            if (HTTPproc.ResponseHeaders["Location"] != null)
            {
                strContent = AutoRedrict();                
            }

            if (strContent.IndexOf("即将开始") > -1)
            {
                Console.WriteLine("发现链接：" + strURL);
            }        
        }

        private static string AutoRedrict()
        {
            string strContent = null;
            string strURL = null;
            bool blCheck = true;

            while (blCheck)
            {
                if (HTTPproc.ResponseHeaders["Location"] != null)
                {
                    strURL = HTTPproc.ResponseHeaders["Location"].ToString();
                    strContent = HTTPproc.OpenRead(strURL);
                }
                else
                {
                    blCheck = false;
                }
            }

            return strContent;
        }
    }
}
