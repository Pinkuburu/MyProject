
#region 版权声明
//=========================================================== 
// 版权声明：LFNet.QQ是基于QQ2009版本的QQ协议开发而成的，协议
// 分析主要参考自小虾的MyQQ(C++)源代码，代码开发主要基于阿布
// 的LumaQQ.NET的C#.NET代码修改而成，故继续遵照使用LumaQQ的开
// 源协议。
//
// 本人没有对LumaQQ.NET的C#.NET代码的框架做过多的改动，主
// 要工作为将MyQQ的C++协议代码部分翻译成符合LumaQQ.Net框架
// 的C#代码，故请尊重LumaQQ作者Luma的著作权和版权声明。
// 
// 代码开源主要用于解决大家在学习和研究协议过程中遇到由于缺乏代码所带来的制约性问题。
// 本代码仅供学习交流使用，大家在使用此开发包前请自行协调好多方面关系，
// 不得用于任何商业用途和非法用途，本人不享受和承担由此产生的任何权利以及任何法律责任。
// 
// 本源代码可通过以下网址获取:
// http://QQCode.lynfo.com, http://www.lynfo.com, http://bbs.lynfo.com, http://hi.baidu.com/dobit.
//
// Copyright @ 2009-2010  Lynfo.com.  All Rights Reserved.   
// Framework: 2.0
// Author: Luma(java版) → Abu(C# QQ2005协议版) → Dobit(C# QQ2009协议版本)
// Email: dobit@msn.cn   
// Created: 2009-3-1~ 2009-11-28
// Last Modified:2009-11-28    
//   
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details. 
//===========================================================   
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

using LFNet.QQ.Net;
using LFNet.QQ.Events;
using LFNet.QQ.Packets;
using LFNet.QQ.Entities;
using LFNet.QQ.Threading;
using LFNet.QQ.Packets.In;
using LFNet.QQ.Packets.Out;
namespace LFNet.QQ
{

    /// <summary>
    /// 登录管理
    /// </summary>
    public class LoginManager
    {
        public static string[] UdpServers = new string[] { "sz.tencent.com", "sz2.tencent.com", "sz3.tencent.com", "sz4.tencent.com", "sz5.tencent.com", "sz6.tencent.com", "sz7.tencent.com", "sz8.tencent.com", "sz9.tencent.com" };
        public static string[] TcpPServers = new string[] { "tcpconn.tencent.com", "tcpconn2.tencent.com", "tcpconn3.tencent.com", "tcpconn4.tencent.com", "tcpconn5.tencent.com", "tcpconn6.tencent.com" };
        
        internal LoginManager() { }
        /// <summary>
        /// </summary>
        /// <value></value>
        public QQClient QQClient { get; private set; }
        public QQUser QQUser { get { return QQClient.QQUser; } }
        internal LoginManager(QQClient client)
        {
            QQClient = client;
        }

        /// <summary>
        /// 登陆IP指向
        /// </summary>
        private int ip_pos = 0;
        /// <summary>
        /// 登陆域名指向
        /// </summary>
        private int host_pos = 0;
        /// <summary>
        /// 登录
        /// </summary>
        public void Login()
        {
            if (string.IsNullOrEmpty(QQClient.LoginServerHost))
            {
                QQClient.LoginServerHost = GetNewServerIP();
            }
            //Check.Require(QQClient.LoginServerHost, "LoginServerHost", Check.NotNull);
            if (QQClient.LoginPort == 0)
            {
                if (QQUser.IsUdp)
                {
                    QQClient.LoginPort = QQGlobal.QQ_PORT_UDP;
                }
                else
                {
                    QQClient.LoginPort = QQGlobal.QQ_PORT_TCP;
                }
            }
            OutPacket outPacket = null;
            outPacket = new LoginTouchPacket(QQClient);
            QQClient.LogManager.Log("login...");
            QQClient.LoginStatus = LoginStatus.Connectting;
            QQClient.PacketManager.SendPacketTimeOut += new EventHandler<QQEventArgs<InPacket, OutPacket>>(PacketManager_SendPacketTimeOut);
            QQClient.PacketManager.SendPacketAnyway(outPacket, QQPort.Main.Name);
        }

