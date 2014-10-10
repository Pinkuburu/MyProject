
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 修改群成员的请求包，格式为：
    /// * 1. 头部
    /// * 2. 命令类型，1字节，修改群成员是0x02
    /// * 3. 群内部id，4字节
    /// * 4. 操作类型，删除还是添加
    /// * 5. 删除或添加的成员QQ号，每个4字节，如果我即删除又添加了，需要发两个包
    /// * 6. 尾部
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class ClusterModifyMemberPacket : ClusterCommandPacket
    {
        public List<int> Members { get; set; }
        public byte Operation { get; set; }
        public ClusterModifyMemberPacket(QQClient client)
            : base(client)
        {
            SubCommand = ClusterCommand.MODIFY_MEMBER;
        }
        public ClusterModifyMemberPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Cluster Modify Member Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put((byte)SubCommand);
            buf.PutInt(ClusterId);
            buf.Put(Operation);
            foreach (int i in Members)
                buf.PutInt(i);
        }
    }
}
