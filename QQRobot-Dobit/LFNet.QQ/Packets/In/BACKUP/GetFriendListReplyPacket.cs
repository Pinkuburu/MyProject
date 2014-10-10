
using System;
using System.Collections.Generic;
using System.Text;
using LFNet.QQ.Entities;
namespace LFNet.QQ.Packets.In
{
    /// <summary>
    ///  * 请求好友列表的应答包，格式为
    /// * 1. 头部
    /// * 2. 下一次好友列表开始位置，这个位置是你所有好友排序后的位置，如果为0xFFFF，那就是你的好友已经全部得到了
    /// *    每次都固定的返回50个好友，所以如果不足50个了，那么这个值一定是0xFFFF了
    /// * 3. 好友QQ号，4字节
    /// * 4. 头像，2字节
    /// * 5. 年龄，1字节
    /// * 6. 性别，1字节
    /// * 7. 昵称长度，1字节
    /// * 8. 昵称，不定字节，由8指定
    /// * 9. 用户标志字节，4字节
    /// * 10. 重复3-9的结构
    /// * 11.尾部
    /// 	<remark>abu 2008-02-22 </remark>
    /// </summary>
    public class GetFriendListReplyPacket : BasicInPacket
    {
        /// <summary>
        /// 是否已经结束
        /// </summary>
        /// <value></value>
        public bool Finished { get; set; }
        public ushort Position { get; set; }
        /// <summary>
        /// 本次获取的好友信息，自动更新到好友列表
        /// </summary>
        public List<QQFriend> Friends { get; set; }
        public GetFriendListReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Get Friend List Reply Packet";
        }
        protected override void ParseBody(ByteBuffer buf)
        {
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
            buf.Position += 10;
            // 当前好友列表位置
            Position = buf.GetUShort();
            Finished = Position == 0xFFFF;
            buf.Position += 5;
            // 只要还有数据就继续读取下一个friend结构
            Friends = new List<QQFriend>();
            while (buf.Position+5<buf.Length)
            {
                int qq = buf.GetInt();
                QQFriend friend=Client.QQUser.Friends.Get(qq);
                if(friend==null)
                    friend=Client.QQUser.Friends.Add(qq);
                    friend.Read(buf);
                    Friends.Add(friend);
               
                
            }
        }
    }
}
