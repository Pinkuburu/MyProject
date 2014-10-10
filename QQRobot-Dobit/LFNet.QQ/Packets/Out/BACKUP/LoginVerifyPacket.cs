using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    public class LoginVerifyPacket : BasicOutPacket
    {

        public LoginVerifyPacket(QQClient client)
            : base(QQCommand.LoginVerify, true, client)
        {
        }

        public LoginVerifyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "LoginVerify Packet ";
        }

        protected override void PutBody(ByteBuffer buf)
        {
            // 初始密钥
            buf.Put(user.QQKey.InitKey);
            #region 加密用户身份数据
            ByteBuffer verifyBuf = new ByteBuffer();
            verifyBuf.PutInt(Utils.Util.Random.Next());//random??
            verifyBuf.PutChar((char)0x0001);
            verifyBuf.PutInt(Client.QQUser.QQ);
            verifyBuf.Put(VersionData.QQ09_VERSION_SPEC);
            verifyBuf.Put(0x00);
            verifyBuf.PutChar((char)0x0001);
            verifyBuf.Put(Client.QQUser.Password);
            verifyBuf.Put(Client.ServerTime);
            verifyBuf.Position += 13;//13个00
            verifyBuf.Put(Client.ClientIP);
            verifyBuf.PutInt(0);
            verifyBuf.PutInt(0);
            verifyBuf.PutChar((char)0x0010);
            verifyBuf.Put(Client.QQUser.QQKey.Verify_Key1);
            verifyBuf.Put(Client.QQUser.QQKey.Verify_Key2);

            //加密
            byte[] EncodeVerifyBuf = crypter.Encrypt(verifyBuf.ToByteArray(), user.QQKey.PasswordKey);
            
#if DEBUG
            Client.LogManager.Log(ToString() + " User Data:" + Utils.Util.ToHex(verifyBuf.ToByteArray()));
            Client.LogManager.Log(ToString() + " Key:" + Utils.Util.ToHex(user.QQKey.PasswordKey));
            Client.LogManager.Log(ToString() + " User Data:" + "len " + EncodeVerifyBuf.Length.ToString("") + "<--" + Utils.Util.ToHex(EncodeVerifyBuf));
#endif
            #endregion

            ByteBuffer DecodedBuf = new ByteBuffer();
            DecodedBuf.PutChar((char)0x00CA);
            DecodedBuf.Put(new byte[] { 0x00, 0x01 });
            DecodedBuf.Put(VersionData.QQ09_LOCALE);
            DecodedBuf.Put(VersionData.QQ09_VERSION_SPEC);
            DecodedBuf.Put((byte)0x00);
            DecodedBuf.Put((byte)Client.QQUser.QQKey.Answer_Token.Length);
            DecodedBuf.Put(Client.QQUser.QQKey.Answer_Token);

            DecodedBuf.PutChar((char)EncodeVerifyBuf.Length);
            DecodedBuf.Put(EncodeVerifyBuf);

            DecodedBuf.PutChar((char)0x0000);
            DecodedBuf.PutChar((char)0x018B);
            DecodedBuf.Put(0x2E);//length of the following info

            DecodedBuf.Put(0x01);
            DecodedBuf.PutInt(Utils.Util.Random.Next());

            DecodedBuf.PutChar((char)0x0010);
            DecodedBuf.Put(new byte[]{0xE9,0xC4,0xD6,0x5C,0x4D,0x9D,
                0xA0,0x17,0xE5,0x24,0x6B,0x55,0x57,0xD3,0xAB,0xF1});//unknow6

            DecodedBuf.Put(0x02);
            DecodedBuf.PutInt(Utils.Util.Random.Next());

            DecodedBuf.PutChar((char)0x0010);
            DecodedBuf.Put(new byte[]{0xCB,0x8D,0xA4,0xE2,0x61,0xC2,
                0xDD,0x27,0x39,0xEC,0x8A,0xCA,0xA6,0x98,0xF8,0x9B});//unknow7
            DecodedBuf.Position += 0x015B;//395 zeros?  348 -去后面补的一位
            DecodedBuf.Put(0x00);//否则后面不补00
            byte[] EncodedBuf = crypter.Encrypt(DecodedBuf.ToByteArray(), user.QQKey.InitKey);
            buf.Put(EncodedBuf);
            verifyBuf = null;
            
#if DEBUG
            Client.LogManager.Log(ToString() + " Key:" + Utils.Util.ToHex(user.QQKey.InitKey));
            Client.LogManager.Log(ToString() + " Uncode Body:" + Utils.Util.ToHex(DecodedBuf.ToByteArray()));
#endif
        }
    }
}
