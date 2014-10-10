
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 这是激活群的请求包，激活的用处是使其能够被其他人搜索到，格式为：
    /// * 1. 头部
    /// * 2. 命令类型字节，激活是0x5
    /// * 3. 内部ID，4字节
    /// * 4. 尾部
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class ClusterActivatePacket : ClusterCommandPacket
    {
        public ClusterActivatePacket(QQClient client)
            : base(client)
        {
            SubCommand = ClusterCommand.ACTIVATE_CLUSTER;
        }
        public ClusterActivatePacket(ByteBuffer buf, int length, QQClient client)
            : base(buf, length, client)
        {
        }
        public override string GetPacketName()
        {
            return "Cluster Activate Packet";
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
