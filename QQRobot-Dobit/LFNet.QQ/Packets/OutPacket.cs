
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
namespace LFNet.QQ.Packets
{
    /// <summary>
    /// 所有输出包基类，这个基类定义了输出包的基本框架
    /// </summary>
    public abstract class OutPacket : Packet
    {
        /// <summary>
        /// 包起始序列号
        /// </summary>
        protected static char seq = (char)Util.Random.Next();
        /// <summary>
        /// 是否需要回应
        /// </summary>
        protected bool ack;
        /// <summary>
        /// 重发计数器
        /// </summary>
        protected int resendCountDown;
        /// <summary>
        /// 超时截止时间，单位ms
        /// </summary>
        public long TimeOut { get; set; }
        /// <summary>
        /// 发送次数，只在包是不需要ack时有效，比如logout包是发4次，但是其他可能只发一次
        /// </summary>
        public int SendCount { get; set; }
        /// <summary>
        /// 加密/解密密钥，只有有些包可能需要一个特定的密钥，如果为null，使用缺省的
        /// </summary>
        public byte[] Key { get; set; }

        
        /// <summary>创建一个基本输出包
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="command">包命令.</param>
        /// <param name="ack">包是否需要回复.</param>
        /// <param name="user">QQ用户对象.</param>
        public OutPacket(byte header, QQCommand command, bool ack, QQClient client)
            : base(header, QQGlobal.QQ_CLIENT_VERSION, command, GetNextSeq(), client)
        {
            this.ack = ack;
            this.resendCountDown = QQGlobal.QQ_SEND_TIME_NOACK_PACKET;
            this.SendCount = 1;
        }
        /// <summary>从buf中构造一个OutPacket，用于调试。这个buf里面可能包含了抓包软件抓来的数据
        /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="user">The user.</param>
        protected OutPacket(ByteBuffer buf, QQClient client) : base(buf, client) { }
        /// <summary>从buf中构造一个OutPacket，用于调试。这个buf里面可能包含了抓包软件抓来的数据
        /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="length">The length.</param>
        /// <param name="user">The user.</param>
        protected OutPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }

        /// <summary>
        /// 解析包体，从buf的开头位置解析起
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void ParseBody(ByteBuffer buf)
        {
        }

        /// <summary>
        /// 回填，有些字段必须填完整个包才能确定其内容，比如长度字段，那么这个方法将在
        /// 尾部填充之后调用
    /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="startPos">The start pos.</param>
        protected abstract void PostFill(ByteBuffer buf, int startPos);
        /// <summary>
        ///  将整个包转化为字节流, 并写入指定的ByteBuffer对象.
        ///  一般而言, 前后分别需要写入包头部和包尾部.
        /// </summary>
        /// <param name="buf">The buf.</param>
        public void Fill(ByteBuffer buf)
        {
            //保存当前pos
            int pos = buf.Position;
            // 填充头部
            PutHeader(buf);
            // 填充包体
            bodyBuf.Initialize();
            PutBody(bodyBuf);
            // 加密包体
            bodyDecrypted = bodyBuf.ToByteArray();
            byte[] enc = EncryptBody(bodyDecrypted, 0, bodyDecrypted.Length);
            // 加密内容写入最终buf
            buf.Put(enc);
            // 填充尾部
            PutTail(buf);
            // 回填
            PostFill(buf, pos);
//#if DEBUG
//            Client.LogManager.Log(ToString() + ":" + Utils.Util.ToHex(buf.ToByteArray()));
//#endif
        }
        protected static char GetNextSeq()
        {
            seq++;
            // 为了兼容iQQ
            // iQQ把序列号的高位都为0，如果为1，它可能会拒绝，wqfox称是因为TX是这样做的
            seq &= (char)0x7FFF;
            if (seq == 0)
            {
                seq++;
            }
            return seq;
        }
        /// <summary>
        /// 包的描述性名称
        /// </summary>
        /// <returns></returns>
        public override string GetPacketName()
        {
            return "Unknown Outcoming Packet";
        }
        /// <summary>
        /// 是否需要重发.
        /// </summary>
        /// <returns>需要重发返回true, 否则返回false.</returns>
        public bool NeedResend()
        {
            return (resendCountDown--) > 0;
        }
        /// <summary>
        /// 是否需要回复
    /// </summary>
        /// <returns>true表示包需要回复</returns>
        public bool NeedAck()
        {
            return ack;
        }

    }
}
