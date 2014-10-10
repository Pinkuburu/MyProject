
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 1. 头部
    /// * 2. 子命令，1字节
    /// * 3. 群内部ID，4字节
    /// * 4. 尾部
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class ClusterDismissPacket : ClusterCommandPacket
    {
        public ClusterDismissPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public ClusterDismissPacket(QQClient client)
            : base(client)
        {
            SubCommand = ClusterCommand.DISMISS_CLUSTER;
        }
        public override string GetPacketName()
        {
            return "Dismiss Cluster Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put((byte)SubCommand);
            buf.PutInt(ClusterId);
        }
    }
}
