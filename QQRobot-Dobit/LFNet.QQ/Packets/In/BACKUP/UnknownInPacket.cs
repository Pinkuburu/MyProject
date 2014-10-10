
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.In
{
    /// <summary>
    ///  * 未知的接收包
    /// * 1. 头部
    /// * 2. 包体
    /// * 3. 尾部
    /// 	<remark>abu 2008-02-26 </remark>
    /// </summary>
    public class UnknownInPacket : BasicInPacket
    {
        public UnknownInPacket(QQCommand command,QQClient client) : base(command, client) { }
        public UnknownInPacket(ByteBuffer buf, int length,QQClient client) : base(buf, length, client) { }

        protected override void ParseBody(ByteBuffer buf)
        {
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
        }
    }
}
