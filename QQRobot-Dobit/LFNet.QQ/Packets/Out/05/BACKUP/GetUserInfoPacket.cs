
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 得到用户的信息，格式为
    /// * 1. 头部
    /// * 2. 用户QQ号的字符串形式
    /// * 3. 尾部
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class GetUserInfoPacket : BasicOutPacket
    {
        public int QQ { get; set; }
        public GetUserInfoPacket(QQClient client)
            : base(QQCommand.Get_UserInfo,true,client)
        {
            QQ = (int)user.QQ;
        }
        public GetUserInfoPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Get User Info Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put(Utils.Util.GetBytes(QQ.ToString()));
        }
    }
}
