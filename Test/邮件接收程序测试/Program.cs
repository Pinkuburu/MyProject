using System;
using System.Collections.Generic;
using System.Text;

namespace 邮件接收程序测试
{
    class Program
    {
        static void Main(string[] args)
        {
            Aspose.Network.Pop3.Pop3Client c_pop3 = new Aspose.Network.Pop3.Pop3Client();
            c_pop3.Host = "pop.163.com";
            c_pop3.Port = 110;
            c_pop3.Username = "cupid0616@163.com";
            c_pop3.Password = "677521++";

            try
            {
                c_pop3.Connect();
                Console.WriteLine("==================");
                Console.WriteLine("connected ");
                Console.WriteLine("==================");
                c_pop3.Login();
                Console.WriteLine("==================");
                Console.WriteLine("logged in ");
                Console.WriteLine("==================");

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            //query mail box status
            try
            {
                Aspose.Network.Pop3.Pop3MailboxInfo info;
                info = c_pop3.GetMailboxInfo();
                Console.Write("Message Counts:");
                Console.WriteLine(info.MessageCount);

                Console.Write("Messages total size:");
                Console.Write(info.OccupiedSize);
                Console.WriteLine(" bytes");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            //query messages
            try
            {
                Aspose.Network.Pop3.Pop3MessageInfoCollection infos = c_pop3.ListMessages();

                foreach (Aspose.Network.Pop3.Pop3MessageInfo info in infos)
                {
                    Console.Write("Id:");
                    Console.WriteLine(info.UniqueId);
                    Console.Write("Index number:");
                    Console.WriteLine(info.SequenceNumber);
                    Console.Write("Subject:");
                    Console.WriteLine(info.Subject);
                    Console.Write("size:");
                    Console.WriteLine(info.Size);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //Retrieve Message
            Console.WriteLine("===================");
            Console.WriteLine("Retrieve Messages");
            try
            {
                int messageCount = c_pop3.GetMessageCount();
                for (int i = 1; i <= messageCount; i++)
                {
                    //mail parser
                    Aspose.Network.Mail.MailMessage msg;

                    //retrieve the message in MimeMessage format directly
                    msg = c_pop3.FetchMessage(i);

                    Console.WriteLine("From:" + msg.From.ToString());
                    Console.WriteLine("Subject:" + msg.Subject);
                    Console.WriteLine(msg.HtmlBody);

                    msg.Save(i + ".eml");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("===================");

            //disconnect from Pop3 server
            try
            {

                c_pop3.Disconnect();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            System.Console.Read();
        }
    }
}
