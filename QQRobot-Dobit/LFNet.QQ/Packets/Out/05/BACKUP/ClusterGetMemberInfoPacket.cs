
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Entities;
namespace LFNet.QQ.Packets.Out
{
    /// <summary> * 得到群中成员信息的请求包，格式为：
    /// * 1. 头部
    /// * 2. 命令类型，得到成员信息是0x0C
    /// * 3. 群内部ID，4字节
    /// * 4. 需要得到信息的成员QQ号，4字节
    /// * 5. 如果要得到多个成员的信息，重复4部分
    /// * 6. 尾部
    /// * 
    /// * 注意：一次最多只能得到61个成员信息，而实际操作中我们按照30个一组来得到
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class ClusterGetMemberInfoPacket : ClusterCommandPacket
    {
        public List<int> Members { get; set; }
        public ClusterGetMemberInfoPacket(QQClient client)
            : base(client)
        {
            SubCommand = ClusterCommand.GET_MEMBER_INFO;
            Members = new List<int>();
        }
        public ClusterGetMemberInfoPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Cluster Get Member Info Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            // 命令类型
            buf.Put((byte)SubCommand);
            // 内部ID
            buf.PutInt(ClusterId);
            // 需要得到信息的成员QQ号列表
            foreach (int qq in Members)
                buf.PutInt(qq);
        }
    }
}
