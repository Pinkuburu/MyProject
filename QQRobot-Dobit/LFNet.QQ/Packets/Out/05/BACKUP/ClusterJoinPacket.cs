
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 申请加入群的包，格式为：
    /// * 1. 头部
    /// * 2. 命令类型，1字节，加入群是0x07
    /// * 3. 群的内部ID，4字节
    /// * 4. 尾部
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class ClusterJoinPacket : ClusterCommandPacket
    {
        public ClusterJoinPacket(QQClient client)
            : base(client)
        {
            SubCommand = ClusterCommand.JOIN_CLUSTER;
        }
        public ClusterJoinPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Cluster Join Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            // 命令类型
            buf.Put((byte)SubCommand);
            // 内部ID
            buf.PutInt(ClusterId);
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
        }
    }
}
