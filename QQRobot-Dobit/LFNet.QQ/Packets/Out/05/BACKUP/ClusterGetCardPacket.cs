
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 得到单个成员的全部群名片信息
    /// * 1. 头部
    /// * 2. 命令，1字节，0x10
    /// * 3. 群内部ID，4字节
    /// * 4. 需要得到群名片的成员QQ号，4字节
    /// * 5. 尾部
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class ClusterGetCardPacket : ClusterCommandPacket
    {
        public int QQ { get; set; }
        public ClusterGetCardPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public ClusterGetCardPacket(QQClient client)
            : base(client)
        {
            SubCommand = ClusterCommand.GET_CARD;
        }
        public override string GetPacketName()
        {
            return "Cluster Get Card Packet"; 
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put((byte)SubCommand);
            buf.PutInt(ClusterId);
            buf.PutInt(QQ);
        }
    }
}
