using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
   public class LoginTouchPacket:BasicOutPacket
    {
       //static byte[] zeros = new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,
       // 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00};
       public LoginTouchPacket(QQClient client) : base(QQCommand.Touch, true, client) { }
       public LoginTouchPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
       public override string GetPacketName()
       {
           return "LoginTouch Packet";
       }

        protected override void PutBody(ByteBuffer buf)
        {
            // 初始密钥
            buf.Put(user.QQKey.InitKey);
            ByteBuffer DecodedBuf = new ByteBuffer();
            DecodedBuf.Put(new byte[] { 0x00, 0x01 });
            DecodedBuf.Put(VersionData.QQ09_LOCALE);
            DecodedBuf.Put(VersionData.QQ09_VERSION_SPEC);
            DecodedBuf.Put((byte)0x00);
            DecodedBuf.Put(Client.ServerInfo.GetBytes());
            //DecodedBuf.Put(zeros);//15字节长度00
            byte[] EncodedBuf = crypter.Encrypt(DecodedBuf.ToByteArray(), user.QQKey.InitKey);
            buf.Put(EncodedBuf); 
#if DEBUG
            Client.LogManager.Log(ToString() + " key:" + Utils.Util.ToHex(user.QQKey.InitKey));
            Client.LogManager.Log(ToString() + " Uncrypt Body:" + Utils.Util.ToHex(DecodedBuf.ToByteArray()));
#endif
        }
    }
}
