
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 退出临时群
    /// * 1. 头部
    /// * 2. 命令，1字节，0x32
    /// * 3. 临时群类型，1字节，0x01是多人对话，0x02是讨论组
    /// * 4. 讨论组的父群内部ID，4字节，多人对话没有父群，所以这里是0
    /// * 5. 讨论组内部ID，4字节
    /// * 6. 尾部
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class ClusterExitTempPacket : ClusterCommandPacket
    {
        public ClusterType Type { get; set; }
        public int ParentClusterId { get; set; }
        public ClusterExitTempPacket(QQClient client) : base(client)
        {
            SubCommand = ClusterCommand.EXIT_TEMP;
        }
        public ClusterExitTempPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Cluster - Exit Temp Cluster Packet"; 
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put((byte)SubCommand);
            buf.Put((byte)Type);
            buf.PutInt(ParentClusterId);
            buf.PutInt(ClusterId);
        }
    }
}
