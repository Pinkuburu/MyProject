
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Utils;
namespace LFNet.QQ.Packets
{
    /// <summary>
    /// 基本协议族的输出包基类
    /// 基本协议族的包都具有以下的格式:
    /// 1. 包头标志，1字节，0x02
    /// 2. 客户端版本代码，2字节
    /// 3. 命令，2字节
    /// 4. 包序号, 2字节
    /// 5. 用户QQ号，4字节
    /// 6. 包体
    /// 7. 包尾标志，1字节，0x03
    /// Note: 6部分将用会话密钥加密, 登录包例外，6部分要用密码密钥加密。请求登录令牌包例外，6部分不需加密
    /// </summary>
    public abstract class BasicOutPacket : OutPacket
    {
        private uint qqNum;
        /// <summary>构造一个参数指定的包.
    /// </summary>
        /// <param name="command">命令.</param>
        /// <param name="ack">是否需要回应.</param>
        /// <param name="user">The user.</param>
        protected BasicOutPacket(QQCommand command, bool ack, QQClient client) : base(QQGlobal.QQ_HEADER_BASIC_FAMILY, command, ack, client) { }
        /// <summary>
    /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="user">The user.</param>
        protected BasicOutPacket(ByteBuffer buf, QQClient client) : base(buf, buf.Length - buf.Position, client) { }
        protected BasicOutPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        /// <summary>
        /// 校验头部
    /// </summary>
        /// <returns></returns>
        protected override bool ValidateHeader()
        {
            return qqNum == user.QQ;
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
            int bodyLen = length - QQGlobal.QQ_LENGTH_BASIC_FAMILY_OUT_HEADER - QQGlobal.QQ_LENGTH_BASIC_FAMILY_TAIL;
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
            return QQGlobal.QQ_LENGTH_BASIC_FAMILY_OUT_HEADER + QQGlobal.QQ_LENGTH_BASIC_FAMILY_TAIL + bodyLength + (user.IsUdp ? 0 : 2);
        }

        protected override void ParseTail(ByteBuffer buf)
        {
            buf.Get();
        }

        public override string GetPacketName()
        {
            return "Unknown Outcoming Packet - Basic Family";
        }

        protected override byte[] DecryptBody(byte[] body, int offset, int length)
        {
#if DEBUG
            Client.LogManager.Log("BasicOutPacket DecryptBody Method is used! ");
#endif
            switch (Command)
            {
                case QQCommand.Touch:
                case QQCommand.LoginRequest:
                case QQCommand.LoginVerify:
                case QQCommand.LoginGetInfo:
                case QQCommand.LoginGetList:
                    


                case QQCommand.Request_Login_Token_05:
                    byte[] undecrypted = new byte[length];
                    Array.Copy(body, offset, undecrypted, 0, length);
                    return undecrypted;
                //case QQCommand.Login_05:
                //    byte[] ret = new byte[QQGlobal.QQ_LENGTH_KEY + QQGlobal.QQ_LENGTH_LOGIN_DATA];
                //    Array.Copy(body, 0, ret, 0, QQGlobal.QQ_LENGTH_KEY);
                //    byte[] b = crypter.Decrypt(body, QQGlobal.QQ_LENGTH_KEY, length - QQGlobal.QQ_LENGTH_KEY, user.QQKey.InitKey);
                //    Array.Copy(b, 0, ret, QQGlobal.QQ_LENGTH_KEY, b.Length);
                //    return ret;
                default:
                    return crypter.Decrypt(body, offset, length, user.QQKey.SessionKey);
            }
        }

        protected override byte[] EncryptBody(byte[] buf, int offset, int length)
        {
            switch (Command)
            {
                case QQCommand.Touch:
                case QQCommand.LoginRequest:
                case QQCommand.LoginVerify:
                case QQCommand.LoginGetInfo:
                case QQCommand.Login_A4:
                case QQCommand.LoginGetList:
                case QQCommand.LoginSendInfo:
                //case QQCommand.Login_05://不需要加密的内容
                    byte[] ret = new byte[length];
                    Array.Copy(buf, offset, ret, 0, length);
                    return ret;
                default:
                    return crypter.Encrypt(buf, offset, length, user.QQKey.SessionKey);
            }
        }
        protected override void PutHeader(ByteBuffer buf)
        {
            if (!user.IsUdp) buf.PutUShort(0);
            buf.Put(Header);
            buf.PutChar(Source);
            buf.PutUShort((ushort)Command);
            buf.PutChar(Sequence);
            buf.PutInt((uint)user.QQ);
        }
        protected override void PostFill(ByteBuffer buf, int startPos)
        {
            // 如果是tcp包，到包的开头处填上包长度，然后回到目前的pos
            if (!user.IsUdp)
            {
                int len = buf.Length - startPos;
                buf.PutUShort(startPos, (ushort)len);
            }
        }
        protected override void PutTail(ByteBuffer buf)
        {
            buf.Put(QQGlobal.QQ_TAIL_BASIC_FAMILY);
        }
        protected override void ParseHeader(ByteBuffer buf)
        {
            if (!user.IsUdp) buf.GetChar();
            Header = buf.Get();
            Source = buf.GetChar();
            Command = (QQCommand)buf.GetUShort();
            Sequence = buf.GetChar();
            qqNum = buf.GetUInt();
        }
        protected override int GetHeaderLength()
        {
            return QQGlobal.QQ_LENGTH_BASIC_FAMILY_OUT_HEADER + (user.IsUdp ? 0 : 2);
        }
        protected override int GetTailLength()
        {
            return QQGlobal.QQ_LENGTH_BASIC_FAMILY_TAIL;
        }
        public override string ToString()
        {
#if DEBUG
            return "Send 包类型: " + Command.ToString() + "(0x" + Command.ToString("X") + ") 序号: " + (int)Sequence + " 时间:" + DateTime.ToString();
#else
            return "包类型: " + Command.ToString() + "(0x" + Command.ToString("X") + ") 序号: " + (int)Sequence + " 时间:" + DateTime.ToString();
#endif
        }
        protected override int GetCryptographStart()
        {
            return -1;
        }
        public override ProtocolFamily GetFamily()
        {
            return ProtocolFamily.Basic;
        }
    }
}
