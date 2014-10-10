using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Happyisland
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Main_Form());
            Application.Run(new Login_Form());
        }
    }
}
