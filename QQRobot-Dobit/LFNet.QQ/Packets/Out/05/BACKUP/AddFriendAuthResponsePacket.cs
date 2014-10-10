
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Utils;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    /// * 这个包是用来处理添加好友需要认证的情况，格式为
    /// * 1. 头部
    /// * 2. 认证的目的对象的QQ号的字符串形式
    /// * 3. 分隔符1字节，0x1F
    /// * 4. 命令，是请求还是拒绝请求，还是同意请求，1字节
    /// * 5. 分隔符1字节，0x1F
    /// * 6. 附带的消息
    /// * 7. 尾部
    /// 	<remark>abu 2008-02-20 </remark>
    /// </summary>
    public class AddFriendAuthResponsePacket : BasicOutPacket
    {
        private const byte DELIMIT = 0x1F;
        public AuthAction Action { get; set; }
        public int To { get; set; }
        public string Message { get; set; }
        /// <summary>
        /// 	<remark>abu 2008-02-20 </remark>
        /// </summary>
        /// <param name="user">The user.</param>
        public AddFriendAuthResponsePacket(QQClient client)
            : base(QQCommand.Add_Friend_Auth_05,true,client)
        {
            Message = string.Empty;
        }
        /// <summary>
        /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="length">The length.</param>
        /// <param name="user">The user.</param>
        public AddFriendAuthResponsePacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }

        protected override void PutBody(ByteBuffer buf)
        {
            // 组装内容
            // 目的的QQ号的字符串形式
            buf.Put(Util.GetBytes(To.ToString()));
            // 分隔符
            buf.Put(DELIMIT);
            // 响应码
            buf.Put((byte)Action);
            // 分隔符
            buf.Put(DELIMIT);
            // 附带消息
            byte[] msg = Util.GetBytes(Message);
            buf.Put(msg);
        }
        public override string GetPacketName()
        {
            return "Add Friend Auth Response Packet";
        }
    }
}
