
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Entities;
namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 修改用户个人信息的请求包，格式是:
    /// * 1. 头部
    /// * 2. 旧密码，新密码以及ContactInfo里面的域，但是不包括第一项QQ号，用0x1F分隔，依次往下排，最后要用
    /// *    一个0x1F结尾。但是开头不需要0x1F，如果哪个字段没有，就是空
    /// * 3. 尾部
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class ModifyInfoPacket : BasicOutPacket
    {
        /// <summary>标识是否有修改密码 阿不添加
        /// Gets or sets a value indicating whether [modify password].
        /// </summary>
        /// <value><c>true</c> if [modify password]; otherwise, <c>false</c>.</value>
        public bool ModifyPassword { get; set; }
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
        public ContactInfo ContactInfo { get; set; }
        private const byte DELIMIT = 0x1F;
        public ModifyInfoPacket(QQClient client) : base(QQCommand.Modify_Info_05,true,client) { }
        public ModifyInfoPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Modify Info Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            // 组装内容，首先是旧密码和新密码
            if (!string.IsNullOrEmpty(OldPassword) && !string.IsNullOrEmpty(NewPassword))
            {
                buf.Put(Utils.Util.GetBytes(OldPassword));
                buf.Put(DELIMIT);
                buf.Put(Utils.Util.GetBytes(NewPassword));
                ModifyPassword = true;
            }
            else
            {
                ModifyPassword = false;
                buf.Put(DELIMIT);
            }
            buf.Put(DELIMIT);
            // 写入contactInfo，除了QQ号
            String[] infos = ContactInfo.GetInfoArray();
            for (int i = 1; i < QQGlobal.QQ_COUNT_MODIFY_USER_INFO_FIELD; i++)
            {
                byte[] b = Utils.Util.GetBytes(infos[i]);
                buf.Put(b);
                buf.Put(DELIMIT);
            }
        }
    }
}
