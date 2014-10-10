
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
    ///  * 临时会话操作请求包，格式为
    /// * 1. 头部
    /// * 2. 子命令，1字节
    /// * 当2部分为0x01时，表示发送临时会话消息，格式为
    /// * 3. 接收者QQ号，4字节
    /// * 4. 未知的4字节
    /// * 5. 发送者昵称长度，1字节
    /// * 6. 发送者昵称
    /// * 7. Site名称长度，1字节
    /// * 8. Site名称
    /// * Note: 所谓Site就是这个临时会话发源的地点，如果用户从某个群中开始一个
    /// * 临时会话，Site就是群名称，这个域可以指定为任意值，没有什么影响
    /// * 9. 未知的1字节
    /// * Note: 测试发现，9部分只有为0x01或者0x02时，才能使对方收到消息
    /// * 10. 未知的4字节
    /// * 11. 后面的内容长度，2字节，exclusive
    /// * 12. 消息内容，结尾追加空格
    /// * 13. 字体属性，参加edu.tsinghua.lumaqq.qq.beans.FontStyle
    /// * 
    /// * Note: 临时会话消息在QQ中是限制发送长度的，而且不支持多条发送
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class TempSessionOpPacket : BasicOutPacket
    {
        public TempSessionSubCmd SubCommand { get; set; }

        // 用于发送临时会话消息时
        public int Receiver { get; set; }
        public string Nick { get; set; }
        public string Site { get; set; }
        public string Message { get; set; }
        public FontStyle FontStyle { get; set; }

        public TempSessionOpPacket(QQClient client)
            : base(QQCommand.Temp_Session_OP_05,true,client)
        {
            Site = Nick = Message = string.Empty;
            FontStyle = new FontStyle();
        }
        public TempSessionOpPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            switch (SubCommand)
            {
                case TempSessionSubCmd.SendIM:
                    return "Send Temp Session IM Packet";
                default:
                    return "Unknown Temp Session Op Packet";
            }
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put((byte)SubCommand);
            switch (SubCommand)
            {
                case TempSessionSubCmd.SendIM:
                    // 接收者
                    buf.PutInt(Receiver);
                    // 未知
                    buf.PutInt(0);
                    // nick
                    byte[] b = Utils.Util.GetBytes(Nick);
                    buf.Put((byte)b.Length);
                    buf.Put(b);
                    // site name
                    b = Utils.Util.GetBytes(Site);
                    buf.Put((byte)b.Length);
                    buf.Put(b);
                    // 未知
                    buf.Put((byte)1);
                    // 未知
                    buf.PutInt(0);
                    // 长度，最后再填
                    int pos = buf.Length;
                    buf.PutChar((char)0);
                    // 消息内容
                    b = Utils.Util.GetBytes(Message);
                    buf.Put(b);
                    buf.Put((byte)0x20);
                    // 字体属性
                    FontStyle.Write(buf);
                    // 回填长度
                    buf.PutChar(pos, (char)(buf.Length - pos - 2));
                    break;
            }
        }
    }
}
