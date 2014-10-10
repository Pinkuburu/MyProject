
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * Logout请求包，这个包不需要服务器的应答，格式为
    /// * 1. 头部
    /// * 2. 16个00
    /// * 3. 尾部
    /// </summary>
    public class LogoutPacket : BasicOutPacket
    {
        public LogoutPacket(QQClient client)
            : base(QQCommand.Logout, false, client)
        {
            SendCount = 4;
        }
        public LogoutPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Logout Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Position += 15;//16个00
            buf.Put(0x00);
            //buf.Put(user.PasswordKey);
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
        }
    }
}
