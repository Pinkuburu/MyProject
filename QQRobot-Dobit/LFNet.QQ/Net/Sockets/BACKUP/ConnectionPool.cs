
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
