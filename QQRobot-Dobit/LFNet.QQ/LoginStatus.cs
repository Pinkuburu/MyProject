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
using System.Globalization;

namespace LFNet.QQ
{
    /// <summary>
    /// 登陆状态
    /// </summary>
    public class LoginStatus
    {
        private static LoginStatus _logout = new LoginStatus("Logout", "未登录");
        private static LoginStatus _needVerifyCode = new LoginStatus("NeedVerifyCode", "需要验证码");
        private static LoginStatus _sendVerifyCode = new LoginStatus("SendVerifyCode", "发送验证码");
        private static LoginStatus _login = new LoginStatus("Login", "登陆成功!");
        private static LoginStatus _connectting = new LoginStatus("Connectting", "正在连接服务器");
        private static LoginStatus _connected = new LoginStatus("Connected", "连接到服务器");
        private static LoginStatus _redirecting = new LoginStatus("redirecting", "正在转向");
        private static LoginStatus _logining = new LoginStatus("Logining", "正在登陆");
        private static LoginStatus _failed = new LoginStatus("Failed", "登陆失败");
        private static LoginStatus _wrongPassword = new LoginStatus("Wrong Password", "密码错误");
        private static LoginStatus _getList = new LoginStatus("Get QQ List", "下载QQ列表");
        private static LoginStatus _changeServer = new LoginStatus("Change Server IP", "更换服务器");

        public static LoginStatus ChangeServer
        {
            get { return LoginStatus._changeServer; }
            set { LoginStatus._changeServer = value; }
        }

        public static LoginStatus WrongPassword
        {
            get { return LoginStatus._wrongPassword; }
        }

        private string Name { get; set; }
        private string CName { get; set; }
        public LoginStatus(string name, string cName)
        {
            this.Name = name;
            this.CName = cName;
        }

        public override string ToString()
        {
            if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "zh")
            {
                return CName;
            }
            else
            { return Name; }
        }

        /// <summary>
        /// 未登录
        /// </summary>
        public static LoginStatus Logout
        {
            get { return _logout; }
        }

        public static LoginStatus Connectting
        {
            get { return _connectting; }
        }

        public static LoginStatus Connected
        {
            get { return _connected; }
        }

        /// <summary>
        /// 登陆中
        /// </summary>
        public static LoginStatus Logining
        {
            get { return _logining; }
        }

        /// <summary>
        /// 需要验证码
        /// </summary>
        public static LoginStatus NeedVerifyCode
        {
            get { return _needVerifyCode; }
        }
        /// <summary>
        /// 发送验证码
        /// </summary>
        public static LoginStatus SendVerifyCode
        {
            get { return _sendVerifyCode; }
        }
        /// <summary>
        /// 登陆成功
        /// </summary>
        public static LoginStatus Login
        {
            get { return _login; }
        }
        public static LoginStatus GetList
        {
            get { return _getList; }
        }
        /// <summary>
        /// 登陆失败
        /// </summary>
        public static LoginStatus Failed
        {
            get { return _failed; }
        }



    }
}
