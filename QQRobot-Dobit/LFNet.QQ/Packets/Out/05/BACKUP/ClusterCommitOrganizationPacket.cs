
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Entities;
namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 提交组织架构到服务器
    /// * 1. 头部
    /// * 2. 命令，1字节，0x11
    /// * 3. 群内部id，4字节
    /// * 4. 组织个数，2字节
    /// * 5. 组织序号，1字节，从1开始
    /// * 6. 组织的层次关系，4字节。层次关系的格式参见ClusterCommandReplyPacket注释
    /// * 7. 组织名称字节长度，1字节
    /// * 8. 组织名称
    /// * 9. 如果有更多组织，重复5-8部分
    /// * 10. 尾部
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class ClusterCommitOrganizationPacket : ClusterCommandPacket
    {
        public List<QQOrganization> Organizations { get; set; }
        public ClusterCommitOrganizationPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public ClusterCommitOrganizationPacket(QQClient client)
            : base(client)
        {
            SubCommand = ClusterCommand.COMMIT_ORGANIZATION;
        }
        public override string GetPacketName()
        {
            return "Cluster Commit Organization Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put((byte)SubCommand);
            buf.PutInt(ClusterId);
            buf.PutChar((char)Organizations.Count);
            foreach (QQOrganization org in Organizations)
            {
                buf.Put((byte)org.Id);
                buf.PutInt(org.Path);
                byte[] nameBytes = Utils.Util.GetBytes(org.Name);
                buf.Put((byte)nameBytes.Length);
                buf.Put(nameBytes);
            }
        }
    }
}
