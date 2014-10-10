
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary>自定义头像变化信息
    /// </summary>
    public class CustomHead
    {
        public int QQ { get; set; }
        public int Timestamp { get; set; }
        public byte[] MD5 { get; set; }
        public void Read(ByteBuffer buf)
        {
            QQ = buf.GetInt();
            Timestamp = buf.GetInt();
            MD5 = buf.GetByteArray(16);
        }
    }
}
