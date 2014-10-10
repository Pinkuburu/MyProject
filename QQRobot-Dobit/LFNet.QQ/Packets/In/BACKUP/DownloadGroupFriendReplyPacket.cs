
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Entities;
namespace LFNet.QQ.Packets.In
{
    /// <summary>
    /// * 请求下载好友分组信息
    /// </summary>
    public class DownloadGroupFriendReplyPacket : BasicInPacket
    {
        /// <summary>
        /// Gets or sets the friends.
        /// </summary>
        /// <value>The friends.</value>
        public List<Group> Groups { get; set; }
        /// <summary>分组好友是否已经下载完
        /// Gets the finished.
        /// </summary>
        /// <value>The finished.</value>
        public bool Finished { get { return BeginFrom == 0; } }
        /// <summary>
        /// 起始好友号
        /// </summary>
        /// <value></value>
        public uint BeginFrom { get; set; }
        /// <summary>
        /// Gets or sets the reply code.
        /// </summary>
        /// <value>The reply code.</value>
        public ReplyCode ReplyCode { get; set; }
        /// <summary>
        /// Gets or sets the sub command.
        /// </summary>
        /// <value>The sub command.</value>
        public byte SubCommand { get; set; }
        public DownloadGroupFriendReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Download Group Friend Reply Packet";
        }
        /// <summary>
        /// 解析包体，从buf的开头位置解析起
        /// <remark>abu 2008-02-18 </remark>
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void ParseBody(ByteBuffer buf)
        {
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
            // 操作字节，下载为0x1F
            SubCommand = buf.Get();
            if (SubCommand == 0x1F)
            {
                // 起始好友号
                BeginFrom = buf.GetUInt();
                if (BeginFrom == 0x1000000) //no group labels info ??
                {
                    return;
                
                }
                if (BeginFrom != 0x00)
                {
                    Client.LogManager.Log("BeginFrom==0x"+BeginFrom.ToString("X"));
                }
                buf.Get();//0x17
                buf.GetChar();

                // 循环读取各好友信息，加入到list中
                Groups = new List<Group>();
                while (buf.HasRemaining())
                {
                    Group g = new Group(buf);
                    Groups.Add(g);
                }

            }
        }
    }
}
