using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CtalkRobot
{
    class RegisterUser
    {
        agsXMPP.XmppClientConnection objXmpp;
        string strSSSS = null;

        public void RegUser(string strUserName, string strPassword)
        {
            objXmpp = new agsXMPP.XmppClientConnection();
            objXmpp.OnRegistered += new agsXMPP.ObjectHandler(objXmpp_OnRegistered);
            objXmpp.OnRegisterError += new agsXMPP.XmppElementHandler(objXmpp_OnRegisterError);
            objXmpp.OnRegisterInformation += new agsXMPP.protocol.iq.register.RegisterEventHandler(objXmpp_OnRegisterInformation);

            strUserName = strUserName + "@ishow.xba.com.cn";

            agsXMPP.Jid jid = new agsXMPP.Jid(strUserName);
            objXmpp.Password = strPassword;
            objXmpp.Username = jid.User;
            objXmpp.Server = jid.Server;
            objXmpp.RegisterAccount = true;
            objXmpp.Open();
        }

        void objXmpp_OnRegisterInformation(object sender, agsXMPP.protocol.iq.register.RegisterEventArgs args)
        {
            //args.Register.RemoveAll<Data>();
            args.Register.Username = objXmpp.Username;
            args.Register.Password = objXmpp.Password;
        }

        void objXmpp_OnRegisterError(object sender, agsXMPP.Xml.Dom.Element e)
        {
            Console.WriteLine(strSSSS + " " + e.ToString());
            objXmpp.Close();
        }

        void objXmpp_OnRegistered(object sender)
        {
            Console.WriteLine(strSSSS + " " + sender.ToString());
            objXmpp.Close();
        }

        public void BatchRegister()
        {
            string[] lines = System.IO.File.ReadAllLines(@"1.txt");

            System.Console.WriteLine("Contents of writeLines2.txt =:");
            foreach (string line in lines)
            {
                RegUser(line.Trim(), "qweqwe123");
                strSSSS = line;
                Thread.Sleep(2000);
            }
            Console.WriteLine("注册完成");
        }
    }
}
