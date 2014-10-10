
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
    /// 好友的信息
    /// </summary>
    public class QQFriend
    {
        public QQFriend()
        {
            this.FriendStatus = new FriendStatus();
            this.QQBasicInfo = new QQBasicInfo(0, QQType.QQ, 0);
        }
        /// <summary>
        /// QQ基本信息 
        /// 包括号码 组id 类型
        /// </summary>
        /// <value></value>
        public QQBasicInfo QQBasicInfo { get; set; }
        public int QQ { get { return QQBasicInfo.QQ; } }
        public FriendStatus FriendStatus { get; set; }
        /// <summary>
        /// 头像，参看ContactInfo的头像注释
        /// </summary>
        /// <value></value>
        public int Header { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        /// <value></value>
        public int Age { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        /// <value></value>
        public Gender Gender { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        /// <value></value>
        public string Nick { get; set; }

        /// <summary>
        /// // 用户属性标志
        /// bit1 => 会员
        /// bit5 => 移动QQ
        /// bit6 => 绑定到手机
        /// bit7 => 是否有摄像头
        /// bit18 => 是否TM登录
        /// </summary>
        /// <value></value>
        public uint UserFlag { get; set; }

        /// <summary>
        /// true如果好友是会员，否则为false
        /// </summary>
        /// <returns></returns>
        public bool IsMember()
        {
            return (UserFlag & QQGlobal.QQ_FLAG_MEMBER) != 0;
        }
        /// <summary>
        /// 是否绑定手机
        /// </summary>
        /// <returns></returns>
        public bool IsBind()
        {
            return (UserFlag & QQGlobal.QQ_FLAG_BIND) != 0;
        }
        /// <summary>是否移动QQ
        /// </summary>
        /// <returns></returns>
        public bool IsMobile()
        {
            return (UserFlag & QQGlobal.QQ_FLAG_MOBILE) != 0;
        }
        /// <summary>
        /// 用户是否有摄像头
        /// </summary>
        /// <returns></returns>
        public bool HasCam()
        {
            return (UserFlag & QQGlobal.QQ_FLAG_CAM) != 0;
        }
        /// <summary>
        /// 用户是否使用TM登录
        /// </summary>
        /// <returns></returns>
        public bool IsTM()
        {
            return (UserFlag & QQGlobal.QQ_FLAG_TM) != 0;
        }
        /// <summary>
        /// 是否是男性
        /// </summary>
        /// <returns></returns>
        public bool IsGG()
        {
            return Gender == Gender.GG;
        }

        /// <summary>
        /// QQ状态
        /// </summary>
        public QQStatus Status
        {
            get
            {
                return FriendStatus.Status == QQStatus.NONE ? QQStatus.OFFLINE : FriendStatus.Status;
            }
        }
        /// <summary>
        /// 给定一个输入流，解析QQFriend结构
        /// </summary>
        /// <param name="buf">The buf.</param>
        public void Read(ByteBuffer buf)
        {
            //// 000-003: 好友QQ号
            //QQ = buf.GetInt();
            // 004-005: 头像
            Header = buf.GetUShort();
            // 006: 年龄
            Age = buf.Get();
            // 007: 性别
            Gender = (Gender)buf.Get();
            // 008: 昵称长度
            int len = (int)buf.Get();
            byte[] b = buf.GetByteArray(len);
            Nick = Util.GetString(b);
            // 用户属性
            UserFlag = buf.GetUInt();
            buf.Position += 23;
        }

        /// <summary>
        /// 从群成员信息GetClusterMemberInfo包中读取相关信息
        /// 2010/2/22 Veonax 添加
        /// </summary>
        /// <param name="buf">The buf.</param>
        public void ReadFromCluster(ByteBuffer buf)
        {

            //// 000-003: 好友QQ号
            QQBasicInfo.QQ = buf.GetInt();
            // 004-005: 头像
            Header = buf.GetUShort();
            // 006: 年龄
            Age = buf.Get();
            // 007: 性别
            Gender = (Gender)buf.Get();
            // 008: 昵称长度
            int len = (int)buf.Get();
            byte[] b = buf.GetByteArray(len);
            Nick = Util.GetString(b, "GB2312");
            // 用户属性
            UserFlag = buf.GetUInt();
        }
        
    }
}
