using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using LFNet.QQ.Net;
using LFNet.QQ.Packets.Out;


namespace LFNet.QQ.Packets.In
{
    public class LoginRequestReplyPacket : BasicInPacket
    {
        public byte ReplyCode { get; private set; }
        public byte Png_Data { get;private set; }
        public  byte[] Answer_Token { get; private set; }
        public  byte[] Png_Token { get; private set; }
        /// <summary>
        /// 验证码存放路径
        /// </summary>
        public string CodeFileName { get; private set; }
        /// <summary>
        /// 是否还有验证码数据
        /// </summary>
        public byte Next { get; private set; }

        public LoginRequestReplyPacket(ByteBuffer buf, int length, QQClient client)
            : base(buf, length, client)
        { }
        public override string GetPacketName()
        {
            return "Login Request Reply Packet";
        }

        protected override void ParseBody(ByteBuffer buf)
        {
            //怎么我得到的数据是//01 00 05 00 00 20 78 09 D7 43 99 8B DD 87 59 82 EA 85 7D 09 9A B2 92 77 53 5B 6D E3 6C B6 66 B3 21 75 6B 0B 37 85
#if DEBUG
            Client.LogManager.Log(ToString() + " Decoded Data:" + Utils.Util.ToHex(buf.ToByteArray()));
#endif
            ReplyCode = buf.Get();//03: ok   04: need verifying 可是我得到的是01是由于前面错了了一个byte
            buf.Get();//0x00
            buf.Get();//0x05
            Png_Data = buf.Get();
            int len = 0;
            if (Png_Data == 0x00 && ReplyCode == 0x01)
            {
                len = (int)buf.Get();
                while (len == 0)
                {
                    len = (int)buf.Get();
                }
            }
            else //ReplyCode != 0x01按下面走 兼容多版本
            {
                buf.GetInt();//需要验证码时为00 00 01 23，不需要时为全0
                len=(int)buf.GetChar();
            }
            Answer_Token = buf.GetByteArray(len);
            if (Png_Data== 0x01)//有验证码数据
            {
                len = (int)buf.GetChar();
                byte[] data = buf.GetByteArray(len);
                buf.Get();
                Next = buf.Get();
                string directory = Utils.Util.MapPath("/Verify/");
                this.CodeFileName = Path.Combine(directory, Client.QQUser.QQ + ".png");
                FileStream fs=null;
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                if (Next != 0x00)
                {
                    fs = new FileStream(this.CodeFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);

                }
                else fs = new FileStream(this.CodeFileName, FileMode.Append, FileAccess.Write, FileShare.Read);
                //fs.Seek(0, SeekOrigin.End);
                fs.Write(data,0,data.Length);
                fs.Close();
                fs=null;

                len = (int)buf.GetChar();
                Png_Token = buf.GetByteArray(len);
            }

            //
            if (Png_Data!=0x00)
            {
                if (Next!=0x00)
                {
                    //prot_login_request(qq, &png_token, 0, 1);
                    Client.LogManager.Log("接收到部分验证码图片数据，继续接收....");
                    OutPacket outPacket = new LoginRequestPacket(Client, Png_Token, 0, 1);//发送一个请求验证码的包
                    Client.PacketManager.SendPacketAnyway(outPacket, QQPort.Main.Name);
                }
                else
                {
                    //qq->data.verify_token = answer_token;
                    //qqclient_set_process(qq, P_VERIFYING);
                    Client.LoginStatus = LoginStatus.NeedVerifyCode;
                    Client.QQUser.QQKey.Verify_Token = Answer_Token;
                    Client.LogManager.Log("Need input Verify Code");
                    //Client.LoginManager.OnLoginNeedVerifyCode(e);
                }
            }
            else
            {
                //DBG("process verify password");
                //qq->data.token_c = answer_token;
                //prot_login_verify(qq);
                Client.LogManager.Log("Process LoginRequest Success! Now Process Verify Password...");
                Client.QQUser.QQKey.Answer_Token = Answer_Token;
                OutPacket outPacket = new LoginVerifyPacket(Client);//发送一个登陆请求包
                Client.PacketManager.SendPacketAnyway(outPacket, QQPort.Main.Name);
            }
        }
    }
}
