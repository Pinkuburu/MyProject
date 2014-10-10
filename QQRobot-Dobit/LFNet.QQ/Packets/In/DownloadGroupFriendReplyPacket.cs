
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

using LFNet.QQ.Entities;
namespace LFNet.QQ.Packets.In
{
    /// <summary>
    /// * 请求下载好友分组信息
    /// </summary>
    public class DownloadGroupFriendReplyPacket : BasicInPacket
    {
        /// <summary>
        /// Gets or sets the friends.
        /// </summary>
        /// <value>The friends.</value>
        public List<Group> Groups { get; set; }
        /// <summary>分组好友是否已经下载完
        /// Gets the finished.
        /// </summary>
        /// <value>The finished.</value>
        public bool Finished { get { return BeginFrom == 0; } }
        /// <summary>
        /// 起始好友号
        /// </summary>
        /// <value></value>
        public uint BeginFrom { get; set; }
        /// <summary>
        /// Gets or sets the reply code.
        /// </summary>
        /// <value>The reply code.</value>
        public ReplyCode ReplyCode { get; set; }
        /// <summary>
        /// Gets or sets the sub command.
        /// </summary>
        /// <value>The sub command.</value>
        public byte SubCommand { get; set; }
        public DownloadGroupFriendReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Download Group Friend Reply Packet";
        }
        /// <summary>
        /// 解析包体，从buf的开头位置解析起
        /// <remark>abu 2008-02-18 </remark>
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void ParseBody(ByteBuffer buf)
        {
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
            // 操作字节，下载为0x1F
            SubCommand = buf.Get();
            if (SubCommand == 0x1F)
            {
                // 起始好友号
                BeginFrom = buf.GetUInt();
                if (BeginFrom == 0x1000000) //no group labels info ??
                {
                    return;
                
                }
                if (BeginFrom != 0x00)
                {
                    Client.LogManager.Log("BeginFrom==0x"+BeginFrom.ToString("X"));
                }
                buf.Get();//0x17
                buf.GetChar();

                // 循环读取各好友信息，加入到list中
                Groups = new List<Group>();
                while (buf.HasRemaining())
                {
                    Group g = new Group(buf);
                    Groups.Add(g);
                }

            }
        }
    }
}
