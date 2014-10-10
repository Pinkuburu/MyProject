
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.In
{
    /// <summary>
    ///  * 这个添加好友的应答包，格式是
    ///  * 1. 头部
    ///  * 2. 要添加的好友的QQ号,4字节
    ///  * 3. 回复码，1字节
    ///  * 4. 附加条件码，1字节，比如是不是需要认证，等等
    ///  * 注：仅当3部分为0x00时，4部分才存在
    ///  * 5. 尾部
    /// 	<remark>abu 2008-02-20 </remark>
    /// </summary>
    public class AddFriendReplyPacket : BasicInPacket
    {
        public ReplyCode ReplyCode { get; set; }
        public AuthType AuthCode { get; set; }
        public int FriendQQ { get; set; }
        public AddFriendReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Add Friend Reply Packet";
        }
        protected override void ParseBody(ByteBuffer buf)
        {
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
            FriendQQ = buf.GetInt();
            ReplyCode = (ReplyCode)buf.Get();
            if (ReplyCode == ReplyCode.OK)
            {
                AuthCode = (AuthType)buf.Get();
            }
        }
    }
}
