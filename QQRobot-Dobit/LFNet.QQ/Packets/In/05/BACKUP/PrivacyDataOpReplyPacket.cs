
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.In
{
    /// <summary>
    ///  * 隐私选项操作包
    /// *1. 头部
    /// *2. 子命令，1字节
    /// *3. 操作，1字节
    /// *4. 回复码，1字节
    /// *5. 尾部
    /// 	<remark>abu 2008-02-22 </remark>
    /// </summary>
    public class PrivacyDataOpReplyPacket : BasicInPacket
    {
        public PrivacySubCmd SubCommand { get; set; }
        public ValueSet OpCode { get; set; }
        public ReplyCode ReplyCode { get; set; }
        public PrivacyDataOpReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Privacy Data Op Reply Packet";
        }
        protected override void ParseBody(ByteBuffer buf)
        {
            SubCommand = (PrivacySubCmd)buf.Get();
            OpCode = (ValueSet)buf.Get();
            ReplyCode = (ReplyCode)buf.Get();
        }
    }
}
