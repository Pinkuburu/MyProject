

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
namespace LFNet.QQ
{
    ///// <summary>
    ///// QQ好友信息
    ///// 	<remark>abu 2008-03-11 </remark>
    ///// </summary>
    //public class FriendInfo
    //{
    //    /// <summary>
    //    /// 	<remark>abu 2008-03-11 </remark>
    //    /// </summary>
    //    /// <param name="basicInfo">The basic info.</param>
    //    public FriendInfo(QQFriend basicInfo)
    //    {
    //        this.BasicInfo = basicInfo;
    //    }
    //    /// <summary>
    //    /// 好友的基本信息
    //    /// </summary>
    //    /// <value></value>
    //    public QQFriend BasicInfo { get; private set; }
    //    /// <summary>
    //    /// 好的状态信息
    //    /// </summary>
    //    /// <value></value>
    //    public FriendStatus Status { get; set; }
    //    /// <summary>
    //    /// 用户是否在线
    //    /// </summary>
    //    /// <returns></returns>
    //    public QQStatus GetStatu()
    //    {
    //        if (Status == null)
    //        {
    //            return QQStatus.OFFLINE;
    //        }
    //        return Status.Status;
    //    }
    //}

    public class FriendList : Dictionary<int,QQFriend> //FriendInfo>
    {
        public QQUser QQUser;
        public FriendList(QQUser qqUser)
            : base()
        {
            this.QQUser = qqUser;
        }

        /// <summary>
        /// 添加一个QQ
        /// </summary>
        /// <param name="q"></param>
        public QQFriend Add(int q)
        {
            QQBasicInfo qq = new QQBasicInfo(q, QQType.QQ, 0);
            QQUser.QQList.Add(q, qq);
            QQFriend qqfriend = new QQFriend();
            qqfriend.QQBasicInfo = qq;
            this.Add(q, qqfriend);
            return qqfriend;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qq"></param>
        /// <returns></returns>
        public QQFriend Get(int qq)
        {
            if (this.ContainsKey(qq))
                return this[qq];
            else
                return null;
        }
        /// <summary>
        /// 在线好友数量
        /// </summary>
        /// <value></value>
        public int Onlines { get; private set; }
        /// <summary>设置好友为上线状态
        /// 	<remark>abu 2008-03-11 </remark>
        /// </summary>
        /// <param name="qq">The qq.</param>
        /// <param name="onlineEntry">The online entry.</param>
        public void SetFriendOnline(int qq, FriendStatus friendStatus)
        {
            this[qq].FriendStatus = friendStatus;
            this.Onlines++;
        }
        /// <summary>设置好友为离线状态
        /// 	<remark>abu 2008-03-11 </remark>
        /// </summary>
        /// <param name="qq">The qq.</param>
        public void SetFriendOffline(int qq)
        {
            if (this[qq] != null)
            {
                if (this[qq].FriendStatus.Status != QQStatus.OFFLINE)
                {
                    this.Onlines--;
                }
                if (this[qq].FriendStatus != null)
                {
                    this[qq].FriendStatus.Status = QQStatus.OFFLINE;
                }
            }

        }
        public void SetFriendStatus(int qq, QQStatus status)
        {
            if (!this.ContainsKey(qq))
            {
                QQFriend friend = new QQFriend();
                friend.QQBasicInfo.QQ=qq;
                friend.QQBasicInfo.GroupId = -1;//应该是陌生人
                this.Add(qq,friend);
            }

            if (this[qq].FriendStatus.Status != QQStatus.OFFLINE && status == QQStatus.OFFLINE)
            {
                this.Onlines--;
            }
            else if (this[qq].FriendStatus.Status == QQStatus.OFFLINE && status != QQStatus.OFFLINE)
            {
                this.Onlines++;
            }
            if (this[qq].FriendStatus != null)
            {
                this[qq].FriendStatus.Status = status;
            }
            else
            {
                this[qq].FriendStatus = new FriendStatus();
                this[qq].FriendStatus.Status = status;
            }

        }
    }
}
