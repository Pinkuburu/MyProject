
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 创建群请求包，格式为：
    /// * 1. 头部
    /// * 2. 群命令类型，1字节，创建是0x01
    /// * 3. 群的类型，固定还是临时，1字节
    /// * 4. 是否需要认证，1字节
    /// * 5. 2004群分类，4字节
    /// * 6. 2005群分类，4字节
    /// * 7. 群名称长度，1字节
    /// * 8. 群名称
    /// * 9. 未知的2字节，0x0000
    /// * 10. 群声明长度，1字节
    /// * 11. 群声明
    /// * 12. 群简介长度，1字节
    /// * 13. 群简介
    /// * 14. 群现有成员的QQ号列表，每个QQ号4字节
    /// * 15. 尾部 
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class ClusterCreatePacket : ClusterCommandPacket
    {
        public ClusterType Type { get; set; }
        public AuthType AuthType { get; set; }
        public int OldCategory { get; set; }
        public int Category { get; set; }
        public string Name { get; set; }
        public string Notice { get; set; }
        public string Description { get; set; }
        public List<int> Members { get; set; }
        public ClusterCreatePacket(QQClient client)
            : base(client)
        {
            this.SubCommand = ClusterCommand.CREATE_CLUSTER;
            this.Type = ClusterType.PERMANENT;
            this.AuthType = AuthType.NeedAuth;
            this.OldCategory = 0;
        }
        public ClusterCreatePacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Cluster Create Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            // 群命令类型
            buf.Put((byte)SubCommand);
            // 群类型
            buf.Put((byte)Type);
            // 认证类型
            buf.Put((byte)AuthType);
            // 2004群分类
            buf.PutInt(OldCategory);
            // 群的分类
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
            // 群中的好友
            foreach (int i in Members)
                buf.PutInt(i);
        }
    }
}
