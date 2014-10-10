
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 修改临时群成员列表：
    /// * 1. 头部
    /// * 2. 群命令类型，1字节，0x31
    /// * 3. 临时群类型，1字节，0x01是多人对话，0x02是讨论组
    /// * 4. 父群内部ID，4字节
    /// * 5. 临时群内部ID，4字节
    /// * 6. 操作类型，0x01是添加，0x02是删除，常量定义在QQ.java中
    /// * 7. 要操作的QQ号，4字节
    /// * 8. 如果有更多成员要操作，重复7部分
    /// * 9. 尾部
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class ClusterModifyTempMemberPacket : ClusterCommandPacket
    {
        public ClusterType Type { get; set; }
        public int ParentClusterId { get; set; }
        public byte Operation { get; set; }
        public List<int> Members { get; set; }
        public ClusterModifyTempMemberPacket(QQClient client)
            : base(client)
        {
            SubCommand = ClusterCommand.MODIFY_TEMP_MEMBER;
        }
        public ClusterModifyTempMemberPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Modify Temp Cluster Member Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            // 命令类型
            buf.Put((byte)SubCommand);
            // 临时群类型
            buf.Put((byte)Type);
            // 父群ID
            buf.PutInt(ParentClusterId);
            // 临时群ID
            buf.PutInt(ClusterId);
            // 操作方式
            buf.Put(Operation);
            // 成员QQ号
            foreach (int i in Members)
                buf.PutInt(i);
        }
    }
}
