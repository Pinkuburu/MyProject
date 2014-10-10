
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Entities;
namespace LFNet.QQ.Packets.In
{
    /// <summary>
    ///  * 发送短消息的回复包，格式为：
    /// * 1. 头部
    /// * 2. 未知1字节
    /// * 3. 四个未知字节，全0
    /// * 4. 未知1字节
    /// * 5. 回复消息长度，1字节
    /// * 6. 回复消息
    /// * 7. 接受者中的手机号码个数，1字节
    /// * 8. 手机的号码，18字节，不够的部分为0
    /// * 9. 未知的2字节，一般为0x0000
    /// * 10. 回复码，1字节，表示对于这个接受者来说，短信发送的状态如何
    /// * 11. 附加消息长度，1字节
    /// * 12. 附加消息
    /// * 13. 未知的1字节，一般都是0x00
    /// * 14. 如果有更多手机号，重复8-13部分
    /// * 注：8-14部分只有当7部分不为0时存在
    /// * 15. 接受者中QQ号码的个数，1字节
    /// * 16. QQ号码，4字节
    /// * 17. 回复码，1字节，表示对于这个接受者来说，短信发送的状态如何
    /// * 18. 附加消息长度，1字节
    /// * 19. 附加消息
    /// * 20. 未知的1字节，一般都是0x00
    /// * 21. 如果有更多QQ号，重复16-20部分
    /// * 注：16-21部分只有当15部分不为0时才存在
    /// * 22. 未知的1字节，一般是0x00
    /// * 23. 参考消息长度，1字节
    /// * 24. 参考消息
    /// * 25. 尾部
    /// 	<remark>abu 2008-02-26 </remark>
    /// </summary>
    public class SendSMSReplyPacket : BasicInPacket
    {
        public string Message { get; set; }
        public List<SMSReply> Replies { get; set; }
        public string Reference { get; set; }
        public SendSMSReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Send SMS Reply Packet";
        }
        protected override void ParseBody(ByteBuffer buf)
        {
            // 未知1字节
            buf.Get();
            // 未知4字节
            buf.GetInt();
            // 未知1字节
            buf.Get();
            // 回复消息
            int len = buf.Get() & 0xFF;
            Message = Utils.Util.GetString(buf, len);

            /*
             * 回复消息
             */
            Replies = new List<SMSReply>();
            // 手机个数
            len = buf.Get() & 0xFF;
            while (len-- > 0)
            {
                SMSReply reply = new SMSReply();
                reply.ReadMobile(buf);
                Replies.Add(reply);
            }
            // QQ号个数
            len = buf.Get() & 0xFF;
            while (len-- > 0)
            {
                SMSReply reply = new SMSReply();
                reply.ReadQQ(buf);
                Replies.Add(reply);
            }
            // 未知1字节
            buf.Get();
            // 参考消息
            len = buf.Get() & 0xFF;
            Reference = Utils.Util.GetString(buf, len);
        }
    }
}
