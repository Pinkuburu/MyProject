
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

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 这个查询QQ号等级的包，格式是
    /// * 1. 头部
    /// * 2. 子命令，1字节
    /// * 3. 查询的号码，4字节
    /// * 4. 如果有更多好友，重复3部分
    /// * 5. 尾部
    /// * 
    /// * QQ的做法是一次最多请求70个。号码必须按照大小排序，本来之前不排序也可以，后来腾讯可能在服务器端动了些手脚，必须
    /// * 得排序了。这种顺序并没有在这个类中维护，所以是否排序目前是上层的责任，这个类假设收到的是一个排好序的用户QQ号
    /// * 列表
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class FriendLevelOpPacket : BasicOutPacket
    {
        /// <summary>
        /// Gets or sets the friends.
        /// </summary>
        /// <value>The friends.</value>
        public List<int> Friends { get; set; }
        /// <summary>
        /// Gets or sets the sub command.
        /// </summary>
        /// <value>The sub command.</value>
        public FriendLevelSubCmd SubCommand { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="FriendLevelOpPacket"/> class.
        /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="length">The length.</param>
        /// <param name="user">The user.</param>
        public FriendLevelOpPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="FriendLevelOpPacket"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        public FriendLevelOpPacket(QQClient client)
            : base(QQCommand.Friend_Level_OP_05,true,client)
        {
            SubCommand = FriendLevelSubCmd.GET;
        }
        /// <summary>
        /// 初始化包体
        /// <remark>abu 2008-02-18 </remark>
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put((byte)SubCommand);
            foreach (int friend in Friends)
                buf.PutInt(friend);
        }
        /// <summary>
        /// Gets the name of the packet.
        /// </summary>
        /// <returns></returns>
        public override string GetPacketName()
        {
            return "Get Friends Level Packet";
        }
    }
}
