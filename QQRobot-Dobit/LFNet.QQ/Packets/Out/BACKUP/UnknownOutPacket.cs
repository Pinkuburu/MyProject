
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary> 
    /// 所有的未知协议包都由这个类来表示
    /// </summary>
    public class UnknownOutPacket : BasicOutPacket
    {
        public UnknownOutPacket(char command, bool ack,QQClient client) : base(QQCommand.Unknown, false, client) { }
        public UnknownOutPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        protected override void PutBody(ByteBuffer buf)
        {
            
        }
    }
}
