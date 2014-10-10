using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    public class LoginA4Packet : BasicOutPacket
    {
        public LoginA4Packet(QQClient client) : base(QQCommand.Login_A4, true, client) { }
        public LoginA4Packet(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "LoginA4 Packet";
        }

        protected override void PutBody(ByteBuffer buf)
        {
            // 初始密钥
            //buf.Put(user.QQKey.InitKey);
            buf.PutChar((char)user.QQKey.LoginInfo_Magic_Token.Length);
            buf.Put(user.QQKey.LoginInfo_Magic_Token);
            ByteBuffer DecodedBuf = new ByteBuffer();
            DecodedBuf.PutChar((char)0x0101);
            DecodedBuf.PutChar((char)0x0000);
            DecodedBuf.Put((byte)Client.QQUser.QQKey.LoginInfo_Token.Length);
            DecodedBuf.Put(Client.QQUser.QQKey.LoginInfo_Token);

            DecodedBuf.Put(new byte[]{0x10,0x03,0xC8,0xEC,0xC8,0x96,
                0x8B,0xF2,0xB3,0x6B,0x4D,0x0C,0x5C,0xE0,0x6A,0x51,0xCE});//unknown data
            //Client.QQUser.QQKey.Key = Client.QQUser.QQKey.LoginInfo_Key1;//可能要用到
            byte[] EncodedBuf = crypter.Encrypt(DecodedBuf.ToByteArray(), user.QQKey.LoginInfo_Key1);
#if DEBUG
            Client.LogManager.Log(ToString() + " key:" + Utils.Util.ToHex(user.QQKey.LoginInfo_Key1));
            Client.LogManager.Log(ToString() + " UnBody:" + Utils.Util.ToHex(DecodedBuf.ToByteArray()));
#endif
            buf.Put(EncodedBuf);
#if DEBUG
            Client.LogManager.Log(ToString() + " EnBody:" + Utils.Util.ToHex(buf.ToByteArray()));
#endif
        }
    }
}
