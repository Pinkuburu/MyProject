
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 得到在线成员的请求包，格式为：
    /// * 1. 头部
    /// * 2. 命令类型，得到成员信息是0x0B
    /// * 3. 群内部ID，4字节
    /// * 4. 尾部
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class ClusterGetOnlineMemberPacket : ClusterCommandPacket
    {
        public ClusterGetOnlineMemberPacket(QQClient client)
            : base(client)
        {
            SubCommand = ClusterCommand.GET_ONLINE_MEMBER;
        }
        public ClusterGetOnlineMemberPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Cluster Get Online Member Packet";
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
