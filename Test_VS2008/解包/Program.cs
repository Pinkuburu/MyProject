using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace 解包
{
    class Program
    {
        static void Main(string[] args)
        {
            long longBase = TimeStamp();
            while (true)
            {
                Console.WriteLine(TimeStamp() - longBase);
                Thread.Sleep(1000);
            }            
            Console.ReadKey();
        }

        private static long TimeStamp()
        {
            long epoch = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000);
            return epoch;
        }
    }
}
