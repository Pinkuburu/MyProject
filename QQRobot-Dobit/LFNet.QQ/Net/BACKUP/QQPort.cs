
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
namespace LFNet.QQ.Net
{
    /// <summary>
    /// 一些缺省的QQ端口定义
    /// </summary>
    public class QQPort
    {
        public static QQPort Main = new QQPort("Main");
        public static QQPort CLUSTER_CUSTOM_FACE = new QQPort("CLUSTER_CUSTOM_FACE");
        public static QQPort CUSTOM_HEAD_INFO = new QQPort("CUSTOM_HEAD_INFO");
        public static QQPort CUSTOM_HEAD_DATA = new QQPort("CUSTOM_HEAD_DATA");
        public static QQPort DISK = new QQPort("DISK");
        static Dictionary<string, QQPort> ports;
        static QQPort()
        {
            ports = new Dictionary<string, QQPort>();
            ports.Add(Main.Name, Main);
            ports.Add(CLUSTER_CUSTOM_FACE.Name, CLUSTER_CUSTOM_FACE);
            ports.Add(CUSTOM_HEAD_INFO.Name, CUSTOM_HEAD_INFO);
            ports.Add(CUSTOM_HEAD_DATA.Name, CUSTOM_HEAD_DATA);
            ports.Add(DISK.Name, DISK);
        }
        /// <summary>根据名称得到QQPort对象
    /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static QQPort GetPort(string name)
        {
            return ports[name];
        }
        public string Name { get; set; }
        QQPort(string name)
        {
            this.Name = name;
        }
        public IConnection Create(QQClient client, string serverHost, int port, bool start)
        {
            IConnection conn = null;
            ConnectionPolicy policy = null;
            EndPoint server = GetEndPoint(serverHost, port);
            switch (Name)
            {
                case "Main":
                    policy = new ConnectionPolicy(client, Name, ProtocolFamily.Basic, ProtocolFamily.Basic);
                    if (client.QQUser.IsUdp)
                    {
                        conn = client.ConnectionManager.ConnectionPool.NewUDPConnection(policy, server, start);
                    }
                    else
                    {
                        conn = client.ConnectionManager.ConnectionPool.NewTCPConnection(policy, server, start);
                    }
                    break;
                case "CLUSTER_CUSTOM_FACE": break;
                case "CUSTOM_HEAD_INFO": break;
                case "CUSTOM_HEAD_DATA": break;
                case "DISK": break;
                default:
                    break;
            }
            return conn;
        }
        public static IPEndPoint GetEndPoint(string host, int port)
        {
            IPAddress ipAddress;
            IPAddress.TryParse(host, out ipAddress);
            if (ipAddress == null)
            {
                try
                {

                    System.Net.IPHostEntry ipHostEntry = System.Net.Dns.GetHostEntry(host);
                    ipAddress = ipHostEntry.AddressList[0];
                }
                catch { }
            }
            Check.Require(ipAddress != null, "获取:" + host + " IP失败！");
            return new IPEndPoint(ipAddress, port);
        }
    }
}
