
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 创建临时群的请求包
    /// * 1. 头部
    /// * 2. 子命令类型，1字节，0x30
    /// * 3. 临时群类型，1字节
    /// * 4. 父群内部ID，4字节
    /// * 5. 名称长度，1字节
    /// * 6. 名称
    /// * 7. 成员QQ号，4字节
    /// * 8. 如果有更多成员，重复6部分
    /// * 10. 尾部
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class ClusterCreateTempPacket : ClusterCommandPacket
    {
        public int ParentClusterId { get; set; }
        public string Name { get; set; }
        public List<int> Members { get; set; }
        public ClusterType Type { get; set; }

        public ClusterCreateTempPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public ClusterCreateTempPacket(QQClient client)
            : base(client)
        {
            SubCommand = ClusterCommand.CREATE_TEMP;
        }
        public override string GetPacketName()
        {
            return "Cluster - Create Temp Cluster Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            // 子命令类型，1字节，0x30
            buf.Put((byte)SubCommand);
            // 临时群类型，1字节
            buf.Put((byte)Type);
            // 父群内部ID，4字节
            buf.PutInt(ParentClusterId);
            // 名称长度，1字节
            byte[] b = Utils.Util.GetBytes(Name);
            buf.Put((byte)(b.Length & 0xFF));
            // 名称
            buf.Put(b);
            // 成员QQ号，4字节
            foreach (int i in Members)
                buf.PutInt(i);
        }
    }
}
