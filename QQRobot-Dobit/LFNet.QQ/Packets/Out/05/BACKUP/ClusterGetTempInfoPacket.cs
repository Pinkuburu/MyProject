
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 得到临时群的资料：
    /// * 1. 头部
    /// * 2. 群命令类型，1字节，0x33
    /// * 3. 临时群类型，1字节，0x01是多人对话，0x02是讨论组
    /// * 4. 父群内部ID，4字节
    /// * 5. 临时群内部ID，4字节
    /// * 5. 尾部
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class ClusterGetTempInfoPacket : ClusterCommandPacket
    {
        public ClusterType Type { get; set; }
        public int ParentClusterId { get; set; }
        public ClusterGetTempInfoPacket(QQClient client)
            : base(client)
        {
            SubCommand = ClusterCommand.GET_TEMP_INFO;
        }
        public ClusterGetTempInfoPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        protected override void PutBody(ByteBuffer buf)
        {
            // 命令类型
            buf.Put((byte)SubCommand);
            // 类型
            buf.Put((byte)Type);
            // 内部ID
            buf.PutInt(ParentClusterId);
            // 外部ID
            buf.PutInt(ClusterId);
        }
        public override string GetPacketName()
        {
            return "Cluster Get Temp Cluster Info Packet"; 
        }
    }
}
