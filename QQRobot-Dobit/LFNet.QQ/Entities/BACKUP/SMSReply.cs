
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary>单条短信的回复信息，针对一个接受者
    /// </summary>
    public class SMSReply 
    {
        /// <summary>true表示接受者是QQ号
    /// </summary>
        /// <value></value>
        public bool IsQQ { get; set; }

        public string Message { get; set; }
        /// <summary>仅当isQQ为true时有效
    /// </summary>
        /// <value></value>
        public int QQ { get; set; }
        /// <summary>仅当isQQ为false时有效
    /// </summary>
        /// <value></value>
        public string Mobile { get; set; }
        public ReplyCode  ReplyCode { get; set; }
        private byte unknown;

        /// <summary>读取回复信息，接受者类型是手机号码
    /// </summary>
        /// <param name="buf">The buf.</param>
        public void ReadMobile(ByteBuffer buf)
        {
            IsQQ = false;
            Mobile = Utils.Util.GetString(buf, (byte)0, QQGlobal.QQ_MAX_SMS_MOBILE_LENGTH);
            buf.GetUShort();
            ReplyCode = (ReplyCode)buf.Get();
            int len = buf.Get() & 0xFF;
            Message = Utils.Util.GetString(buf, len);
            unknown = buf.Get();
        }

        /// <summary>读取回复信息，接受者是一个QQ号
    /// </summary>
        /// <param name="buf">The buf.</param>
        public void ReadQQ(ByteBuffer buf)
        {
            IsQQ = true;
            QQ = buf.GetInt();
            ReplyCode = (ReplyCode)buf.Get();
            int len = buf.Get() & 0xFF;
            Message = Utils.Util.GetString(buf, len);
            unknown = buf.Get();
        }
    }
}
