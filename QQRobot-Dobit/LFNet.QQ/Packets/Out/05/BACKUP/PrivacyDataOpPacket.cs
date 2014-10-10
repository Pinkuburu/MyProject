
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 隐私选项操作包
    /// * 1. 头部
    /// * 2. 子命令，1字节
    /// * 3. 操作，1字节，一般0x01是选择，0x00是不选
    /// * 4. 尾部
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class PrivacyDataOpPacket : BasicOutPacket
    {
        public PrivacySubCmd SubCommand { get; set; }
        public ValueSet OpCode { get; set; }
        public PrivacyDataOpPacket(QQClient client) : base(QQCommand.Privacy_Data_OP_05,true,client) { }
        public PrivacyDataOpPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Privacy Data Op Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put((byte)SubCommand);
            buf.Put((byte)OpCode);
        }
    }
}
