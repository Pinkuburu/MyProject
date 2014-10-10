
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Entities;
namespace LFNet.QQ.Packets.Out
{
    /// <summary> * 提交成员分组情况到服务器
    /// * 1. 头部
    /// * 2. 命令，1字节，0x13
    /// * 3. 群内部id，4字节
    /// * 4. 未知1字节，0x00
    /// * 5. 成员QQ号，4字节
    /// * 6. 成员所属组织序号，1字节，没有组织时是0x00
    /// * 7. 如果有更多成员，重复5-6部分
    /// * 8. 尾部
    /// * 
    /// * 注意：不需要一次提交所有成员分组情况，如果只有个别成员的分组变动了（比如拖动操作），
    /// * 那么只需要提交改变的成员。所以这个操作不象修改临时群成员那样，又有添加又有删除的，
    /// * 可以一个包搞定了
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class ClusterCommitMemberOrganizationPacket : ClusterCommandPacket
    {
        public List<Member> Members { get; set; }
        public ClusterCommitMemberOrganizationPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public ClusterCommitMemberOrganizationPacket(QQClient client)
            : base(client)
        {
            SubCommand = ClusterCommand.COMMIT_MEMBER_ORGANIZATION;
        }
        public override string GetPacketName()
        {
            return "Cluster Commit Member Organization Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put((byte)SubCommand);
            buf.PutInt(ClusterId);
            buf.Put((byte)0);
            foreach (Member m in Members)
            {
                buf.PutInt(m.QQ);
                buf.Put((byte)m.Organization);
            }
        }
    }
}
