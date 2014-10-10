using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
namespace SysStartCheckUser
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool bCreatedNew;
            Mutex m = new Mutex(false, "SysStartCheckUserCntvs", out bCreatedNew);
            if (bCreatedNew)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1(args));
            }
        }
    }
}