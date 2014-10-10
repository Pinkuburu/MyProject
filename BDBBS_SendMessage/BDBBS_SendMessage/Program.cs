using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BDBBS_SendMessage
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

            //调试选择器
            BD_Login_Form Login_Form = new BD_Login_Form();
            if (Login_Form.debug)
            {
                Application.Run(new BD_Main_Form());
            }
            else
            {
                Application.Run(new BD_Login_Form());
            }
        }
    }
}