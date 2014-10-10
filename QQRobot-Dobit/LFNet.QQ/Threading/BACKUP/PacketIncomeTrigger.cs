
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Packets;

namespace LFNet.QQ.Threading
{
    /// <summary>包括处理触发器
    /// </summary>
    public class PacketIncomeTrigger : ICallable
    {
        private QQClient client;
        public PacketIncomeTrigger(QQClient client)
        {
            this.client = client;
        }
        #region ICallable Members

        /// <summary>
    /// </summary>
        /// <value></value>
        public bool IsRunning
        {
            get;
            private set;
        }

        /// <summary>
        /// WaitCallback回调
    /// </summary>
        /// <param name="state">The state.</param>
        public void Call(object state)
        {
            if (!IsRunning)
            {
                lock (this)
                {
                    if (!IsRunning)
                    {
                        IsRunning = true;
                        InPacket inPacket = client.PacketManager.RemoveIncomingPacket();
                        while (inPacket != null)
                        {
                            client.PacketManager.FirePacketArrivedEvent(inPacket);
                            inPacket = client.PacketManager.RemoveIncomingPacket();
                        }
                        IsRunning = false;
                    }
                }

            }
        }

        #endregion
    }
}
