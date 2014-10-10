using System;
using System.Text;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.Xml.Dom;

namespace CtalkRobot
{
    class CtalkClass
    {
        agsXMPP.XmppClientConnection objXmpp;
        SysInfoClass SIC = new SysInfoClass();
        IQ siIq;

        #region 登录CTALK Login(string strUserName, string strPassword)
        /// <summary>
        /// 登录CTALK
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="strPassword"></param>
        public void Login(string strUserName, string strPassword)
        {
            objXmpp = new agsXMPP.XmppClientConnection();

            if (strUserName.Split('@').Length == 1)
            {
                strUserName = strUserName + "@ishow.xba.com.cn";
            }

            agsXMPP.Jid jid = new agsXMPP.Jid(strUserName);
            objXmpp.Password = strPassword;
            objXmpp.Username = jid.User;
            objXmpp.Server = jid.Server;
            objXmpp.AutoResolveConnectServer = true;            

            try
            {
                objXmpp.OnLogin += new agsXMPP.ObjectHandler(objXmpp_OnLogin);//登录
                objXmpp.OnAuthError += new agsXMPP.XmppElementHandler(objXmpp_OnAuthError);//验证出错
                objXmpp.OnMessage += new agsXMPP.protocol.client.MessageHandler(objXmpp_OnMessage);//接收消息
                objXmpp.OnPresence += new PresenceHandler(objXmpp_OnPresence);//状态处理
                objXmpp.OnRosterItem += new XmppClientConnection.RosterHandler(objXmpp_OnRosterItem);//加载好友列表
                objXmpp.OnReadSocketData += new agsXMPP.net.BaseSocket.OnSocketDataHandler(objXmpp_OnReadSocketData);
                objXmpp.OnWriteSocketData += new agsXMPP.net.BaseSocket.OnSocketDataHandler(objXmpp_OnWriteSocketData);
                objXmpp.OnRosterEnd += new ObjectHandler(objXmpp_OnRosterEnd);
                objXmpp.OnIq += new IqHandler(objXmpp_OnIq);
                objXmpp.OnSocketError += new ErrorHandler(objXmpp_OnSocketError);
                objXmpp.OnStreamError += new XmppElementHandler(objXmpp_OnStreamError);
                objXmpp.OnXmppConnectionStateChanged += new XmppConnectionStateHandler(objXmpp_OnXmppConnectionStateChanged);
                objXmpp.Resource = "Maitiam IM";//客户端版本信息
                objXmpp.Open();

                G_Status("Login", "【正在登陆】");
            }
            catch (System.Exception ex)
            {
                G_Status("Error", ex.ToString());
            }
        }

        void objXmpp_OnXmppConnectionStateChanged(object sender, XmppConnectionState state)
        {
            Console.WriteLine("1." + state.ToString());
            if (state.ToString() == "Disconnected")
            {
                Login("test1", "qweqwe123");
            }
        }

        void objXmpp_OnStreamError(object sender, Element e)
        {
            Console.WriteLine("2." + e.ToString());
        }

        void objXmpp_OnSocketError(object sender, Exception ex)
        {
            Console.WriteLine("3." + ex.ToString());
            Login("test1", "qweqwe123");
        }

        void objXmpp_OnRosterEnd(object sender)
        {
            Console.WriteLine("4." + sender);
        }

        void objXmpp_OnIq(object sender, IQ iq)
        {
            if (iq != null)
            {
                // No Iq with query
                if (iq.HasTag(typeof(agsXMPP.protocol.extensions.si.SI)))
                {
                    if (iq.Type == IqType.set)
                    {
                        agsXMPP.protocol.extensions.si.SI si = iq.SelectSingleElement(typeof(agsXMPP.protocol.extensions.si.SI)) as agsXMPP.protocol.extensions.si.SI;

                        agsXMPP.protocol.extensions.filetransfer.File file = si.File;
                        if (file != null)
                        {
                            // somebody wants to send a file to us
                            //frmFileTransfer frmFile = new frmFileTransfer(XmppCon, iq);
                            //frmFile.Show();
                            Console.WriteLine(file.Description);
                            FileTransfer FT = new FileTransfer(objXmpp, iq);
                        }
                    }
                }
                else
                {
                    Element query = iq.Query;

                    if (query != null)
                    {
                        if (query.GetType() == typeof(agsXMPP.protocol.iq.version.Version))
                        {
                            // its a version IQ VersionIQ
                            agsXMPP.protocol.iq.version.Version version = query as agsXMPP.protocol.iq.version.Version;
                            if (iq.Type == IqType.get)
                            {
                                // Somebody wants to know our client version, so send it back
                                iq.SwitchDirection();
                                iq.Type = IqType.result;

                                version.Name = "MiniClient";
                                version.Ver = "0.5";
                                version.Os = Environment.OSVersion.ToString();

                                objXmpp.Send(iq);
                            }
                        }
                    }
                }
            }
        }

