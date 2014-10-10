using System;
using System.Collections.Generic;
using System.Text;

namespace QQRobot
{
    class Program
    {  
        static void Main(string[] args)
        {
            string strKey = null;
            Console.ForegroundColor = ConsoleColor.White;
            //声明INI操作类
            IniConfig IniOp = new IniConfig("");
            //声明QQ操作类
            QQClass QQClass = new QQClass();            
            
            Console.Write("QQ帐号:");
            string strQQ = Console.ReadLine();
            Console.Write("QQ密码:");
            StringBuilder password = new StringBuilder();
            ConsoleKeyInfo con;
            do
            {
                con = Console.ReadKey(true);
                password.Append(con.KeyChar.ToString());
                Console.Write("*");
            } while (con.Key != ConsoleKey.Enter);
            Console.WriteLine("\nQQ号:" + strQQ); 
            Console.WriteLine("密码:" + password.ToString());            

            if (strQQ.Trim() != "" && password.ToString().Trim() != "")
            {
                QQClass.Login(strQQ, password.ToString().Trim());
                if (strKey != "")
                {
                    ////读取分组
                    //Console.WriteLine(QQClass.GetUserGroup());
                    ////读取好友列表
                    //Console.WriteLine(QQClass.GetFriendList());
                    //QQClass.Command();
                }
            }
            Console.ReadKey();
        }
    }
}
