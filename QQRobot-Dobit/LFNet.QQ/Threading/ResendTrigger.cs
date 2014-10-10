
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
using System.Threading;

using LFNet.QQ.Net;
using LFNet.QQ.Packets;
namespace LFNet.QQ.Threading
{
    /// <summary>重发包触发器
    /// </summary>
    public class ResendTrigger : IRunable
    {
        private QQClient client;
        // 超时队列
        private List<OutPacket> timeOutQueue;

        //string portName;
        public ResendTrigger(QQClient client)
        {
            this.client = client;
            timeOutQueue = new List<OutPacket>();
            //toPort = new Dictionary<OutPacket, string>();
            ThreadExcutor.RegisterIntervalObject(this, this, QQGlobal.QQ_TIMEOUT_SEND, true);
        }
        /// <summary>添加一个包到超时队列
    /// </summary>
        /// <param name="packet">The packet.</param>
        public void Add(OutPacket packet, string portName)
        {
            packet.PortName = portName;
            timeOutQueue.Add(packet);
        }
        /// <summary>清空重发队列
    /// </summary>
        public void Clear()
        {
            timeOutQueue.Clear();
            //toPort.Clear();
        }
        /// <summary> 得到超时队列的第一个包，不把它从队列中删除
    /// </summary>
        /// <returns></returns>
        public OutPacket Get()
        {
            if (timeOutQueue.Count > 0)
            {
                return timeOutQueue[timeOutQueue.Count - 1];
            }
            return null;
        }
        /// <summary>
        /// 得到超时队列的第一个包，并把它从队列中删除
    /// </summary>
        /// <returns></returns>
        public OutPacket Remove()
        {
            if (timeOutQueue.Count > 0)
            {
                OutPacket packet = timeOutQueue[timeOutQueue.Count - 1];
                timeOutQueue.Remove(packet);
                //portName = toPort[packet];
                //toPort.Remove(packet);
                return packet;
            }
            return null;

        }
        /// <summary>删除ack对应的请求包
    /// </summary>
        /// <param name="ack">The ack.</param>
        public void Remove(InPacket ack)
        {
            foreach (OutPacket packet in timeOutQueue)
            {
                if (packet.Equals(ack))
                {
                    timeOutQueue.Remove(packet);
                    //toPort.Remove(packet);
                    break;
                }
            }
        }
        /// <summary>得到下一个包的超时时间
        /// 下一个包的超时时间，如果队列为空，返回一个固定值
    /// </summary>
        /// <returns></returns>
        private long GetTimeoutLeft()
        {
            OutPacket packet = Get();
            if (packet == null)
            {
                return QQGlobal.QQ_TIMEOUT_SEND;
            }
            else
            {
                return packet.TimeOut - Utils.Util.GetTimeMillis(DateTime.Now);
            }
        }
        /// <summary>触发超时事件
    /// </summary>
        /// <param name="packet">The packet.</param>
        private void FireOperationTimeOutEvent(OutPacket packet, string portName)
        {
            ErrorPacket error = new ErrorPacket(ErrorPacketType.ERROR_TIMEOUT, client);
            error.TimeOutPacket = packet;
            error.Header = packet.Header;
            error.Family = packet.GetFamily();
            error.ConnectionId = portName;
            client.PacketManager.AddIncomingPacket(error, portName);
        }

        #region IRunable Members

        public bool IsRunning
        {
            get;
            private set;
        }
        public WaitHandle WaitHandler { get; set; }
        #endregion

        #region IRunable Members


        public void Run(object state, bool timedOut)
        {
            if (IsRunning == false)
            {
                lock (this)
                {
                    if (!IsRunning)
                    {
                        IsRunning = true;
                        long t = GetTimeoutLeft();
                        while (t <= 0)
                        {
                            OutPacket packet = Remove();
                            IConnection conn = client.ConnectionManager.ConnectionPool.GetConnection(packet.PortName);
                            if (conn != null && packet != null && !conn.Policy.IsReplied(packet, false))
                            {
                                if (packet.NeedResend())
                                {
                                    // 重发次数未到最大，重发
                                    client.PacketManager.SendPacketAnyway(packet, packet.PortName);
                                }
                                else
                                {
                                    // 触发操作超时事件
                                    FireOperationTimeOutEvent(packet, packet.PortName);
                                }
                            }
                            t = GetTimeoutLeft();
                        }
                        IsRunning = false;

                        // 继续等待 t 时间后再执行 // 先反注册原来的线程
                        this.RegisterdHandler.Unregister(this.WaitHandler);
                        ThreadExcutor.RegisterIntervalObject(this, this, t, true);
                    }
                }
            }
        }

        public System.Threading.RegisteredWaitHandle RegisterdHandler
        {
            get;
            set;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (this.WaitHandler != null && this.RegisterdHandler != null)
            {
                RegisterdHandler.Unregister(this.WaitHandler);
            }
        }

        #endregion
    }
}
