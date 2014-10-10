
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary>
    /// 普通消息的头部，普通消息是指从好友或者陌生人那里来的消息。什么不是普通消息？比如系统消息
    /// * 这个头部跟在ReceiveIMHeader之后
    /// </summary>
    public class NormalIMHeader
    {
        public char SenderVersion { get; set; }
        public int Sender { get; set; }
        public int Receiver { get; set; }
        public byte[] FileSessionKey { get; set; }
        public NormalIMType Type { get; set; }
        public char Sequence { get; set; }
        public long SendTime { get; set; }
        public char SenderHeader { get; set; }


        /// <summary>
        /// 给定一个输入流，解析NormalIMHeader结构
    /// </summary>
        /// <param name="buf">The buf.</param>
        public void Read(ByteBuffer buf)
        {
            // 发送者的QQ版本
            SenderVersion = buf.GetChar();
            // 发送者的QQ号
            Sender = buf.GetInt();
            // 接受者的QQ号
            Receiver = buf.GetInt();
            // md5处理的发送方的uid和session key，用来在传送文件时加密一些消息
            FileSessionKey = buf.GetByteArray(16);
            // 普通消息类型，比如是文本消息还是其他什么消息
            Type = (NormalIMType)buf.GetUShort();
            // 消息序号
            Sequence = buf.GetChar();
            // 发送时间
            SendTime = (long)buf.GetUInt() * 1000L;
            // 发送者头像
            SenderHeader = buf.GetChar();
        }

        public String toString()
        {
            StringBuilder temp = new StringBuilder();
            temp.Append("NormalIMHeader [");
            temp.Append("senderVersion(char): ");
            temp.Append((int)SenderVersion);
            temp.Append(", ");
            temp.Append("sender: ");
            temp.Append(Sender);
            temp.Append(", ");
            temp.Append("receiver: ");
            temp.Append(Receiver);
            temp.Append(", ");
            temp.Append("type(char): ");
            temp.Append((int)Type);
            temp.Append(", ");
            temp.Append("sequence(char): ");
            temp.Append((int)Sequence);
            temp.Append(", ");
            temp.Append("sendTime: ");
            temp.Append(SendTime);
            temp.Append(", ");
            temp.Append("senderHead(char): ");
            temp.Append((int)SenderHeader);
            temp.Append("]");
            return temp.ToString();
        }
    }
}
