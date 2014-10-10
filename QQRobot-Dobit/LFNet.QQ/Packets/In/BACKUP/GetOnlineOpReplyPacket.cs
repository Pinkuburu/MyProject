
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Entities;
namespace LFNet.QQ.Packets.In
{
    /// <summary>
    ///  * 得到在线好友列表的应答包，格式为
    /// * 1. 头部
    /// * 2. 在线好友是否已经全部得到，1字节
    /// * 3. 31字节的FriendStatus结构
    /// * 4. 2个未知字节
    /// * 5. 1个字节扩展标志
    /// * 6. 1个字节通用标志
    /// * 7. 2个未知字节
    /// * 8. 1个未知字节
    /// * 9. 如果有更多在线好友，重复2-8部分
    /// * 10. 尾部
    /// * 
    /// * 这个回复包最多返回30个在线好友，如果有更多，需要继续请求
    /// 	<remark>abu 2008-02-22 </remark>
    /// </summary>
    public class GetOnlineOpReplyPacket : BasicInPacket
    {
        /// <summary>true表示没有更多在线好友了
        /// 	<remark>abu 2008-02-22 </remark>
        /// </summary>
        /// <value></value>
        public bool Finished { get; set; }
        /// <summary>
        /// 下一个请求包的起始位置，仅当finished为true时有效
        /// 	<remark>abu 2008-02-22 </remark>
        /// </summary>
        /// <value></value>
        public int Position { get; set; }
        // 在线好友bean列表
        public List<FriendStatus> OnlineFriends { get; set; }
        public GetOnlineOpReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Get Friend Online Reply Packet";
        }
        protected override void ParseBody(ByteBuffer buf)
        {
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
            // 当前好友列表位置
            Finished = buf.Get() == QQGlobal.QQ_POSITION_ONLINE_LIST_END;
            Position = 0;
            //只要还有数据就继续读取下一个friend结构
            OnlineFriends = new List<FriendStatus>();
            while (buf.HasRemaining())
            {
                //int QQ = buf.GetInt();
                FriendStatus entry =new FriendStatus();
                //if(Client.QQUser.Friends.Get(QQ)!=null)
                //    entry = Client.QQUser.Friends.Get(QQ).FriendStatus;//new FriendStatus();
                entry.Read(buf);
                // 添加到List
                OnlineFriends.Add(entry);
                // 如果还有更多好友，计算position
                if (!Finished)
                    Position = Math.Max(Position, entry.QQ);
            }
            Position++;
        }
    }
}
