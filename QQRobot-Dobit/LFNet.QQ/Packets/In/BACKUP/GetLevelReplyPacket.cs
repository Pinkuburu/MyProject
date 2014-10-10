
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.In
{
    /// <summary>
    ///  * 用户等级
    /// </summary>
    public class GetLevelReplyPacket : BasicInPacket
    {
        public byte ReplyCode { get;private set; }
        public ushort Level { get; private set; }
        public ushort ActiveDays { get; private set; }
        public ushort UpgradeDays { get; private set; }
        public GetLevelReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "GetLevel Reply Packet";
        }
        protected override void ParseBody(ByteBuffer buf)
        {
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
            ReplyCode = buf.Get();
            buf.GetInt();
            switch (ReplyCode)
            {
                case 0x88:
                    buf.GetInt();//00000003 unknown
                    this.Level = buf.GetUShort();
                    this.ActiveDays = buf.GetUShort();
                    buf.GetChar();//unknown
                    this.UpgradeDays = buf.GetUShort();
                    Client.LogManager.Log(ToString() + " " + string.Format("level:{0} active_days:{1} upgrade_days:{2}", this.Level, this.ActiveDays, this.UpgradeDays));
                    break;
                default:
                    Client.LogManager.Log(ToString()+"unknown ReplyCode:0x"+ReplyCode.ToString("X"));
                    break;



            
            }
        }
    }
}
