
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.In
{
    /// <summary>
    /// 好友认证处理确认包
    ///  * 这个包是发送好友认证信息的确认包，格式为
    ///  * 1. 头部
    ///  * 2. 应答码，1字节
    ///  * 3. 尾部
    /// 	<remark>abu 2008-02-20 </remark>
    /// </summary>
    public class AddFriendAuthResponseReplyPacket : BasicInPacket
    {
        public ReplyCode ReplyCode { get; set; }
        /// <summary>
        /// 	<remark>abu 2008-02-20 </remark>
        /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="length">The length.</param>
        /// <param name="user">The user.</param>
        public AddFriendAuthResponseReplyPacket(ByteBuffer buf, int length, QQClient client)
            : base(buf, length, client)
        {
        }
        /// <summary>
        /// 包的描述性名称
        /// <remark>abu 2008-02-18 </remark>
        /// </summary>
        /// <returns></returns>
        public override string GetPacketName()
        {
            return "Add Friend Auth Response Reply Packet";
        }
        /// <summary>
        /// 解析包体，从buf的开头位置解析起
        /// <remark>abu 2008-02-18 </remark>
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void ParseBody(ByteBuffer buf)
        {
            ReplyCode = (ReplyCode)buf.Get();
        }

    }
}