        void PacketManager_SendPacketTimeOut(object sender, QQEventArgs<InPacket, OutPacket> e)
        {
            if (QQClient.LoginStatus == LoginStatus.Connectting)
            { 
                
                //更换IP地址
                string newserverIp=GetNewServerIP();
                if(newserverIp!=null)
                {
                    QQClient.ConnectionManager.ConnectionPool.Release(QQPort.Main.Name);
                    QQClient.LoginServerHost = newserverIp;
                    QQClient.LoginStatus = LoginStatus.ChangeServer;
                    this.QQClient.LogManager.Log("change ServerIP to："+newserverIp);
                    QQClient.Login();
                }
            }
        }


        private string GetNewServerIP()
        {
            if (QQUser.IsUdp)
            {
                if (this.host_pos == UdpServers.Length)
                {
                    QQClient.LoginStatus = LoginStatus.Failed;
                    this.host_pos = 0;
                    return null;
                }
                else
                {
                    try
                    {
                        string ip=System.Net.Dns.GetHostAddresses(UdpServers[this.host_pos])[0].ToString();
                        this.host_pos++;
                        return ip;
                    }
                    catch 
                    {
                        this.host_pos++;
                        return GetNewServerIP();
                        
                    }
                }
            }
            else
            {
                if (this.host_pos ==TcpPServers.Length)
                {
                    QQClient.LoginStatus = LoginStatus.Failed;
                    this.host_pos = 0;
                    return null;
                }
                else
                {
                    try
                    {
                        string ip= System.Net.Dns.GetHostAddresses(TcpPServers[this.host_pos])[0].ToString();
                        this.host_pos++;
                        return ip;
                    }
                    catch
                    {
                        this.host_pos++;
                        return GetNewServerIP();
                    }
                }
            }
        }

        public void LoginSendVerifyCode(string verifyCode)
        {
            byte[] bytes=Utils.Util.GetBytes(verifyCode);
            
            ByteBuffer buf = new ByteBuffer(bytes);
            
            uint vCode = buf.GetUInt();
            if (QQClient.LoginStatus==LoginStatus.Login)
            {
                //qqclient_set_process(qq, P_LOGIN);	//原来漏了这个  20090709
                //prot_user_request_token(qq, qq->data.operating_number, qq->data.operation, 1, code);
                QQClient.LogManager.Log("send Verify Code:" + vCode.ToString());
                OutPacket outPacket=new LoginRequestPacket(QQClient,QQClient.QQUser.QQKey.Verify_Token,vCode,0);
                QQClient.PacketManager.SendPacketAnyway(outPacket, QQPort.Main.Name);
            }
            else
            {
                //qqclient_set_process(qq, P_LOGGING);	//原来这个是P_LOGIN，错了。 20090709
                //prot_login_request(qq, &qq->data.verify_token, code, 0);
                QQClient.LogManager.Log("send Verify Code:" + vCode.ToString());
                OutPacket outPacket = new LoginRequestPacket(QQClient, QQClient.QQUser.QQKey.Verify_Token, vCode, 0);
                QQClient.PacketManager.SendPacketAnyway(outPacket, QQPort.Main.Name);
            }
        }

        /// <summary>
        /// 登陆下载QQ列表
        /// </summary>
        private void LoginGetList()
        {
            LoginGetList(0);
        }
        /// <summary>
        /// 登陆下载QQ列表
        /// </summary>
        /// <param name="pos"></param>
        private void LoginGetList(int pos)
        {
            this.QQClient.LoginStatus = LoginStatus.GetList;
            LoginGetListPacket outPacket = new LoginGetListPacket(this.QQClient);
            outPacket.Pos = (ushort)pos;
            QQClient.PacketManager.SendPacketAnyway(outPacket, QQPort.Main.Name);
        }
        /// <summary>
        /// 在成功获取登录令牌后向服务器发送用户名密码
        /// </summary>
        private void LoginUser()
        {
            //LoginPacket packet = new LoginPacket(QQClient);
            //QQClient.PacketManager.SendPacketAnyway(packet, QQPort.Main.Name);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        public void Logout()
        {
            if (QQClient.IsLogon)
            {
                LogoutPacket packet = new LogoutPacket(QQClient);
                QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name, true);
            }

            SetLogout();
        }
        /// <summary>
        /// </summary>
        internal void SetLogout()
        {
            QQClient.LoginStatus = LoginStatus.Logout;
            QQClient.IsLogon = false;
            QQUser.IsLoggedIn = false;
            QQUser.QQKey.LoginToken = null;
            QQClient.LoginRedirect = false;
            QQClient.ConnectionManager.Dispose();

            //注销轮循线程
            QQClient.PacketManager.SetdownTrigger();
        }
        #region events

