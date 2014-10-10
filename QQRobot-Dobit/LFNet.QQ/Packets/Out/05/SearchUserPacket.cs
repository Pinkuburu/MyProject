
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
    ///  * 搜索在线用户的包，格式为
    /// * 1. 头部
    /// * 2. 1个字节，表示搜索类型，比如搜索全部在线用户是0x31，自定义搜索是0x30
    /// * 3. 1字节分隔符: 0x1F
    /// * 4. 搜索参数
    /// * 	  i.  对于搜索全部在线用户的请求，是一个页号，用字符串表示，从0开始
    /// *    ii. 对于自定义搜索类型，是4个域，用0x1F分隔，依次是
    /// * 		   a. 要搜索的用户的QQ号的字符串形式
    /// * 		   b. 要搜索的用户的昵称
    /// * 		   c. 要搜索的用户的email
    /// *         d. 页号的字符串形式，这后面没有分隔符了，是用0x0结尾的         
    /// * 5. 尾部
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class SearchUserPacket : BasicOutPacket
    {
        public FriendSearchType SearchType { get; set; }
        public string Page { get; set; }
        public string QQStr { get; set; }
        public string Nick { get; set; }
        public string Email { get; set; }
        /** 分隔符 */
        private const byte DELIMIT = 0x1F;
        /** 如果字段为空，用0x2D替代，即'-'字符 */
        private const byte NULL = 0x2D;

        public SearchUserPacket(QQClient client)
            : base(QQCommand.Search_User_05,true,client)
        {
            Page = "0";
            SearchType = FriendSearchType.SEARCH_ALL;
            QQStr = Nick = Email = string.Empty;
        }
        public SearchUserPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Search User Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            // 开始组装内容
            if (SearchType == FriendSearchType.SEARCH_ALL)
            {
                buf.Put((byte)SearchType);
                buf.Put(DELIMIT);
                buf.Put(Utils.Util.GetBytes(Page));
            }
            else if (SearchType == FriendSearchType.SEARCH_CUSTOM)
            {
                buf.Put((byte)SearchType);
                buf.Put(DELIMIT);
                // QQ号
                if (string.IsNullOrEmpty(QQStr)) buf.Put(NULL);
                else buf.Put(Utils.Util.GetBytes(QQStr));
                buf.Put(DELIMIT);
                // 昵称
                if (string.IsNullOrEmpty(Nick)) buf.Put(NULL);
                else
                    buf.Put(Utils.Util.GetBytes(Nick));
                buf.Put(DELIMIT);
                // email
                if (string.IsNullOrEmpty(Email)) buf.Put(NULL);
                else
                    buf.Put(Utils.Util.GetBytes(Email));
                buf.Put(DELIMIT);
                // 结尾
                buf.Put(Utils.Util.GetBytes(Page));
                buf.Put((byte)0x0);
            }
        }
    }
}
