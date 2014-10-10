using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace 领取淘金币
{
    class Program
    {
        static WebClient HTTPproc = new WebClient();

        static void Main(string[] args)
        {            
            string strUserName = null;
            string strPassword = null;
            int intFriendCount = 0;

            TaobaoWapClass twc = new TaobaoWapClass();

            foreach (string strUser in twc.ReadTXT())
            {
                string strSID = null;
                string[] split = strUser.Split('|');
                strUserName = split[0];
                strPassword = split[1];
                strSID = twc.WapLogin(strUserName, strPassword);
                if (strSID.IndexOf("sid=") > -1)
                {
                    strSID = strSID.Replace("sid=", "");
                    intFriendCount = Convert.ToInt32(twc.FriendCount(strSID));

                    if (intFriendCount >= 5)
                    {
                        Console.WriteLine(twc.ReceiveCoin(strSID, strUserName));
                    }
                    else
                    {
                        Console.WriteLine("帐号：" + strUserName + " 好友数不够，无法领取");
                    }
                    //Thread.Sleep(5000);
                }
                else
                {
                    Console.WriteLine(strSID);
                }
            }
            Console.WriteLine("来，按下回车，赶紧让这一切结束。");
            Console.ReadKey();
        }
    }
}
