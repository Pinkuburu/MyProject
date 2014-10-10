using agsXMPP;
using System.Windows.Forms;
using System.Collections;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.vcard;
using System.Threading;

namespace Client
{
    class XMPPClass
    {
        agsXMPP.XmppClientConnection objXmpp;

        public delegate void EventHandler(object sender);
        public event EventHandler OnlineStatus;//在线状态事件

        public delegate void ConnectChangeHandler(string strStatus);
        public event ConnectChangeHandler ConnectChange;//连接状态事件

        public delegate void MsgHandler(agsXMPP.protocol.client.Message msg);
        public event MsgHandler ReciveMsg;//接收消息事件

        public delegate void SocketError(string strError);
        public event SocketError SocketOnError;

        public ArrayList alServerList = new ArrayList();
        public string strMonitorServer = null;

        Thread thSendMessage;
        Library lib = new Library();
        SysInfoClass SIC = new SysInfoClass();

        public string _UserName, _strPassword, _strServer, _strServerName, _strMonitorServer;

        #region 登录 XMPPClass(string strUserName, string strPassword, string strServer, string strServerName, string strMonitorServer)
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="strUserName">登录帐号</param>
        /// <param name="strPassword">登录密码</param>
        /// <param name="strServer">登录服务器地址</param>
        /// <param name="strServerName">本台服务器名称</param>
        /// <param name="strMonitorServer">监控服务器jid</param>
        public XMPPClass(string strUserName, string strPassword, string strServer, string strServerName, string strMonitorServer)
        {
            this.strMonitorServer = strMonitorServer;

            this._UserName = strUserName;
            this._strPassword = strPassword;
            this._strServer = strServer;
            this._strServerName = strServerName;
            this._strMonitorServer = strMonitorServer;

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

            objXmpp.OnLogin += new ObjectHandler(objXmpp_OnLogin);//登录
            objXmpp.OnAuthError += new XmppElementHandler(objXmpp_OnAuthError);//验证出错
            objXmpp.OnMessage += new agsXMPP.protocol.client.MessageHandler(objXmpp_OnMessage);//消息处理
            objXmpp.OnPresence += new agsXMPP.protocol.client.PresenceHandler(objXmpp_OnPresence);//状态处理
            objXmpp.OnRosterItem += new XmppClientConnection.RosterHandler(objXmpp_OnRosterItem);//加载好友列表
            objXmpp.OnRosterEnd += new ObjectHandler(objXmpp_OnRosterEnd);//好友列表加载完成
            objXmpp.OnXmppConnectionStateChanged += new XmppConnectionStateHandler(objXmpp_OnXmppConnectionStateChanged);
            objXmpp.OnSocketError += new ErrorHandler(objXmpp_OnSocketError);
            objXmpp.Resource = "Maitiam IM";//客户端版本信息
            objXmpp.ClientVersion = "1.0";
            objXmpp.Status = strServerName;
            objXmpp.Open();

        }
        #endregion 登录 XMPPClass(string strUserName, string strPassword, string strServer, string strServerName, string strMonitorServer)

        #region 断线重登陆 Login()
        /// <summary>
        /// 断线重登陆
        /// </summary>
        private void Login()
        {
            string strUserName = this._UserName;
            string strPassword = this._strPassword;
            string strServer = this._strServer;
            string strServerName = this._strServerName;
            string strMonitorServer = this._strMonitorServer;

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
            
            objXmpp.OnLogin += new ObjectHandler(objXmpp_OnLogin);//登录
            objXmpp.OnAuthError += new XmppElementHandler(objXmpp_OnAuthError);//验证出错
            objXmpp.OnMessage += new agsXMPP.protocol.client.MessageHandler(objXmpp_OnMessage);//消息处理
            objXmpp.OnPresence += new agsXMPP.protocol.client.PresenceHandler(objXmpp_OnPresence);//状态处理
            objXmpp.OnRosterItem += new XmppClientConnection.RosterHandler(objXmpp_OnRosterItem);//加载好友列表
            objXmpp.OnRosterEnd += new ObjectHandler(objXmpp_OnRosterEnd);//好友列表加载完成
            objXmpp.OnXmppConnectionStateChanged += new XmppConnectionStateHandler(objXmpp_OnXmppConnectionStateChanged);
            objXmpp.OnSocketError += new ErrorHandler(objXmpp_OnSocketError);
            objXmpp.OnError += new ErrorHandler(objXmpp_OnError);
            objXmpp.Resource = "Maitiam IM";//客户端版本信息
            objXmpp.ClientVersion = "1.0";
            objXmpp.Status = strServerName;
            objXmpp.Open();
            
        }        
        #endregion 断线重登陆 Login()

        #region 事件

        void objXmpp_OnError(object sender, System.Exception ex)
        {
            //MessageBox.Show("2");
            if (thSendMessage != null)
            {
                //thSendMessage.IsBackground = false;
                //MessageBox.Show(thSendMessage.ThreadState.ToString());
                if (thSendMessage.ThreadState == ThreadState.WaitSleepJoin)
                {
                    thSendMessage.Suspend();
                }
            }
            
            //objXmpp.Close();
            lib.LogError("objXmpp_OnError " + ex.ToString());
            Thread.Sleep(5000);
            //MessageBox.Show("2" + " " + ex.ToString());
        }

