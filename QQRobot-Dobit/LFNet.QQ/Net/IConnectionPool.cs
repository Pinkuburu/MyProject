
#region 版权声明
//=========================================================== 
// 版权声明：LFNet.QQ是基于QQ2009版本的QQ协议开发而成的，协议
// 分析主要参考自小虾的MyQQ(C++)源代码，代码开发主要基于阿布
// 的LumaQQ.NET的C#.NET代码修改而成，故继续遵照使用LumaQQ的开
// 源协议。
//
// 本人没有对LumaQQ.NET的C#.NET代码的框架做过多的改动，主
// 要工作为将MyQQ的C++协议代码部分翻译成符合LumaQQ.Net框架
// 的C#代码，故请尊重LumaQQ作者Luma的著作权和版权声明。
// 
// 代码开源主要用于解决大家在学习和研究协议过程中遇到由于缺乏代码所带来的制约性问题。
// 本代码仅供学习交流使用，大家在使用此开发包前请自行协调好多方面关系，
// 不得用于任何商业用途和非法用途，本人不享受和承担由此产生的任何权利以及任何法律责任。
// 
// 本源代码可通过以下网址获取:
// http://QQCode.lynfo.com, http://www.lynfo.com, http://bbs.lynfo.com, http://hi.baidu.com/dobit.
//
// Copyright @ 2009-2010  Lynfo.com.  All Rights Reserved.   
// Framework: 2.0
// Author: Luma(java版) → Abu(C# QQ2005协议版) → Dobit(C# QQ2009协议版本)
// Email: dobit@msn.cn   
// Created: 2009-3-1~ 2009-11-28
// Last Modified:2009-11-28    
//   
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details. 
//===========================================================   
#endregion

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
