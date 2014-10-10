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
    /// 返回用户好友群列表
    /// </summary>
    public class LoginGetListReplyPacket : BasicInPacket
    {
        public LoginGetListReplyPacket(ByteBuffer buf, int length, QQClient client)
            : base(buf, length, client)
        {
        }

        public override string GetPacketName()
        {
            return "Login GetList Reply Packet";
        }
        /// <summary>
        /// 当为0x038A时表示后面还有数据
        /// </summary>
        public char ReplyCode { get; private set; }
        public bool Finished { get; private set; }
        /// <summary>
        /// 请求下一个包
        /// </summary>
        public ushort NextPos { get; set; }
        ///// <summary>
        ///// 本次获取的QQ好友列表
        ///// </summary>
        //public List<QQFriend> QQFriendList { get; set; }
        public List<QQBasicInfo> QQList { get; set; }
        ///// <summary>
        ///// QQ群列表
        ///// </summary>
        //public List<ClusterInfo> ClusterList { get; set; }
        protected override void ParseBody(ByteBuffer buf)
        {
#if DEBUG
            Client.LogManager.Log(ToString() + " Decoded Data:" + Utils.Util.ToHex(buf.ToByteArray()));
#endif

            ReplyCode = buf.GetChar();//00 9C
            buf.GetInt();//00 00 00 00
            NextPos = buf.GetUShort();
            Finished = !(ReplyCode == 0x038A && NextPos > 0);
            //this.ClusterList = new List<ClusterInfo>();
            //this.QQFriendList = new List<QQFriend>();
            this.QQList = new List<QQBasicInfo>();
            while (buf.Position + 2 < buf.Length)
            {
                int number = buf.GetInt();
                QQType type =(QQType) buf.Get();
                byte gid = buf.Get();

                QQBasicInfo qq = new QQBasicInfo(number, type, ((int)gid) / 4);
                //qq.UIN = number;
                //qq.GroupId = ((int)gid) / 4;
                //qq.Type = (QQType)type;
                this.QQList.Add(qq);
                //if (type == 0x04)
                //{
                //    ClusterInfo ci = new ClusterInfo();
                //    ci.ClusterId =(uint) number;//群内部号码
                //    this.ClusterList.Add(ci);
                //}
                //else if (type == 0x01)
                //{
                //    QQFriend friend = new QQFriend();
                    
                //    friend.QQ = number;
                //    friend.GroupId = ((int)gid)/4;
                //    this.QQFriendList.Add(friend);
                //}
                //else
                //{
                //    Client.LogManager.Log("unknown type: type=0x"+type.ToString("X2")+" number="+number.ToString() +" gid=0x"+gid.ToString("X2"));
                //}
            
            }


            
        }


    }
}
