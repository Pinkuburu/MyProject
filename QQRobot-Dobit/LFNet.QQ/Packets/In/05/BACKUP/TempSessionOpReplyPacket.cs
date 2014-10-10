
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.In
{
    /// <summary>
    ///  * 临时会话操作回复包，格式为
    /// * 1. 头部
    /// * 2. 子命令，1字节
    /// * 当2部分为0x01时，格式为
    /// * 3. 接收者QQ号，4字节
    /// * 4. 回复码，1字节
    /// * 5. 回复消息长度，1字节
    /// * 6. 回复消息
    /// * 7. 尾部
    /// 	<remark>abu 2008-02-26 </remark>
    /// </summary>
    public class TempSessionOpReplyPacket : BasicInPacket
    {
        public string ReplyMessage { get; set; }
        public int Receiver { get; set; }
        public ReplyCode ReplyCode { get; set; }
        public TempSessionSubCmd SubCommand { get; set; }
        public TempSessionOpReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            switch (SubCommand)
            {
                case TempSessionSubCmd.SendIM:
                    return "Temp Session IM Reply Packet";
                default:
                    return "Unknown Temp Session Op Reply Packet";                    
            }
        }
        protected override void ParseBody(ByteBuffer buf)
        {
            SubCommand = (TempSessionSubCmd)buf.Get();
            switch (SubCommand)
            {
                case TempSessionSubCmd.SendIM:
                    Receiver = buf.GetInt();
                    ReplyCode = (ReplyCode)buf.Get();
                    int len = buf.Get() & 0xFF;
                    ReplyMessage = Utils.Util.GetString(buf, len);
                    break;
                default:
                    break;
            }
        }
    }
}
