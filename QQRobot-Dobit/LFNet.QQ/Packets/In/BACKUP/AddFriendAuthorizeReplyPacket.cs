
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.In
{
    /// <summary>
    ///  * 发送验证消息的回复包
    ///  * 1. 头部
    ///  * 2. 子命令，1字节
    ///  * 3. 要添加的QQ号，4字节
    ///  * 4. 回复码，1字节
    ///  * 5. 尾部
    /// 	<remark>abu 2008-02-20 </remark>
    /// </summary>
    public class AddFriendAuthorizeReplyPacket : BasicInPacket
    {
        public AddFriendAuthSubCmd SubCommand { get; set; }
        /// <summary>
        /// 操作的QQ
        /// </summary>
        public int To { get; set; }
        public ReplyCode ReplyCode { get; set; }
        public AddFriendAuthorizeReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        protected override void ParseBody(ByteBuffer buf)
        {
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
            SubCommand =(AddFriendAuthSubCmd) buf.Get();
            To = buf.GetInt();
            ReplyCode = (ReplyCode)buf.Get();
        }
        public override string GetPacketName()
        {
            return "Authorize Reply Packet";
        }
    }
}
