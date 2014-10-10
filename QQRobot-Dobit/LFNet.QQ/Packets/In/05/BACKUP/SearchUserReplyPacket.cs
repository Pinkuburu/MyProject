
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Entities;

namespace LFNet.QQ.Packets.In
{
    /// <summary>
    /// * 搜索在线用户的回复包，格式为
    /// * 1. 头部
    /// * 2. 有两种形式
    /// *    第一种为搜索到了用户
    /// * 	  以0x1F相隔的用户数据，其中，一个用户的数据分4个域，域之间用0x1E相隔，四个域为
    /// * 	  i.   用户QQ号的字符串形式
    /// *    ii.  用户昵称
    /// *    iii. 用户所在地区
    /// *    iv.  用户的头像号码
    /// *    第二种是没有更多的匹配了，表示本次搜索的全部匹配已取得
    /// *    i. 字符串"-1"
    /// * 3. 尾部
    /// 	<remark>abu 2008-02-26 </remark>
    /// </summary>
    public class SearchUserReplyPacket : BasicInPacket
    {
        public List<UserInfo> Users { get; set; }
        public bool Finished { get; set; }
        public SearchUserReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Search User Reply Packet";
        }
        protected override void ParseBody(ByteBuffer buf)
        {
            // 判断搜索是否已经结束
            if (!buf.HasRemaining() || buf.Get() == 0x2D && buf.Get() == 0x31)
            {
                Finished = true;
                return;
            }
            buf.Rewind();
            // 只要还有数据就继续读取下一个friend结构
            Users = new List<UserInfo>();
            while (buf.HasRemaining())
            {
                UserInfo ui = new UserInfo();
                ui.Read(buf);

                // 添加到list
                Users.Add(ui);
            }
        }
    }
}
