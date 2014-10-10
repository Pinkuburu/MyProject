
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Entities;
namespace LFNet.QQ.Packets.In
{
    /// <summary>
    ///  * 个性签名操作的回复包
    /// * 1. 头部
    /// * 2. 子命令，1字节
    /// * 3. 回复码，1字节
    /// * 
    /// * 如果2部分为0x00, 0x01，则
    /// * 4. 尾部
    /// * 
    /// * 如果2部分为0x02，即得到个性签名，则还有
    /// * 4. 下一个起始的QQ号，4字节。为这个回复包中所有QQ号的最大值加1
    /// * 5. QQ号，4字节
    /// * 6. 个性签名最后修改时间，4字节。这个修改时间的用处在于减少网络I/O，只有第一次我们需要
    /// *    得到所有的个性签名，以后我们只要送出个性签名，然后服务器会比较最后修改时间，修改过的
    /// *    才发回来
    /// * 7. 个性签名字节长度，1字节
    /// * 8. 个性签名
    /// * 9. 如果有更多，重复5-8部分
    /// * 10. 尾部
    /// 	<remark>abu 2008-02-26 </remark>
    /// </summary>
    public class SignatureOpReplyPacket : BasicInPacket
    {
        public SignatureSubCmd SubCommand { get; set; }
        public ReplyCode ReplyCode { get; set; }
        public int NextQQ { get; set; }
        public List<Signature> Signatures { get; set; }

        public SignatureOpReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        protected override void ParseBody(ByteBuffer buf)
        {
            SubCommand = (SignatureSubCmd)buf.Get();
            ReplyCode = (ReplyCode)buf.Get();
            if (SubCommand == SignatureSubCmd.GET)
            {
                NextQQ = buf.GetInt();
                Signatures = new List<Signature>();
                while (buf.HasRemaining())
                {
                    Signature sig = new Signature();
                    sig.Read(buf);
                    Signatures.Add(sig);
                }
            }
        }
        public override string GetPacketName()
        {
            return "Signature Op Reply Packet"; 
        }
    }
}
