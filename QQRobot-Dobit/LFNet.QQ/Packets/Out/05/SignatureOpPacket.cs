
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
    ///  * 个性签名操作请求包
    /// * 1. 头部
    /// * 2. 子命令，1字节
    /// * 
    /// * 根据2部分的不同
    /// * 为0x01时：
    /// * 3. 未知1字节
    /// * 4. 个性签名的字节长度，1字节
    /// * 5. 个性签名
    /// * 6. 尾部
    /// * 
    /// * 为0x00时，无后续内容
    /// * 3. 尾部
    /// * 
    /// * 为0x02时
    /// * 3. 未知的1字节
    /// * 4. 需要得到个性签名的QQ号数量，1字节
    /// * 5. QQ号，4字节
    /// * 6. 本地的个性签名修改时间，4字节
    /// * 7. 如果有更多QQ号，重复5-6部分
    /// * 8. 尾部 
    /// * 
    /// * 在得到好友的个性签名时，QQ的做法是对所有的QQ号排个序，每次最多请求33个。
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class SignatureOpPacket : BasicOutPacket
    {
        public SignatureSubCmd SubCommand { get; set; }
        public string Signature { get; set; }
        public List<Signature> Signatures { get; set; }

        public SignatureOpPacket(QQClient client)
            : base(QQCommand.Signature_OP_05,true,client)
        {
            SubCommand = SignatureSubCmd.MODIFY;
            Signature = string.Empty;
        }
        public SignatureOpPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Signature Op Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put((byte)SubCommand);
            switch (SubCommand)
            {
                case SignatureSubCmd.MODIFY:
                    buf.Put((byte)0x01);
                    byte[] b = Utils.Util.GetBytes(Signature);
                    buf.Put((byte)b.Length);
                    buf.Put(b);
                    break;
                case SignatureSubCmd.GET:
                    buf.Put((byte)0);
                    buf.Put((byte)Signatures.Count);
                    foreach (Signature sig in Signatures)
                    {
                        buf.PutInt(sig.QQ);
                        buf.PutInt(sig.ModifiedTime);
                    }
                    break;
            }
        }
    }
}
