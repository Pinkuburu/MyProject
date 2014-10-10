
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary> * 退出群的请求包，格式为：
    /// * 1. 头部
    /// * 2. 命令类型，退出是0x9
    /// * 3. 群内部ID，4字节
    /// * 4. 尾部
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class ClusterExitPacket : ClusterCommandPacket
    {
        public ClusterExitPacket(QQClient client) : base(client) {
            this.SubCommand = ClusterCommand.EXIT_CLUSTER;
        }
        public ClusterExitPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Cluster Exit Packet";
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
