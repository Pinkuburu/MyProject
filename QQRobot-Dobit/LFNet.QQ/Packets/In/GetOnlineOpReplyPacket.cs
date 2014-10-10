
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
    ///  * 得到在线好友列表的应答包，格式为
    /// * 1. 头部
    /// * 2. 在线好友是否已经全部得到，1字节
    /// * 3. 31字节的FriendStatus结构
    /// * 4. 2个未知字节
    /// * 5. 1个字节扩展标志
    /// * 6. 1个字节通用标志
    /// * 7. 2个未知字节
    /// * 8. 1个未知字节
    /// * 9. 如果有更多在线好友，重复2-8部分
    /// * 10. 尾部
    /// * 
    /// * 这个回复包最多返回30个在线好友，如果有更多，需要继续请求
    /// 	<remark>abu 2008-02-22 </remark>
    /// </summary>
    public class GetOnlineOpReplyPacket : BasicInPacket
    {
        /// <summary>true表示没有更多在线好友了
        /// 	<remark>abu 2008-02-22 </remark>
        /// </summary>
        /// <value></value>
        public bool Finished { get; set; }
        /// <summary>
        /// 下一个请求包的起始位置，仅当finished为true时有效
        /// 	<remark>abu 2008-02-22 </remark>
        /// </summary>
        /// <value></value>
        public int Position { get; set; }
        // 在线好友bean列表
        public List<FriendStatus> OnlineFriends { get; set; }
        public GetOnlineOpReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Get Friend Online Reply Packet";
        }
        protected override void ParseBody(ByteBuffer buf)
        {
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
            // 当前好友列表位置
            Finished = buf.Get() == QQGlobal.QQ_POSITION_ONLINE_LIST_END;
            Position = 0;
            //只要还有数据就继续读取下一个friend结构
            OnlineFriends = new List<FriendStatus>();
            while (buf.HasRemaining())
            {
                //int QQ = buf.GetInt();
                FriendStatus entry =new FriendStatus();
                //if(Client.QQUser.Friends.Get(QQ)!=null)
                //    entry = Client.QQUser.Friends.Get(QQ).FriendStatus;//new FriendStatus();
                entry.Read(buf);
                // 添加到List
                OnlineFriends.Add(entry);
                // 如果还有更多好友，计算position
                if (!Finished)
                    Position = Math.Max(Position, entry.QQ);
            }
            Position++;
        }
    }
}
