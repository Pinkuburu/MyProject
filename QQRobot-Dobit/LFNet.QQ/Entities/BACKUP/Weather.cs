
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary>天气情况数据
    /// </summary>
    public class Weather
    {
        /// <summary>服务器返回的整型表示的日期
    /// </summary>
        /// <value></value>
        public int Time { get; set; }
        /// <summary>年
    /// </summary>
        /// <value></value>
        public int Year { get; set; }
        /// <summary>月
    /// </summary>
        /// <value></value>
        public int Month { get; set; }
        /// <summary>日期
    /// </summary>
        /// <value></value>
        public int Day { get; set; }
        /// <summary>描述
    /// </summary>
        /// <value></value>
        public string ShortDesc { get; set; }
        /// <summary>风
    /// </summary>
        /// <value></value>
        public string Wind { get; set; }
        /// <summary>最低温度
    /// </summary>
        /// <value></value>
        public int LowTemperature { get; set; }
        /// <summary>最高温度
    /// </summary>
        /// <value></value>
        public int HighTemperature { get; set; }
        public string Hint { get; set; }
        public void Read(ByteBuffer buf)
        {
            Time = buf.GetInt();
            int len = buf.Get() & 0xFF;
            ShortDesc = Utils.Util.GetString(buf, len);
            len = buf.Get() & 0xFF;
            Wind = Utils.Util.GetString(buf, len);
            LowTemperature = buf.GetUShort();
            HighTemperature = buf.GetUShort();
            buf.Get();
            len = buf.Get() & 0xFF;
            Hint = Utils.Util.GetString(buf, len);

            DateTime date = Utils.Util.GetDateTimeFromMillis((long)Time * 1000L);
            Year = date.Year;
            Month = date.Month;
            Day = date.Day;
        }
    }
}
