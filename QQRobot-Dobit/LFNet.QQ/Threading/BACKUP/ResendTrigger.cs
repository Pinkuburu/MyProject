
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
