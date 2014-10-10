
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Entities;
namespace LFNet.QQ.Packets.In
{
    /// <summary>
    /// * 用户属性回复包
    /// * 1. 头部
    /// * 2. 子命令，1字节
    /// * 当2部分为0x01时：
    /// * 3. 下一个包的起始位置，2字节
    /// * 4. 6部分的长度，1字节
    /// * 5. QQ号，4字节
    /// * 6. 用户属性字节，已知位如下
    /// * 	  bit30 -> 是否有个性签名
    /// * 7. 如果有更多好友，重复5-6部分
    /// * Note: 当2部分为其他值时，尚未仔细解析过后面的格式，非0x01值一般出现在TM中
    /// * 8. 尾部
    /// 	<remark>abu 2008-02-26 </remark>
    /// </summary>
    public class UserPropertyOpReplyPacket : BasicInPacket
    {
        public UserPropertySubCmd SubCommand { get; set; }
        public bool Finished { get; set; }
        public ushort StartPosition { get; set; }
        public List<UserProperty> Properties { get; set; }
        public UserPropertyOpReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "User Property Op Reply Packet";
        }
        protected override void ParseBody(ByteBuffer buf)
        {
            SubCommand = (UserPropertySubCmd)buf.Get();
            switch (SubCommand)
            {
                case UserPropertySubCmd.GET:
                    StartPosition = buf.GetUShort();
                    Finished = StartPosition == QQGlobal.QQ_POSITION_USER_PROPERTY_END;
                    int pLen = buf.Get() & 0xFF;
                    Properties = new List<UserProperty>();
                    while (buf.HasRemaining())
                    {
                        UserProperty p = new UserProperty(pLen);
                        p.Read(buf);
                        Properties.Add(p);
                    }
                    break;
            }
        }
    }
}
