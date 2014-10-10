using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DZ抢楼机X3
{
    class Program
    {
        ///// <summary>
        ///// 应用程序的主入口点。
        ///// </summary>
        //[STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new Main_Form());
        //}
        [STAThread]
        static void Main(string[] args)
        {
            Main_Form mf = new Main_Form();
            mf.Show();
            Console.ReadLine();
            Console.WriteLine("asdfasdfasdf");
        }
    }
}
