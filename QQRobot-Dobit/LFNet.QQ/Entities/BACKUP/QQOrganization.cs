
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary>组织信息封装
    /// </summary>
    public class QQOrganization
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public uint Path { get; set; }
        public void Read(ByteBuffer buf)
        {
            Id = (uint)buf.Get();
            Path = buf.GetUInt();
            int len = (int)buf.Get();
            Name = Utils.Util.GetString(buf.GetByteArray(len));
        }
    }
}
