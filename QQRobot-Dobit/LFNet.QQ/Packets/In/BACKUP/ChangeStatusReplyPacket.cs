
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.In
{
    /// <summary>
    ///  * 这个是用户自己改变在线状态的应答包，格式是
    /// </summary>
    public class ChangeStatusReplyPacket : BasicInPacket
    {
        public ReplyCode ReplyCode { get; set; }
        public ChangeStatusReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Change Status Reply Packet";
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
