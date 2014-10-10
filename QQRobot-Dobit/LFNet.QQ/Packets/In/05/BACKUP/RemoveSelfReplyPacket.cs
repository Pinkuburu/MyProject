
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.In
{
    /// <summary>
    ///  * 删除别人好友列表中的自己的回复包，格式
    /// * 1. 头部
    /// * 2. 一个字节的应答码
    /// * 3. 尾部
    /// 	<remark>abu 2008-02-26 </remark>
    /// </summary>
    public class RemoveSelfReplyPacket : BasicInPacket
    {
        public ReplyCode ReplyCode { get; set; }

        public RemoveSelfReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }

        public override string GetPacketName()
        {
            return "Remove Self Reply Packet";
        }
        protected override void ParseBody(ByteBuffer buf)
        {
            ReplyCode = (ReplyCode)buf.Get();
        }
    }
}
