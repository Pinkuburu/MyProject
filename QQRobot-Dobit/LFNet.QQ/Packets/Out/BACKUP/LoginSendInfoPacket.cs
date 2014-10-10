using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
   public class LoginSendInfoPacket:BasicOutPacket
    {
       static byte[] unknown5 = {0x00,0x00,0x00,0x00,0x00,0x00,0x00, 
		0x00,0x00,0x00 };
	static byte[] unknown6 = {0xE9,0xC4,0xD6,0x5C,0x4D,0x9D,
		0xA0,0x17,0xE5,0x24,0x6B,0x55,0x57,0xD3,0xAB,0xF1 };
	static byte[] unknown7 = {0xCB,0x8D,0xA4,0xE2,0x61,0xC2,
		0xDD,0x27,0x39,0xEC,0x8A,0xCA,0xA6,0x98,0xF8,0x9B };

       public LoginSendInfoPacket(QQClient client) : base(QQCommand.LoginSendInfo, true, client) { }
       public LoginSendInfoPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "LoginSendInfo Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            // 初始密钥
            buf.PutChar((char)user.QQKey.LoginInfo_Magic_Token.Length);
            buf.Put(user.QQKey.LoginInfo_Magic_Token);
            ByteBuffer DecodedBuf = new ByteBuffer();
            DecodedBuf.PutChar((char)0x0001);
            
            DecodedBuf.Put(VersionData.QQ09_VERSION_SPEC);
            DecodedBuf.Put(Client.QQUser.QQKey.LoginInfo_UnknowData2);
            DecodedBuf.Put(Client.ServerTime);
            DecodedBuf.Put(Client.ClientIP);
            DecodedBuf.Position += 4;//00 00 00 00
            DecodedBuf.PutChar((char)Client.QQUser.QQKey.LoginInfo_Large_Token.Length);
            DecodedBuf.Put(Client.QQUser.QQKey.LoginInfo_Large_Token);
            DecodedBuf.Position += 35;// 00 00 00......
            DecodedBuf.Put(VersionData.QQ09_EXE_HASH);
            DecodedBuf.Put((byte)Utils.Util.Random.Next());
            DecodedBuf.Put((byte)Client.QQUser.LoginMode);
            DecodedBuf.Put(unknown5);
            ServerInfo si=Client.ServerInfo;
            si.CSP_dwConnIP=Client.QQUser.ServerIp;

            DecodedBuf.Put(0x00);
            DecodedBuf.Put(si.GetBytes());

            DecodedBuf.Put(VersionData.QQ09_LOCALE);
            DecodedBuf.Position += 16;
            DecodedBuf.PutUShort((ushort)Client.QQUser.QQKey.Answer_Token.Length);
            DecodedBuf.Put(Client.QQUser.QQKey.Answer_Token);
            DecodedBuf.PutInt(0x00000007);
            DecodedBuf.PutInt(0x00000000);
            DecodedBuf.PutInt(0x08041001);
            DecodedBuf.Put(0x40);//length of the following
            DecodedBuf.Put(0x01);
            DecodedBuf.PutInt(Utils.Util.Random.Next());
            //DecodedBuf.PutInt(0x0741E9748);
            DecodedBuf.PutChar((char)unknown6.Length);
            DecodedBuf.Put(unknown6);
            DecodedBuf.Put(unknown5);

            DecodedBuf.Put(0x00);
            DecodedBuf.Put(si.GetBytes());

            DecodedBuf.Put(0x02);
            DecodedBuf.PutInt(Utils.Util.Random.Next());
            //DecodedBuf.PutInt(0x8BED382E);
            DecodedBuf.PutChar((char)unknown7.Length);
            DecodedBuf.Put(unknown7);
            DecodedBuf.Position += 248;//all zeros
            DecodedBuf.Put(0x00);

            byte[] EncodedBuf = crypter.Encrypt(DecodedBuf.ToByteArray(), user.QQKey.LoginInfo_Key1);
#if DEBUG
            Client.LogManager.Log(ToString() + " QQKey.LoginInfo_Key1:" + Utils.Util.ToHex(user.QQKey.LoginInfo_Key1));
            Client.LogManager.Log(ToString() + " Uncoded Body:" + Utils.Util.ToHex(DecodedBuf.ToByteArray()));
#endif
            buf.Put(EncodedBuf);
#if DEBUG
            Client.LogManager.Log(ToString() + " Encoded Body:" + Utils.Util.ToHex(buf.ToByteArray()));
#endif
        }
    }
}
