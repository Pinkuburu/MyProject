
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.In
{
    /// <summary>
    /// 无需在事件中更改用户状态了
    ///  * 好友状态改变包，这个是从服务器发来的包，格式为
    /// * 1. 头部
    /// * 2. 好友QQ号，4字节
    /// * 3. 未知的4字节
    /// * 4. 未知的4字节
    /// * 5. 好友改变到的状态，1字节
    /// * 6. 好友的客户端版本，2字节。这个版本号不是包头中的source，是内部表示，比如2004是0x04D1
    /// * 7. 未知用途的密钥，16字节
    /// * 8. 用户属性标志，4字节
    /// * 9. 我自己的QQ号，4字节
    /// * 10. 未知的2字节
    /// * 11. 未知的1字节
    /// * 12. 尾部
    /// </summary>
    public class FriendChangeStatusPacket : BasicInPacket
    {
        public uint FriendQQ { get; set; }
        public uint MyQQ { get; set; }
        public QQStatus Status { get; set; }
        public uint UserFlag { get; set; }
        public byte[] UnknownKey { get; set; }
        public char ClientVersion { get; set; }
        public FriendChangeStatusPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Friend Change Status Packet";
        }
        protected override void ParseBody(ByteBuffer buf)
        {
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
            FriendQQ = buf.GetUInt();
            buf.GetUInt();
            buf.GetUInt();
            Status = (QQStatus)buf.Get();
            Client.QQUser.Friends.SetFriendStatus((int)FriendQQ, Status);
            ClientVersion = buf.GetChar();
            UnknownKey = buf.GetByteArray(QQGlobal.QQ_LENGTH_KEY);
            UserFlag = buf.GetUInt();
            MyQQ = buf.GetUInt();
            buf.GetChar();
            buf.Get();
            
        }
    }
}
