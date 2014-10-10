
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.In
{
    /// <summary>
    ///  * 下载分组名称的回复包，格式为
    /// *1. 头部
    /// *2. 子命令，1字节，下载是0x1
    /// *3. 回复码，1字节
    /// *5. 未知的4字节
    /// *6. 组序号，从1开始，0表示我的好友组，因为是缺省组，所以不包含在包中
    /// *7. 16字节的组信息，开始是组名，以0结尾，如果长度不足16字节，则其余部分可能为0，也可能
    /// *   为其他字节，含义不明
    /// *8. 若有多个组，重复6，7部分
    /// *9. 尾部
    /// *
    /// *上传分组名称的回复包，格式为
    /// *1. 头部
    /// *2. 子命令，1字节
    /// *3. 回复码，1字节
    /// *4. 组需要，从1开始，0表示我的好友组，因为是缺省组，所以不包含在包中
    /// *5. 如果有更多组，重复4部分
    /// *6. 尾部
    /// 	<remark>abu 2008-02-22 </remark>
    /// </summary>
    public class GroupDataOpReplyPacket : BasicInPacket
    {
        public List<string> GroupNames { get; set; }
        public GroupSubCmd SubCommand { get; set; }
        public ReplyCode ReplyCode { get; set; }
        public GroupDataOpReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            switch (SubCommand)
            {
                case GroupSubCmd.UPLOAD:
                    return "Group Data Reply Packet - Upload Group";
                case GroupSubCmd.DOWNLOAD:
                    return "Group Data Reply Packet - Download Group";
                default:
                    return "Group Data Reply Packet - Unknown Sub Command";
            }
        }
        protected override void ParseBody(ByteBuffer buf)
        {
            // 得到操作类型
            SubCommand = (GroupSubCmd)buf.Get();
            // 回复码
            ReplyCode = (ReplyCode)buf.Get();
            if (ReplyCode == ReplyCode.OK)
            {
                // 如果是下载包，继续解析内容
                if (SubCommand == GroupSubCmd.DOWNLOAD)
                {
                    // 创建list
                    GroupNames = new List<String>();
                    // 未知4个字节
                    buf.GetUInt();
                    // 读取每个组名
                    while (buf.HasRemaining())
                    {
                        buf.Get();
                        GroupNames.Add(Utils.Util.GetString(buf, (byte)0x00, 16));
                    }
                }
            }
        }
    }
}
