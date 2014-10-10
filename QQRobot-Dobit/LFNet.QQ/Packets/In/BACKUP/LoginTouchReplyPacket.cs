using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.In
{
    public class LoginTouchReplyPacket : BasicInPacket
    {

        public ReplyCode ReplyCode { get; set; }
        public byte[] ServerTime { get; set; }
        public byte[] ClientIP { get; set; }
        public byte[] Token { get; set; }
        public byte[] RedirectIP { get; set; }
        public bool IsRedirect { get; set; }
        public LoginTouchReplyPacket(ByteBuffer buf, int length, QQClient client)
            : base(buf, length, client)
        { }
        public override string GetPacketName()
        {
            return "Login Touch Reply Packet";
        }

        protected override void ParseBody(ByteBuffer buf)
        {
#if DEBUG
            Client.LogManager.Log(ToString() + " Decoded data:" + Utils.Util.ToHex(buf.ToByteArray()));
#endif
            //byte result = buf.Get();
            IsRedirect = false;
            ReplyCode = (ReplyCode)buf.Get();
            if (ReplyCode == ReplyCode.OK)
            {
                ServerTime = buf.GetByteArray(4);
                ClientIP = buf.GetByteArray(4);
                buf.Position += 9;
                int len = (int)buf.Get();
                Token = buf.GetByteArray(len);
                byte result=buf.Get();
                if (result != 0x00)
                {
                    IsRedirect = true;
                    Client.LoginRedirect = true;
                    Client.ServerInfo.CSP_wRedirectCount = result;
                    Client.ServerInfo.CSP_cRedirectCount = buf.Get();
                    Client.ServerInfo.CSP_dwConnIspID = buf.GetByteArray(4);
                    Client.ServerInfo.CSP_dwServerReserve = buf.GetByteArray(4);
                    RedirectIP=buf.GetByteArray(4);
                    Client.ServerInfo.CSP_dwConnIP =RedirectIP ;
                }

            }
            else
            {
                Client.LogManager.Log(string.Format(ToString() + " ReplyCode!=ReplyCode.OK: {0:X}", ReplyCode));

            }

        }
    }
}
