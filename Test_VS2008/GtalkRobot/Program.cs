using System;
using System.Collections.Generic;
using System.Text;

namespace CtalkRobot
{
    class Program
    {
        static void Main(string[] args)
        {
            CtalkClass gtalk = new CtalkClass();
            //RegisterUser reg = new RegisterUser();
            gtalk.Login("test1", "qweqwe123");
            //reg.RegUser("test6", "qweqwe123");
            Console.ReadKey();
        }
    }
}
