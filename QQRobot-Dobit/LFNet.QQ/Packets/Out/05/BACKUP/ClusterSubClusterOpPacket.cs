
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 讨论组操作请求：
    /// * 1. 头部
    /// * 2. 命令类型，1字节，0x36
    /// * 3. 子命令，1字节
    /// * 4. 根据3的不同，有：
    /// * 		i. 3为0x02(得到讨论组)时，4为群内部ID，4字节
    /// * 		ii. 3为0x01(得到多人对话)时，这里为0，4字节
    /// * 5. 尾部
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class ClusterSubClusterOpPacket : ClusterCommandPacket
    {
        public ClusterSubCmd OpByte { get; set; }
        public ClusterSubClusterOpPacket(QQClient client)
            : base(client)
        {
            SubCommand = ClusterCommand.SUB_CLUSTER_OP;
        }
        public ClusterSubClusterOpPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Cluster Subject Op Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            // 命令类型
            buf.Put((byte)SubCommand);
            // 子命令
            buf.Put((byte)OpByte);
            switch (OpByte)
            {
                case ClusterSubCmd.GET_SUBJECT_LIST:
                    buf.PutInt(ClusterId);
                    break;
                case ClusterSubCmd.GET_DIALOG_LIST:
                    buf.PutInt(0);
                    break;
            }
        }
    }
}
