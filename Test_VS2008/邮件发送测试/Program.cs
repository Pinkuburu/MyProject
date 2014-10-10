using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace 邮件发送测试
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Net.Mail.SmtpClient client = new SmtpClient();
            client.Host = "smtp.exmail.qq.com";
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("cupid@xba.com.cn", "qweqwe123");
            //星号改成自己邮箱的密码
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            System.Net.Mail.MailMessage message = new MailMessage("cupid@xba.com.cn", "cupid0426@163.com");
            message.Subject = "测试";
            message.Body = "用自己写的软件发的邮件！";
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            //添加附件
            //Attachment data = new Attachment(@"附件地址如：e:\a.jpg", System.Net.Mime.MediaTypeNames.Application.Octet);
            //message.Attachments.Add(data);
            try
            {
                client.Send(message);
                Console.WriteLine("Email successfully send.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Send Email Failed." + ex.ToString());
            }
            Console.ReadKey();
        }
    }
}
