
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.In
{
    /// <summary>
    ///  * 删除好友的回复包，格式为
    ///  * 1. 头部
    ///  * 2. 应答码，1字节
    ///  * 3. 尾部
    /// 	<remark>abu 2008-02-22 </remark>
    /// </summary>
    public class DeleteFriendReplyPacket : BasicInPacket
    {
        public ReplyCode ReplyCode { get; set; }
        public DeleteFriendReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Delete Friend Reply Packet";
        }
        protected override void ParseBody(ByteBuffer buf)
        {
            ReplyCode = (ReplyCode)buf.Get();
        }
    }
}
