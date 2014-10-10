
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 天气数据操作请求包
    /// * 1. 头部
    /// * 2. 子命令，1字节
    /// * 3. 我的IP地址，4字节
    /// * 4. 我的端口，2字节，查天气预报不需要，一般都是0x0000
    /// * 5. 尾部
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class WeatherOpPacket : BasicOutPacket
    {
        public WeatherSubCmd SubCommand { get; set; }
        public byte[] IP { get; set; }
        public WeatherOpPacket(QQClient client) : base(QQCommand.Weather_OP_05,true,client)
        {
            SubCommand = WeatherSubCmd.Get;
        }
        public WeatherOpPacket(ByteBuffer buf, int length, QQClient client)
            : base(buf, length, client)
        {

        }
        public override string GetPacketName()
        {
            return "Weather Op Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put((byte)SubCommand);
            buf.Put(IP);
            buf.PutChar((char)0);
        }
    }
}
