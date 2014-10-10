
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 收到消息之后我们发出的确认包
    /// * 1. 头部
    /// * 2. 消息发送者QQ号，4字节
    /// * 3. 消息接收者QQ号，4字节，也就是我
    /// * 4. 消息序号，4字节
    /// * 5. 发送者IP，4字节
    /// * 6. 尾部
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class ReceiveIMReplyPacket : BasicOutPacket
    {
        public byte[] Reply { get; set; }
        public ReceiveIMReplyPacket(byte[] reply, QQClient client)
            : base(QQCommand.Recv_IM, false, client)
        {
            this.Reply = reply;
            this.SendCount = 1;
        }
        public ReceiveIMReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Receive IM Reply Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put(Reply);
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
        }
    }
}
