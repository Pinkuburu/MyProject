
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 发送认证信息的包，格式为：
    /// * 1. 头部
    /// * 2. 群命令类型，1字节，认证消息是0x8
    /// * 3. 群内部ID，4字节
    /// * 4. 认证消息的类型，比如是请求，拒绝还是同意，1字节
    /// * 5. 接收者QQ号，4字节，如果是请求加入一个群，这个字段没有用处，为全0
    /// * 6. 附加消息的长度，1字节
    /// * 7. 附加消息
    /// * 8. 尾部
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class ClusterAuthPacket : ClusterCommandPacket
    {
        public int Type { get; set; }
        public string Message { get; set; }
        public int Receiver { get; set; }
        public ClusterAuthPacket(QQClient client)
            : base(client)
        {
            this.SubCommand = ClusterCommand.JOIN_CLUSTER_AUTH;
        }
        public ClusterAuthPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Cluster Auth Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            // 群命令类型
            buf.Put((byte)SubCommand);
            // 群内部ID
            buf.PutInt(ClusterId);
            // 认证消息类型
            buf.Put((byte)Type);
            // 接收者QQ号
            buf.PutInt(Receiver);
            // 附加消息长度
            byte[] b = Utils.Util.GetBytes(Message);
            buf.Put((byte)(b.Length & 0xFF));
            // 附加消息
            buf.Put(b);
        }
    }
}
