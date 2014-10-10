
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary>
    /// 讨论组信息封装类 
    /// </summary>
    public class SimpleClusterInfo
    {
        public uint ID { get; set; }
        public string Name { get; set; }
        /// <summary>给定一个输入流，解析Subject结构
    /// </summary>
        /// <param name="buf">The buf.</param>
        public void Read(ByteBuffer buf)
        {
            ID = buf.GetUInt();
            int len = (int)buf.Get();
            Name = Utils.Util.GetString(buf.GetByteArray(len));
        }
    }
}
