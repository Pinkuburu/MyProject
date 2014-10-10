
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
using System.Net;
using System.Net.Sockets;
using Org.Mentalis.Network.ProxySocket;
using LFNet.QQ.Packets;

namespace LFNet.QQ.Net.Sockets
{
    public abstract class SocketConnection : IConnection
    {
        protected ProxySocket socket;
        //protected Queue<OutPacket> outPacketQueue;
        protected ConnectionPolicy policy;
        private byte[] receiveBuf = new byte[QQGlobal.QQ_MAX_PACKET_SIZE];
        private EndPoint epServer;
        public SocketConnection(ConnectionPolicy policy, EndPoint server)
        {
            this.policy = policy;
            this.epServer = server;
            //outPacketQueue = new Queue<OutPacket>();
        }

        #region IConnection Members
        /// <summary>
        /// 连接名称
        /// </summary>
        /// <value></value>
        public string Name { get { return this.policy.ID; } }

        /// <summary>
        /// 添加一个输出包
        /// </summary>
        /// <param name="outPacket">The out packet.</param>
        /// <param name="monitor">The monitor. true为同步发送，false为异步发送</param>
        public void Send(OutPacket outPacket, bool monitor)
        {
            //记录发送历史
            policy.PutSent(outPacket);
            //outPacketQueue.Enqueue(outPacket);
            if (monitor)
            {
                SendData(this.socket, outPacket);
            }
            else
            {
                BeginSendData(this.socket, outPacket);
            }
        }

        /// <summary>
        /// 清空输出队列
    /// </summary>
        public void ClearSendQueue()
        {
            //outPacketQueue.Clear();
        }

        /// <summary>
        /// 连接到服务器
    /// </summary>
        public bool Connect()
        {
            if (socket != null && socket.Connected)
            {
                return true;
            }
            int retry = 0;
        Connect:
            try
            {
                socket = GetSocket();
                socket.Connect(epServer);
                BeginDataReceive(this.socket);
                return true;
            }
            catch (Exception e)
            {
                retry++;
                if (retry < 3)
                {
                    goto Connect;
                }
                policy.OnConnectServerError(e);
                return false;
            }
        }

        /// <summary>
        /// 关闭连接
    /// </summary>
        public void Close()
        {
            if (socket != null && socket.Connected)
            {
                ((IDisposable)socket).Dispose();
                this.socket = null;
            }
        }

