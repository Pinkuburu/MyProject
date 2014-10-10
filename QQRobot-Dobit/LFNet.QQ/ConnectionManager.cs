

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
    /// 连接管理
    /// 	<remark>abu 2008-03-06 </remark>
    /// </summary>
    public class ConnectionManager : IDisposable
    {
        internal ConnectionManager() { }
        /// <summary>
        /// </summary>
        /// <value></value>
        public IConnectionPool ConnectionPool { get; private set; }
        /// <summary>
        /// </summary>
        /// <value></value>
        public QQClient QQClient { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionManager"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        internal ConnectionManager(QQClient client)
        {
            QQClient = client;
            ConnectionPool = new LFNet.QQ.Net.Sockets.ConnectionPool();
        }
        /// <summary>用户可以使用这个方法更改连接池的实现
        /// 	<remark>abu 2008-03-07 </remark>
        /// </summary>
        /// <param name="pool">The pool.</param>
        public void SetConnectionPool(IConnectionPool pool)
        {
            this.ConnectionPool = pool;
        }

        /// <summary>
        /// 确认指定的PortName的连接存在
        /// </summary>
        /// <param name="serverHost">The server host.</param>
        /// <param name="port">The port.</param>
        /// <param name="portName">Name of the port.</param>
        /// <param name="start">if set to <c>true</c> [start].</param>
        public bool EnsureConnection(string portName, bool start)
        {
            if (ConnectionPool.HasConnection(portName) && ConnectionPool.GetConnection(portName).IsConnected)
            {
                return true;
            }
            else
            {
                IConnection conn = QQPort.GetPort(portName).Create(QQClient, QQClient.LoginServerHost, QQClient.LoginPort, start);
                if (conn == null)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 根据连接名称获得连接
        /// </summary>
        /// <param name="portName">Name of the port.</param>
        public IConnection GetConnection(string portName)
        {
            return ConnectionPool.GetConnection(portName);
        }
        /// <summary>
        /// 一般网络错误事件
        /// 	<remark>abu 2008-03-06 </remark>
        /// </summary>
        public event EventHandler<ErrorEventArgs> NetworkError;
 
        /// <summary>
        /// 网络连接成功
        /// 	<remark>abu 2008-03-06 </remark>
        /// </summary>
        public event EventHandler ConnectSuccessed;

        /// <summary>
        /// 连接服务器失败
        /// 	<remark>abu 2008-03-10 </remark>
        /// </summary>
        public event EventHandler<ErrorEventArgs> ConnectServerError;
        /// <summary>
        /// Raises the <see cref="E:ConnectServerError"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.ErrorEventArgs"/> instance containing the event data.</param>
        internal void OnConnectServerError(Exception e)
        {
            QQClient.LogManager.Log(e.Message + "\r\n" + e.StackTrace);
            if (ConnectServerError != null)
            {
                ConnectServerError(this, new ErrorEventArgs(e));
            }
        }
        /// <summary>
        /// Called when [network error].
        /// </summary>
        /// <param name="e">The e.</param>
        internal protected void OnNetworkError(Exception e)
        {
            QQClient.LogManager.Log(e.Message + "\r\n" + e.StackTrace);
            if (NetworkError != null)
            {
                NetworkError(this, new ErrorEventArgs(e));
            }
        }


        /// <summary>
        /// Called when [connect successed].
        /// </summary>
        internal protected void OnConnectSuccessed()
        {
            if (ConnectSuccessed != null)
            {
                ConnectSuccessed(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 接收到保持连接回复包
        /// </summary>
        public event EventHandler<QQEventArgs<KeepAliveReplyPacket, KeepAlivePacket>> ReceivedKeepAlive;
        /// <summary>
        /// Raises the <see cref="E:ReceivedKeepAlive"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.KeepAliveReplyPacket&gt;"/> instance containing the event data.</param>
        internal void OnReceivedKeepAlive(QQEventArgs<KeepAliveReplyPacket, KeepAlivePacket> e)
        {
            if (ReceivedKeepAlive != null)
            {
                ReceivedKeepAlive(this, e);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            ConnectionPool.Dispose();
        }

        #endregion
    }
}
