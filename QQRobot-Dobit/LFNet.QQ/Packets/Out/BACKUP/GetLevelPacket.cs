using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
   public class GetLevelPacket:BasicOutPacket
    {
       public GetLevelPacket(QQClient client)
            : base(QQCommand.GetLevel, true, client)
        {  
        }

        public GetLevelPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put(0x88);
            buf.PutInt(Client.QQUser.QQ);
            buf.Put(0x00);
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
        }
    }
}
