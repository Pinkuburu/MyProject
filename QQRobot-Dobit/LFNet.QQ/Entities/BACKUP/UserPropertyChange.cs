
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary>用户属性变化
    /// </summary>
    public class UserPropertyChange
    {
        public int QQ { get; set; }
        public int Property { get; set; }
        public void Read(ByteBuffer buf)
        {
            QQ = buf.GetInt();
            Property = buf.GetInt();
            // 未知16字节
            buf.Position = buf.Position + 16;
        }
        /// <summary>
    /// </summary>
        /// <returns></returns>
        public bool HasSignature()
        {
            return (Property & QQGlobal.QQ_FLAG_HAS_SIGNATURE) != 0;
        }
        /// <summary>
    /// </summary>
        /// <returns></returns>
        public bool HasCustomHead()
        {
            return (Property & QQGlobal.QQ_FLAG_HAS_CUSTOM_HEAD) != 0;
        }
    }
}
