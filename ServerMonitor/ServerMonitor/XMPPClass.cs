using agsXMPP;
using agsXMPP.protocol.client;

namespace ServerMonitor
{
    class XMPPClass
    {
        agsXMPP.XmppClientConnection objXmpp;
        public delegate void EventHandler(object sender);
        public event EventHandler OnlineStatus;//在线状态事件

        public delegate void OnlineHandler(agsXMPP.protocol.client.Presence pres);
        public event OnlineHandler UserStatus;//用户状态事件

        public delegate void FriendHandler(agsXMPP.protocol.iq.roster.RosterItem item);
        public event FriendHandler FriendList;//好友列表事件

        public delegate void MessageHandler(agsXMPP.protocol.client.Message msg);
        public event MessageHandler Messages;//消息事件

        public XMPPClass(string strUserName, string strPassword, string strServer)
        {
            objXmpp = new agsXMPP.XmppClientConnection();

            if (strUserName.Split('@').Length == 1)
            {
                strUserName = strUserName + "@" + strServer;
            }

            agsXMPP.Jid jid = new agsXMPP.Jid(strUserName);
            objXmpp.Password = strPassword;
            objXmpp.Username = jid.User;
            objXmpp.Server = jid.Server;
            objXmpp.AutoResolveConnectServer = true;

            try
            {
                objXmpp.OnLogin += new ObjectHandler(objXmpp_OnLogin);//登录
                objXmpp.OnAuthError += new XmppElementHandler(objXmpp_OnAuthError);//验证出错
                objXmpp.OnMessage += new agsXMPP.protocol.client.MessageHandler(objXmpp_OnMessage);//消息处理
                objXmpp.OnPresence += new agsXMPP.protocol.client.PresenceHandler(objXmpp_OnPresence);//状态处理
                objXmpp.OnRosterItem += new XmppClientConnection.RosterHandler(objXmpp_OnRosterItem);//加载好友列表

                objXmpp.Resource = "Maitiam IM";//客户端版本信息
                objXmpp.Open();
            }
            catch(System.Exception ex)
            {
                LogError(ex.ToString());
            }
        }

        /// <summary>
        /// 命令发送
        /// </summary>
        /// <param name="ToServerName"></param>
        /// <param name="strCommand"></param>
        public void SendCommand(string ToServerName, string strCommand)
        {
            agsXMPP.Jid jid = new agsXMPP.Jid(ToServerName);
            agsXMPP.protocol.client.Message sendMsg = new agsXMPP.protocol.client.Message(jid, MessageType.chat, strCommand);
            objXmpp.Send(sendMsg);
            LogTrace(ToServerName + " " + strCommand);
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="ServerID"></param>
        public void RemoveServer(string ServerID)
        {
            objXmpp.RosterManager.RemoveRosterItem(new Jid(ServerID));
        }

        #region 日志方法
        private void LogTrace(string strContent)
        {
            Log.WriteLog(LogFile.Trace, strContent);
        }

        private void LogError(string strContent)
        {
            Log.WriteLog(LogFile.Error, strContent);
        }
        #endregion 日志方法

        void objXmpp_OnRosterItem(object sender, agsXMPP.protocol.iq.roster.RosterItem item)
        {
            FriendList(item);
        }

        void objXmpp_OnPresence(object sender, agsXMPP.protocol.client.Presence pres)
        {
            if (pres.Type == PresenceType.available)
            {
                UserStatus(pres);
            }
            else if (pres.Type == PresenceType.unavailable)
            {
                UserStatus(pres);
            }
            else if (pres.Type == PresenceType.subscribe)//自动处理加好友请求
            {
                PresenceManager PM = new PresenceManager(objXmpp);
                PM.ApproveSubscriptionRequest(new Jid(pres.From.ToString()));//同意订阅
                PM.Subscribe(new Jid(pres.From.ToString()));
            }
        }

        void objXmpp_OnMessage(object sender, agsXMPP.protocol.client.Message msg)
        {
            Messages(msg);
        }

        void objXmpp_OnAuthError(object sender, agsXMPP.Xml.Dom.Element e)
        {
            //throw new NotImplementedException();
        }

        void objXmpp_OnLogin(object sender)
        {
            OnlineStatus("登录成功");
            Log.WriteLog(LogFile.Trace, "登录成功");
        }
    }
}
