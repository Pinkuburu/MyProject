
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
using LFNet.QQ.Entities;
namespace LFNet.QQ
{
    /// <summary>
    /// 登陆QQ用户信息
    /// </summary>
    public class QQUser
    {
        #region 构造函数
        /// <summary>
        /// 登陆QQ用户信息
        /// </summary>
        /// <param name="qqNum">QQ号.</param>
        /// <param name="pwd">密码.</param>
        public QQUser(int qqNum, string pwd)
        {
            this.QQ = qqNum;
            this.Password = Crypter.MD5(Util.GetBytes(pwd));
            Initialize();
        }

        public QQUser(int qqNum, byte[] md5pwd)
        {
            this.QQ = qqNum;
            this.Password = md5pwd;
            this.Initialize();
        }
        #endregion
        #region Methods
        private void Initialize()
        {
            IP = new byte[4];
            ServerIp = new byte[4];
            LastLoginIp = new byte[4];
            IsLoggedIn = false;
            LoginMode = QQStatus.ONLINE;
            IsUdp = true;
            ContactInfo = new ContactInfo();
            IsShowFakeCam = false;
            Friends = new FriendList(this);
            QQList = new QQList();
            ClusterList = new ClusterList();
            this.QQKey = new QQKey(this);
        }

        
        #endregion
        #region Objects
        /// <summary>
        /// QQ好友列表
        /// </summary>
        /// <value></value>
        public FriendList Friends { get; private set; }

        /// <summary>
        /// 所有QQ号列表
        /// </summary>
        public QQList QQList { get; set; }

        /// <summary>
        /// QQ群列表
        /// </summary>
        public ClusterList ClusterList { get; set; }

        /// <summary>
        /// QQ密匙和Token
        /// </summary>
        public QQKey QQKey { get; private set; }

        /// <summary>
        /// 当前的状态，比如在线，隐身等等
        /// </summary>
        /// <value></value>
        public QQStatus Status { get; set; }
        /// <summary>
        /// ContactInfo 
        /// </summary>
        /// <value></value>
        public ContactInfo ContactInfo { get; set; }

        /// <summary>
        /// 登陆模式，隐身还是非隐身
        /// </summary>
        /// <value></value>
        public QQStatus LoginMode { get; set; }
        #endregion
        #region 属性
        /// <summary>
        /// QQ号
        /// </summary>
        /// <value></value>
        public int QQ { get; set; }

        /// <summary>
        /// md5一次加密密码
        /// </summary>
        public byte[] Password { get; private set; }

        /// <summary>
        /// 本地IP
        /// </summary>
        /// <value></value>
        public byte[] IP { get; set; }
        /// <summary>
        /// 上一次登陆IP
        /// </summary>
        /// <value></value>
        public byte[] LastLoginIp { get; set; }
        /// <summary>
        /// 本地端口，在QQ中其实只有两字节
        /// </summary>
        /// <value></value>
        public int Port { get; set; }
        /// <summary>
        /// 服务器IP
        /// </summary>
        /// <value></value>
        public byte[] ServerIp { get; set; }
        /// <summary>
        /// 服务器端口，在QQ中其实只有两字节
        /// </summary>
        /// <value></value>
        public int ServerPort { get; set; }
        /// <summary>
        /// 上一次登陆时间，在QQ中其实只有4字节
        /// </summary>
        /// <value></value>
        public long LastLoginTime { get; set; }
        /// <summary>
        /// 本次登陆时间
        /// </summary>
        /// <value></value>
        public long LoginTime { get; set; }
        /// <summary>
        /// 当前登陆状态，为true表示已经登陆
        /// 已经过期，应该从QQClient.LoginStatus判断
        /// </summary>
        /// <value></value>
        [Obsolete]
        public bool IsLoggedIn { get; set; }

        /// <summary>
        /// 设置登陆服务器的方式是UDP还是TCP
        /// </summary>
        /// <value></value>
        public bool IsUdp { get; set; }


        /// <summary>
        /// 是否显示虚拟摄像头
        /// </summary>
        /// <value></value>
        public bool IsShowFakeCam { get; set; }


        /// <summary>
        /// 个性签名
        /// </summary>
        /// <value>The signature.</value>
        public string Signature { get; set; }
        #endregion
    }
}
