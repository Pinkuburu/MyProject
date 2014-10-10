
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 刷新群内组织架构的请求包
    /// * 1. 头部
    /// * 2. 命令，1字节，0x12
    /// * 3. 群内部id，4字节
    /// * 4. 未知1字节，0x00
    /// * 5. 未知4字节，0x00000000
    /// * 6. 尾部
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class ClusterUpdateOrganizationPacket : ClusterCommandPacket
    {
        public ClusterUpdateOrganizationPacket(QQClient client)
            : base(client)
        {
            SubCommand = ClusterCommand.UPDATE_ORGANIZATION;
        }
        public ClusterUpdateOrganizationPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Cluster Update Organization Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put((byte)SubCommand);
            buf.PutInt(ClusterId);
            buf.Put((byte)0);
            buf.PutInt(0);
        }
    }
}
