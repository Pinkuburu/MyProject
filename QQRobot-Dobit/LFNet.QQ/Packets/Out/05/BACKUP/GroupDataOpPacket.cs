
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 上传下载分组名字的消息包，格式为
    /// * 1. 头部
    /// * 2. 操作方式字节，如果为0x2，为上传组名，如果为0x1，为请求下载组名
    /// *    如果为0x2，后面的部分为
    /// * 	  i.   组序号，qq缺省的组，比如我的好友，序号是0，其他我们自己添加的组，从1开始，一个字节。
    /// *         但是要注意的是，这里不包括我的好友组，因为我的好友组是QQ的缺省组，无需上传名称
    /// *    ii.  16个字节的组名，如果组名长度少于16个字节，后面的填0。之所以是16个，是因为QQ的组名长度最多8个汉字
    /// *    iii. 如果有更多组，重复i，ii部分
    /// *    如果为0x1，后面的部分为
    /// *    i.   未知字节0x2
    /// *    ii.  4个未知字节，全0 
    /// * 3. 尾部
    /// * 
    /// * 这个包没有限制添加的组名叫什么，也没有明确规定第一个组必须是
    /// * 我的好友组，这些规范需要在上层程序中实现。当然也可以不一定非要第一个组是
    /// * 我的好友组，这些客户端的trick随便你怎么搞
    /// * 
    /// * 每次上传都必须上传所有组名
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class GroupDataOpPacket : BasicOutPacket
    {
        public List<string> Groups { get; set; }
        public GroupSubCmd Type { get; set; }
        public GroupDataOpPacket(QQClient client)
            : base(QQCommand.Group_Data_OP_05,true,client)
        {
            Type = GroupSubCmd.UPLOAD;
            Groups = new List<string>();
        }
        public GroupDataOpPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            switch (Type)
            {
                case GroupSubCmd.DOWNLOAD:
                    return "Group Data Packet - Download Group";
                case GroupSubCmd.UPLOAD:
                    return "Group Data Packet - Upload Group";
                default:
                    return "Group Data Packet - Unknown Sub Command";
            }
        }
        protected override void PutBody(ByteBuffer buf)
        {
            // 上传操作标志字节
            buf.Put((byte)Type);
            if (Type == GroupSubCmd.UPLOAD)
            {
                // 循环写入各个组
                int size = Groups.Count;
                for (int i = 0; i < size; i++)
                {
                    String name = Groups[i];
                    // 组序号
                    buf.Put((byte)(i + 1));
                    // 组名称
                    byte[] nameBytes = Utils.Util.GetBytes(name);
                    // 超过最大长度的，截短；小于最大长度的，补0
                    if (nameBytes.Length > QQGlobal.QQ_MAX_GROUP_NAME)
                        buf.Put(nameBytes, 0, QQGlobal.QQ_MAX_GROUP_NAME);
                    else
                    {
                        buf.Put(nameBytes);
                        int j = QQGlobal.QQ_MAX_GROUP_NAME - nameBytes.Length;
                        while (j-- > 0)
                            buf.Put((byte)0);
                    }
                }
            }
            else
            {
                // 未知字节0x2
                buf.Put((byte)0x2);
                // 未知4字节，全0
                buf.PutInt(0);
            }
        }
    }
}
