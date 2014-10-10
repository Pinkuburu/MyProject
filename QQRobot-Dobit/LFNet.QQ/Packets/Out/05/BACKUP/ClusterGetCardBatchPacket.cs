
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 批量得到群名片中的真实姓名
    /// * 1. 头部
    /// * 2. 命令，1字节，0x0F
    /// * 3. 群内部ID，4字节
    /// * 4. 未知的4字节，全0
    /// * 5. 起始记录位置，4字节，从0开始，为1表示从第二条记录开始得到
    /// * 6. 尾部
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class ClusterGetCardBatchPacket : ClusterCommandPacket
    {
        public int Start { get; set; }
        public ClusterGetCardBatchPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public ClusterGetCardBatchPacket(QQClient client)
            : base(client)
        {
            SubCommand = ClusterCommand.GET_CARD_BATCH;
        }
        public override string GetPacketName()
        {
            return "Cluster Get Card Batch Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put((byte)SubCommand);
            buf.PutInt(ClusterId);
            buf.PutInt(0);
            buf.PutInt(Start);
        }
    }
}
