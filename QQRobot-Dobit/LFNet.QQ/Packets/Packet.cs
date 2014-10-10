
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
    /// QQ所有包对象的基类
    /// </summary>
    public abstract class Packet
    {
        /// <summary>输入包使用的连接名称
    /// </summary>
        /// <value></value>
        public string PortName { get; set; }

        /// <summary>
        /// 加密解密对象
        /// </summary>
        protected static Crypter crypter = new Crypter();

        /// <summary>
        /// 包体缓冲区，有back array，用来存放未加密时的包体，子类应该在putBody方法中
        /// 使用这个缓冲区。使用之前先执行clear() 
        /// </summary>
        protected static ByteBuffer bodyBuf = new ByteBuffer();


        /// <summary>
        /// 包命令, 0x03~0x04.
    /// </summary>
        /// <value></value>
        public QQCommand Command { get; set; }

        /// <summary>源标志, 0x01~0x02.
    /// </summary>
        /// <value></value>
        public char Source { get; set; }

        /// <summary>包序号, 0x05~0x06.
    /// </summary>
        /// <value></value>
        public char Sequence { get; set; }
        /// <summary>
        /// 包头字节 
    /// </summary>
        /// <value></value>
        public byte Header { get; set; }
        /// <summary>
        /// true表示这个包是一个重复包，重复包本来是不需要处理的，但是由于LumaQQ较常发生
        ///  消息确认包丢失的问题，所以，这里加一个字段来表示到来的消息包是重复的。目前这个
        ///  字段只对消息有效，姑且算个解决办法吧，虽然不是太好看
    /// </summary>
        /// <value></value>
        public bool IsDuplicated { get; set; }

        /// <summary>QQUser
        /// 为了支持一个JVM中创建多个QQClient，包中需要保持一个QQUser的引用以
        /// 确定包的用户相关字段如何填写
        /// dobit 修正
        /// </summary>
        /// <value></value>
        protected QQUser user {
            get { return Client.QQUser; }
        }
        //protected QQUser user;
        /// <summary>
        /// 一个client应该对应一个QQ所以发送的包应该对应一个<see cref="T:LFNet.QQ.QQClient"/>QQClient</see>
        /// dobit 2009-7-20
        /// </summary>
        protected QQClient Client;
        /// <summary>
        /// 明文包体
        /// </summary>
        protected byte[] bodyDecrypted;

        /// <summary>构造一个指定参数的包
    /// </summary>
        /// <param name="header">包头</param>
        /// <param name="source">包源</param>
        /// <param name="command">包命令 </param>
        /// <param name="sequence">包序号 </param>
        /// <param name="user">QQ用户对象</param>
        public Packet(byte header, char source, QQCommand command, char sequence, QQClient client)
        {
            //this.user = user;
            this.Client = client;
            this.Source = source;
            this.Command = command;
            this.Sequence = sequence;
            this.IsDuplicated = false;
            this.Header = header;
            this.DateTime = DateTime.Now;
        }
        /// <summary>从buf中构造一个OutPacket，用于调试。这个buf里面可能包含了抓包软件抓来的数据
    /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="user">The user.</param>
        protected Packet(ByteBuffer buf, QQClient client)
            : this(buf, buf.Length - buf.Position, client)
        {
        }
        /// <summary>从buf中构造一个OutPacket，用于调试。这个buf里面可能包含了抓包软件抓来的数据
    /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="length">要解析的内容长度</param>
        /// <param name="user">The user.</param>
        protected Packet(ByteBuffer buf, int length, QQClient client)
        {
            this.Client = client;
            ParseHeader(buf);
            if (!ValidateHeader())
                throw new PacketParseException("包头有误，抛弃该包: " + ToString());
            // 得到包体
            byte[] body = GetBodyBytes(buf, length);
            bodyDecrypted = DecryptBody(body, 0, body.Length);
            if (bodyDecrypted == null)
                throw new PacketParseException("包内容解析出错，抛弃该包: " + ToString());
            // 包装到ByteBuffer
            ByteBuffer tempBuf = new ByteBuffer(bodyDecrypted);
            try
            {
                ParseBody(tempBuf);
            }
            catch (Exception e)
            {
                throw new PacketParseException(e.Message, e);
            }
            ParseTail(buf);
            this.DateTime = DateTime.Now;
        }

        /// <summary> 构造一个包对象，什么字段也不填，仅限于子类使用
    /// </summary>
        protected Packet()
        {
            this.DateTime = DateTime.Now;
        }
        /// <summary>得到UDP形式包的总长度，不考虑TCP形式
    /// </summary>
        /// <param name="bodyLength">包体长度.</param>
        /// <returns>包长度</returns>
        protected abstract int GetLength(int bodyLength);
        /// <summary>从buf的当前未知解析包尾
    /// </summary>
        /// <param name="buf">The buf.</param>
        protected abstract void ParseTail(ByteBuffer buf);

        /// <summary>
        /// 解析包体，从buf的开头位置解析起
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected abstract void ParseBody(ByteBuffer buf);
        /// <summary>解密包体
    /// </summary>
        /// <param name="body">包体字节数组.</param>
        /// <param name="offset">包体开始偏移.</param>
        /// <param name="length">包体长度.</param>
        /// <returns>解密的包体字节数组</returns>
        protected abstract byte[] DecryptBody(byte[] body, int offset, int length);
        /// <summary>得到包体的字节数组
    /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="length">包总长度</param>
        /// <returns>包体字节数组</returns>
        protected abstract byte[] GetBodyBytes(ByteBuffer buf, int length);
        /// <summary>校验头部
    /// </summary>
        /// <returns></returns>
        protected abstract bool ValidateHeader();
        /// <summary>从buf的当前位置解析包头
    /// </summary>
        /// <param name="buf">The buf.</param>
        protected abstract void ParseHeader(ByteBuffer buf);
        /// <summary>包头长度
    /// </summary>
        /// <returns>包头长度</returns>
        protected abstract int GetHeaderLength();
        /// <summary>
        /// 包尾长度
    /// </summary>
        /// <returns>包尾长度</returns>
        protected abstract int GetTailLength();
        /// <summary>
        /// 将包头部转化为字节流, 写入指定的ByteBuffer对象.
    /// </summary>
        /// <param name="buf">The buf.</param>
        protected abstract void PutHeader(ByteBuffer buf);
        /// <summary>初始化包体
    /// </summary>
        /// <param name="buf">The buf.</param>
        protected abstract void PutBody(ByteBuffer buf);
        /// <summary>
        /// 标识这个包属于哪个协议族
        /// </summary>
        /// <returns></returns>
        public abstract ProtocolFamily GetFamily();
        /// <summary>
        /// 将包尾部转化为字节流, 写入指定的ByteBuffer对象.
    /// </summary>
        /// <param name="buf">The buf.</param>
        protected abstract void PutTail(ByteBuffer buf);
        /// <summary>
        /// 加密包体
    /// </summary>
        /// <param name="buf">未加密的字节数组.</param>
        /// <param name="offset">包体开始的偏移.</param>
        /// <param name="length">包体长度.</param>
        /// <returns>加密的包体</returns>
        protected abstract byte[] EncryptBody(byte[] buf, int offset, int length);

        /// <summary>
        /// 密文的起始位置，这个位置是相对于包体的第一个字节来说的，如果这个包是未知包，
        /// 返回-1，这个方法只对某些协议族有意义
    /// </summary>
        /// <returns></returns>
        protected abstract int GetCryptographStart();

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
        public override bool Equals(object obj)
        {
            if (obj is Packet)
            {
                Packet packet = (Packet)obj;
                return Header == packet.Header && Command == packet.Command && Sequence == packet.Sequence;
            }
            return base.Equals(obj);
        }
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return Hash(Sequence, Command);
        }


        /// <summary>
        /// 得到hash值
    /// </summary>
        /// <param name="sequence">The sequence.</param>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public static int Hash(char sequence, QQCommand command)
        {
            return (sequence << 16) | (ushort)command;
        }
        /// <summary>包的描述性名称
    /// </summary>
        /// <returns></returns>
        public virtual string GetPacketName()
        {
            return "Unknown Packet";
        }

        /// <summary>
        /// 包的接收时间或发送时间
    /// </summary>
        /// <value></value>
        public DateTime DateTime { get;  set; }
    }
}