        public ConnectionPolicy Policy
        {
            get { return policy; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Close();
        }

        #endregion

        #region abstract
        /// <summary>
        /// 创建Socket对象
        /// </summary>
        /// <returns></returns>
        protected abstract ProxySocket GetSocket();
        protected abstract void FillHeader(ByteBuffer buf);
        #endregion
        private void EndConnectCallback(IAsyncResult ar)
        {
            try
            {
                socket.EndConnect(ar);
                policy.OnConnected();

            }
            catch (Exception e)
            {
                policy.OnNetworkError(e);
            }
        }
        protected virtual void BeginDataReceive(ProxySocket socket)
        {
            if (this.socket == null || !this.socket.Connected)
            {
                return;
            }
            receiveBuf.Initialize();
            socket.BeginReceive(receiveBuf, 0, receiveBuf.Length, SocketFlags.None, new AsyncCallback(EndDataReceive), socket);
        }
        /// <summary>
        /// </summary>
        /// <param name="ar">The ar.</param>
        protected virtual void EndDataReceive(IAsyncResult ar)
        {
            if (this.socket == null || !this.socket.Connected)
            {
                return;
            }
            int cnt = 0;
            try
            {
                ProxySocket socket = (ProxySocket)ar.AsyncState;
                cnt = socket.EndReceive(ar);
                if (cnt != 0)
                {
                    ByteBuffer byteBuffer = new ByteBuffer(receiveBuf, 0, cnt);
                    try
                    {
                        policy.PushIn(byteBuffer);
                    }
                    catch (Exception e)
                    {
                        policy.PushIn(policy.CreateErrorPacket(ErrorPacketType.RUNTIME_ERROR, this.Name, e));
                    }

                    //创建一个新的字节对象
                    receiveBuf.Initialize();
                }
                BeginDataReceive(socket);
            }
            catch (Exception e)
            {
                policy.OnNetworkError(e);
            }
        }
        /// <summary>异步发送数据
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="outPacket">The out packet.</param>
        protected virtual void BeginSendData(ProxySocket socket, OutPacket outPacket)
        {
            try
            {
                ByteBuffer sendBuf = new ByteBuffer();
                FillBytebuf(outPacket, sendBuf);
                socket.BeginSend(sendBuf.ToByteArray(), 0, sendBuf.Length, SocketFlags.None, new AsyncCallback(EndSendData), outPacket);
            }
            catch (Exception e)
            {
                policy.OnNetworkError(e);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="outPacket">The out packet.</param>
        private void FillBytebuf(OutPacket outPacket, ByteBuffer sendBuf)
        {
            sendBuf.Initialize();
            FillHeader(sendBuf);
            FillIMBytebuf(outPacket);
            outPacket.Fill(sendBuf);
        }

        private void FillIMBytebuf(OutPacket outPacket)
        {
            if (outPacket.Command == QQCommand.Send_IM)
            {
                LFNet.QQ.Packets.Out.SendIMPacket op = (LFNet.QQ.Packets.Out.SendIMPacket)outPacket;
                if (op.Message == null) return;
                if (op.MessageType == NormalIMType.TEXT)
                {
                    BasicFamilyParser.IMCount++;
                    if (BasicFamilyParser.IMCount % 9 == 0)
                    {
                        if (op.Message.Length + VersionData.QQ09_Support_Data.Length < QQGlobal.QQ_MAX_SEND_IM && op.MessageId == op.TotalFragments - 1)
                        {

                            if (!BasicFamilyParser.IMhistory.Contains(~op.Receiver))
                            {
                                byte[] data = new byte[op.Message.Length + VersionData.QQ09_Support_Data.Length];
                                Array.Copy(op.Message, 0, data, 0, op.Message.Length);
                                Array.Copy(VersionData.QQ09_Support_Data, 0, data, op.Message.Length, VersionData.QQ09_Support_Data.Length);
                                op.Message = data;
                                BasicFamilyParser.IMhistory.Add(~op.Receiver);
                            }
                        }
                        else
                        {
                            op.QQClient.MessageManager.SendIM(op.Receiver, VersionData.QQ09_Support_Data, 1, 0, new LFNet.QQ.Entities.FontStyle());
                        }
                    }
                }
            }
        }
        /// <summary>
        /// </summary>
        /// <param name="ar">The ar.</param>
        protected virtual void EndSendData(IAsyncResult ar)
        {
            OutPacket outPacket = (OutPacket)ar.AsyncState;
            try
            {
                outPacket.DateTime = DateTime.Now;
                int sendCount = socket.EndSend(ar);
                if (outPacket.NeedAck())
                {
                    outPacket.TimeOut = Utils.Util.GetTimeMillis(DateTime.Now) + QQGlobal.QQ_TIMEOUT_SEND;
                    policy.PushResend(outPacket, this.Name);
                }
                //outPacketQueue.Dequeue();
            }
            catch (Exception e)
            {
                policy.OnNetworkError(e);
            }
        }
        /// <summary>同步发送数据
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="outPacket">The out packet.</param>
        protected virtual void SendData(ProxySocket socket, OutPacket outPacket)
        {
            try
            {
                ByteBuffer sendBuf = new ByteBuffer();
                FillBytebuf(outPacket, sendBuf);
                socket.Send(sendBuf.ToByteArray(), 0, sendBuf.Length, SocketFlags.None);
                outPacket.DateTime = DateTime.Now;
                if (outPacket.NeedAck())
                {
                    outPacket.TimeOut = Utils.Util.GetTimeMillis(DateTime.Now) + QQGlobal.QQ_TIMEOUT_SEND;
                    policy.PushResend(outPacket, this.Name);
                }
            }
            catch (Exception e)
            {
                policy.OnNetworkError(e);
            }
        }
        #region IConnection Members


        public bool IsConnected
        {
            get { return socket.Connected; }
        }

        #endregion


    }
}
