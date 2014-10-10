using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    public class LoginGetListPacket : BasicOutPacket
    {
        //public static Dictionary<int, ushort> RequestCount = new Dictionary<int, ushort>();
        /// <summary>
        /// 分片次数
        /// </summary>
        public ushort Pos
        {
            // get { return RequestCount[this.Client.QQUser.QQ]; }
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public LoginGetListPacket(QQClient client)
            : base(QQCommand.LoginGetList, true, client)
        {
            //this.Pos = 0;
            //if (!RequestCount.ContainsKey(client.QQUser.QQ))
            //{
            //    RequestCount.Add(client.QQUser.QQ, 0);
            //}
            //else
            //    RequestCount[this.Client.QQUser.QQ]++;
        }
        public LoginGetListPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }

        public override string GetPacketName()
        {
            return "LoginGetList Packet";
        }

        protected override void PutBody(ByteBuffer buf)
        {
            
            buf.PutChar((char)Client.QQUser.QQKey.LoginInfo_Magic_Token.Length);
            buf.Put(user.QQKey.LoginInfo_Magic_Token);
            ByteBuffer DecodedBuf = new ByteBuffer();
            DecodedBuf.PutChar((char)0x011A);
            DecodedBuf.PutChar((char)0x0001);
            DecodedBuf.Put(VersionData.QQ09_LOCALE);
            DecodedBuf.Put(VersionData.QQ09_VERSION_SPEC);
            DecodedBuf.Put((byte)0x00);
            DecodedBuf.Put((byte)Client.QQUser.QQKey.Answer_Token.Length);
            DecodedBuf.Put(Client.QQUser.QQKey.Answer_Token);
            DecodedBuf.Put(Client.QQUser.QQKey.LoginInfo_UnknowData2);
            DecodedBuf.Put(Client.ServerTime);
            DecodedBuf.Put(Client.ClientIP);
            DecodedBuf.Position += 4;//00 00 00 00
            DecodedBuf.PutChar((char)Client.QQUser.QQKey.LoginInfo_Large_Token.Length);
            DecodedBuf.Put(Client.QQUser.QQKey.LoginInfo_Large_Token);
            DecodedBuf.PutUShort(this.Pos);
            DecodedBuf.Position += 2;// 00 00
            DecodedBuf.PutChar((char)0x0071);
            DecodedBuf.Position += 0x0070;//0x0071 zeros
            DecodedBuf.Put(0x00);

            byte[] EncodedBuf = crypter.Encrypt(DecodedBuf.ToByteArray(), user.QQKey.LoginInfo_Key1);
#if DEBUG
            Client.LogManager.Log(ToString() + " QQKey.LoginInfo_Key1:" + Utils.Util.ToHex(user.QQKey.LoginInfo_Key1));
            Client.LogManager.Log(ToString() + " pos:"+this.Pos+" UnBody:" + Utils.Util.ToHex(DecodedBuf.ToByteArray()));
#endif
            buf.Put(EncodedBuf);
#if DEBUG
            Client.LogManager.Log(ToString() + " EnBody:" + Utils.Util.ToHex(buf.ToByteArray()));
#endif
        }
    }
}
