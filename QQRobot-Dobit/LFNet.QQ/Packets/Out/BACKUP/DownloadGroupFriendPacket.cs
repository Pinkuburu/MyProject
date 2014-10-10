
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 请求下载分组好友列表的消息包
    /// </summary>
    public class DownloadGroupFriendPacket : BasicOutPacket
    {
        public int BeginFrom { get; set; }
        public DownloadGroupFriendPacket(QQClient client) : base(QQCommand.GroupLabel,true,client) { }
        public DownloadGroupFriendPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }

        public override string GetPacketName()
        {
            return "Download Group Friend Packet";
        }
        /// <summary>
        /// 初始化包体
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put((byte)0x1F);
            buf.Put((byte)0x01);
            buf.PutInt(BeginFrom);
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
        }
    }
}