        /// <summary>
        /// 登陆状态改变时
        /// </summary>
        public event EventHandler<LoginStatusChangedEventArgs<LoginStatus>> LoginStatusChanged;
        internal void OnLoginStatusChanged(LoginStatusChangedEventArgs<LoginStatus> e)
        {
            if (LoginStatusChanged != null)
            {
                LoginStatusChanged(this, e);
            }
        }

        /// <summary>
        /// 登陆需要验证码事件
        /// </summary>
        public event EventHandler<QQEventArgs<LoginRequestReplyPacket, LoginRequestPacket>> LoginNeedVerifyCode;
        /// <summary>
        /// Raises the <see cref="E:LoginNeedVerifyCode"/> event.
        /// </summary>
        /// <param name="e"></param>
        internal void OnLoginNeedVerifyCode(QQEventArgs<LoginRequestReplyPacket, LoginRequestPacket> e)
        {
            if (LoginNeedVerifyCode != null)
            {
                LoginNeedVerifyCode(this, e);
            }
        }

        /// <summary>
        /// 请求登录令牌成功
        /// 	<remark>abu 2008-03-08 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<RequestLoginTokenReplyPacket, RequestLoginTokenPacket>> RequestLoginTokenSuccessed;
        /// <summary>
        /// Raises the <see cref="E:RequestLoginTokenSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.RequestLoginTokenReplyPacket&gt;"/> instance containing the event data.</param>
        internal void OnRequestLoginTokenSuccessed(QQEventArgs<RequestLoginTokenReplyPacket, RequestLoginTokenPacket> e)
        {
            if (RequestLoginTokenSuccessed != null)
            {
                RequestLoginTokenSuccessed(this, e);
            }
            //发送用户名密码
            LoginUser();
        }

        /// <summary>
        /// 请求登录令牌失败
        /// 	<remark>abu 2008-03-08 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<RequestLoginTokenReplyPacket, RequestLoginTokenPacket>> RequestLoginTokenFailed;
        /// <summary>
        /// Raises the <see cref="E:RequestLoginTokenFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.RequestLoginTokenReplyPacket&gt;"/> instance containing the event data.</param>
        internal void OnRequestLoginTokenFailed(QQEventArgs<RequestLoginTokenReplyPacket, RequestLoginTokenPacket> e)
        {
            if (RequestLoginTokenFailed != null)
            {
                RequestLoginTokenFailed(this, e);
            }
        }

        ///// <summary>登录成功事件
        ///// 	<remark>abu 2008-03-08 </remark>
        ///// </summary>
        //public event EventHandler<QQEventArgs<LoginReplyPacket, LoginPacket>> LoginSuccessed;
        ///// <summary>
        ///// Raises the <see cref="E:LoginSuccessed"/> event.
        ///// </summary>
        ///// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.LoginReplyPacket&gt;"/> instance containing the event data.</param>
        //internal void OnLoginSuccessed(QQEventArgs<LoginReplyPacket, LoginPacket> e)
        //{
        //    this.QQClient.IsLogon = true;
        //    this.QQUser.IsLoggedIn = true;

        //    if (LoginSuccessed != null)
        //    {
        //        LoginSuccessed(this, e);
        //    }
        //}
        // //<summary>
        // //登录重定向事件
        // //</summary>
        //public event EventHandler<QQEventArgs<LoginReplyPacket, LoginPacket>> LoginRedirect;
        //internal void OnLoginRedirect(QQEventArgs<LoginReplyPacket, LoginPacket> e)
        //{
        //    // 如果是登陆重定向，继续登陆
        //    QQClient.LoginRedirect = true;
        //    QQClient.ConnectionManager.ConnectionPool.Release(QQPort.Main.Name);
        //    QQClient.Login(Utils.Util.GetIpStringFromBytes(e.InPacket.RedirectIP), e.InPacket.RedirectPort);
        //    if (LoginRedirect != null)
        //    {
        //        LoginRedirect(this, e);
        //    }
        //}

        // //<summary>
        // //重定向登录时，重定向服务器为空
        // //   <remark>abu 2008-03-10 </remark>
        // //</summary>
        //public event EventHandler<QQEventArgs<LoginReplyPacket, LoginPacket>> LoginRedirectNull;
        // //<summary>
        // //Raises the <see cref="E:LoginRedirectNull"/> event.
        // //</summary>
        // //<param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.LoginReplyPacket&gt;"/> instance containing the event data.</param>
        //internal void OnLoginRedirectNull(QQEventArgs<LoginReplyPacket, LoginPacket> e)
        //{
        //    if (LoginRedirectNull != null)
        //    {
        //        LoginRedirectNull(this, e);
        //    }
        //}

