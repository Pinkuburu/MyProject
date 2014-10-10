
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Entities;
namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 个性签名操作请求包
    /// * 1. 头部
    /// * 2. 子命令，1字节
    /// * 
    /// * 根据2部分的不同
    /// * 为0x01时：
    /// * 3. 未知1字节
    /// * 4. 个性签名的字节长度，1字节
    /// * 5. 个性签名
    /// * 6. 尾部
    /// * 
    /// * 为0x00时，无后续内容
    /// * 3. 尾部
    /// * 
    /// * 为0x02时
    /// * 3. 未知的1字节
    /// * 4. 需要得到个性签名的QQ号数量，1字节
    /// * 5. QQ号，4字节
    /// * 6. 本地的个性签名修改时间，4字节
    /// * 7. 如果有更多QQ号，重复5-6部分
    /// * 8. 尾部 
    /// * 
    /// * 在得到好友的个性签名时，QQ的做法是对所有的QQ号排个序，每次最多请求33个。
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class SignatureOpPacket : BasicOutPacket
    {
        public SignatureSubCmd SubCommand { get; set; }
        public string Signature { get; set; }
        public List<Signature> Signatures { get; set; }

        public SignatureOpPacket(QQClient client)
            : base(QQCommand.Signature_OP_05,true,client)
        {
            SubCommand = SignatureSubCmd.MODIFY;
            Signature = string.Empty;
        }
        public SignatureOpPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Signature Op Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put((byte)SubCommand);
            switch (SubCommand)
            {
                case SignatureSubCmd.MODIFY:
                    buf.Put((byte)0x01);
                    byte[] b = Utils.Util.GetBytes(Signature);
                    buf.Put((byte)b.Length);
                    buf.Put(b);
                    break;
                case SignatureSubCmd.GET:
                    buf.Put((byte)0);
                    buf.Put((byte)Signatures.Count);
                    foreach (Signature sig in Signatures)
                    {
                        buf.PutInt(sig.QQ);
                        buf.PutInt(sig.ModifiedTime);
                    }
                    break;
            }
        }
    }
}
