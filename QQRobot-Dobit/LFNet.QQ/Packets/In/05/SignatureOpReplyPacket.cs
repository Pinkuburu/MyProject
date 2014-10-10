
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
    ///  * 个性签名操作的回复包
    /// * 1. 头部
    /// * 2. 子命令，1字节
    /// * 3. 回复码，1字节
    /// * 
    /// * 如果2部分为0x00, 0x01，则
    /// * 4. 尾部
    /// * 
    /// * 如果2部分为0x02，即得到个性签名，则还有
    /// * 4. 下一个起始的QQ号，4字节。为这个回复包中所有QQ号的最大值加1
    /// * 5. QQ号，4字节
    /// * 6. 个性签名最后修改时间，4字节。这个修改时间的用处在于减少网络I/O，只有第一次我们需要
    /// *    得到所有的个性签名，以后我们只要送出个性签名，然后服务器会比较最后修改时间，修改过的
    /// *    才发回来
    /// * 7. 个性签名字节长度，1字节
    /// * 8. 个性签名
    /// * 9. 如果有更多，重复5-8部分
    /// * 10. 尾部
    /// 	<remark>abu 2008-02-26 </remark>
    /// </summary>
    public class SignatureOpReplyPacket : BasicInPacket
    {
        public SignatureSubCmd SubCommand { get; set; }
        public ReplyCode ReplyCode { get; set; }
        public int NextQQ { get; set; }
        public List<Signature> Signatures { get; set; }

        public SignatureOpReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        protected override void ParseBody(ByteBuffer buf)
        {
            SubCommand = (SignatureSubCmd)buf.Get();
            ReplyCode = (ReplyCode)buf.Get();
            if (SubCommand == SignatureSubCmd.GET)
            {
                NextQQ = buf.GetInt();
                Signatures = new List<Signature>();
                while (buf.HasRemaining())
                {
                    Signature sig = new Signature();
                    sig.Read(buf);
                    Signatures.Add(sig);
                }
            }
        }
        public override string GetPacketName()
        {
            return "Signature Op Reply Packet"; 
        }
    }
}
