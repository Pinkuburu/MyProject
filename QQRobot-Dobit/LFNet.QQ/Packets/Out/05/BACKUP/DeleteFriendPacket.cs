
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    /// 这个包用来删除一个好友，格式为:
    /// * 1. 头部
    /// * 2. 要删除的好友的QQ号的字符串形式
    /// * 3. 尾部
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class DeleteFriendPacket : BasicOutPacket
    {
        public int To { get; set; }
        public DeleteFriendPacket(QQClient client) : base(QQCommand.Delete_Friend_05,true,client) { }
        public DeleteFriendPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Delete Friend Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            // 好友的QQ号的字符串形式
            buf.Put(Utils.Util.GetBytes(To.ToString()));
        }

    }
}
