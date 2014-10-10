
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.In
{
    /// <summary>
    /// * 修改信息的回复包，格式是
    /// *1. 头部
    /// *2. 我的QQ号的字符串形式（成功了就应该是我的QQ号，否则就是失败了）
    /// *3. 尾部
    /// 	<remark>abu 2008-02-22 </remark>
    /// </summary>
    public class ModifyInfoReplyPacket : BasicInPacket
    {
        public bool Success { get; set; }
        public int QQ { get; set; }
        public ModifyInfoReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Modify Info Reply Packet";
        }
        protected override void ParseBody(ByteBuffer buf)
        {
            byte[] b = buf.ToByteArray();
            QQ = Utils.Util.GetInt(Utils.Util.GetString(b), 0);
            if (QQ == user.QQ)
                Success = true;
            else
                Success = false;
        }
    }
}
