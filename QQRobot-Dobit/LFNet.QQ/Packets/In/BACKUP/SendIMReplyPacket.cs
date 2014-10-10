
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.In
{
    /// <summary>
    ///  * 发送消息的回复消息，格式为
    /// * 1. 头部
    /// * 2. 一个字节的应答码，0表示成功
    /// * 3. 尾部
    /// 	<remark>abu 2008-02-26 </remark>
    /// </summary>
    public class SendIMReplyPacket : BasicInPacket
    {
        public ReplyCode ReplyCode { get; set; }
        public SendIMReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Send IM Reply Packet";
        }
        protected override void ParseBody(ByteBuffer buf)
        {
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
            ReplyCode = (ReplyCode)buf.Get();
        }
    }
}
