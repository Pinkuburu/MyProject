using System;
using System.Collections.Generic;
using System.Text;

namespace 登录注销监控
{
    class Program
    {
        static void Main(string[] args)
        {
            // 
            // TODO: 在此处添加代码以启动应用程序 
            // 
            TSControl.WTS_SESSION_INFO[] pSessionInfo = TSControl.SessionEnumeration();

            for (int i = 0; i < pSessionInfo.Length; i++)
            {
                if (pSessionInfo[i].SessionID != 0)
                {
                    try
                    {
                        int count = 0;
                        IntPtr buffer = IntPtr.Zero;
                        StringBuilder sb = new StringBuilder();
                        StringBuilder sa = new StringBuilder();
                        //上面的种种代码就不解释了其实重点就是这句话，WTSQuerySessionInformation函数
                        // 函数里参数 我知道的做一下介绍，IntPtr.Zero 这个不明白 还请大哥们告诉。 
                        //pSessionInfo[i]是 正在远程登录的用户信息（视乎也包括自己）。SessionID是这个用户的ID。。。 
                        //函数会通过这个ID 知道你要得到哪个用户的信息。 之后的TSControl.WTSInfoClass.WTSUserName呢就是你想要的到的哪种信息。 比如他这里是，WTSUserName 是要得到登录的名字。这个枚举里面还有很多，比如说ip地址啦，用户电脑名等等。
                        //sb是。。。。传出的参数了。你要的得到的结果要从sb里面拿。。。 
                        bool bsuccess = TSControl.WTSQuerySessionInformation(IntPtr.Zero, pSessionInfo[i].SessionID, TSControl.WTSInfoClass.WTSUserName, out sb, out count);

                        //这句话呢是，远程登录的用户，可以得到自己的电脑名，似乎这个功能没用，但是我做的一个项目确实用到了。不同的电脑通过远程登录，运行同样的程序，程序要判断不同的电脑要展现不同的画面。
                        //这里唯一的区别就是，第二个参数 变成了-1 ，这个-1应该是标识自己的意思。这样远程登录的用户会找到自己的信息。ip地址或者用户名等等。
                        //bool bsuccess = TSControl.WTSQuerySessionInformation(IntPtr.Zero, -1, TSControl.WTSInfoClass.WTSClientName, out sa, out count);

                        if (bsuccess)
                        {
                            Console.WriteLine(sb.ToString().Trim());
                            //如果用户名为Administrator，则注销它。您可以通过改变 Administrator注销其它的用户
                            if (sb.ToString().Trim() != "Administrator")
                            {                                
                                while (TSControl.WTSLogoffSession(0, pSessionInfo[i].SessionID, true))
                                {
                                    System.Threading.Thread.Sleep(3000);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
