
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
    /// 包管理器
    /// 发送包，输入包，包事件
    /// 	<remark>abu 2008-03-08 </remark>
    /// </summary>
    public class PacketManager
    {
        internal PacketManager() { }
        /// <summary>
        /// 	<remark>abu 2008-03-08 </remark>
        /// </summary>
        /// <value></value>
        public QQClient QQClient { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        private Queue<InPacket> receiveQueue;
        private PacketIncomeTrigger packetIncomTrigger;
        private ResendTrigger resendTrigger;
        private ProcessorRouter router;
        private KeepAliveTrigger keepAliveTrigger;
        /// <summary>
        /// 	<remark>abu 2008-03-10 </remark>
        /// </summary>
        /// <param name="client">The client.</param>
        internal PacketManager(QQClient client)
        {
            router = new ProcessorRouter(client);
            router.InstallProcessor(new BasicFamilyProcessor(client));

            this.QQClient = client;
            receiveQueue = new Queue<InPacket>();

            SetupTrigger();
        }
        /// <summary>
        /// Setups the trigger.
        /// </summary>
        private void SetupTrigger()
        {
            this.packetIncomTrigger = new PacketIncomeTrigger(this.QQClient);
            this.resendTrigger = new ResendTrigger(this.QQClient);
            keepAliveTrigger = new KeepAliveTrigger(this.QQClient);
        }
        /// <summary>
        /// Setdowns the trigger.
        /// </summary>
        internal void SetdownTrigger()
        {
            ThreadExcutor.UnRegisterIntervaluObject(resendTrigger);
            ThreadExcutor.UnRegisterIntervaluObject(keepAliveTrigger);
        }
        #region 输入包处理

        /// <summary>
        /// 添加输入包
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        public void AddIncomingPacket(InPacket inPacket, string portName)
        {
            lock (receiveQueue)
            {
                if (inPacket == null)
                {
                    return;
                }
                inPacket.PortName = portName;
                receiveQueue.Enqueue(inPacket);
                //inConn.Add(inPacket, portName);
                ThreadExcutor.Submit(this.packetIncomTrigger, this);
            }            
        }
        /// <summary> 从接收队列中得到第一个包，并且把这个包从队列中移除
        /// 	<remark>abu 2008-03-07 </remark>
        /// </summary>
        /// <returns></returns>
        public InPacket RemoveIncomingPacket()
        {
            if (receiveQueue.Count == 0)
            {
                return null;
            }
            return receiveQueue.Dequeue();
        }

        /// <summary>
        /// 收到服务器确认
        /// 删除一个重发包
        /// 	<remark>abu 2008-03-08 </remark>
        /// </summary>
        /// <param name="packet">The packet.</param>
        public void RemoveResendPacket(InPacket packet)
        {
            resendTrigger.Remove(packet);
        }

        /// <summary>通知包处理器包到达事件
        /// 	<remark>abu 2008-03-07 </remark>
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        public void FirePacketArrivedEvent(InPacket inPacket)
        {
            router.PacketArrived(inPacket);
        }
        /// <summary>
        /// 添加重发包
        /// 	<remark>abu 2008-03-07 </remark>
        /// </summary>
        /// <param name="outPacket">The out packet.</param>
        public void AddResendPacket(OutPacket outPacket, string portName)
        {
            resendTrigger.Add(outPacket, portName);
        }
        #endregion

        #region 发送包
        /// <summary>
        /// 通用方法，发送一个packet
        ///* 这个方法用在一些包构造比较复杂的情况下，比如上传分组信息这种包，
        ///* 包中数据的来源是无法知道的也不是用几个参数就能概括的，可能也和实现有关。
        /// 	<remark>abu 2008-03-07 </remark>
        /// </summary>
        /// <param name="packet">The packet.</param>
        public void SendPacket(OutPacket packet)
        {
            SendPacket(packet, QQPort.Main.Name);
        }
        /// <summary> 通过指定port发送一个包
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="port">The port.</param>
        public void SendPacket(OutPacket packet, string port)
        {
            SendPacket(packet, port, false);
        }

        /// <summary>
        /// 通过指定port发送一个包
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="port">The port.</param>
        /// <param name="monitor">if set to <c>true</c> [monitor].</param>
        public void SendPacket(OutPacket packet, string port, bool monitor)
        {
            if (QQClient.QQUser.IsLoggedIn)
            {
                if (QQClient.ConnectionManager.EnsureConnection(port, true))
                {
                    QQClient.ConnectionManager.ConnectionPool.Send(port, packet, monitor);
                }
                else
                {
                    OnLostConnection(new QQEventArgs<InPacket, OutPacket>(QQClient, null, packet));
                }

            }
        }
        /// <summary>
        /// 不管有没有登录，都把包发出去
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="port">The port.</param>
        public void SendPacketAnyway(OutPacket packet, string port)
        {
            if (QQClient.ConnectionManager.EnsureConnection(port, true))
            {
                QQClient.ConnectionManager.ConnectionPool.Send(port, packet, false);
            }
            else
            {
                OnLostConnection(new QQEventArgs<InPacket, OutPacket>(QQClient, null, packet));
            }
        }
        #endregion

        #region events

        /// <summary>
        /// 收到未知包
        /// 	<remark>abu 2008-03-08 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<UnknownInPacket, OutPacket>> ReceivedUnknownPacket;
        /// <summary>
        /// Raises the <see cref="E:ReceivedUnknownPacket"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.UnknownInPacket&gt;"/> instance containing the event data.</param>
        internal void OnReceivedUnknownPacket(QQEventArgs<UnknownInPacket, OutPacket> e)
        {
            QQClient.LogManager.Log("Received Unknown Packet:" + e.InPacket.ToString());
            if (ReceivedUnknownPacket != null)
            {
                ReceivedUnknownPacket(this, e);
            }
        }

        /// <summary>
        /// 当一个包向服务器发送成功，并且收到服务器确认后
        /// 	<remark>abu 2008-03-08 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<InPacket, OutPacket>> SendPacketSuccessed;
        /// <summary>
        /// Raises the <see cref="E:SendedPacketSuccess"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.OutPacket&gt;"/> instance containing the event data.</param>
        internal void OnSendPacketSuccessed(QQEventArgs<InPacket, OutPacket> e)
        {
            QQClient.LogManager.Log("Send Packet Successed:" + e.OutPacket.ToString());
            if (SendPacketSuccessed != null)
            {
                SendPacketSuccessed(this, e);
            }
        }

        /// <summary>
        /// Occurs when [send packet time out].包发送超时事件 InPacket为null
        /// </summary>
        public event EventHandler<QQEventArgs<InPacket, OutPacket>> SendPacketTimeOut;
        /// <summary>
        /// Raises the <see cref="E:SendPacketTimeOut"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.InPacket,LFNet.QQ.Packets.OutPacket&gt;"/> instance containing the event data.</param>
        internal void OnSendPacketTimeOut(QQEventArgs<InPacket, OutPacket> e)
        {
            QQClient.LogManager.Log("Send Packet TimeOut:" + e.OutPacket.ToString());
            if (SendPacketTimeOut != null)
            {
                SendPacketTimeOut(this, e);
            }
        }

        /// <summary>无法得到有效的网络连接来发送包
        /// 	<remark>abu 2008-03-13 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<InPacket, OutPacket>> LostConnection;
        /// <summary>
        /// Raises the <see cref="E:LostConnection"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.InPacket,LFNet.QQ.Packets.OutPacket&gt;"/> instance containing the event data.</param>
        internal void OnLostConnection(QQEventArgs<InPacket, OutPacket> e)
        {
            QQClient.LogManager.Log("Lost Connection:" + e.OutPacket.ToString());
            if (LostConnection != null)
            {
                LostConnection(this, e);
            }
        }
        #endregion

    }

}
