
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 请求得到好友列表基本信息
    /// </summary>
    public class GetFriendListPacket : BasicOutPacket
    {
        /// <summary>
        /// 好友列表开始位置，缺省是0
        /// </summary>
        /// <value></value>
        public ushort StartPosition { get; set; }
        public GetFriendListPacket(QQClient client)
            : base(QQCommand.Get_Friend_List, true, client)
        {
            StartPosition = QQGlobal.QQ_POSITION_FRIEND_LIST_START;
        }
        public GetFriendListPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Get Friend List Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put(0x01);
            buf.PutInt(0);
            buf.PutInt(0);
            buf.Put(0x02);
            buf.PutUShort(StartPosition);
            buf.Put(0);
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
        }
    }
}
