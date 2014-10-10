
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 请求登录令牌的包，格式为：
    /// * 1. 头部
    /// * 2. 未知的1字节，0x00
    /// * 3. 尾部
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class RequestLoginTokenPacket : BasicOutPacket
    {
        public RequestLoginTokenPacket(QQClient client) : base(QQCommand.Request_Login_Token_05, true, client) { }
        public RequestLoginTokenPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Request Login Token Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put((byte)0);
        }
    }
}
