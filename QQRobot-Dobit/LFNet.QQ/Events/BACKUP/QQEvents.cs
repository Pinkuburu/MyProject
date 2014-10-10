
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Packets;
using LFNet.QQ.Packets.In;
namespace LFNet.QQ.Events
{
    /// <summary>
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="O"></typeparam>
    public class QQEventArgs<I, O> : EventArgs
        where I : InPacket
        where O : OutPacket
    {
        /// <summary>
    /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="outPacket">The out packet.</param>
        public QQEventArgs(QQClient client, I inPacket, O outPacket)
        {
            this.QQClient = client;
            this.InPacket = inPacket;
            this.OutPacket = outPacket;
        }
        /// <summary>回复包
    /// </summary>
        /// <value></value>
        public I InPacket { get; private set; }
        /// <summary>对应的发送包
    /// </summary>
        /// <value></value>
        public O OutPacket { get; private set; }
        /// <summary>
    /// </summary>
        /// <value></value>
        public QQClient QQClient { get; private set; }
    }
}
