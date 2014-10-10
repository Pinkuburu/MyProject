
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

using Org.Mentalis.Network.ProxySocket;

namespace LFNet.QQ.Net.Sockets
{
    public class UDPConnection : SocketConnection
    {
        public UDPConnection(ConnectionPolicy policy, EndPoint server)
            : base(policy, server)
        {

        }
        protected override Org.Mentalis.Network.ProxySocket.ProxySocket GetSocket()
        {
            if (socket == null)
            {
                socket = new ProxySocket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socket.ProxyType = ProxyTypes.None;
                //socket.SetSocketOption(SocketOptionLevel.Udp, SocketOptionName.SendTimeout, 3000);
            }
            return socket;
        }

        /// <summary>
        /**
        * 添加代理包的头部，Socks5代理包的格式为
        * +----+------+------+----------+----------+----------+
        * |RSV | FRAG | ATYP | DST.ADDR | DST.PORT |   DATA   |
        * +----+------+------+----------+----------+----------+
        * | 2  |  1   |  1   | Variable |    2     | Variable |
        * +----+------+------+----------+----------+----------+
        */
    /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void FillHeader(ByteBuffer buf)
        {
            //  buf.PutChar((char)0);
            //buf.Put((byte)0)
            //buf.Put(0x1);
            //buf.Put(this.)
            //buf.putChar((char)proxy.remotePort);
        }
    }
}
