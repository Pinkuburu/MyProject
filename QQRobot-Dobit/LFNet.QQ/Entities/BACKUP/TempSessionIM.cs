
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary> 临时会话消息
    /// </summary>
    public class TempSessionIM
    {
        public int Sender { get; set; }
        public string Nick { get; set; }
        public string Site { get; set; }
        public string Message { get; set; }
        public long Time { get; set; }
        public FontStyle FontStyle { get; set; }

        /// <summary>
    /// </summary>
        /// <param name="buf">The buf.</param>
        public void Read(ByteBuffer buf)
        {
            // 发送者
            Sender = buf.GetInt();
            // 未知的4字节
            buf.GetInt();
            // 昵称
            int len = buf.Get() & 0xFF;
            Nick = Utils.Util.GetString(buf, len);
            // 群名称
            len = buf.Get() & 0xFF;
            Site = Utils.Util.GetString(buf, len);
            // 未知的1字节
            buf.Get();
            // 时间
            Time = (long)buf.GetInt() * 1000L;
            // 后面的内容长度
            len = buf.GetUShort();
            // 得到字体属性长度，然后得到消息内容
            int fontStyleLength = buf.Get(buf.Position + len - 1) & 0xFF;
            Message = Utils.Util.GetString(buf, len - fontStyleLength);
            // 字体属性
            FontStyle = new FontStyle();
            FontStyle.Read(buf);
        }
    }
}
