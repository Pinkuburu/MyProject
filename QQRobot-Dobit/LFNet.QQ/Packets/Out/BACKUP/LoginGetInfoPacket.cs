using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    public class LoginGetInfoPacket : BasicOutPacket
    {
        public LoginGetInfoPacket(QQClient client) : base(QQCommand.LoginGetInfo, true, client) { }
        public LoginGetInfoPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "LoginGetInfo Packet";
        }

        protected override void PutBody(ByteBuffer buf)
        {
            // 初始密钥
            buf.PutChar((char)user.QQKey.LoginInfo_Magic_Token.Length);
            buf.Put(user.QQKey.LoginInfo_Magic_Token);
            ByteBuffer DecodedBuf = new ByteBuffer();
            DecodedBuf.PutChar((char)0x010D);
            DecodedBuf.Put(0x00);
            DecodedBuf.Put(new byte[] { 0x01, 0x01 });
            DecodedBuf.Put(VersionData.QQ09_LOCALE);
            DecodedBuf.Put(VersionData.QQ09_VERSION_SPEC);
            DecodedBuf.PutChar((char)Client.QQUser.QQKey.Answer_Token.Length);
            DecodedBuf.Put(Client.QQUser.QQKey.Answer_Token);

            DecodedBuf.PutChar((char)Client.QQUser.QQKey.LoginInfo_Token.Length);
            DecodedBuf.Put(Client.QQUser.QQKey.LoginInfo_Token);
            DecodedBuf.Put(Client.QQUser.QQKey.LoginInfo_UnknowData);
            DecodedBuf.Put(Client.ServerTime);

            DecodedBuf.PutChar((char)Client.QQUser.QQKey.LoginInfo_Data_Token.Length);
            DecodedBuf.Put(Client.QQUser.QQKey.LoginInfo_Data_Token);
            DecodedBuf.PutChar((char)0x0000);
            DecodedBuf.PutInt(0x00000000);
            //Client.QQUser.QQKey.Key = Client.QQUser.QQKey.LoginInfo_Key1;//可能要用到
            byte[] EncodedBuf = crypter.Encrypt(DecodedBuf.ToByteArray(), user.QQKey.LoginInfo_Key1);
            buf.Put(EncodedBuf);
#if DEBUG
            Client.LogManager.Log(ToString() + " key:" + Utils.Util.ToHex(user.QQKey.InitKey));
            Client.LogManager.Log(ToString() + " UnBody: " + Utils.Util.ToHex(DecodedBuf.ToByteArray()));
#endif
        }
    }
}
