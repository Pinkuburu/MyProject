using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    public class LoginRequestPacket : BasicOutPacket
    {
       
        public LoginRequestPacket(QQClient client)
            : base(QQCommand.LoginRequest, true, client)
        {
            this.Token = null;
            this.GetCode = 0;
            this.Png_Data = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client">QQclient</param>
        /// <param name="token">这个包发出去获取的令牌</param>
        /// <param name="getCode">1或0，1和验证码有关的请求</param>
        /// <param name="code">验证码的值</param>
        public LoginRequestPacket(QQClient client, byte[] token, uint getCode, byte png_data)
            : base(QQCommand.LoginRequest, true, client)
        {
            this.Token = token;
            this.GetCode = getCode;
            this.Png_Data = png_data;
        }
        public LoginRequestPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "LoginRequest Packet";
        }
        /// <summary>
        /// Token获取验证码的令牌
        /// </summary>
        public byte[] Token { get; set; }

        /// <summary>
        /// 0,
        /// 1 获取验证码图片或发送验证码信息
        /// </summary>
        public uint GetCode { get; set; }

        /// <summary>
        /// png_data 0 或1
        /// </summary>
        public byte Png_Data { get; set; }

        protected override void PutBody(ByteBuffer buf)
        {
            // 初始密钥
            buf.Put(user.QQKey.InitKey);
            ByteBuffer DecodedBuf = new ByteBuffer();
            DecodedBuf.Put(new byte[] { 0x00, 0x01 });
            DecodedBuf.Put(VersionData.QQ09_LOCALE);
            DecodedBuf.Put(VersionData.QQ09_VERSION_SPEC);
            DecodedBuf.Put((byte)0x00);
            DecodedBuf.Put((byte)Client.QQUser.QQKey.LoginRequestToken.Length);
            DecodedBuf.Put(Client.QQUser.QQKey.LoginRequestToken);
            if (GetCode != 0) DecodedBuf.Put(0x04);
            else DecodedBuf.Put(0x03);//开头写成0x04了，结果后面出现Token不一致了
            DecodedBuf.Put(0x00);
            DecodedBuf.Put(0x05);
            DecodedBuf.PutInt(0);
            DecodedBuf.Put((byte)Png_Data);
            if (GetCode != 0x00 && Token != null)
            {
                DecodedBuf.Put(0x04);
                DecodedBuf.PutInt(GetCode);
                //answer token
                DecodedBuf.PutChar((char)Token.Length);
                DecodedBuf.Put(Token);

            }
            else if (Png_Data == 0x01 && Token != null)
            {
                //png token
                DecodedBuf.PutChar((char)Token.Length);
                DecodedBuf.Put(Token);
            }
            else
            {
                DecodedBuf.Put(0x00);
                DecodedBuf.Put(0x00);
            }
            byte[] EncodedBuf = crypter.Encrypt(DecodedBuf.ToByteArray(), user.QQKey.InitKey);
            buf.Put(EncodedBuf);
#if DEBUG
            Client.LogManager.Log(ToString() + " Key:" + Utils.Util.ToHex(user.QQKey.InitKey));
            Client.LogManager.Log(ToString() + " UnBody:" + Utils.Util.ToHex(DecodedBuf.ToByteArray()));
#endif
        }
    }
}
