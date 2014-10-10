using System;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Threading;
using System.Text.RegularExpressions;

namespace Dialup
{
    class Program
    {
        static void Main(string[] args)
        {
            string strADSLusername = "";
            string strADSLpassword = "";
            int intDialMode = 0;
            int intCount = 0;

            Console.Write("ADSL帐号：");
            strADSLusername = Console.ReadLine();
            Console.Write("ADSL密码：");
            strADSLpassword = Console.ReadLine();
            //Console.Write("(0)ADSL直连/(1)路由器拔号：");
            //intDialMode = Convert.ToInt32(Console.ReadLine());
            Console.Write("投票次数：");
            intCount = Convert.ToInt32(Console.ReadLine());
            strADSLusername = "a053203289293";
            strADSLpassword = "mt200607";
            intDialMode = 0;

            for (int i = 1; i <= intCount; i++)
            {
                SendVote(strADSLusername, strADSLpassword, 0);
                Thread.Sleep(2000);
            }
            Console.WriteLine(">>>>投票完成<<<<");
            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }

        #region 投票方法 SendVote(string strADSLusername, string strADSLpassword)
        /// <summary>
        /// 投票方法
        /// </summary>
        /// <param name="strADSLusername">ADSL帐号</param>
        /// <param name="strADSLpassword">ADSL密码</param>
        private static void SendVote(string strADSLusername, string strADSLpassword, int intDialMode)
        {
            switch (intDialMode)
            {
                case 0:
                    ADSL_Dial(strADSLusername, strADSLpassword);
                    break;
                case 1:
                    Router_Dial();
                    break;
                default:
                    break;
            }


            Dialup.WebClient client = new Dialup.WebClient();
            client.Encoding = System.Text.Encoding.UTF8;//默认编码方式，根据需要设置其他类型
            try
            {
                Console.WriteLine("开始投票...");
                client.OpenRead("http://hi.games.sina.com.cn/Poll.php?project_id=2262&id=45");//普通get请求
                Console.WriteLine("投票完成...");
                try
                {
                    string resultString = null;
                    try
                    {
                        resultString = client.OpenRead("http://hi.games.sina.com.cn/games/kefu/list_new.php?dpc=1");//普通get请求
                        resultString = Regex.Match(resultString, @"http://hi\.games\.sina\.com\.cn/Poll\.php\?project_id=2262&amp;id=45.*\s*.*").Value.Replace("http://hi.games.sina.com.cn/Poll.php?project_id=2262&amp;id=45\" target=\"_blank\"><img src=\"http://www.sinaimg.cn/gm/images/gm/new/017.jpg\" alt=\"支持他们\" width=\"104\" height=\"45\" /></a></td>", "").Replace("                          <td width=\"39%\" class=\"c5\">", "").Replace("</td>", "");
                        Console.WriteLine("当前票数：" + resultString);
                    }
                    catch (ArgumentException ex)
                    {
                        // Syntax error in the regular expression
                    }
                }
                catch
                {
                    SendVote(strADSLusername, strADSLpassword, 0);
                    Thread.Sleep(2000);
                }
            }
            catch
            {
                SendVote(strADSLusername, strADSLpassword, 0);
                Thread.Sleep(2000);
            }
        }
        #endregion 投票方法

        #region ADSL拔号模式 ADSL_Dial(string strADSLusername, string strADSLpassword)
        /// <summary>
        /// ADSL拔号模式
        /// </summary>
        /// <param name="strADSLusername"></param>
        /// <param name="strADSLpassword"></param>
        private static void ADSL_Dial(string strADSLusername, string strADSLpassword)
        {
            Dial AutoDial = new Dial();
            Console.WriteLine("断开ADSL连接...");
            AutoDial.Disconnect("ADSL");
            Thread.Sleep(5000);
            Console.WriteLine("ADSL连接断开...");

            Console.WriteLine("ADSL开始拔号...");
            AutoDial.Connect("ADSL", strADSLusername, strADSLpassword);
            Console.WriteLine("ADSL登录完成...");
        }
        #endregion ADSL拔号模式

        #region 路由器拔号模式 Router_Dial()
        /// <summary>
        /// 路由器拔号模式
        /// </summary>
        private static void Router_Dial()
        {
            //http://192.168.1.1/rstatus.tri  action=Disconnect&wan_pro=2&conn_stats=300&layout=sc  post
            //http://192.168.1.1/rstatus.tri  action=Connect&wan_pro=2&conn_stats=-1&layout=sc  post

            Dialup.WebClient clinet = new Dialup.WebClient();
            clinet.Encoding = System.Text.Encoding.Default;//默认编码方式，根据需要设置其他类型
            //Disconnect
            Console.WriteLine("断开路由器连接...");
            clinet.OpenRead("http://192.168.1.1/rstatus.tri", "action=Disconnect&wan_pro=2&conn_stats=300&layout=sc");
            Thread.Sleep(3000);
            Console.WriteLine("路由器断开完成...");
            //Connect
            Console.WriteLine("路由器开始拔号...");
            clinet.OpenRead("http://192.168.1.1/rstatus.tri", "action=Connect&wan_pro=2&conn_stats=-1&layout=sc");
            Thread.Sleep(5000);
            Console.WriteLine("路由器拔号完成...");
        }
        #endregion 路由器拔号模式

    }
}
