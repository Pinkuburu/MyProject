
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 把自己从某人的好友名单中删除，这种情况发生在你把一个人拖进黑名单时，格式是
    /// * 1. 头部
    /// * 2. 对方QQ号，4个字节
    /// * 3. 尾部
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class RemoveSelfPacket : BasicOutPacket
    {
        public int RemoveFrom { get; set; }
        public RemoveSelfPacket(QQClient client) : base(QQCommand.Remove_Self_05,true,client) { }
        public RemoveSelfPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Remove Self Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.PutInt(RemoveFrom);
        }
    }
}
