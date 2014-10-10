
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.In.QQ2005
{
    /// <summary>
    ///  * Keep Alive的应答包，格式为
    /// *1. 头部
    /// *2. 6个域，分别是"0", "0", 所有在线用户数，我的IP，我的端口，未知含义字段，用ascii码31分隔
    /// *3. 尾部
    /// 	<remark>abu 2008-02-22 </remark>
    /// </summary>
    public class KeepAliveReplyPacket : BasicInPacket
    {
        public static string DIVIDER = System.Text.Encoding.Default.GetString(new byte[] { (byte)31 });
        public int Onlines { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        private const int FIELD_COUNT = 6;
        public KeepAliveReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Keep Alive Reply Packet";
        }
        protected override void ParseBody(ByteBuffer buf)
        {
            // 用分隔符分隔各个字段
            byte[] b = buf.ToByteArray();
            string[] result = Utils.Util.GetString(b).Split(DIVIDER.ToCharArray());
            //检查字段数是否正确
            if (result.Length != FIELD_COUNT)
                throw new PacketParseException("Keep Alive回复字段数不对");
            //解析各字段
            Onlines = Utils.Util.GetInt(result[2], 0);
            if (Onlines == 0)
            {
                throw new PacketParseException("解析在线好友数出错，错误出处：KeepAliveReplyPacket");
            }
            IP = result[3];
            Port = Utils.Util.GetInt(result[4], 0);
        }
    }
}
