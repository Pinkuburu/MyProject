
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary>
    /// 这个Bean用在下载好友分组时
    /// </summary>
    public class DownloadFriendEntry
    {
        public uint QQ { get; set; }
        /// <summary>
        ///  好友类型，是好友，还是群
        /// </summary>
        /// <value></value>
        public FriendType Type { get; set; }

        /// <summary>
        /// 好友所在的组
        /// </summary>
        /// <value></value>
        public int Group { get; set; }
        /// <summary>
        /// 给定一个字节流，解析结构
        /// </summary>
        /// <param name="buf">The buf.</param>
        public void Read(ByteBuffer buf)
        {
            QQ = buf.GetUInt();
            Type = (FriendType)buf.Get();
            Group = buf.Get() >> 2;
        }
        /// <summary>
        /// true表示这一项表示一个群 
        /// </summary>
        /// <returns></returns>
        public bool IsCluster()
        {
            return Type == FriendType.IS_CLUSTER;
        }
    }
}
