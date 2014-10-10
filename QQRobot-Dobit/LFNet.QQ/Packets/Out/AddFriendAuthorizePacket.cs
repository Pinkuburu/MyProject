
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
    ///  * 用来发送验证消息
    /// * 1. 头部
    /// * 2. 子命令，1字节
    /// * 3. 要添加的QQ号，4字节
    /// * 4. 是否允许对方加自己为好友，1字节
    /// * 5. 把好友加到第几组，我的好友组是0，然后以此类推，1字节
    /// * 6. 验证消息字节长度，1字节
    /// * 7. 验证消息
    /// * 8. 尾部
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class AddFriendAuthorizePacket : BasicOutPacket
    {
        public AddFriendAuthSubCmd SubCommand { get; set; }
        public int To { get; set; }
        public RevenseAdd ReverseAdd { get; set; }
        public int DestGroup { get; set; }
        public string Message { get; set; }
        /// <summary>
        /// 添加好友操作 通过0xAE请求的令牌
        /// </summary>
        public byte[] AddFriendToken { get; set; }
        /// <summary>
        /// 添加好友操作 回答问题后得到的令牌
        /// </summary>
        public byte[] AnswerToken { get; set; }
        public AddFriendAuthorizePacket(QQClient client)
            : base(QQCommand.AddFriendAuthorize,true,client)
        {
            //SubCommand = AddFriendAuthSubCmd.Request;// 0x02;
            //ReverseAdd = RevenseAdd.Allow;
            DestGroup = 0;
            Message = string.Empty;
        }
        public AddFriendAuthorizePacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        protected override void PutBody(ByteBuffer buf)
        {
            switch (SubCommand)
            { 
                case AddFriendAuthSubCmd.Approve:
                case AddFriendAuthSubCmd.ApproveAndAdd:
                case AddFriendAuthSubCmd.Reject:
                case AddFriendAuthSubCmd.NoAuth:
                    //03 (01表示不需要验证时的加对方为好友,03表示接受并加对方为好友,04表示只接受,05表示拒绝)
                    //25 D0 1F E1 
                    //00 00 00
                    buf.Put((byte)SubCommand);
                    buf.PutInt(To);
                    buf.PutUShort(0);//00 00
                    byte[] b = Utils.Util.GetBytes(Message);
                    buf.Put((byte)b.Length);//长度
                    buf.Put(b);
                    break;
                case AddFriendAuthSubCmd.Add:
                case AddFriendAuthSubCmd.AnswerAdd:
                case AddFriendAuthSubCmd.NeedAuthor:
                    buf.Put((byte)SubCommand);
                    buf.PutInt(To);
                    buf.Put((byte)ReverseAdd);//00
                    buf.Put((byte)DestGroup);//00
                    buf.PutUShort((ushort)AddFriendToken.Length);
                    buf.Put(AddFriendToken);
                    if (AnswerToken != null)
                    {
                        buf.PutUShort((ushort)AnswerToken.Length);
                        buf.Put(AnswerToken);
                    }
                    buf.Put(0x01);
                    buf.Put(0x00);
                    if (!string.IsNullOrEmpty(Message))
                    {
                        b = Utils.Util.GetBytes(Message);
                        buf.Put((byte)b.Length);
                        buf.Put(b);
                    }
                    break;
                default:
                    throw new Exception("unknown AddFriendAuthSubCmd=0x" + SubCommand.ToString("X"));
            
            }
            

#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
            
        }
    }
}
