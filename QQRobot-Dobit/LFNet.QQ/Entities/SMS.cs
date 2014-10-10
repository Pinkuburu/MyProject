
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

namespace LFNet.QQ.Entities
{
    /// <summary>
    /// 短消息封装类
    /// </summary>
    public class SMS
    {
        public string Message { get; set; }
        public int Sender { get; set; }
        public int Header { get; set; }
        public long Time { get; set; }
        // 如果sender是10000，则senderName为手机号码
        public string SenderName { get; set; }

        /// <summary>给定一个输入流，解析SMS结构
    /// </summary>
        /// <param name="buf">The buf.</param>
        public void ReadBindUserSMS(ByteBuffer buf)
        {
            // 未知1字节，0x0
            buf.Get();
            // 发送者QQ号，4字节
            Sender = buf.GetInt();
            // 发送者头像
            Header = (int)buf.GetUShort();
            // 发送者名称，最多13字节，不足后面补0
            SenderName = Utils.Util.GetString(buf, (byte)0, QQGlobal.QQ_MAX_SMS_SENDER_NAME);
            // 未知的1字节，0x4D
            buf.Get();
            // 消息内容
            Message = Utils.Util.GetString(buf, (byte)0);

            Time = DateTime.Now.Millisecond;
        }

        /// <summary>读取移动QQ用户的短信
    /// </summary>
        /// <param name="buf">The buf.</param>
        public void ReadMobileQQSMS(ByteBuffer buf)
        {
            // 未知1字节
            buf.Get();
            // 发送者QQ号，4字节
            Sender = buf.GetInt();
            // 发送者头像
            Header = (int)buf.GetUShort();
            // 发送者名称，最多13字节，不足后面补0
            SenderName = Utils.Util.GetString(buf, (byte)0, QQGlobal.QQ_MAX_SMS_SENDER_NAME);
            // 未知的1字节，0x4D
            buf.Get();
            // 发送时间
            Time = (long)buf.GetInt() * 1000L;
            // 未知的1字节，0x03
            buf.Get();
            // 消息内容
            Message = Utils.Util.GetString(buf, (byte)0);
        }

        /// <summary>读取移动QQ用户消息（通过手机号描述）
    /// </summary>
        /// <param name="buf">The buf.</param>
        public void ReadMobileQQ2SMS(ByteBuffer buf)
        {
            // 未知1字节
            buf.Get();
            // 发送者，这种情况下都置为10000
            Sender = 10000;
            // 手机号码
            SenderName = Utils.Util.GetString(buf, (byte)0, 18);
            // 未知2字节
            buf.GetChar();
            // 时间
            Time = (long)buf.GetInt() * 1000L;
            // 未知的1字节，0x03
            buf.Get();
            // 消息内容
            Message = Utils.Util.GetString(buf, (byte)0);
        }
        /// <summary>读取普通手机的短信
    /// </summary>
        /// <param name="buf">The buf.</param>
        public void ReadMobileSMS(ByteBuffer buf)
        {
            // 未知1字节，0x0
            buf.Get();
            // 发送者
            Sender = 10000;
            // 手机号码
            SenderName = Utils.Util.GetString(buf, (byte)0, 20);
            // 消息内容
            Message = Utils.Util.GetString(buf, (byte)0);

            Time = DateTime.Now.Ticks;
        }
    }
}