        ///// <summary>
        ///// 登录失败触发的事件
        ///// 	<remark>abu 2008-03-10 </remark>
        ///// </summary>
        //public event EventHandler<QQEventArgs<LoginReplyPacket, LoginPacket>> LoginFailed;
        ///// <summary>
        ///// Raises the <see cref="E:LoginFailed"/> event.
        ///// </summary>
        ///// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.LoginReplyPacket&gt;"/> instance containing the event data.</param>
        //internal void OnLoginFailed(QQEventArgs<LoginReplyPacket, LoginPacket> e)
        //{
        //    if (LoginFailed != null)
        //    {
        //        LoginFailed(this, e);
        //    }
        //}

        ///// <summary>
        ///// 登录过程中的未知错误
        ///// </summary>
        //public event EventHandler<QQEventArgs<LoginReplyPacket, LoginPacket>> LoginUnknownError;
        ///// <summary>
        ///// Raises the <see cref="E:LoginUnknownError"/> event.
        ///// </summary>
        ///// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.LoginReplyPacket&gt;"/> instance containing the event data.</param>
        //internal void OnLoginUnknownError(QQEventArgs<LoginReplyPacket, LoginPacket> e)
        //{
        //    if (LoginUnknownError != null)
        //    {
        //        LoginUnknownError(this, e);
        //    }
        //}
        
        #endregion

        internal void ProcessLoginTouchReply(LoginTouchReplyPacket loginTouchReplyPacket, LoginTouchPacket loginTouchPacket)
        {
            QQEventArgs<LoginTouchReplyPacket, LoginTouchPacket> e = new QQEventArgs<LoginTouchReplyPacket, LoginTouchPacket>(QQClient, loginTouchReplyPacket, loginTouchPacket);
            if (loginTouchReplyPacket.ReplyCode == ReplyCode.OK)
            {
                QQClient.LoginStatus = LoginStatus.Connected;
                if (loginTouchReplyPacket.IsRedirect)//转向
                {
                    if (Utils.Util.IsIPZero(loginTouchReplyPacket.RedirectIP))
                    {
                        //OnLoginRedirectNull(e);
                        QQClient.LogManager.Log("RedirectIp Is Zero!");
                        QQClient.LoginStatus = LoginStatus.Failed;
                    }
                    else
                    {
                        QQClient.LoginStatus = new LoginStatus("Redirect to" + Utils.Util.GetIpStringFromBytes(loginTouchReplyPacket.RedirectIP), "重定向到服务器" + Utils.Util.GetIpStringFromBytes(loginTouchReplyPacket.RedirectIP));
                        //OnLoginRedirect(e);
                        QQClient.LoginRedirect = true;
                        QQClient.ConnectionManager.ConnectionPool.Release(QQPort.Main.Name);
                        QQClient.LogManager.Log("Redirect to " + Utils.Util.GetIpStringFromBytes(loginTouchReplyPacket.RedirectIP));
                        string ipTemp = QQClient.LoginServerHost;
                        QQClient.LoginServerHost = Utils.Util.GetIpStringFromBytes(QQClient.ServerInfo.CSP_dwConnIP);
                        QQClient.ServerInfo.CSP_dwConnIP = Utils.Util.IpToBytes(ipTemp);
                        //重新登录
                        QQClient.Login();
                    }
                }
                else
                {
                    //将服务器返回的时间和IP存入QQClient等待使用
                    QQClient.ServerTime = loginTouchReplyPacket.ServerTime;
                    QQClient.ClientIP = loginTouchReplyPacket.ClientIP;
                    //0xba开始登陆
                    QQUser.QQKey.LoginRequestToken = loginTouchReplyPacket.Token;
                    OutPacket outPacket = new LoginRequestPacket(this.QQClient);
                    QQClient.PacketManager.SendPacketAnyway(outPacket, QQPort.Main.Name);
                    //QQUser.QQKey = new QQKey(this.QQUser);
                    //QQClient.LoginManager.


                }

            }
            else
            { QQClient.LoginStatus = LoginStatus.Failed; }
        }

