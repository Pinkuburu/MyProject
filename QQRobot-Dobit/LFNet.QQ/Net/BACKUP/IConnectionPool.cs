
using System;
using System.Net;
using System.Text;
using System.Collections.Generic;

using LFNet.QQ.Packets;
namespace LFNet.QQ.Net
{
    /// <summary>
    /// 连接池接口，用于管理所有连接
    /// </summary>
    public interface IConnectionPool
    {
        /// <summary>
        /// 立刻发送所有包
        /// </summary>
        void Flush();
        /// <summary>
        /// 启动连接池
        /// </summary>
        void Start();

        /// <summary>
        /// 释放连接
        /// </summary>
        /// <param name="conn">The conn.</param>
        void Release(IConnection conn);

        /// <summary>
        /// 释放指定id的连接
        /// </summary>
        /// <param name="id">The id.</param>
        void Release(string id);

        /// <summary>
        /// 发送一个包
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="packet">The packet.</param>
        /// <param name="keepSent">if set to <c>true</c> [keep sent].</param>
        void Send(string id, OutPacket packet, bool keepSent);

        /// <summary>
        /// 新建一个UDP连接
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <param name="server">The server.</param>
        /// <param name="start">if set to <c>true</c> [start].</param>
        /// <returns></returns>
        IConnection NewUDPConnection(ConnectionPolicy policy,EndPoint server , bool start);

        /// <summary>
        /// 新建一个TCP连接
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <param name="server">The server.</param>
        /// <param name="start">if set to <c>true</c> [start].</param>
        /// <returns></returns>
        IConnection NewTCPConnection(ConnectionPolicy policy, EndPoint server, bool start);

        /// <summary>
        /// 根据id得到连接
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        IConnection GetConnection(string id);

        /// <summary>
        /// </summary>
        /// <param name="server">The server.</param>
        /// <returns></returns>
        IConnection GetConnection(EndPoint server);

        /// <summary>关闭这个连接池，释放所有资源。一个释放掉的连接池不可继续使用，必须新建一个新的连接池对象
        /// </summary>
        void Dispose();
        /// <summary>检测是否存在某个id的连接
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        bool HasConnection(string id);
        /// <summary>连接对象列表
        /// </summary>
        /// <returns></returns>
        List<IConnection> GetConnections();
 
    }
}
