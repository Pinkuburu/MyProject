using System;
using System.Collections.Generic;
using System.Text;

namespace 中文参数测试
{
    class Program
    {
        static void Main(string[] args)
        {
            string strContent = args[1].ToString() + "--" + args[2].ToString();
            Console.WriteLine(strContent);
            Console.ReadKey();
        }
    }
}
