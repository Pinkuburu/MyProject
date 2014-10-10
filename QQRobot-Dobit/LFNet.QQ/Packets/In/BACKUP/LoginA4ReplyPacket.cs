using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.In
{
    public class LoginA4ReplyPacket : BasicInPacket
    {
        public LoginA4ReplyPacket(ByteBuffer buf, int length, QQClient client)
            : base(buf, length, client)
        { }

        public override string GetPacketName()
        {
            return "Login A4 Reply Packet";
        }

        protected override void ParseBody(ByteBuffer buf)
        {
#if DEBUG
            Client.LogManager.Log(ToString() + " Decoded Data:" + Utils.Util.ToHex(buf.ToByteArray()));
            Client.LogManager.Log(ToString() + ":No use data until now!You can check this dat! It's said it has a key");
#endif
            

        }
    }
}
