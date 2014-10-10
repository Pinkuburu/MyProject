
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
