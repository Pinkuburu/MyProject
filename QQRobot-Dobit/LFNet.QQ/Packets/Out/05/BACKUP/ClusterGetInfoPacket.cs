
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 1. 头部
    /// * 2. 群命令类型，1字节，得到信息是0x04
    /// * 3. 群内部ID，4字节
    /// * 4. 尾部
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class ClusterGetInfoPacket : ClusterCommandPacket
    {
        public ClusterGetInfoPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public ClusterGetInfoPacket(QQClient client)
            : base(client)
        {
            SubCommand = ClusterCommand.GET_CLUSTER_INFO;
        }
        public override string GetPacketName()
        {
            return "Cluster Get Info Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            // 命令类型
            buf.Put((byte)SubCommand);
            // 内部ID
            buf.PutInt(ClusterId);
        }
    }
}
