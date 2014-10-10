
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary>
    /// 接收到的消息的头部格式封装类
    /// </summary>
    public class ReceiveIMHeader
    {
        public int Sender { get; set; }
        public uint Receiver { get; set; }
        public uint Sequence { get; set; }
        public byte[] SenderIP { get; set; }
        public uint SenderPort { get; set; }
        public RecvSource Type { get; set; }

        /// <summary>
        /// 给定一个输入流，解析ReceiveIMHeader结构
    /// </summary>
        /// <param name="buf">The buf.</param>
        public void Read(ByteBuffer buf)
        {
            // 发送者QQ号或者群内部ID
            Sender = buf.GetInt();
            // 接收者QQ号
            Receiver = buf.GetUInt();
            // 包序号，这个序号似乎和我们发的包里面的序号不同，至少这个是int，我们发的是char
            //     可能这个序号是服务器端生成的一个总的消息序号
            Sequence = buf.GetUInt();
            // 发送者IP，如果是服务器转发的，那么ip就是服务器ip
            SenderIP = buf.GetByteArray(4);
            // 发送者端口，如果是服务器转发的，那么就是服务器的端口 两个字节
            SenderPort = buf.GetUShort();
            // 消息类型，是好友发的，还是陌生人发的，还是系统消息等等
            Type = (RecvSource)buf.GetUShort();
        }
    }
}
