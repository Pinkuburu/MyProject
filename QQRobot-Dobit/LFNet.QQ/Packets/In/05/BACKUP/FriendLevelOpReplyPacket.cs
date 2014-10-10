
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Entities;
namespace LFNet.QQ.Packets.In
{
    /// <summary>
    ///  * 这个查询QQ号等级的应答包，格式是
    /// * 1. 头部
    /// * 2. 子命令，1字节
    /// * 3. 查询的号码, 4字节
    /// * 4. 活跃天数, 4字节
    /// * 5. 等级, 2字节
    /// * 6. 升级剩余天数, 2字节
    /// * 7. 如果有更多好友，重复3-6部分
    /// * 8. 尾部
    /// 	<remark>abu 2008-02-22 </remark>
    /// </summary>
    public class FriendLevelOpReplyPacket : BasicInPacket
    {
        public List<FriendLevel> FriendLevels { get; set; }
        public byte SubCommand { get; set; }
        public FriendLevelOpReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        protected override void ParseBody(ByteBuffer buf)
        {
            SubCommand = buf.Get();
            FriendLevels = new List<FriendLevel>();
            while (buf.HasRemaining())
            {
                FriendLevel friendLevel = new FriendLevel();
                friendLevel.Read(buf);
                FriendLevels.Add(friendLevel);
            }
        }
        public override string GetPacketName()
        {
            return "Get Friend Level Reply Packet";
        }
    }
}
