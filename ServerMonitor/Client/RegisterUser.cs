using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    class RegisterUser
    {
        agsXMPP.XmppClientConnection objXmpp;

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
            Console.WriteLine(e.ToString());
            objXmpp.Close();
        }

        void objXmpp_OnRegistered(object sender)
        {
            Console.WriteLine(sender.ToString());
            objXmpp.Close();
        }
    }
}
