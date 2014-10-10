using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.In
{
   public class LoginVerifyReplyPacket:BasicInPacket
    {
       public LoginVerifyReplyPacket(ByteBuffer buf, int length, QQClient client)
            : base(buf, length, client)
        { }
       
       public byte ReplyCode { get; private set; }
       /// <summary>
       /// 错误信息,当出现错误时才能看到该信息
       /// </summary>
       public string ReplyMessage { get; private set; }
        public override string GetPacketName()
        {
            return "Login Verify Reply Packet";
        }

        protected override void ParseBody(ByteBuffer buf)
        {

#if DEBUG
            Client.LogManager.Log(ToString() + " Decoded Data:" + Utils.Util.ToHex(buf.ToByteArray()));
#endif

            buf.GetChar();//length or sth..
            ReplyCode = buf.Get();
            int len = 0;
            switch (ReplyCode)
            { 
                case 0x00://success!
                    Client.LogManager.Log(ToString() + ":0x" + ReplyCode.ToString("X2") + " Login Success!");
                    len = buf.GetChar();//0x0020
                    Client.QQUser.QQKey.LoginInfo_Token = buf.GetByteArray(len);
                    Client.QQUser.QQKey.LoginInfo_UnknowData = buf.GetByteArray(4); //buf.GetInt()
                    Client.ServerTime = buf.GetByteArray(4);

                    len = buf.GetChar();
                    Client.QQUser.QQKey.LoginInfo_Data_Token = buf.GetByteArray(len);
                    len = buf.GetChar();
                    Client.QQUser.QQKey.LoginInfo_Magic_Token = buf.GetByteArray(len);
                    Client.QQUser.QQKey.LoginInfo_Key1 = buf.GetByteArray(16);
                    buf.GetChar();//0x00 00
                    if (buf.Position + 3 < buf.Length)//判断来的包是否包含LoginInfo_Key3 因为有的版本没这个key 应该说本人用的正式版本没这个
                    {
                        Client.QQUser.QQKey.LoginInfo_Key3 = buf.GetByteArray(16);
#if DEBUG
                        Client.LogManager.Log(ToString() + "Client.QQUser.QQKey.LoginInfo_Key3:" + Utils.Util.ToHex(Client.QQUser.QQKey.LoginInfo_Key3));
#endif
                    }
                    buf.GetChar();//0x00 00
                    return;
                case 0x33:
                case 0x51://denied!
                    Client.LogManager.Log(ToString() + ":0x" + ReplyCode.ToString("X2") + " Denied!");
                    break;
                case 0xBF:
                    Client.LogManager.Log(ToString() + ":0x" + ReplyCode.ToString("X2") + " No this QQ number!");
                    break;
                case 0x34:
                    Client.LogManager.Log(ToString() + ":0x" + ReplyCode.ToString("X2") + " Wrong password!");
                    break;
                default:
                    Client.LogManager.Log(ToString() + ":0x" + ReplyCode.ToString("X2") + " Unknow ReplyCode!");
                    break;
                
            }
            buf.Position = 11;
            len =(int) buf.GetChar();
            byte[] data = buf.GetByteArray(len);
            ReplyMessage = Utils.Util.GetString(data);

            Client.LogManager.Log(ToString() + ":0x" + ReplyCode.ToString("X2") + " Message Data(UTF-8): "+Utils.Util.ToHex(data)+"-->" + ReplyMessage);
        }
    }
}
