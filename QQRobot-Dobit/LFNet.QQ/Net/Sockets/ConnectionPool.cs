
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
using System.Collections.Generic;
using System.Text;
using LFNet.QQ.Packets;

namespace LFNet.QQ.Net.Sockets
{
    public class ConnectionPool : IConnectionPool
    {
        private Dictionary<string, IConnection> registry;
        private Dictionary<IConnection, int> references;

        public ConnectionPool()
        {
            registry = new Dictionary<string, IConnection>();
            references = new Dictionary<IConnection, int>();
        }
        #region IConnectionPool Members

        public void Flush()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Release(IConnection conn)
        {
            lock (registry)
            {
                conn.Close();
                conn.Dispose();
                foreach (string name in registry.Keys)
                {
                    if (registry[name] == conn)
                    {
                        registry.Remove(name);
                        break;
                    }
                }
                references.Remove(conn);
            }
        }

        public void Release(string id)
        {
            IConnection conn = GetConnection(id);
            if (conn != null)
            {
                Release(conn);
            }
        }

        /// <summary>
        /// 发送一个包，由于是异步发送包，所以keepSent目前暂时无用
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="packet">The packet.</param>
        /// <param name="keepSent">if set to <c>true</c> [keep sent].</param>
        public void Send(string id, OutPacket packet, bool keepSent)
        {
            IConnection conn = GetConnection(id);
            if (conn != null)
            {
                conn.Send(packet, keepSent);
            }
        }

        public IConnection NewUDPConnection(ConnectionPolicy policy, System.Net.EndPoint server, bool start)
        {
            IConnection conn = null;
            if (policy.Proxy.ProxyType == ProxyType.None)
            {
                conn = new UDPConnection(policy, server);
            }
            else
            {
                return null;
            }
            registry.Add(policy.ID, conn);
            if (start)
            {
                conn.Connect();
            }
            return conn;
        }

        public IConnection NewTCPConnection(ConnectionPolicy policy, System.Net.EndPoint server, bool start)
        {
            IConnection conn = null;
            if (policy.Proxy.ProxyType == ProxyType.None)
            {
                conn = new TCPConnection(policy, server);
            }
            else
            {
                conn = new ProxyTCPConnection(policy, server, policy.Proxy);
            }
            if (start)
            {
                //如果网络连接失败，则返回null
                if (!conn.Connect())
                {
                    return null;
                }
            }
            registry.Add(policy.ID, conn);
            return conn;
        }

        public IConnection GetConnection(string id)
        {
            return registry[id];
        }

        public IConnection GetConnection(System.Net.EndPoint server)
        {
            return null;
        }

        public void Dispose()
        {
            foreach (IConnection conn in registry.Values)
            {
                conn.Dispose();
            }
            registry.Clear();
        }

        public bool HasConnection(string id)
        {
            return registry.ContainsKey(id);
        }

        public List<IConnection> GetConnections()
        {
            return new List<IConnection>(registry.Values);
        }

        #endregion
    }
}