        void objXmpp_OnWriteSocketData(object sender, byte[] data, int count)
        {
            Console.WriteLine("5." + Encoding.Default.GetString(data));
        }

        void objXmpp_OnReadSocketData(object sender, byte[] data, int count)
        {
            Console.WriteLine("6." + Encoding.Default.GetString(data));
        }
        #endregion 登录CTALK Login(string strUserName, string strPassword)

        //好友列表处理
        void objXmpp_OnRosterItem(object sender, agsXMPP.protocol.iq.roster.RosterItem item)
        {
            G_Status("", String.Format("【加载好友】: {0}", item.Jid));
        }

        //状态处理
        void objXmpp_OnPresence(object sender, Presence pres)
        {
            G_Status("", String.Format("Got presence from: {0}", pres.From.ToString()));
            G_Status("", String.Format("type: {0}", pres.Type.ToString()));
            G_Status("", String.Format("status: {0}", pres.Status));
            //自动处理加好友请求
            if (pres.Type == PresenceType.subscribe)
            {
                PresenceManager PM = new PresenceManager(objXmpp);              
                PM.ApproveSubscriptionRequest(new Jid(pres.From.ToString()));//同意订阅
                PM.Subscribe(new Jid(pres.From.ToString()));
            }
            //PresenceType.available    在线
            //PresenceType.unavailable  离线
        }

        //消息处理
        void objXmpp_OnMessage(object sender, agsXMPP.protocol.client.Message msg)
        {
            string strMsg = null;

            //G_Status("ReciveMsg", "【接收消息】 From:" + msg.From.Bare + " Msg:" + msg.Body);
            
            if (SystemCommand(msg.Body))
            {
                string[] arrCmd = msg.Body.Split(' ');
                if (arrCmd[0].ToLower() == "status")
                {
                    switch (arrCmd[1].ToLower())
                    {
                        case "online":
                            objXmpp.Show = ShowType.NONE;
                            objXmpp.SendMyPresence();
                            G_Status("", "【状态改变】在线");
                            strMsg = "【状态改变】在线";
                            break;
                        case "away":
                            objXmpp.Show = ShowType.away;
                            objXmpp.SendMyPresence();
                            G_Status("", "【状态改变】闲置");
                            strMsg = "【状态改变】闲置";
                            break;
                        case "busy":
                            objXmpp.Show = ShowType.dnd;
                            objXmpp.SendMyPresence();
                            G_Status("", "【状态改变】忙碌");
                            strMsg = "【状态改变】忙碌";
                            break;
                        case "cpu":
                            strMsg = SIC._CPU;
                            break;
                        case "disk":
                            strMsg = SIC.DiskInfo();
                            break;
                        case "file":
                            FileTransfer FT = new FileTransfer(objXmpp, new Jid("test1@ishow.xba.com.cn"));
                            break;
                    }
                }
                //agsXMPP.Jid jid = new agsXMPP.Jid(msg.From.Bare);
                //Message autoReply = new Message(jid, MessageType.chat, strMsg);
                //objXmpp.Send(autoReply);
                //G_Status("SendMsg", "【发送消息】 To:" + msg.From.Bare + " Msg:" + autoReply.Body);
            }
            else
            {
                //strMsg = msg.Body;
                //agsXMPP.Jid jid = new agsXMPP.Jid(msg.From.Bare);
                //Message autoReply = new Message(jid, MessageType.chat, strMsg);
                //objXmpp.Send(autoReply);
                //G_Status("SendMsg", "【发送消息】 To:" + msg.From.Bare + " Msg:" + autoReply.Body);
            }
        }

        //验证出错处理
        void objXmpp_OnAuthError(object sender, agsXMPP.Xml.Dom.Element e)
        {
            G_Status("Error", "【登陆失败】");
        }

        //登录成功处理
        void objXmpp_OnLogin(object sender)
        {
            G_Status("Login", "【登陆成功】");            
        }

        #region 消息处理 G_Status(string strType, string strMessage)
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="strType"></param>
        /// <param name="strMessage"></param>
        private void G_Status(string strType, string strMessage)
        {
            DateTime dt = DateTime.Now;
            switch (strType)
            {
                case "Login":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "Error":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "SendMsg":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case "ReciveMsg":
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
            }
            Console.WriteLine(dt + "|" + strMessage);
        }
        #endregion 消息处理 G_Status(string strType, string strMessage)

        #region 系统命令 SystemCommand(string strCommand)
        /// <summary>
        /// 系统命令
        /// </summary>
        /// <param name="strCommand"></param>
        /// <returns></returns>
        private bool SystemCommand(string Command)
        {
            bool blCmd = false;
            string[] arrCmd = Command.Split(' ');
            if (arrCmd[0].ToLower() == "status")
            {
                blCmd = true;
            }            
            return blCmd;
        }
        #endregion 系统命令 SystemCommand(string strCommand)
    }
}
