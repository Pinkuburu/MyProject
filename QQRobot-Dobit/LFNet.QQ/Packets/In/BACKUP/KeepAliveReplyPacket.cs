
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.In
{
    /// <summary>
    ///  * Keep Alive的应答包
    /// </summary>
    public class KeepAliveReplyPacket : BasicInPacket
    {
        public int Onlines { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public DateTime ServerTime { get; set; }
        public KeepAliveReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Keep Alive Reply Packet";
        }
        protected override void ParseBody(ByteBuffer buf)
        {
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
            buf.Get();//00
            Onlines = buf.GetInt();
            IP = Utils.Util.GetIpStringFromBytes(buf.GetByteArray(4));//client ip
            Port = (int)buf.GetChar();
            buf.GetChar();//unknown 00 3c
            ServerTime = Utils.Util.GetDateTimeFromMillis(buf.GetInt() * 1000L);
            Client.LogManager.Log(ToString() + " Onlines:" +Onlines.ToString()+" IP:"+IP+" ServerTime:"+ServerTime.ToString());
            
        }
    }
}
