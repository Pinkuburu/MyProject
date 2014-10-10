
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Entities
{
    /// <summary>
    /// 好友状态结构
    /// </summary>
    public class FriendStatus
    {
        public int QQ { get; set; }
        public byte Unknown1 { get; set; }
        /// <summary>
        /// 好友IP
        /// </summary>
        /// <value></value>
        public byte[] IP { get; set; }
        /// <summary>
        /// 好友端口
        /// </summary>
        /// <value></value>
        public ushort Port { get; set; }

        public byte Unknown2 { get; set; }
        /// <summary>
        /// 好友状态，定义在QQ接口中
        /// </summary>
        /// <value></value>
        public QQStatus Status { get; set; }
        public char Version { get; set; }
        /// <summary>
        /// 未知的密钥，会不会是用来加密和好友通讯的一些信息的，比如点对点通信的时候
        /// </summary>
        /// <value></value>
        public byte[] UnknownKey { get; set; }


        /// <summary>
        /// 用户属性标志
        /// </summary>
        /// <value></value>
        public uint UserFlag { get; set; }
        /// <summary>未知
        /// </summary>
        /// <value></value>
        public ushort Unknown3 { get; set; }
        /// <summary>未知字节
        /// </summary>
        /// <value></value>
        public byte Unknown4 { get; set; }

        public bool IsOnline()
        {
            return Status == QQStatus.ONLINE;
        }
        public bool IsAway()
        {
            return Status == QQStatus.AWAY;
        }
        /// <summary>
        ///  给定一个输入流，解析FriendStatus结构
        /// </summary>
        /// <param name="buf">The buf.</param>
        public void Read(ByteBuffer buf)
        {
            // 000-003: 好友QQ号
            QQ = buf.GetInt();
            // 004: 0x01，未知含义
            Unknown1 = buf.Get();
            // 005-008: 好友IP
            IP = buf.GetByteArray(4);
            // 009-010: 好友端口
            Port = buf.GetUShort();
            // 011: 0x01，未知含义
            Unknown2 = buf.Get();
            // 012: 好友状态
            Status = (QQStatus)buf.Get();
            // 013-014: 未知含义
            Version = buf.GetChar();
            // 015-030: key，未知含义
            UnknownKey = buf.GetByteArray(QQGlobal.QQ_LENGTH_KEY);

            UserFlag = buf.GetUInt();
            // 2个未知字节
            Unknown3 = buf.GetUShort();
            // 1个未知字节
            Unknown4 = buf.Get();
            buf.GetInt();
        }
    }
}
