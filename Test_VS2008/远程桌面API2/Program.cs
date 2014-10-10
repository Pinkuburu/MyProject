using System;
using System.Collections.Generic;
using System.Text;

namespace 远程桌面API2
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                String serverName = args[0];
                IList<string> slist = TSManager.ListSessions(args[0]);
                foreach (string s in slist)
                {
                    Console.WriteLine(s);
                }
            }
            else
            {
                Console.WriteLine("Usage: ");
                Console.WriteLine("> TSClient <ServerName>");
            }
        }
    }
}
