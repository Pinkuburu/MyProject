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

namespace Lynfo.QQ2009
{
    /// <summary>
    /// 一些常量
    /// </summary>
    public static class Global
    {
        #region Packet Const
        /// <summary>
        /// QQ基本协议族包头 
        /// </summary>
        public const byte[] QQ_BASIC_FAMILY_HEADER =new byte[] { 0x02};
        /// <summary>
        /// QQ基本协议族包尾
        /// </summary>
        public const byte[] QQ_BASIC_FAMILY_TAIL =new byte[] { 0x03 };
        #endregion

        #region QQ客户端版本
        /// <summary>
        /// 程序缺省使用的客户端版本号
        /// </summary>
        public const byte[] QQ_CLIENT_VERSION = QQ_CLIENT_VERSION_2009_ZS;
        /// <summary>
        /// QQ2009正式版
        /// </summary>
        public const byte[] QQ_CLIENT_VERSION_2009_ZS = new byte { 0x16, 0x45 };
        #endregion


        //public const char QQ_VERSION = QQ_VERSION_ZS;//QQ版本
        //public const char QQ_VERSION_QF =	0x1205;	//祈福版
        //public const char QQ_VERSION_HS = 0x115b;	//贺岁版
        //public const char QQ_VERSION_Pre = 0x1525;	//QQ2009Preview4
        //public const char QQ_VERSION_ZS = 0x1663;	//QQ2009正式版
        
        //public const int MAX_LOOP_PACKET = 32;
        //public const int MAX_COMMAND = 0x0200;
        //public const int MAX_BUDDY = 1200;	//最大好友个数
        //public const int MAX_QUN = 128;	//最多群个数
        //public const int MAX_QUN_MEMBER = 800;	//群最多800成员
        //public const int MAX_GROUP = 128;	//最多分组个数
        //public const int MAX_EVENT = 128;	//最多事件缓冲个数
        //public const int USER_INFO_LEN = 256;
        //public const int MAX_USER_INFO = 38;
        //public const int SIGNITURE_LEN = 256;	//100
        //public const int TOKEN_LEN = 256;	//256
        //public const int ACCOUNT_LEN = 64;		//
        //public const int NICKNAME_LEN = 64;	//12
        //public const int GROUPNAME_LEN = 256;
        //public const int AUTO_REPLY_LEN = 256;
        //public const int ALIAS_LEN = 32;	//8
        //public const int PATH_LEN = 1024;
        //public const int MAX_SERVER_ADDR = 16;

    }

    public class token
    {
       public short	len;
	   public char[] data=new char[Global.TOKEN_LEN];
    }

    public enum QQSTATUS
    {
        QQ_NONE = 0x00,
        QQ_ONLINE = 0x0a,
        QQ_OFFLINE = 0x14,
        QQ_AWAY = 0x1e,
        QQ_BUSY = 0x32,
        QQ_KILLME = 0x3C,
        QQ_QUIET = 0x46,
        QQ_HIDDEN = 0x28
    }

    public enum MESSAGE_TYPE
    {
        MT_BUDDY,
        MT_QUN,
        MT_SYSTEM,
        MT_NEWS,
        MT_QUN_MEMBER
    }
}
