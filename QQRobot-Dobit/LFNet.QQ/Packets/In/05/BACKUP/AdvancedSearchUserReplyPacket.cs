
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Entities;
namespace LFNet.QQ.Packets.In
{
    /// <summary>
    ///  * 高级搜索的回复包
    ///  * 1. 头部
    ///  * 2. 回复码，1字节，0x00表示还有数据，0x01表示没有更多数据了，当为0x01时，后面没有内容了
    ///  *    当为0x00时，后面才有内容
    ///  * 3. 页号，从1开始，2字节，如果页号后面没有内容了，那也说明是搜索结束了
    ///  * 4. QQ号，4字节
    ///  * 5. 性别，1字节，表示下拉框索引
    ///  * 6. 年龄，2字节
    ///  * 7. 在线，1字节，0x01表示在线，0x00表示离线
    ///  * 8. 昵称长度，1字节
    ///  * 9. 昵称
    ///  * 10. 省份索引，2字节
    ///  * 11. 城市索引，2字节，这个索引是以"不限"为0开始算的，shit
    ///  * 13. 头像索引，2字节
    ///  * 14. 如果有更多结果，重复4 - 13部分
    ///  * 15. 尾部
    /// 	<remark>abu 2008-02-20 </remark>
    /// </summary>
    public class AdvancedSearchUserReplyPacket : BasicInPacket
    {
        public ReplyCode ReplyCode { get; set; }
        public uint Page { get; set; }
        public List<AdvancedUserInfo> Users { get; set; }
        public bool Finished { get; set; }
        /// <summary>
        /// 	<remark>abu 2008-02-20 </remark>
        /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="length">The length.</param>
        /// <param name="user">The user.</param>
        public AdvancedSearchUserReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        /// <summary>
        /// 包的描述性名称
        /// <remark>abu 2008-02-18 </remark>
        /// </summary>
        /// <returns></returns>
        public override string GetPacketName()
        {
            return "Advanced Search User Reply Packet";
        }
        /// <summary>
        /// 解析包体，从buf的开头位置解析起
        /// <remark>abu 2008-02-18 </remark>
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void ParseBody(ByteBuffer buf)
        {
            ReplyCode = (ReplyCode)buf.Get();
            if (ReplyCode == ReplyCode.OK)
            {
                Page = buf.GetUInt();
                Users = new List<AdvancedUserInfo>();
                while (buf.HasRemaining())
                {
                    AdvancedUserInfo aui = new AdvancedUserInfo();
                    aui.ReadBean(buf);
                    Users.Add(aui);
                }
                Finished = Users.Count == 0;
            }
            else
                Finished = true;
        }
    }
}
