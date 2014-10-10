
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
    /// </summary>
    public class QQClient
    {
        public QQUser QQUser { get; private set; }

       
        /// <summary>
        /// 服务器地址
        /// </summary>
        /// <value></value>
        public string LoginServerHost { get; set; }
        /// <summary>
        /// 登录端口
        /// </summary>
        /// <value></value>
        public int LoginPort { get; set; }

        private LoginStatus _loginStatus=LoginStatus.Logout;
        /// <summary>
        /// 登陆状态
        /// </summary>
        public LoginStatus LoginStatus
        {
            get { return _loginStatus; }
            set
            {
                if (_loginStatus != value)
                {
                    _loginStatus = value;
                    //LoginStatusChangedEventArgs e = new LoginStatusChangedEventArgs(value);
                    LoginStatusChangedEventArgs<LoginStatus> e = new LoginStatusChangedEventArgs<LoginStatus>(value);
                    this.LoginManager.OnLoginStatusChanged(e);
                }
            }
        }

        /// <summary>
        /// 是否已登录
        /// </summary>
        /// <value></value>
        [Obsolete]
        public bool IsLogon { get; set; }
        /// <summary>
        /// 经过重定向登录
        /// </summary>
        /// <value></value>
        public bool LoginRedirect { get; set; }
        /// <summary>
        /// 连接管理
        /// 	<remark>abu 2008-03-06 </remark>
        /// </summary>
        /// <value></value>
        public ConnectionManager ConnectionManager { get; private set; }
        /// <summary>包管理
        /// Gets or sets the packet manager.
        /// </summary>
        /// <value>The packet manager.</value>
        public PacketManager PacketManager { get; private set; }
        /// <summary>登录管理
        /// Gets or sets the login manager.
        /// </summary>
        /// <value>The login manager.</value>
        public LoginManager LoginManager { get; private set; }
        /// <summary>
        /// 日志管理
        /// </summary>
        public QQLog LogManager { get; private set; }

        /// <summary>
        /// 服务器信息
        /// </summary>
        public ServerInfo ServerInfo { get; private set; }

        /// <summary>
        /// 服务器返回的时间
        /// </summary>
        public byte[] ServerTime { get; set; }
        /// <summary>
        /// 服务器返回的客户端IP
        /// </summary>
        public byte[] ClientIP { get; set; }
        /// <summary>消信管理
        /// Gets or sets the message manager.
        /// </summary>
        /// <value>The message manager.</value>
        public MessageManager MessageManager { get; private set; }
        /// <summary>好友管理
        /// Gets or sets the friend manager.
        /// </summary>
        /// <value>The friend manager.</value>
        public FriendManager FriendManager { get; private set; }
         ///<summary>个人资料管理
         ///Gets or sets the private manager.
         ///</summary>
         ///<value>The private manager.</value>
        public PrivateManager PrivateManager { get; private set; }

        /// <summary>群管理
        /// 	<remark>abu 2008-03-29 </remark>
        /// </summary>
        /// <value></value>
        public ClusterManager ClusterManager { get; private set; }
        /// <summary>
        /// 使用的代理服务器
        /// </summary>
        /// <value></value>
        public Proxy Proxy { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="user">The user.</param>
        public QQClient(QQUser user)
        {
            PacketManager = new PacketManager(this);
            ConnectionManager = new ConnectionManager(this);
            LoginManager = new LoginManager(this);
            MessageManager = new MessageManager(this);
            FriendManager = new FriendManager(this);
            PrivateManager = new PrivateManager(this);
            ClusterManager = new ClusterManager(this);

            // this.inConn = new Dictionary<InPacket, string>();
            this.QQUser = user;
            this.Proxy = new Proxy();
            this.LogManager = new QQLog(this);
#if DEBUG
            this.LogManager.Log("uin:" + this.QQUser.QQ.ToString() + "-->" + this.QQUser.QQ.ToString("X8") + "\r\nPassword md5_1: " + Utils.Util.ToHex(this.QQUser.Password) + "\r\nPassword md5_2 (key): " + Utils.Util.ToHex(this.QQUser.QQKey.PasswordKey));
#endif
            this.ServerInfo = new ServerInfo();
            
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="port">The port.</param>
        public void Login(string server, int port)
        {
            this.LoginServerHost = server;
            this.LoginPort = port;
            this.Login();
        }

        /// <summary>
        /// </summary>
        public void Login()
        {
            LoginManager.Login();
        }
        /// <summary>
        /// 保持登录状态
        /// </summary>
        public void KeepAlive()
        {
            if (IsLogon)
            {
                KeepAlivePacket packet = new KeepAlivePacket(this);
                PacketManager.SendPacket(packet, QQPort.Main.Name);
            }
        }


        #region Events
        /// <summary>
        /// 错误事件
        /// 	<remark>abu 2008-03-06 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<ErrorPacket, OutPacket>> Error;
        /// <summary>
        /// Called when [error].
        /// </summary>
        /// <param name="e">The e.</param>
        internal void OnError(QQEventArgs<ErrorPacket, OutPacket> e)
        {
            if (Error != null)
            {
                Error(this, e);
            }
        }
        #endregion

        /// <summary>
        /// 在程序出现运行时异常时产生一个崩溃报告
        /// </summary>
        /// <param name="e">The e.</param>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public string GenerateCrashReport(Exception e, Packets.Packet p)
        {
            return e.Message + "\n" + e.StackTrace;
        }
    }
}
