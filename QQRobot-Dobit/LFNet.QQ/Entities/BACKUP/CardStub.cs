
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary>
    /// 群名片存根
    /// </summary>
    public class CardStub
    {
        public uint QQ { get; set; }
        public string Name { get; set; }
        public void Read(ByteBuffer buf)
        {
            QQ = buf.GetUInt();
            int len = (int)buf.Get();
            Name = Utils.Util.GetString(buf.GetByteArray(len));
        }
    }
}
