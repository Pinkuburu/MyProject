
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Entities;
namespace LFNet.QQ.Packets.In
{
    /// <summary>
    ///  * 请求用户信息的回复包，格式为
    /// *1. 头部
    /// *2. 由ascii 30分隔的各个字段
    /// *3. 尾部
    /// 	<remark>abu 2008-02-22 </remark>
    /// </summary>
    public class GetUserInfoReplyPacket : BasicInPacket
    {
        public ContactInfo ContactInfo { get; set; }
        public GetUserInfoReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Get User Info Reply Packet";
        }
        protected override void ParseBody(ByteBuffer buf)
        {
            // 创建contact info
            ContactInfo = new ContactInfo(buf);
            // 检查字段数
            if (ContactInfo.FieldCount < QQGlobal.QQ_COUNT_GET_USER_INFO_FIELD)
            {
                 throw new PacketParseException("用户信息字段数少于期望的字段数");
            }
            else
            {
                //if(ContactInfo.FieldCount > QQGlobal.QQ_COUNT_GET_USER_INFO_FIELD)
                    //  log.warn("用户信息字段数大于期望的字段数，危险，但是继续使用");
            }
        }
    }
}
