
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary> * Keep Alive包，这个包的格式是
    /// * 1. 头部
    /// * 2. 用户QQ号的字符串形式
    /// * 3. 尾部
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class KeepAlivePacket : BasicOutPacket
    {
        public KeepAlivePacket(QQClient client)
            : base(QQCommand.Keep_Alive, true, client)
        {
            // 刻意增加了keep alive包的发送次数，实验性质，不知能否减少网络包
            // 太多情况下的掉线可能性
            SendCount = 10;
        }
        public KeepAlivePacket(ByteBuffer buf, int length,QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Keep Alive Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            byte[] data = new byte[16];
            byte[] QQBytes = Utils.Util.GetBytes(user.QQ.ToString());
            Array.Copy(QQBytes,data,QQBytes.Length);
            buf.Put(data);
#if DEBUG
            Client.LogManager.Log(ToString() + " UnBody:" + Utils.Util.ToHex(buf.ToByteArray()));
#endif
        }
    }
}
