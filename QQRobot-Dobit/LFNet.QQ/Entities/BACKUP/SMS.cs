
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary>
    /// 短消息封装类
    /// </summary>
    public class SMS
    {
        public string Message { get; set; }
        public int Sender { get; set; }
        public int Header { get; set; }
        public long Time { get; set; }
        // 如果sender是10000，则senderName为手机号码
        public string SenderName { get; set; }

        /// <summary>给定一个输入流，解析SMS结构
    /// </summary>
        /// <param name="buf">The buf.</param>
        public void ReadBindUserSMS(ByteBuffer buf)
        {
            // 未知1字节，0x0
            buf.Get();
            // 发送者QQ号，4字节
            Sender = buf.GetInt();
            // 发送者头像
            Header = (int)buf.GetUShort();
            // 发送者名称，最多13字节，不足后面补0
            SenderName = Utils.Util.GetString(buf, (byte)0, QQGlobal.QQ_MAX_SMS_SENDER_NAME);
            // 未知的1字节，0x4D
            buf.Get();
            // 消息内容
            Message = Utils.Util.GetString(buf, (byte)0);

            Time = DateTime.Now.Millisecond;
        }

        /// <summary>读取移动QQ用户的短信
    /// </summary>
        /// <param name="buf">The buf.</param>
        public void ReadMobileQQSMS(ByteBuffer buf)
        {
            // 未知1字节
            buf.Get();
            // 发送者QQ号，4字节
            Sender = buf.GetInt();
            // 发送者头像
            Header = (int)buf.GetUShort();
            // 发送者名称，最多13字节，不足后面补0
            SenderName = Utils.Util.GetString(buf, (byte)0, QQGlobal.QQ_MAX_SMS_SENDER_NAME);
            // 未知的1字节，0x4D
            buf.Get();
            // 发送时间
            Time = (long)buf.GetInt() * 1000L;
            // 未知的1字节，0x03
            buf.Get();
            // 消息内容
            Message = Utils.Util.GetString(buf, (byte)0);
        }

        /// <summary>读取移动QQ用户消息（通过手机号描述）
    /// </summary>
        /// <param name="buf">The buf.</param>
        public void ReadMobileQQ2SMS(ByteBuffer buf)
        {
            // 未知1字节
            buf.Get();
            // 发送者，这种情况下都置为10000
            Sender = 10000;
            // 手机号码
            SenderName = Utils.Util.GetString(buf, (byte)0, 18);
            // 未知2字节
            buf.GetChar();
            // 时间
            Time = (long)buf.GetInt() * 1000L;
            // 未知的1字节，0x03
            buf.Get();
            // 消息内容
            Message = Utils.Util.GetString(buf, (byte)0);
        }
        /// <summary>读取普通手机的短信
    /// </summary>
        /// <param name="buf">The buf.</param>
        public void ReadMobileSMS(ByteBuffer buf)
        {
            // 未知1字节，0x0
            buf.Get();
            // 发送者
            Sender = 10000;
            // 手机号码
            SenderName = Utils.Util.GetString(buf, (byte)0, 20);
            // 消息内容
            Message = Utils.Util.GetString(buf, (byte)0);

            Time = DateTime.Now.Ticks;
        }
    }
}
