
using System;
using System.Collections.Generic;
using System.Text;


using LFNet.QQ.Packets;

namespace LFNet.QQ.Net
{
    public interface IConnection : IDisposable
    {
        /// <summary>
        /// 添加一个输出包
    /// </summary>
        /// <param name="outPacket">The out packet.</param>
        /// <param name="monitor">if set to <c>true</c> [monitor].true为同步发送，false为异步发送</param>
        void Send(OutPacket outPacket,bool monitor);
        /// <summary>
        /// 清空输出队列
    /// </summary>
        void ClearSendQueue();
        /// <summary>
        /// 连接到服务器
    /// </summary>
        bool Connect();
        /// <summary>
        /// 关闭连接
    /// </summary>
        void Close();
        /// <summary>
        /// 
    /// </summary>
        /// <value></value>
        ConnectionPolicy Policy { get; }
        /// <summary>
        /// 是否处于连接状态
    /// </summary>
        /// <value></value>
        bool IsConnected { get; }

        /// <summary>连接名称
    /// </summary>
        /// <value></value>
        string Name { get; }
    }
}
