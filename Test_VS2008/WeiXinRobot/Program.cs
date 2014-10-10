using System;
using System.Threading;

namespace WeiXinRobot
{
    class Program
    {
        static void Main(string[] args)
        {
            //我的微信ID:128623
            bool blMsg = false;
            bool blLogin = false;
            blLogin = ClassWX.Login("182536608@qq.com", "loveemma++");
            Console.WriteLine(blLogin);
            if (true)
            {
                Console.WriteLine("登陆成功");
                blMsg = ClassWX.SendMSG("128623", "这就是生活啊小胖儿");
                if (blMsg)
                {
                    Console.WriteLine("消息发送成功");
                }
                else
                {
                    Console.WriteLine("消息发送失败");
                }                
            }
            else
            {
                Console.WriteLine("登陆失败");
            }
            Console.ReadKey();
        }
    }
}
