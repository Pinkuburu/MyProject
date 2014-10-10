
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.In
{
    /// <summary>
    ///  * 上传分组好友列表的回复包，格式为
    /// * 1. 头部
    /// * 2. 应答码，0为成功，其他未知
    /// * 3. 尾部
    /// 	<remark>abu 2008-02-26 </remark>
    /// </summary>
    public class UploadGroupFriendReplyPacket : BasicInPacket
    {
        public ReplyCode ReplyCode { get; set; }
        public UploadGroupFriendReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Upload Group Friend Reply Packet";
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
