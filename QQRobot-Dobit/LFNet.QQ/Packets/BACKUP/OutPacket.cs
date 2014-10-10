
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Utils;
namespace LFNet.QQ.Packets
{
    /// <summary>
    /// 所有输出包基类，这个基类定义了输出包的基本框架
    /// </summary>
    public abstract class OutPacket : Packet
    {
        /// <summary>
        /// 包起始序列号
        /// </summary>
        protected static char seq = (char)Util.Random.Next();
        /// <summary>
        /// 是否需要回应
        /// </summary>
        protected bool ack;
        /// <summary>
        /// 重发计数器
        /// </summary>
        protected int resendCountDown;
        /// <summary>
        /// 超时截止时间，单位ms
        /// </summary>
        public long TimeOut { get; set; }
        /// <summary>
        /// 发送次数，只在包是不需要ack时有效，比如logout包是发4次，但是其他可能只发一次
        /// </summary>
        public int SendCount { get; set; }
        /// <summary>
        /// 加密/解密密钥，只有有些包可能需要一个特定的密钥，如果为null，使用缺省的
        /// </summary>
        public byte[] Key { get; set; }

        /// <summary>创建一个基本输出包
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="command">包命令.</param>
        /// <param name="ack">包是否需要回复.</param>
        /// <param name="user">QQ用户对象.</param>
        public OutPacket(byte header, QQCommand command, bool ack, QQClient client)
            : base(header, QQGlobal.QQ_CLIENT_VERSION, command, GetNextSeq(), client)
        {
            this.ack = ack;
            this.resendCountDown = QQGlobal.QQ_SEND_TIME_NOACK_PACKET;
            this.SendCount = 1;
        }
        /// <summary>从buf中构造一个OutPacket，用于调试。这个buf里面可能包含了抓包软件抓来的数据
        /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="user">The user.</param>
        protected OutPacket(ByteBuffer buf, QQClient client) : base(buf, client) { }
        /// <summary>从buf中构造一个OutPacket，用于调试。这个buf里面可能包含了抓包软件抓来的数据
        /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="length">The length.</param>
        /// <param name="user">The user.</param>
        protected OutPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }

        /// <summary>
        /// 解析包体，从buf的开头位置解析起
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void ParseBody(ByteBuffer buf)
        {
        }

        /// <summary>
        /// 回填，有些字段必须填完整个包才能确定其内容，比如长度字段，那么这个方法将在
        /// 尾部填充之后调用
    /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="startPos">The start pos.</param>
        protected abstract void PostFill(ByteBuffer buf, int startPos);
        /// <summary>
        ///  将整个包转化为字节流, 并写入指定的ByteBuffer对象.
        ///  一般而言, 前后分别需要写入包头部和包尾部.
        /// </summary>
        /// <param name="buf">The buf.</param>
        public void Fill(ByteBuffer buf)
        {
            //保存当前pos
            int pos = buf.Position;
            // 填充头部
            PutHeader(buf);
            // 填充包体
            bodyBuf.Initialize();
            PutBody(bodyBuf);
            // 加密包体
            bodyDecrypted = bodyBuf.ToByteArray();
            byte[] enc = EncryptBody(bodyDecrypted, 0, bodyDecrypted.Length);
            // 加密内容写入最终buf
            buf.Put(enc);
            // 填充尾部
            PutTail(buf);
            // 回填
            PostFill(buf, pos);
//#if DEBUG
//            Client.LogManager.Log(ToString() + ":" + Utils.Util.ToHex(buf.ToByteArray()));
//#endif
        }
        protected static char GetNextSeq()
        {
            seq++;
            // 为了兼容iQQ
            // iQQ把序列号的高位都为0，如果为1，它可能会拒绝，wqfox称是因为TX是这样做的
            seq &= (char)0x7FFF;
            if (seq == 0)
            {
                seq++;
            }
            return seq;
        }
        /// <summary>
        /// 包的描述性名称
        /// </summary>
        /// <returns></returns>
        public override string GetPacketName()
        {
            return "Unknown Outcoming Packet";
        }
        /// <summary>
        /// 是否需要重发.
        /// </summary>
        /// <returns>需要重发返回true, 否则返回false.</returns>
        public bool NeedResend()
        {
            return (resendCountDown--) > 0;
        }
        /// <summary>
        /// 是否需要回复
    /// </summary>
        /// <returns>true表示包需要回复</returns>
        public bool NeedAck()
        {
            return ack;
        }

    }
}
