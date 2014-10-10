
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
            outPacket.Fill(sendBuf);
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
