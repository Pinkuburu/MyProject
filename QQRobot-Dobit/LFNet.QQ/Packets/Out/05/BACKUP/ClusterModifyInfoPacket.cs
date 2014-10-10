
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 修改群信息的请求包，格式为：
    /// * 1. 头部
    /// * 2. 命令类型，1字节，修改群信息是0x03
    /// * 3. 群的内部ID，4字节
    /// * 4. 群类型，1字节
    /// * 5. 群的认证类型，1字节
    /// * 6. 2004群分类，4字节
    /// * 7. 2005群分类，4字节
    /// * 8. 群名称长度，1字节
    /// * 9. 群名称
    /// * 10. 未知的两字节，全0
    /// * 11. 群声明长度，1字节
    /// * 12. 群声明
    /// * 13. 群简介长度，1字节
    /// * 14. 群简介
    /// * 16. 尾部
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class ClusterModifyInfoPacket : ClusterCommandPacket
    {
        public AuthType AuthType { get; set; }
        public int Category { get; set; }
        public int OldCategory { get; set; }
        public string Name { get; set; }
        public string Notice { get; set; }
        public string Description { get; set; }
        public ClusterType Type { get; set; }
        public ClusterModifyInfoPacket(QQClient client)
            : base(client)
        {
            SubCommand = ClusterCommand.MODIFY_CLUSTER_INFO;
            AuthType = AuthType.NeedAuth;
            Name = Notice = Description = string.Empty;
            Type = ClusterType.PERMANENT;
        }
        public ClusterModifyInfoPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Cluster Modify Info Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            // 群命令类型
            buf.Put((byte)SubCommand);
            // 群内部ID
            buf.PutInt(ClusterId);
            // 未知1字节
            buf.Put((byte)Type);
            // 认证类型
            buf.Put((byte)AuthType);
            // 2004群分类
            buf.PutInt(OldCategory);
            // 群分类，同学，朋友，之类的
            buf.PutInt(Category);
            // 群名称长度和群名称
            byte[] b = Utils.Util.GetBytes(Name);
            buf.Put((byte)(b.Length & 0xFF));
            buf.Put(b);
            // 未知的2字节
            buf.PutChar((char)0);
            // 群声明长度和群声明
            b = Utils.Util.GetBytes(Notice);
            buf.Put((byte)(b.Length & 0xFF));
            buf.Put(b);
            // 群描述长度和群描述
            b = Utils.Util.GetBytes(Description);
            buf.Put((byte)(b.Length & 0xFF));
            buf.Put(b);
        }
    }
}
