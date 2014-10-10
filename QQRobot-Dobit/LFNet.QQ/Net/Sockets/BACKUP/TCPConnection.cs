

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

using Org.Mentalis.Network.ProxySocket;
namespace LFNet.QQ.Net.Sockets
{
    public class TCPConnection : SocketConnection
    {
        public TCPConnection(ConnectionPolicy policy, EndPoint server)
            : base(policy, server)
        {

        }
        protected override Org.Mentalis.Network.ProxySocket.ProxySocket GetSocket()
        {
            if (socket == null)
            {
                socket = new ProxySocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.ProxyType = ProxyTypes.None;
                //socket.SetSocketOption(SocketOptionLevel.Udp, SocketOptionName.SendTimeout, 3000);
            }
            return socket;
        }

        protected override void FillHeader(ByteBuffer buf)
        {

        }
    }
}
