
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary>个性签名
    /// </summary>
    public class Signature
    {
        public string Sig { get; set; }
        public int ModifiedTime { get; set; }
        public int QQ { get; set; }
        public void Read(ByteBuffer buf)
        {
            QQ = buf.GetInt();
            ModifiedTime = buf.GetInt();
            int len = buf.Get() & 0xFF;
            Sig = Utils.Util.GetString(buf, len);
        }
    }
}
