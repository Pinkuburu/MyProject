
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.In
{
    /// <summary>
    /// * 请求登录令牌的回复包，这个包的source字段和其他包不同，为QQ.QQ_SERVER_0000
    /// * 1. 头部
    /// * 2. 回复码，1字节，0x00表示成功
    /// * 3. 登录令牌长度，1字节
    /// * 4. 登录令牌
    /// * 5. 尾部
    /// 	<remark>abu 2008-02-26 </remark>
    /// </summary>
    public class RequestLoginTokenReplyPacket : BasicInPacket
    {
        public ReplyCode ReplyCode { get; set; }
        public byte[] LoginToken { get; set; }
        public RequestLoginTokenReplyPacket(ByteBuffer buf, QQClient client) : base(buf, client) { }
        public RequestLoginTokenReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Request Login Token Reply Packet";
        }
        protected override void ParseBody(ByteBuffer buf)
        {
            ReplyCode = (ReplyCode)buf.Get();
            if (ReplyCode == ReplyCode.OK)
            {
                int len = buf.Get() & 0xFF;
                LoginToken = buf.GetByteArray(len);
            }
        }
    }
}
