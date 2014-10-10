
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

namespace LFNet.QQ.Packets.In
{
    /// <summary>
    ///  * 下载分组名称的回复包，格式为
    /// *1. 头部
    /// *2. 子命令，1字节，下载是0x1
    /// *3. 回复码，1字节
    /// *5. 未知的4字节
    /// *6. 组序号，从1开始，0表示我的好友组，因为是缺省组，所以不包含在包中
    /// *7. 16字节的组信息，开始是组名，以0结尾，如果长度不足16字节，则其余部分可能为0，也可能
    /// *   为其他字节，含义不明
    /// *8. 若有多个组，重复6，7部分
    /// *9. 尾部
    /// *
    /// *上传分组名称的回复包，格式为
    /// *1. 头部
    /// *2. 子命令，1字节
    /// *3. 回复码，1字节
    /// *4. 组需要，从1开始，0表示我的好友组，因为是缺省组，所以不包含在包中
    /// *5. 如果有更多组，重复4部分
    /// *6. 尾部
    /// 	<remark>abu 2008-02-22 </remark>
    /// </summary>
    public class GroupDataOpReplyPacket : BasicInPacket
    {
        public List<string> GroupNames { get; set; }
        public GroupSubCmd SubCommand { get; set; }
        public ReplyCode ReplyCode { get; set; }
        public GroupDataOpReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            switch (SubCommand)
            {
                case GroupSubCmd.UPLOAD:
                    return "Group Data Reply Packet - Upload Group";
                case GroupSubCmd.DOWNLOAD:
                    return "Group Data Reply Packet - Download Group";
                default:
                    return "Group Data Reply Packet - Unknown Sub Command";
            }
        }
        protected override void ParseBody(ByteBuffer buf)
        {
            // 得到操作类型
            SubCommand = (GroupSubCmd)buf.Get();
            // 回复码
            ReplyCode = (ReplyCode)buf.Get();
            if (ReplyCode == ReplyCode.OK)
            {
                // 如果是下载包，继续解析内容
                if (SubCommand == GroupSubCmd.DOWNLOAD)
                {
                    // 创建list
                    GroupNames = new List<String>();
                    // 未知4个字节
                    buf.GetUInt();
                    // 读取每个组名
                    while (buf.HasRemaining())
                    {
                        buf.Get();
                        GroupNames.Add(Utils.Util.GetString(buf, (byte)0x00, 16));
                    }
                }
            }
        }
    }
}
