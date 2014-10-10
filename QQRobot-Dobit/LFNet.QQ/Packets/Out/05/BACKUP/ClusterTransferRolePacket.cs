
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 转让角色的请求包
 /// * 1. 头部
 /// * 2. 群命令类型，1字节，0x1B
 /// * 3. 群内部ID，4字节
 /// * 4. 要转让到的QQ号，4字节
 /// * 5. 尾部
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class ClusterTransferRolePacket : ClusterCommandPacket
    {
        public int QQ { get; set; }
        public ClusterTransferRolePacket(QQClient client)
            : base(client)
        {
            this.SubCommand = ClusterCommand.TRANSFER_ROLE;
        }
        public ClusterTransferRolePacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Cluster Set Role Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            // 群命令类型
            buf.Put((byte)SubCommand);
            // 群内部ID
            buf.PutInt(ClusterId);
            // 接收者QQ号
            buf.PutInt(QQ);
        }
    }
}
