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
namespace LFNet.QQ
{
    /// <summary>
    /// QQ使用中的各类密匙
    /// </summary>
   public class QQKey
    {
       public QQUser QQUser { get; private set; }
       public QQKey(QQUser user)
       {
           this.QQUser = user;
           InitKey = Util.RandomKey();
           Verify_Key1 = Util.RandomKey();
           Verify_Key2 = Util.RandomKey();
           this.PasswordKey = Crypter.MD5(QQUser.Password);
       }
       /// <summary>
       /// 设置用户的密码，不会保存明文形式的密码，立刻用Double MD5算法加密
       /// </summary>
       /// <param name="pwd">明文形式的密码</param>
       public void SetPassword(string pwd)
       {
           PasswordKey = Crypter.MD5(Crypter.MD5(Util.GetBytes(pwd)));
       }

       /// <summary>
       /// MD5处理的用户密码
       /// </summary>
       /// <value></value>
       public byte[] PasswordKey { get; private set; }
       /// <summary>
       /// 0x91 Touch命令得到的Token
       /// 用到0xba LoginRequest命令
       /// </summary>
       public byte[] LoginRequestToken { get; set; }
       /// <summary>
       /// 0xba LoginRequest获取的Token
       /// 用于GetInfo
       /// </summary>
       public byte[] Answer_Token { get; set; }
       /// <summary>
       /// 0xba LoginRequest获取的Token 和 Answer_Token 是同一个Token数据
       /// 用于验证码登陆验证
       /// </summary>
       public byte[] Verify_Token { get; set; }

       /// <summary>
       /// Login Verify 用到的随机Key1
       /// </summary>
       public byte[] Verify_Key1 { get; private set; }
       /// <summary>
       /// Login Verify 用到的随机Key2 用来解LoginVerifyReplyPacket
       /// </summary>
       public byte[] Verify_Key2 { get; private set; }

       /// <summary>
       /// Login Verify得到的Token
       /// 用于LoginInfo
       /// </summary>
       public byte[] LoginInfo_Token { get; set; }
       /// <summary>
       /// Login Verify得到的Key
       /// 用于LoginInfo
       /// </summary>
       public byte[] LoginInfo_Key { get; set; }

       /// <summary>
       /// Login Verify得到的未知数据
       /// 用于LoginInfo
       /// </summary>
       public byte[] LoginInfo_UnknowData { get; set; }

       public byte[] LoginInfo_Data_Token { get; set; }
       public byte[] LoginInfo_Magic_Token { get; set; }
       /// <summary>
       /// 用在0xe5,0xa4, 0x30加密解密用
       /// </summary>
       public byte[] LoginInfo_Key1 { get; set; }
       /// <summary>
       /// 用来解密
       /// QQCommand.LoginGetInfo
       /// </summary>
       public byte[] LoginInfo_Key3 { get; set; }

       /// <summary>
       /// GetInfo时得到 解密
       /// QQCommand.LoginGetList
       /// QQCommand.LoginSendInfo
       /// </summary>
       public byte[] LoginInfo_Key2 { get; set; }
       /// <summary>
       /// GetInfo时得到
       /// </summary>
       public byte[] LoginInfo_UnknowData2 { get; set; }
       /// <summary>
       /// GetInfo时得到
       /// </summary>
       public byte[] LoginInfo_Large_Token { get; set; }

       /// <summary>
       /// Request Token 中用到
       /// 通过web方式下载验证码时得 getQQSession 字段
       /// </summary>
       public byte[] QQSessionToken { get; set; }

       public byte[] IM_Key { get; set; }

       private byte[] sessionKey;
       
       /// <summary>
       /// 会话密钥
       /// </summary>
       /// <value></value>
       public byte[] SessionKey
       {
           get { return this.sessionKey; }
           set
           {
               this.sessionKey = value;
               byte[] b = new byte[4 + QQGlobal.QQ_LENGTH_KEY];
               b[0] = (byte)((QQUser.QQ >> 24) & 0xFF);
               b[1] = (byte)((QQUser.QQ >> 16) & 0xFF);
               b[2] = (byte)((QQUser.QQ >> 8) & 0xFF);
               b[3] = (byte)(QQUser.QQ & 0xFF);
               Array.Copy(this.sessionKey, 0, b, 4, QQGlobal.QQ_LENGTH_KEY);
               this.FileSessionKey = Crypter.MD5(b);
           }
       }
       
       /// <summary>
       /// 文件传输会话密钥
       /// </summary>
       /// <value></value>
       public byte[] FileSessionKey { get; set; }
       /// <summary>
       /// 文件中转服务器通讯密钥，来自0x001D - 0x4
       /// </summary>
       /// <value></value>
       public byte[] FileAgentKey { get; set; }

       /// <summary>
       /// 文件中转认证令牌
       /// </summary>
       /// <value></value>
       public byte[] FileAgentToken { get; set; }
       /// <summary>
       /// 未知令牌
       /// </summary>
       /// <value></value>
       public byte[] Unknown03Token { get; set; }
      
       /// <summary>
       /// 客户端key
       /// </summary>
       /// <value></value>
       public byte[] ClientKey { get; set; }
       /// <summary>
       /// 初始密钥
       /// </summary>
       /// <value></value>
       public byte[] InitKey { get; private set; }
       /// <summary>
       /// 登录令牌
       /// </summary>
       /// <value></value>
       public byte[] LoginToken { get; set; }
       /// <summary>
       /// 未知用途密钥，来自0x001D
       /// </summary>
       /// <value></value>
       public byte[] Unknown03Key { get; set; }
       public byte[] Unknown06Key { get; set; }
       public byte[] Unknown07Key { get; set; }
       public byte[] Unknown08Key { get; set; }
       /// <summary>
       /// 未知令牌
       /// </summary>
       /// <value></value>
       public byte[] Unknown06Token { get; set; }
       public byte[] Unknown07Token { get; set; }
       public byte[] Unknown08Token { get; set; }
       /// <summary> 
       /// 认证令牌
       /// </summary>
       /// <value></value>
       public byte[] AuthToken { get; set; }
    }
}
