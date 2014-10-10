using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace GetToken
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            while (true)
            {
                Console.WriteLine(rnd.Next(100000, 999999));
                Thread.Sleep(3000);
            }            
        }
    }
}
