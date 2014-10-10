using System;
using System.Collections.Generic;
using System.Text;

namespace 翻页测试
{
    class Program
    {
        static void Main(string[] args)
        {
            int intPageCount = 3;
            if (intPageCount % 8 > 0)
            {
                intPageCount = intPageCount / 8 + 1;
            }
            else
            {
                intPageCount = intPageCount / 8;
            }

            Console.WriteLine(intPageCount);
            Console.ReadKey();
        }
    }
}
