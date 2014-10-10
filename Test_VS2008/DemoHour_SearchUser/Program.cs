using System;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;

namespace DemoHour_SearchUser
{
    class Program
    {
        public static int intCount = 1000581;

        public Program()  
        {  
        }        

        public void SearchUser(object state)
        {
            WebClient HTTPproc = new WebClient();
            HTTPproc.Encoding = Encoding.UTF8;
            while (true)
            {
                intCount++;
                string strContent = "";
                string strRequest = "http://www.demohour.com/" + intCount;
                strContent = HTTPproc.OpenRead(strRequest);

                try
                {
                    strContent = Regex.Match(strContent, @"(?<=\b(?=\w)strong\b(?!\w).*).*(?=</strong>)").Value;
                }
                catch (ArgumentException ex)
                {
                    // Syntax error in the regular expression
                }

                Console.WriteLine(intCount + "|" + strContent);
                AddLog(intCount + "|" + strContent);
                
                Thread.Sleep(100);
            }            
        }

        public void Work()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(SearchUser));
        }

        #region 日志 AddLog(string strContent)
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="strContent"></param>
        private static void AddLog(string strContent)
        {
            Log.WriteLog(LogFile.Trace, strContent);
        }

        private static void AddErrorLog(string strContent)
        {
            Log.WriteLog(LogFile.Error, strContent);
        }
        #endregion 

        static void Main(string[] args)
        {
            Program test1 = new Program();
            test1.Work();

            Console.ReadKey();
        }
    }
}
