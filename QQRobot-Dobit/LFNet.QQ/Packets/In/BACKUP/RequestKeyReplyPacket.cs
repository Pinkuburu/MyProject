
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.In
{
    /// <summary> * 请求密钥的回复包，格式为:
    /// * 1. 头部
    /// * 2. 子命令，1字节
    /// * 3. 未知字节，应该是回复码，0表示成功
    /// * 4. 密钥，16字节
    /// * 5. 未知的8字节
    /// * 6. 未知的4字节
    /// * 7. 文件中转认证令牌字节长度
    /// * 8. 令牌
    /// * 9. 未知的4字节
    /// * 10. 尾部
    /// 	<remark>abu 2008-02-26 </remark>
    /// </summary>
    public class RequestKeyReplyPacket : BasicInPacket
    {
        public byte[] Key { get; set; }
        public byte[] Token { get; set; }
        public byte SubCommand { get; set; }
        public ReplyCode ReplyCode { get; set; }
        public RequestKeyReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Request Key Reply Packet";
        }
        protected override void ParseBody(ByteBuffer buf)
        {
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
            SubCommand = buf.Get();//4 file key
            ReplyCode = (ReplyCode)buf.Get();
            if (ReplyCode == ReplyCode.OK)
            {
                //密钥
                Key = buf.GetByteArray(QQGlobal.QQ_LENGTH_KEY);
                // 未知的8字节
                // 未知的4字节
                buf.Position = buf.Position + 12;
                // 文件中转认证令牌字节长度
                int len = buf.Get() & 0xFF;
                // 令牌
                Token = buf.GetByteArray(len);
            }
        }
    }
}
