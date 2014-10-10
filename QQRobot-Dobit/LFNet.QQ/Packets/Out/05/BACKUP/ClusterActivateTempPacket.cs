
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary> * 激活临时群：
    /// * 1. 头部
    /// * 2. 群命令类型，1字节，0x37
    /// * 3. 临时群类型，1字节，0x01是多人对话，0x02是讨论组
    /// * 4. 父群内部ID，4字节
    /// * 5. 临时群内部ID，4字节
    /// * 6. 尾部
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class ClusterActivateTempPacket : ClusterCommandPacket
    {
        public byte Type { get; set; }
        public int ParentClusterId { get; set; }
        public ClusterActivateTempPacket(QQClient client) : base(client)
        {
            SubCommand = ClusterCommand.ACTIVATE_TEMP;
        }
        public ClusterActivateTempPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Cluster Get Temp Cluster Info Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            // 命令类型
            buf.Put((byte)SubCommand);
            // 类型
            buf.Put(Type);
            // 内部ID
            buf.PutInt(ParentClusterId);
            // 外部ID
            buf.PutInt(ClusterId);
        }
    }
}
