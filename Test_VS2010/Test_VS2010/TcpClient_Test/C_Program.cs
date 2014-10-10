using System;
using SocketClass;
using System.Diagnostics;
using System.Threading;

namespace TcpClient_Test
{
    public class TcpClient
    {
        public TcpClient()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        static TcpCli cli1 = new TcpCli(new Coder(Coder.EncodingMothord.Default));
        public static void Main()
        {
            Console.WriteLine("Begin to Test TcpCli Class..");
            //ArrayList al = new ArrayList();

            TcpClient test = new TcpClient();
            

            cli1.Resovlver = new DatagramResolver("]}");
            cli1.ReceivedDatagram += new NetEvent(test.RecvData);
            cli1.DisConnectedServer += new NetEvent(test.ClientClose);
            cli1.ConnectedServer += new NetEvent(test.ClientConn);

            try
            {
                //命令控制循环
                while (true)
                {
                    Console.Write(">");
                    string cmd = Console.ReadLine();

                    if (cmd.ToLower() == "exit")
                    {
                        break;
                    }
                    if (cmd.ToLower() == "close")
                    {
                        //cli.Close();
                        continue;
                    }

                    if (cmd.ToLower().IndexOf("conn") != -1)
                    {
                        cmd = cmd.ToLower();
                        string[] para = cmd.Split(' ');

                        if (para.Length != 3)
                        {
                            Console.WriteLine("Error Command");
                            continue;
                        }

                        try
                        {
                            string conn = para[1];
                            ushort port = ushort.Parse(para[2]);
                            cli1.Connect(conn, port);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            break;
                        }

                        continue;
                    }

                    if (cmd.ToLower().IndexOf("send") != -1)
                    {
                        cmd = cmd.ToLower();
                        string[] para = cmd.Split(' ');

                        if (para.Length != 2)
                        {
                            Console.WriteLine("Error Command");
                        }
                        else
                        {
                            try
                            {
                                cli1.Send(para[1] + "]}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        continue;
                    }
                    Console.WriteLine("Unkown Command");

                }//end of while

                Console.WriteLine("End service");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        void ClientConn(object sender, NetEventArgs e)
        {
            string info = string.Format("A Client:{0} connect server :{1}", e.Client,
             e.Client.ClientSocket.RemoteEndPoint.ToString());

            Console.WriteLine(info);
            Console.Write(">");
        }

        void ClientClose(object sender, NetEventArgs e)
        {
            string info;

            if (e.Client.TypeOfExit == Session.ExitType.ExceptionExit)
            {
                info = string.Format("A Client Session:{0} Exception Closed.", e.Client.ID);
            }
            else
            {
                info = string.Format("A Client Session:{0} Normal Closed.", e.Client.ID);
            }

            Console.WriteLine(info);
            Console.Write(">");
        }

        void RecvData(object sender, NetEventArgs e)
        {
            string info = string.Format("recv data:{0} from:{1}.", e.Client.Datagram.Replace("]}", ""), e.Client);
            Console.WriteLine(info);
            Console.Write(">");

            if (e.Client.Datagram.Replace("]}", "").ToLower() == "time")
            {
                Console.WriteLine(CheckSystime());
                cli1.Send("The System time is:" + CheckSystime() + "]}");
            }

            if (e.Client.Datagram.Replace("]}", "").ToLower() == "cpu")
            {
                cli1.Send(PerformanceCounterFun("Processor", "_Total", "% Processor Time") + "]}");
            }
            Console.Write(">");
        }

        private string CheckSystime()
        {            
            DateTime dt = DateTime.Now;
            string strTime = dt.ToString();
            return strTime;
        }

        private float PerformanceCounterFun(string CategoryName, string InstanceName, string CounterName)
        {
            float cpuLoad = 0;

            PerformanceCounter pc = new PerformanceCounter(CategoryName, CounterName, InstanceName);
            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(1000);
                cpuLoad = pc.NextValue();
            }
            return cpuLoad;
        }
    }
}
