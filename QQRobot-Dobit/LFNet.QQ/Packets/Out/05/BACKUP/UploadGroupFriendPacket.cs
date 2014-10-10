
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 上传分组中好友列表的消息包，格式为
    /// * 1. 头部
    /// * 2. 好友的QQ号，4字节
    /// * 3. 好友所在的组序号，0表示我的好友组，自己添加的组从1开始
    /// * 4. 如果有更多好友，重复2，3部分
    /// * 5. 尾部
    /// * 
    /// * 并不需要每次都上传所有的好友，比如如果在使用的过程中添加了一个好友，那么
    /// * 可以只上传这个好友即可
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class UploadGroupFriendPacket : BasicOutPacket
    {
        public Dictionary<int, List<int>> Friends { get; set; }
        public UploadGroupFriendPacket(QQClient client)
            : base(QQCommand.Upload_Group_Friend_05,true,client)
        {
            Friends = new Dictionary<int, List<int>>();
        }
        public UploadGroupFriendPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Upload Group Friend Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            int i = 0;
            // 写入每一个好友的QQ号和组序号
            foreach (List<int> list in Friends.Values)
            {
                // 等于null说明这是一个空组，不用处理了			
                if (list != null)
                {
                    foreach (int qq in list)
                    {
                        buf.PutInt(qq);
                        buf.Put((byte)i);
                    }
                }
                i++;
            }
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
        }
        /// <summary>添加好友信息
        /// 	<remark>abu 2008-02-29 </remark>
        /// </summary>
        /// <param name="gIndex">Index of the g.</param>
        /// <param name="qqNum">The qq num.</param>
        public void addFriend(int gIndex, int qqNum)
        {
            List<int> gList = null;
            if (Friends.ContainsKey(gIndex))
                gList = Friends[gIndex];
            else
            {
                gList = new List<int>();
                Friends.Add(gIndex, gList);
            }
            gList.Add(qqNum);
        }
    }
}