        //Socket连接出错
        void objXmpp_OnSocketError(object sender, System.Exception ex)
        {
            //MessageBox.Show("1" + " " + ex.ToString());
            if (thSendMessage != null)
            {
                if (thSendMessage.ThreadState == ThreadState.Running)
                {
                    thSendMessage.Suspend();
                }
            }
            lib.LogError("objXmpp_OnSocketError " + ex.ToString());
            Login();
            Thread.Sleep(5000);
        }

        //状态改变
        void objXmpp_OnXmppConnectionStateChanged(object sender, XmppConnectionState state)
        {
            lib.LogTrace(state.ToString());
            if (state.ToString() == "Disconnected")
            {
                ConnectChange("Disconnected");
                if (thSendMessage.ThreadState == ThreadState.Running)
                {
                    thSendMessage.Suspend();
                }
                Login();
            }
        }

        //自动加载主服务器
        void objXmpp_OnRosterEnd(object sender)
        {
            Jid jMonitorServer = lib.ReadINI("ClientConfig", "MonitorServer");

            if (alServerList.Count >= 0)
            {
                if (alServerList.IndexOf(jMonitorServer) < 0)
                {
                    objXmpp.RosterManager.AddRosterItem(jMonitorServer);
                    objXmpp.PresenceManager.Subscribe(jMonitorServer);
                }
            }
        }
        //加载好友列表
        void objXmpp_OnRosterItem(object sender, agsXMPP.protocol.iq.roster.RosterItem item)
        {
            alServerList.Add(item.Jid);
        }
        //用户状态事件
        void objXmpp_OnPresence(object sender, agsXMPP.protocol.client.Presence pres)
        {
            if (pres.Type == PresenceType.available)//当服务端在线时
            {
                //UserStatus(pres);
                if (pres.From.Bare == strMonitorServer)
                {
                    //thSendMessage.IsBackground = false;
                    if (thSendMessage == null)
                    {
                        Monitor_Start();
                    }
                    else if (thSendMessage.ThreadState == ThreadState.Suspended)
                    {
                        thSendMessage.Resume();
                    }
                }
            }
            else if (pres.Type == PresenceType.unavailable)//当服务端离线时
            {
                if (pres.From.Bare == strMonitorServer)
                {
                    thSendMessage.Suspend();
                }
            }
            else if (pres.Type == PresenceType.subscribe)//自动处理加好友请求
            {
                PresenceManager PM = new PresenceManager(objXmpp);
                PM.ApproveSubscriptionRequest(new Jid(pres.From.ToString()));//同意订阅
                PM.Subscribe(new Jid(pres.From.ToString()));
            }
        }
        //收到消息
        void objXmpp_OnMessage(object sender, agsXMPP.protocol.client.Message msg)
        {
            ReciveMsg(msg);
        }
        //验证出错
        void objXmpp_OnAuthError(object sender, agsXMPP.Xml.Dom.Element e)
        {
            lib.LogError("objXmpp_OnAuthError | 请检查登录帐号是否注册验证服务器");
        }
        //登录成功
        void objXmpp_OnLogin(object sender)
        {            
            lib.LogTrace("登录成功");
            OnlineStatus("登录成功");

            if (thSendMessage != null)
            {
                //thSendMessage.IsBackground = false;
                if (thSendMessage.ThreadState == ThreadState.Suspended)
                {
                    thSendMessage.Resume();
                }
            }
        }

        #endregion 事件

        private void Monitor_Start()
        {
            thSendMessage = new Thread(new ThreadStart(AutoSendMessage));
            //thSendMessage.IsBackground = true;
            thSendMessage.Start();
            //thSendMessage.Suspend();
        }

        #region 自动发送消息 AutoSendMessage()
        /// <summary>
        /// 自动发送消息
        /// </summary>
        private void AutoSendMessage()
        {
            string strContent;
            string strDebug;

            while (true)
            {
                strDebug = lib.ReadINI("ClientConfig", "Debug");
                strContent = SIC.ShowInfo();
                agsXMPP.Jid jid = new agsXMPP.Jid(this.strMonitorServer);
                agsXMPP.protocol.client.Message autoReply = new agsXMPP.protocol.client.Message(jid, MessageType.chat, strContent);
                if (strDebug == "1")
                {
                    lib.LogTrace(strContent);
                }
                objXmpp.Send(autoReply);
                Thread.Sleep(5000);
            }
        }
        #endregion 自动发送消息 AutoSendMessage()

        #region 发送消息 SendMsg(string ToServerName, string strCommand)
        public void SendMsg(string ToServerName, string strCommand)
        {
            agsXMPP.Jid jid = new agsXMPP.Jid(ToServerName);
            agsXMPP.protocol.client.Message sendMsg = new agsXMPP.protocol.client.Message(jid, MessageType.chat, strCommand);
            lib.LogTrace(ToServerName + " " + strCommand);
            objXmpp.Send(sendMsg);
        }
        #endregion 发送消息 SendMsg(string ToServerName, string strCommand)

        #region 退出时调用 SetThreadExit()
        /// <summary>
        /// 退出时调用
        /// </summary>
        public void SetThreadExit()
        {
            thSendMessage.IsBackground = true;
        }
        #endregion 退出时调用 SetThreadExit()
    }
}
