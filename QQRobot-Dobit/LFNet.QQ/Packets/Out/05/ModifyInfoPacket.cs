
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
namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 修改用户个人信息的请求包，格式是:
    /// * 1. 头部
    /// * 2. 旧密码，新密码以及ContactInfo里面的域，但是不包括第一项QQ号，用0x1F分隔，依次往下排，最后要用
    /// *    一个0x1F结尾。但是开头不需要0x1F，如果哪个字段没有，就是空
    /// * 3. 尾部
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class ModifyInfoPacket : BasicOutPacket
    {
        /// <summary>标识是否有修改密码 阿不添加
        /// Gets or sets a value indicating whether [modify password].
        /// </summary>
        /// <value><c>true</c> if [modify password]; otherwise, <c>false</c>.</value>
        public bool ModifyPassword { get; set; }
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
        public ContactInfo ContactInfo { get; set; }
        private const byte DELIMIT = 0x1F;
        public ModifyInfoPacket(QQClient client) : base(QQCommand.Modify_Info_05,true,client) { }
        public ModifyInfoPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Modify Info Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            // 组装内容，首先是旧密码和新密码
            if (!string.IsNullOrEmpty(OldPassword) && !string.IsNullOrEmpty(NewPassword))
            {
                buf.Put(Utils.Util.GetBytes(OldPassword));
                buf.Put(DELIMIT);
                buf.Put(Utils.Util.GetBytes(NewPassword));
                ModifyPassword = true;
            }
            else
            {
                ModifyPassword = false;
                buf.Put(DELIMIT);
            }
            buf.Put(DELIMIT);
            // 写入contactInfo，除了QQ号
            String[] infos = ContactInfo.GetInfoArray();
            for (int i = 1; i < QQGlobal.QQ_COUNT_MODIFY_USER_INFO_FIELD; i++)
            {
                byte[] b = Utils.Util.GetBytes(infos[i]);
                buf.Put(b);
                buf.Put(DELIMIT);
            }
        }
    }
}
