
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

namespace LFNet.QQ.Entities
{
    /// <summary>
    /// 好友状态结构
    /// </summary>
    public class FriendStatus
    {
        public int QQ { get; set; }
        public byte Unknown1 { get; set; }
        /// <summary>
        /// 好友IP
        /// </summary>
        /// <value></value>
        public byte[] IP { get; set; }
        /// <summary>
        /// 好友端口
        /// </summary>
        /// <value></value>
        public ushort Port { get; set; }

        public byte Unknown2 { get; set; }
        /// <summary>
        /// 好友状态，定义在QQ接口中
        /// </summary>
        /// <value></value>
        public QQStatus Status { get; set; }
        public char Version { get; set; }
        /// <summary>
        /// 未知的密钥，会不会是用来加密和好友通讯的一些信息的，比如点对点通信的时候
        /// </summary>
        /// <value></value>
        public byte[] UnknownKey { get; set; }


        /// <summary>
        /// 用户属性标志
        /// </summary>
        /// <value></value>
        public uint UserFlag { get; set; }
        /// <summary>未知
        /// </summary>
        /// <value></value>
        public ushort Unknown3 { get; set; }
        /// <summary>未知字节
        /// </summary>
        /// <value></value>
        public byte Unknown4 { get; set; }

        public bool IsOnline()
        {
            return Status == QQStatus.ONLINE;
        }
        public bool IsAway()
        {
            return Status == QQStatus.AWAY;
        }
        /// <summary>
        ///  给定一个输入流，解析FriendStatus结构
        /// </summary>
        /// <param name="buf">The buf.</param>
        public void Read(ByteBuffer buf)
        {
            // 000-003: 好友QQ号
            QQ = buf.GetInt();
            // 004: 0x01，未知含义
            Unknown1 = buf.Get();
            // 005-008: 好友IP
            IP = buf.GetByteArray(4);
            // 009-010: 好友端口
            Port = buf.GetUShort();
            // 011: 0x01，未知含义
            Unknown2 = buf.Get();
            // 012: 好友状态
            Status = (QQStatus)buf.Get();
            // 013-014: 未知含义
            Version = buf.GetChar();
            // 015-030: key，未知含义
            UnknownKey = buf.GetByteArray(QQGlobal.QQ_LENGTH_KEY);

            UserFlag = buf.GetUInt();
            // 2个未知字节
            Unknown3 = buf.GetUShort();
            // 1个未知字节
            Unknown4 = buf.Get();
            buf.GetInt();
        }
    }
}
