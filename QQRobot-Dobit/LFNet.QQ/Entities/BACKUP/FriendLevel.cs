
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary>
    /// 好友等级信息数据结构
    /// </summary>
    public class FriendLevel
    {
        public uint QQ { get; set; }
        public uint ActiveDays { get; set; }
        public ushort Level { get; set; }
        public ushort UpgradeDays { get; set; }
        /// <summary>从缓冲区中读取一个FriendLevel结构
    /// </summary>
        /// <param name="buf">The buf.</param>
        public void Read(ByteBuffer buf)
        {
            QQ = buf.GetUInt();
            ActiveDays = buf.GetUInt();
            Level = buf.GetUShort();
            UpgradeDays = buf.GetUShort();
        }
    }
}
