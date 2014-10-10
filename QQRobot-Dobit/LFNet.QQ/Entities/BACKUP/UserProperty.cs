
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary>用户属性
    /// </summary>
    public class UserProperty
    {
        private int PropertyLength;
        public int QQ { get; set; }
        public int Property { get; set; }
        public UserProperty(int len)
        {
            PropertyLength = len;
        }
        public void Read(ByteBuffer buf)
        {
            QQ = buf.GetInt();
            Property = buf.GetInt();
            buf.Position = buf.Position + PropertyLength;
        }
        public bool HasSignature()
        {
            return (Property & QQGlobal.QQ_FLAG_HAS_SIGNATURE) != 0;
        }

        public bool HasCustomHead()
        {
            return (Property & QQGlobal.QQ_FLAG_HAS_CUSTOM_HEAD) != 0;
        }
    }
}
