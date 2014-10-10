using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FxRobot
{
    class Program
    {
        static FxClass Fx = new FxClass();
        public static bool boolDebug = true;
        static void Main(string[] args)
        {
            string strMobile = null;
            string strPwd = null;
            string strCode = null;
            bool boolRun = true;
            int intStatus = 400;
                        
            Fx.Fx_ImageCode();
            Console.WriteLine("请输入验证码");
            strCode = Console.ReadLine();
            Console.Write("Mobile:");
            strMobile = Console.ReadLine();
            strMobile = "13406802804";
            Console.Write("Pass:");
            strPwd = Console.ReadLine();
            strPwd = "677521cupid0426";

            Console.WriteLine("\nMobile:" + strMobile);
            Console.WriteLine("Pass:" + strPwd);  
            //Console.WriteLine(Fx.Fx_Login(strMobile, strPwd, strCode, intStatus));
            ShowCommand(Fx.Fx_Login(strMobile, strPwd, strCode, intStatus), "");

            Program p = new Program();
            Thread nonParameterThread = new Thread(new ThreadStart(p.Run));
            nonParameterThread.Start();

            while (boolRun)
            {
                Console.Write(">");
                string cmd = Console.ReadLine();
                //退出服务
                if (cmd.ToLower() == "exit")
                {
                    break;
                }
                if (cmd.ToLower() == "list")
                {
                    Console.WriteLine(Fx.ShowFriendList());
                }
                if (cmd.ToLower() == "debug")
                {
                    boolDebug = true;
                }
                if (cmd.ToLower() == "debugon")
                {
                    boolDebug = false;
                }
            }
        }

        public void Run()
        {
            while (true)
            {
                ShowCommand("进入线程操作", "");
                ShowCommand(Fx.GetConnect(), "Debug");
            }
        }

        #region 显示命令行 ShowCommand(string strContent, string strColor)
        /// <summary>
        /// 显示命令行
        /// </summary>
        /// <param name="strContent"></param>
        /// <param name="strColor"></param>
        private static void ShowCommand(string strContent, string strColor)
        {
            if (boolDebug)
            {
                switch (strColor)
                {
                    case "Debug":
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(strContent);
                        break;
                    case "User":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(strContent);
                        break;
                    case "Cmd":
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(strContent);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(strContent);
                        break;
                }
            }
        }
        #endregion 显示命令行 ShowCommand(string strContent, string strColor)
    }
}
