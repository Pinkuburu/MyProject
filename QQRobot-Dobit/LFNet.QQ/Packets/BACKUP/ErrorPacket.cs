
using System;
using System.Collections.Generic;
using System.Text;

namespace LFNet.QQ.Packets
{
    /// <summary>
    /// 错误代码
    /// </summary>
    public enum ErrorPacketType
    {
        /// <summary>
        /// 远端已经关闭连接 
        /// </summary>
        ERROR_CONNECTION_BROKEN = 0,
        /// <summary>
        ///  操作超时
        /// </summary>
        ERROR_TIMEOUT = 1,
        /// <summary>
        /// 代理服务器错误
        /// </summary>
        ERROR_PROXY = 2,
        /// <summary>
        /// 网络错误
        /// </summary>
        ERROR_NETWORK = 3,
        /// <summary>
        /// 运行时错误，调试用
        /// </summary>
        RUNTIME_ERROR = 4
    }
    /// <summary>这个包和协议无关，它用来通知上层，有些错误发生了，上层应该检查errorCode字段
    /// 来获得更具体的信息
    /// </summary>
    public class ErrorPacket : BasicInPacket
    {
        public ProtocolFamily Family { get; set; }
        public ErrorPacketType ErrorType;
        public string ConnectionId { get; set; }
        public string ErrorMessage { get; set; }
        /// <summary>在运行时错误的异常
    /// </summary>
        /// <value></value>
        public Exception e { get; private set; }
        /// <summary>
        /// 用在超时错误中
        /// </summary>
        public OutPacket TimeOutPacket { get; set; }
        public ErrorPacket(ErrorPacketType errorType, QQClient client, Exception e)
            : this(errorType, client)
        {
            this.e = e;
        }
        /// <summary>
    /// </summary>
        /// <param name="errorType">Type of the error.</param>
        /// <param name="user">The user.</param>
        public ErrorPacket(ErrorPacketType errorType, QQClient client)
            : base(QQCommand.Unknown, client)
        {
            this.ErrorType = errorType;
            this.Family = ProtocolFamily.All;
            ErrorMessage = "";
        }
        protected override void ParseBody(ByteBuffer buf)
        {

        }
        public override ProtocolFamily GetFamily()
        {
            return this.Family;
        }
    }
}
