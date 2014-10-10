
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

using LFNet.QQ.Utils;

namespace LFNet.QQ.Entities
{
    /// <summary>
    /// 封装群信息
    /// </summary>
    public class ClusterInfo
    {
        public QQBasicInfo QQBasicInfo { get; set; }
        /// <summary>
        /// </summary>
        /// <value></value>
        public uint ClusterId { get; set; }
        /// <summary>
        /// // 如果是固定群，这个表示外部ID，如果是临时群，这个表示父群ID
        /// </summary>
        /// <value></value>
        public uint ExternalId { get; set; }
        /// <summary>
        /// type字段表示固定群或者临时群的群类型
        /// </summary>
        /// <value></value>
        public ClusterType Type { get; set; }
        /// <summary>
        /// </summary>
        /// <value></value>
        public uint Unknown1 { get; set; }
        public uint Creator { get; set; }
        public AuthType AuthType { get; set; }
        /// <summary>
        /// 2004的群分类
        /// </summary>
        /// <value></value>
        public uint OldCategory { get; set; }
        public char Unknown2 { get; set; }
        /// <summary>
        /// 2005采用的分类
        /// </summary>
        /// <value></value>
        public uint Category { get; set; }
        public char Unknown3 { get; set; }
        public byte Unknown4 { get; set; }
        public uint VersionId { get; set; }
        public string Name { get; set; }
        public char Unknown5 { get; set; }
        public string Description { get; set; }
        public string Notice { get; set; }
        /// <summary>
        /// </summary>
        public ClusterInfo()
        {
            Type = ClusterType.PERMANENT;
            AuthType = AuthType.NeedAuth;
            Description = Notice = Name = string.Empty;
        }
        /// <summary>
        /// 读取临时群信息
        /// </summary>
        /// <param name="buf">The buf.</param>
        public void ReadTempClusterInfo(ByteBuffer buf)
        {
            Type = (ClusterType)buf.Get();
            // 父群内部ID
            ExternalId = buf.GetUInt();
            // 临时群内部ID
            ClusterId = buf.GetUInt();
            Creator = buf.GetUInt();
            AuthType = (AuthType)buf.Get();
            // 未知的1字节
            buf.Get();
            Category = buf.GetChar();
            // 群组名称的长度
            int len = (int)buf.Get();
            byte[] b1 = buf.GetByteArray(len);
            Name = Util.GetString(b1);
        }

        /// <summary>
        /// 给定一个输入流，解析ClusterInfo结构，这个方法适合于得到群信息的回复包
        /// 2010/2/22 Veonax 修改
        /// </summary>
        /// <param name="buf">The buf.</param>
        public void ReadClusterInfo(ByteBuffer buf)
        {
            ClusterId = buf.GetUInt();
            ExternalId = buf.GetUInt();
            Type = (ClusterType)buf.Get();
            Unknown1 = buf.GetUInt();
            Creator = buf.GetUInt();
            AuthType = (AuthType)buf.Get();
            OldCategory = buf.GetUInt();
            Unknown2 = buf.GetChar();
            Category = buf.GetUInt();
            Unknown3 = buf.GetChar();
            Unknown4 = buf.Get();
            VersionId = buf.GetUInt();

            // unknown' 未知的4字节
            buf.GetUInt();

            // 群组名称的长度
            int len = (int)buf.Get();
            byte[] b1 = buf.GetByteArray(len);
            Unknown5 = buf.GetChar();
            // 群声明长度
            len = (int)buf.Get();
            byte[] b2 = buf.GetByteArray(len);
            // 群描述长度
            len = (int)buf.Get();
            byte[] b3 = buf.GetByteArray(len);
            // 转换成字符串
            Name = Util.GetString(b1, "GB2312");
            Notice = Util.GetString(b2, "GB2312");
            Description = Util.GetString(b3, "GB2312");
        }

        /// <summary>
        /// 从搜索群的回复中生成一个ClusterInfo结构
        /// </summary>
        /// <param name="buf">The buf.</param>
        public void ReadClusterInfoFromSearchReply(ByteBuffer buf)
        {
            ClusterId = buf.GetUInt();
            ExternalId = buf.GetUInt();
            Type = (ClusterType)buf.Get();
            // 未知的4字节
            buf.GetUInt();
            Creator = buf.GetUInt();
            OldCategory = buf.GetUInt();
            // 未知的2字节
            buf.GetChar();
            // 群名称长度和群名称
            int len = (int)buf.Get();
            byte[] b1 = buf.GetByteArray(len);
            // 两个未知字节
            buf.GetChar();
            // 认证类型
            AuthType = (AuthType)buf.Get();
            // 群描述长度和群描述
            len = (int)buf.Get();
            byte[] b2 = buf.GetByteArray(len);

            Name = Util.GetString(b1);
            Description = Util.GetString(b2);
        }
    }
}
