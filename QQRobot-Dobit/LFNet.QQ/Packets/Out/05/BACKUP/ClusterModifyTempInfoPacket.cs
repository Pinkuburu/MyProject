
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 修改临时群资料
    /// * 1. 头部
    /// * 2. 命令，1字节, 0x34
    /// * 3. 临时群类型, 1字节
    /// * 4. 父群内部id，4字节
    /// * 5. 临时群内部id，4字节
    /// * 6. 临时群名称字节长度，1字节
    /// * 7. 临时群名称
    /// * 8. 尾部
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class ClusterModifyTempInfoPacket : ClusterCommandPacket
    {
        public int ParentClusterId { get; set; }
        public string Name { get; set; }
        public ClusterType Type { get; set; }
        public ClusterModifyTempInfoPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public ClusterModifyTempInfoPacket(QQClient client)
            : base(client)
        {
            SubCommand = ClusterCommand.MODIFY_TEMP_INFO;
        }
        public override string GetPacketName()
        {
            return "Cluster Modify Temp Cluster Info Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put((byte)SubCommand);
            buf.Put((byte)Type);
            buf.PutInt(ParentClusterId);
            buf.PutInt(ClusterId);
            byte[] nameBytes = Utils.Util.GetBytes(Name);
            buf.Put((byte)(nameBytes.Length & 0xFF));
            buf.Put(nameBytes);
        }
    }
}
