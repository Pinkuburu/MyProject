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
            //����INI������
            IniConfig IniOp = new IniConfig("");
            //����QQ������
            QQClass QQClass = new QQClass();            
            
            Console.Write("QQ�ʺ�:");
            string strQQ = Console.ReadLine();
            Console.Write("QQ����:");
            StringBuilder password = new StringBuilder();
            ConsoleKeyInfo con;
            do
            {
                con = Console.ReadKey(true);
                password.Append(con.KeyChar.ToString());
                Console.Write("*");
            } while (con.Key != ConsoleKey.Enter);
            Console.WriteLine("\nQQ��:" + strQQ); 
            Console.WriteLine("����:" + password.ToString());            

            if (strQQ.Trim() != "" && password.ToString().Trim() != "")
            {
                QQClass.Login(strQQ, password.ToString().Trim());
                if (strKey != "")
                {
                    ////��ȡ����
                    //Console.WriteLine(QQClass.GetUserGroup());
                    ////��ȡ�����б�
                    //Console.WriteLine(QQClass.GetFriendList());
                    //QQClass.Command();
                }
            }
            Console.ReadKey();
        }
    }
}
