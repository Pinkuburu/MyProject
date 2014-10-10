
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Entities;
namespace LFNet.QQ.Packets.In
{
    /// <summary>
    /// * 天气操作回复包
    /// * 1. 头部
    /// * 2. 子命令，1字节
    /// * 3. 回复码，1字节
    /// * 4. 省名称字节长度，1字节
    /// * 5. 省
    /// * 6. 市名称字节长度，1字节
    /// * 7. 市
    /// * Note: 如果4部分为0，则可以认为腾讯无法找到你要的天气预报信息，不应再往下解析
    /// * 8. 未知的2字节
    /// * 9. 市(2)名称字节长度，1字节
    /// * 10. 市(2)
    /// * Note: 不明白为什么有两个市，这两个市有时候都有内容，有时候只有一个，要注意各种情况
    /// * 11. 预报的天数，1字节，如果72小时预报，这个就是0x03
    /// * 12. 时间，4字节，天气数据的开始时间
    /// * 13. 天气情况字节长度，1字节
    /// * 14. 天气情况
    /// * 15. 风向字节长度，1字节
    /// * 16. 风向
    /// * 17. 最低温度，2字节，单位是摄氏度
    /// * 18. 最高温度，2字节，单位是摄氏度
    /// * Note: 要注意温度为零下时，是负数，用java处理时要注意转换
    /// * 19. 未知的1字节
    /// * 20. 提示字节长度，1字节
    /// * 21. 提示
    /// * 22. 如果还有更多数据，重复12-21部分
    /// * 23. 未知的2字节
    /// * 24. 尾部
    /// 	<remark>abu 2008-02-26 </remark>
    /// </summary>
    public class WeatherOpReplyPacket : BasicInPacket
    {
        public byte SubCommand { get; set; }
        public ReplyCode ReplyCode { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public List<Weather> Weathers { get; set; }
        public WeatherOpReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        protected override void ParseBody(ByteBuffer buf)
        {
            SubCommand = buf.Get();
            ReplyCode = (ReplyCode)buf.Get();

            int len = buf.Get() & 0xFF;
            if (len == 0)
                return;
            Province = Utils.Util.GetString(buf, len);

            len = buf.Get() & 0xFF;
            if (len > 0)
            {
                City = Utils.Util.GetString(buf, len);
                buf.GetUShort();
                len = buf.Get() & 0xFF;
                buf.Position  = buf.Position + len;
            }
            else
            {
                buf.GetUShort();
                len = buf.Get() & 0xFF;
                City = Utils.Util.GetString(buf, len);
            }
            int count = buf.Get() & 0xFF;

            Weathers = new List<Weather>();
            while (count-- > 0)
            {
                Weather w = new Weather();
                w.Read(buf);
                Weathers.Add(w);
            }
        }
    }
}
