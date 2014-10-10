
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Packets;
namespace LFNet.QQ.Net
{
    /// <summary>
    /// 连接策略
    /// </summary>
    public class ConnectionPolicy
    {
        QQClient client;
        PacketHelper helper;
        public ProtocolFamily SupportedFamily { get; private set; }
        public ProtocolFamily RelocateFamily { get; private set; }
        /// <summary>连接ID
    /// </summary>
        /// <value></value>
        public string ID { get; private set; }
        public ConnectionPolicy(QQClient client, string id, ProtocolFamily supportedFamily, ProtocolFamily relocateFamily)
        {
            this.ID = id;
            this.client = client;
            this.SupportedFamily = supportedFamily;
            this.RelocateFamily = relocateFamily;
            helper = new PacketHelper();
        }
        /// <summary>
        /// 一般网络错误时
        /// Called when [exception].
        /// </summary>
        /// <param name="e">The e.</param>
        public void OnNetworkError(Exception e)
        {
            client.ConnectionManager.OnNetworkError(e);
        }
        public void OnConnectServerError(Exception e)
        {
            client.ConnectionManager.OnConnectServerError(e);
        }

        /// <summary>
        /// 连接服务器成功后
        /// Called when [connected].
        /// </summary>
        public void OnConnected()
        {
            client.ConnectionManager.OnConnectSuccessed();
        }


        /// <summary>
        /// 创建一个错误包
    /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <returns></returns>
        public ErrorPacket CreateErrorPacket(ErrorPacketType errorCode, string portName, Exception e)
        {
            ErrorPacket errorPacket = new ErrorPacket(errorCode, client, e);
            errorPacket.Family = SupportedFamily;
            errorPacket.ConnectionId = portName;
            return errorPacket;
        }

        /// <summary>压入一个重发包
    /// </summary>
        /// <param name="outPacket">The out packet.</param>
        public void PushResend(OutPacket outPacket, string portName)
        {
            client.PacketManager.AddResendPacket(outPacket, portName);
        }

        public void PushIn(InPacket inPacket)
        {
            client.PacketManager.AddIncomingPacket(inPacket, ID);
        }
        public void PushIn(ByteBuffer receiveIn)
        {
            InPacket inPacket = ParseIn(receiveIn);
            this.PushIn(inPacket);
        }

        /// <summary>
        /// 解析输入包
    /// </summary>
        /// <param name="buf">The buf.</param>
        /// <returns></returns>
        public InPacket ParseIn(ByteBuffer buf)
        {
            return helper.ParseIn(SupportedFamily, buf, client);
        }
        public void PutSent(OutPacket outPacket)
        {
            helper.PutSent(outPacket);
        }
        public bool IsReplied(OutPacket packet, bool add)
        {
            return helper.IsReplied(packet, add);
        }
        public OutPacket RetrieveSent(InPacket inPacket)
        {
            return helper.RetriveSent(inPacket);
        }
        /// <summary>
        /// 使用的代理信息
    /// </summary>
        /// <value></value>
        public Proxy Proxy
        {
            get { return client.Proxy; }
        }
    }
}
