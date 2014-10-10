using System;
using System.Collections.Generic;
using System.Text;

namespace 命令行带参数启动
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(args.Length);

            if (args.Length > 0)
            {
                Console.WriteLine("参数内容：" + args[0]);
            }
            else
            {
                Console.WriteLine("无参数启动");
            }
            Console.ReadKey();

        }
    }
}
