
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.In
{
    /// <summary>
    /// * 请求Token回复包
    /// </summary>
    public class RequestTokenReplyPacket : BasicInPacket
    {
        public byte[] Key { get; set; }
        /// <summary>
        /// 得到的Token
        /// </summary>
        public byte[] Token { get; set; }
        /// <summary>
        /// 01 add
        /// 02 delete
        /// </summary>
        public byte SubCommand { get; set; }

        /// <summary>
        /// 是否需要输入验证码
        /// </summary>
        public bool NeedVerify { get; set; }

        /// <summary>
        /// 输入的验证码是否错误
        /// </summary>
        public bool IsWrongVerifyCode { get; set; }
        /// <summary>
        /// 验证码下载地址
        /// </summary>
        public string Url { get; set; }
        //public ReplyCode ReplyCode { get; set; }
        public RequestTokenReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Request Token Reply Packet";
        }
        protected override void ParseBody(ByteBuffer buf)
        {
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
            SubCommand = buf.Get();
            buf.GetChar();//0006
            byte verify = buf.Get();
            if (verify != 0)
            {
                if (buf.Position == buf.Length)//输入的验证码不正确
                {
                    //应该返回验证码不正确的消息
                    IsWrongVerifyCode = true;
                    Client.LogManager.Log(ToString() + " 验证码不正确！");
                    return;
                }
                NeedVerify = true;//需要输入验证码
                int len = buf.GetUShort();
                Url = Utils.Util.GetString(buf.GetByteArray(len));
//                string getQQSession = "";
//                VerifyCodeFileName=Utils.Util.MapPath("\\verify\\" + Client.QQUser.QQ + ".jpg");
//                Utils.Util.DownLoadFileFromUrl(url, VerifyCodeFileName, out getQQSession);
//#if DEBUG
//                Client.LogManager.Log(ToString() + string.Format(" url:{0}, Session:{1},Code File:{2}", url, getQQSession, VerifyCodeFileName));
//#endif
//                Client.QQUser.QQKey.QQSessionToken = Utils.Util.GetBytes(getQQSession);
            }
            else
            {
                int len = buf.GetUShort();
                Token = buf.GetByteArray(len);
            }

            
        }
    }
}
