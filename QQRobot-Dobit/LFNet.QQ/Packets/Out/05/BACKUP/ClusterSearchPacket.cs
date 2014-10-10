
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 搜索群的包，格式为：
    /// * 1. 头部
    /// * 2. 命令类型，1字节，搜索群是0x06
    /// * 3. 查找方式，1字节，是搜索示范群还是根据ID搜索等等
    /// * 4. 群的外部ID，4字节，如果是搜索示范群，全0
    /// * 5. 尾部
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class ClusterSearchPacket : ClusterCommandPacket
    {
        public ClusterSearchType SearchType { get; set; }
        public int ExternalId { get; set; }
        public ClusterSearchPacket(QQClient client)
            : base(client)
        {
            SubCommand = ClusterCommand.SEARCH_CLUSTER;
        }
        public ClusterSearchPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Cluster Search Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            // 命令类型
            buf.Put((byte)SubCommand);
            // 群类型
            buf.Put((byte)SearchType);
            // 内部ID
            if (SearchType == ClusterSearchType.Demo)
                buf.PutInt(0);
            else
                buf.PutInt(ExternalId);
        }
    }
}
