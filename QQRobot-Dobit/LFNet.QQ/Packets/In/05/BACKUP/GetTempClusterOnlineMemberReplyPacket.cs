
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.In
{
    /// <summary>
    /// * 得到临时群在线成员回复包
    /// *1. 头部
    /// *2. 未知1字节，0x01
    /// *3. 回复码，1字节
    /// *4. 未知1字节，0x3C
    /// *5. 在线成员QQ号，4字节
    /// *6. 如果有更多在线成员，重复4部分
    /// *7. 尾部
    /// 	<remark>abu 2008-02-22 </remark>
    /// </summary>
    public class GetTempClusterOnlineMemberReplyPacket : BasicInPacket
    {
        public ReplyCode ReplyCode { get; set; }
        public List<uint> Onlines { get; set; }
        public GetTempClusterOnlineMemberReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Get Temp Cluster Online Member Reply Packet";
        }
        protected override void ParseBody(ByteBuffer buf)
        {
            buf.Get();
            ReplyCode = (ReplyCode)buf.Get();
            buf.Get();
            Onlines = new List<uint>();
            while (buf.HasRemaining())
                Onlines.Add(buf.GetUInt());
        }
    }
}