        internal void ProcessLoginRequestReply(LoginRequestReplyPacket loginRequestReplyPacket, LoginRequestPacket loginRequestPacket)
        {
            QQEventArgs<LoginRequestReplyPacket, LoginRequestPacket> e = new QQEventArgs<LoginRequestReplyPacket, LoginRequestPacket>(QQClient, loginRequestReplyPacket, loginRequestPacket);
            //if (loginRequestReplyPacket.Png_Data != 0x00)//需要验证码
            //{
            //    if (loginRequestReplyPacket.Next != 0x00)//还有验证码数据没接受完
            //    {
            //        QQClient.LogManager.Log("接收到部分验证码图片数据，继续接收....");
            //        OutPacket outPacket = new LoginRequestPacket(QQClient, loginRequestReplyPacket.Png_Token, 0, 1);//发送一个请求验证码的包
            //        QQClient.PacketManager.SendPacketAnyway(outPacket, QQPort.Main.Name);
            //    }
            //    else//已经得到验证码文件
            //    {
            //        QQClient.LoginStatus = LoginStatus.NeedVerifyCode;
            //        QQClient.QQUser.QQKey.Verify_Token = loginRequestReplyPacket.Answer_Token;
            //        QQClient.LogManager.Log("Need input Verify Code");
            //        OnLoginNeedVerifyCode(e);
                    
                    
            //    }
            //}
            //else//不需要验证码
            //{
            //    QQClient.LogManager.Log("Process LoginRequest Success! Now Process Verify Password...");
            //    QQClient.QQUser.QQKey.Answer_Token = loginRequestReplyPacket.Answer_Token;
            //    OutPacket outPacket =new LoginVerifyPacket(this.QQClient) ;//发送一个登陆请求包
            //    QQClient.PacketManager.SendPacketAnyway(outPacket, QQPort.Main.Name);

            //}
        }

        



        internal void ProcessLoginVerifyReply(LoginVerifyReplyPacket loginVerifyReplyPacket, LoginVerifyPacket loginVerifyPacket)
        {
            QQEventArgs<LoginVerifyReplyPacket, LoginVerifyPacket> e = new QQEventArgs<LoginVerifyReplyPacket, LoginVerifyPacket>(QQClient, loginVerifyReplyPacket, loginVerifyPacket);
            switch (loginVerifyReplyPacket.ReplyCode)
            {
                case 0x00://success!
                    QQClient.LogManager.Log(loginVerifyReplyPacket.ToString() + ":0x" + loginVerifyReplyPacket.ReplyCode.ToString("X2") + " Login Success!");
                    //触发事件
                    OutPacket outPacket = new LoginGetInfoPacket(this.QQClient);//发送一个登陆请求包
                    QQClient.PacketManager.SendPacketAnyway(outPacket, QQPort.Main.Name);
                    break;
                case 0x33:
                case 0x51://denied!
                    QQClient.LogManager.Log(loginVerifyReplyPacket.ToString() + ":0x" + loginVerifyReplyPacket.ReplyCode.ToString("X2") + " Denied!");
                    break;
                case 0xBF:
                    QQClient.LogManager.Log(loginVerifyReplyPacket.ToString() + ":0x" + loginVerifyReplyPacket.ReplyCode.ToString("X2") + " No this QQ number!");
                    break;
                case 0x34:
                    QQClient.LogManager.Log(loginVerifyReplyPacket.ToString() + ":0x" + loginVerifyReplyPacket.ReplyCode.ToString("X2") + " Wrong password!");
                    QQClient.LoginStatus = LoginStatus.WrongPassword;
                    break;
                default:
                    QQClient.LogManager.Log(loginVerifyReplyPacket.ToString() + ":0x" + loginVerifyReplyPacket.ReplyCode.ToString("X2") + " Unknow ReplyCode!");
                    break;
            
            }
            //return;
        }

        internal void ProcessLoginGetInfoReply(LoginGetInfoReplyPacket loginGetInfoReplyPacket, LoginGetInfoPacket loginGetInfoPacket)
        {
            QQEventArgs<LoginGetInfoReplyPacket, LoginGetInfoPacket> e = new QQEventArgs<LoginGetInfoReplyPacket, LoginGetInfoPacket>(QQClient, loginGetInfoReplyPacket, loginGetInfoPacket);
            OutPacket outPacket = new LoginA4Packet(this.QQClient);
            QQClient.PacketManager.SendPacketAnyway(outPacket, QQPort.Main.Name);
        }

