using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.In
{
    public class LoginGetInfoReplyPacket : BasicInPacket
    {
        public LoginGetInfoReplyPacket(ByteBuffer buf, int length, QQClient client)
            : base(buf, length, client)
        { }

        public override string GetPacketName()
        {
            return "Login GetInfo Reply Packet";
        }
        public string NickName { get; private set; }
        protected override void ParseBody(ByteBuffer buf)
        {
#if DEBUG
            Client.LogManager.Log(ToString() + " Decoded Data:" + Utils.Util.ToHex(buf.ToByteArray()));
#endif
            buf.GetChar();//01 66 length or sth...
            buf.GetChar();//01 00
            Client.QQUser.QQKey.LoginInfo_Key2 = buf.GetByteArray(16);
            buf.Position += 8;//00 00 00 01 00 00 00 64 
            Client.QQUser.QQKey.LoginInfo_UnknowData2 = buf.GetByteArray(4);
            Client.ServerTime = buf.GetByteArray(4);
            Client.ClientIP = buf.GetByteArray(4);
            buf.GetInt();//00000000
            int len = (int)buf.GetChar();
            Client.QQUser.QQKey.LoginInfo_Large_Token = buf.GetByteArray(len);
            buf.GetInt();//????
            len = (int)buf.Get();
            NickName = Utils.Util.GetString(buf.GetByteArray(len));
            Client.LogManager.Log(ToString() + ": Hello," + NickName);

        }
    }
}
