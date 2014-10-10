
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Entities;
namespace LFNet.QQ.Packets.Out
{
    /// <summary> * 修改群名片请求包
    /// * 1. 头部 
    /// * 2. 子命令，1字节，0x0E
    /// * 3. 群内部ID，4字节
    /// * 4. 我的QQ号，4字节
    /// * 5. 真实姓名长度，1字节
    /// * 6. 真实姓名
    /// * 7. 性别索引，1字节，性别的顺序是'男', '女', '-'，所以男是0x00，等等
    /// * 8. 电话字符串长度，1字节
    /// * 9. 电话的字符串表示
    /// * 10. 电子邮件长度，1字节
    /// * 11. 电子邮件
    /// * 12. 备注长度，1字节
    /// * 13. 备注内容
    /// * 14. 尾部
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class ClusterModifyCardPacket : ClusterCommandPacket
    {
        public Card Card { get; set; }
        public ClusterModifyCardPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public ClusterModifyCardPacket(QQClient client) : base(client) { SubCommand = ClusterCommand.MODIFY_CARD; }
        public override string GetPacketName()
        {
            return "Cluster Modify Card Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put((byte)SubCommand);
            buf.PutInt(ClusterId);
            buf.PutInt(user.QQ);
            Card.Write(buf);
        }
    }
}