        internal void ProcessLoginA4Reply(LoginA4ReplyPacket loginA4ReplyPacket, LoginA4Packet loginA4Packet)
        {
            QQEventArgs<LoginA4ReplyPacket, LoginA4Packet> e = new QQEventArgs<LoginA4ReplyPacket, LoginA4Packet>(QQClient, loginA4ReplyPacket, loginA4Packet);
            OutPacket outPacket = new LoginGetListPacket(this.QQClient);
            QQClient.PacketManager.SendPacketAnyway(outPacket, QQPort.Main.Name);
        }

        internal void ProcessKeepAliveReply(KeepAliveReplyPacket keepAliveReplyPacket, KeepAlivePacket keepAlivePacket)
        {
            QQClient.LogManager.Log("Recived KeepAlive Packet:Onlines: " + keepAliveReplyPacket.Onlines.ToString() + " ServerTime:" + keepAliveReplyPacket.ServerTime.ToString());

        }

        private static int lisPackets = 0;
        internal void ProcessLoginGetListReply(LoginGetListReplyPacket loginGetListReplyPacket, LoginGetListPacket loginGetListPacket)
        {
            QQEventArgs<LoginGetListReplyPacket, LoginGetListPacket> e = new QQEventArgs<LoginGetListReplyPacket, LoginGetListPacket>(QQClient, loginGetListReplyPacket, loginGetListPacket);
            foreach (QQ.Entities.QQBasicInfo qq in loginGetListReplyPacket.QQList)
            {
                if(!QQClient.QQUser.QQList.ContainsKey(qq.QQ))
                QQClient.QQUser.QQList.Add(qq.QQ, qq);

            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("本次得到QQ " + loginGetListReplyPacket.QQList.Count + " 个。");
#if DEBUG
                foreach (QQ.Entities.QQBasicInfo qq in loginGetListReplyPacket.QQList)
            {
                    sb.AppendLine(string.Format("\t\tQQ:{0} type:{1}   group: {2} ", qq.QQ,qq.Type, qq.GroupId));
                    
               }

#endif
            QQClient.LogManager.Log(sb.ToString());
            //应该将QQ好友，QQ群存入存入QQuser对象
            if (!e.InPacket.Finished)//好友没有下载完，继续下载
            {
                //LoginGetList(e.InPacket.NextPos);

                LoginGetList(++lisPackets);
            }
            else //好友下载完毕 触发事件
            { 
                QQClient.LogManager.Log("获取全部好友完成！");
                //分到各自的列表组
                foreach(QQBasicInfo qq in QQClient.QQUser.QQList.Values)
                {
                    switch(qq.Type)
                    {
                        case QQType.Cluster:
                            ClusterInfo ClusterInfo=new ClusterInfo();
                            ClusterInfo.QQBasicInfo=qq;
                            QQClient.QQUser.ClusterList.Add(qq.QQ,ClusterInfo);
                            break;
                        case QQType.QQ:
                            QQFriend QQFriend=new QQFriend();
                            QQFriend.QQBasicInfo=qq;
                            QQClient.QQUser.Friends.Add(qq.QQ,QQFriend);
                            break;
                        default:
                            QQClient.LogManager.Log("unknown QQ.Type: 0x"+qq.Type.ToString("X")+" qq="+qq.QQ.ToString() +" gid=0x"+qq.GroupId.ToString("X2"));
                            break;
                   
                    }
                }
                OutPacket outPacket = new LoginSendInfoPacket(this.QQClient);
                QQClient.PacketManager.SendPacketAnyway(outPacket, QQPort.Main.Name);

            }
        }

        internal void ProcessLoginSendInfoReply(LoginSendInfoReplyPacket loginSendInfoReplyPacket, LoginSendInfoPacket loginSendInfoPacket)
        {
            QQEventArgs<LoginSendInfoReplyPacket, LoginSendInfoPacket> e = new QQEventArgs<LoginSendInfoReplyPacket, LoginSendInfoPacket>(QQClient, loginSendInfoReplyPacket, loginSendInfoPacket);
            if (loginSendInfoReplyPacket.ReplyCode != 0x00)
            {

                QQClient.LogManager.Log("Err:ReplyCode=0x" + loginSendInfoReplyPacket.ReplyCode.ToString("X"));
                //Events
            }
            else 
            {
                QQClient.IsLogon = true;
                QQClient.QQUser.IsLoggedIn = true;
                QQClient.LoginStatus = LoginStatus.Login;
                QQClient.LogManager.Log("Login Successed!");
                //Events
            }
        }
    }
    
}
