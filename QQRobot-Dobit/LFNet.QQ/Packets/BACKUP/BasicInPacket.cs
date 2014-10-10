
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets
{
    /// <summary>
    /// 基本协议族的输入包基类:
    ///  1. 包头标志，1字节，0x02
    ///  2. 服务器端版本代码, 2字节
    ///  3. 命令，2字节
    ///  4. 包序号，2字节
    ///  5. 包体
    ///  6. 包尾标志，1字节，0x03
    /// 	在LumaQQ中，这边还定义了元数据。
    /// </summary>
    public abstract class BasicInPacket : InPacket
    {
       
        /// <summary>
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="user">The user.</param>
        public BasicInPacket(QQCommand command, QQClient client) : base(QQGlobal.QQ_HEADER_BASIC_FAMILY, QQGlobal.QQ_SERVER_VERSION_0100, command, client) { }
        /// <summary>
        /// 构造一个指定参数的包.从buf的当前位置开始解析直到limit
        /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="user">The user.</param>
        public BasicInPacket(ByteBuffer buf, QQClient client) : base(buf, client) { }
        /// <summary>
        /// 构造一个InPacket，从buf的当前位置解析length个字节
        /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="length">The length.</param>
        /// <param name="user">The user.</param>
        public BasicInPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        /// <summary>
        /// 从buf的当前位置解析包头
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void ParseHeader(ByteBuffer buf)
        {
            if (!user.IsUdp)
                buf.GetChar();
            Header = buf.Get();
            Source = buf.GetChar();
            Command = (QQCommand)buf.GetUShort();
            Sequence = buf.GetChar();
        }
        protected override void PutHeader(ByteBuffer buf)
        {
            if (!user.IsUdp)
                buf.PutUShort(0);
            buf.Put(Header);
            buf.PutChar(Source);
            buf.PutUShort((ushort)Command);
            buf.PutChar(Sequence);
        }
        /// <summary>
        /// 从buf的当前未知解析包尾
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void ParseTail(ByteBuffer buf)
        {
            buf.Get();
        }
        /// <summary>
        /// 初始化包体
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void PutBody(ByteBuffer buf)
        {

        }
        /// <summary>
        /// 将包尾部转化为字节流, 写入指定的ByteBuffer对象.
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void PutTail(ByteBuffer buf)
        {
            buf.Put(QQGlobal.QQ_TAIL_BASIC_FAMILY);
        }
        /// <summary>
        /// 包的描述性名称
        /// </summary>
        /// <returns></returns>
        public override string GetPacketName()
        {
            return "Unknown Incoming Packet - 0x" + Command.ToString("X");
        }
        /// <summary>
        /// 解密包体
        /// </summary>
        /// <param name="body">包体字节数组.</param>
        /// <param name="offset">包体开始偏移.</param>
        /// <param name="length">包体长度.</param>
        /// <returns>解密的包体字节数组</returns>
        protected override byte[] DecryptBody(byte[] body, int offset, int length)
        {
            byte[] temp = null;
            switch (Command)
            {
                case QQCommand.Touch:
                case QQCommand.LoginRequest:
                
                    temp = crypter.Decrypt(body, offset, length, user.QQKey.InitKey);
                    return temp;
                case QQCommand.LoginVerify:
                    temp = crypter.Decrypt(body, offset, length, user.QQKey.Verify_Key2);
                    return temp;
                case QQCommand.LoginGetInfo:
                    temp = crypter.Decrypt(body, offset, length, user.QQKey.LoginInfo_Key3);
                    return temp;
                case QQCommand.LoginGetList:
                case QQCommand.LoginSendInfo:
                    temp = crypter.Decrypt(body, offset, length, user.QQKey.LoginInfo_Key2);
                    return temp;
                case QQCommand.Login_A4:
                    return crypter.Decrypt(body, offset, length, user.QQKey.LoginInfo_Key1);
                    //byte[] undecrypted = new byte[length];
                    //Array.Copy(body, offset, undecrypted, 0, length);
                    //return undecrypted;
                case QQCommand.Request_Login_Token_05:
                    byte[] undecrypted = new byte[length];
                    Array.Copy(body, offset, undecrypted, 0, length);
                    return undecrypted;
                //case QQCommand.Login_05:
                //    temp = crypter.Decrypt(body, offset, length, user.QQKey.PasswordKey);
                //    if (temp == null)
                //    {
                //        temp = crypter.Decrypt(body, offset, length, user.QQKey.InitKey);
                //    }
                //    return temp;
                default:
                    if (user.QQKey.SessionKey != null)
                    {
                        temp = crypter.Decrypt(body, offset, length, user.QQKey.SessionKey);
                    }
                    if (temp == null)
                    {
                        temp = crypter.Decrypt(body, offset, length, user.QQKey.PasswordKey);
                    }
                    return temp;
            }
        }
        /// <summary>
        /// 加密包体
    /// </summary>
        /// <param name="buf">未加密的字节数组.</param>
        /// <param name="offset">包体开始的偏移.</param>
        /// <param name="length">包体长度.</param>
        /// <returns>加密的包体</returns>
        protected override byte[] EncryptBody(byte[] buf, int offset, int length)
        {
            return null;
        }
        /// <summary>
        /// 得到包体的字节数组
    /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="length">包总长度</param>
        /// <returns>包体字节数组</returns>
        protected override byte[] GetBodyBytes(ByteBuffer buf, int length)
        {
            // 得到包体长度
            int bodyLen = length - QQGlobal.QQ_LENGTH_BASIC_FAMILY_IN_HEADER - QQGlobal.QQ_LENGTH_BASIC_FAMILY_TAIL;
            if (!user.IsUdp) bodyLen -= 2;
            // 得到加密的包体内容
            byte[] body = buf.GetByteArray(bodyLen);
            return body;
        }
        /// <summary>
        /// 得到UDP形式包的总长度，不考虑TCP形式
    /// </summary>
        /// <param name="bodyLength">包体长度.</param>
        /// <returns>包长度</returns>
        protected override int GetLength(int bodyLength)
        {
            return QQGlobal.QQ_LENGTH_BASIC_FAMILY_IN_HEADER + QQGlobal.QQ_LENGTH_BASIC_FAMILY_TAIL + bodyLength + (user.IsUdp ? 0 : 2);
        }
        /// <summary>
        /// 包头长度
    /// </summary>
        /// <returns>包头长度</returns>
        protected override int GetHeaderLength()
        {
            return QQGlobal.QQ_LENGTH_BASIC_FAMILY_IN_HEADER + (user.IsUdp ? 0 : 2);
        }
        /// <summary>
        /// 包尾长度
    /// </summary>
        /// <returns>包尾长度</returns>
        protected override int GetTailLength()
        {
            return QQGlobal.QQ_LENGTH_BASIC_FAMILY_TAIL;
        }
        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
#if DEBUG
            return "Recv 包类型: " + Command.ToString() + " 序号: " + (int)Sequence;
#else
            return "包类型: " + Command.ToString() + " 序号: " + (int)Sequence;
#endif
        }
        /// <summary>
        /// 密文的起始位置，这个位置是相对于包体的第一个字节来说的，如果这个包是未知包，
        /// 返回-1，这个方法只对某些协议族有意义
    /// </summary>
        /// <returns></returns>
        protected override int GetCryptographStart()
        {
            return -1;
        }
        /// <summary>
        /// 标识这个包属于哪个协议族
    /// </summary>
        /// <returns></returns>
        public override ProtocolFamily GetFamily()
        {
            return ProtocolFamily.Basic;
        }
    }
}
