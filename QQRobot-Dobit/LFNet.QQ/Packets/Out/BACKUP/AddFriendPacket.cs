
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    /// * 这个是添加好友的时候用的包
    /// * 1. 头部
    /// * 2. 要加的人的QQ号的字符串形式
    /// * 3. 尾部
    /// 如果不需要验证时会向对方发送添加成功信息，并将通知添加成功
    /// 否则会返回，需要验证，需要回答问题，拒绝任何人添加的信息
    /// </summary>
    public class AddFriendPacket : BasicOutPacket
    {
        public int To { get; set; }

        public AddFriendPacket(QQClient client) : base(QQCommand.Add_Friend,true,client) { this.resendCountDown = 10; }
        public AddFriendPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        protected override void PutBody(ByteBuffer buf)
        {
            // 要加的QQ号的字符串形式
            buf.PutInt(To);
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
        }
        public override string GetPacketName()
        {
            return "Add Friend Ex Packet";
        }
    }
}
