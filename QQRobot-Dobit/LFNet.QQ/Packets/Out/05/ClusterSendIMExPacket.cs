
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
    /// * 扩展固定群消息发送包，相对于已经有的固定群消息发送包来说，这个多了一些标志，
    /// * 同时旧的群消息发送包为了兼容性的考虑TX依然保留：
    /// * 1.  头部
    /// * 2.  子命令，1字节，0x1A
    /// * 3.  群内部ID，4字节
    /// * 4.  后面的数据的总长度，2字节
    /// * 5.  Content Type, 2字节，0x0001表示纯文件，0x0002表示有自定义表情
    /// * 6.  消息分片数，1字节。群消息分片也是最大700字节，但是这个和普通消息不一样的是：这个700包含了这些控制信息
    /// * 7.  分片序号，1字节，从0开始
    /// * 8.  消息id，2字节，同一条消息的不同分片id相同
    /// * 9.  4字节，未知
    /// * 10. 消息内容，最后一个分片追加空格
    /// * Note: 结尾处的空格是必须的，如果不追加空格，会导致有些缺省表情显示为乱码
    /// * 11. 消息的尾部，包含一些消息的参数，比如字体颜色啦，等等等等，顺序是
    /// *     1. 字体修饰属性，bold，italic之类的，2字节，具体的设置是
    /// *         i.   bit0-bit4用来表示字体大小，所以最大是32
    /// *         ii.  bit5表示是否bold
    /// *         iii. bit6表示是否italic
    /// *         iv.  bit7表示是否underline
    /// *     2. 颜色Red，1字节
    /// *     3. 颜色Green，1字节
    /// *     4. 颜色Blue，1字节
    /// *     5. 1个未知字节，置0先
    /// *     6. 消息编码，2字节，0x8602为GB，0x0000为EN，其他未知，好像可以自定义，因为服务器好像不干涉
    /// *     7. 可变长度的一段信息，字体名后面跟一个回车符，比如0xcb, 0xce, 0xcc, 0xe5,表示宋体
    /// * 12. 1字节，表示11和12部分的字节长度
    /// * 13. 尾部
    /// * 
    /// * 注意：只有最后一个分片有11, 12部分
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class ClusterSendIMExPacket : ClusterCommandPacket
    {
        /// <summary>
        /// Gets or sets the font style.
        /// </summary>
        /// <value>The font style.</value>
        public FontStyle FontStyle { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }
        private byte[] messageBytes;
        /// <summary>
        /// 字节数据
        /// </summary>
        public byte[] MessageBytes
        {
            get
            {
                if (messageBytes != null)
                {
                    return messageBytes;
                }
                else
                {
                    return Utils.Util.GetBytes(Message);
                }
            }
            set { messageBytes = value; }
        }
        /// <summary>
        /// Gets or sets the total fragments.
        /// </summary>
        /// <value>The total fragments.</value>
        public int TotalFragments { get; set; }
        /// <summary>
        /// Gets or sets the fragment sequence.
        /// </summary>
        /// <value>The fragment sequence.</value>
        public int FragmentSequence { get; set; }
        /// <summary>
        /// Gets or sets the message id.
        /// </summary>
        /// <value>The message id.</value>
        public ushort MessageId { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ClusterSendIMExPacket"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        public ClusterSendIMExPacket(QQClient client)
            : base(client)
        {
            SubCommand = ClusterCommand.SEND_IM_EX09;
            TotalFragments = 1;
            FragmentSequence = 0;
            MessageId = (ushort)Utils.Util.Random.Next();
            FontStyle = new FontStyle();
            Message = string.Empty;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ClusterSendIMExPacket"/> class.
        /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="length">The length.</param>
        /// <param name="user">The user.</param>
        public ClusterSendIMExPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        /// <summary>
        /// Gets the name of the packet.
        /// </summary>
        /// <returns></returns>
        public override string GetPacketName()
        {
            return "Cluster Send IM Ex Packet";
        }
        /// <summary>
        /// Puts the body.
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void PutBody(ByteBuffer buf)
        {
            // 命令类型
            buf.Put((byte)SubCommand);
            // 群内部ID
            buf.PutInt(ClusterId);
            // 后面数据的长度，这个长度需要根据后面的长度计算才能知道，
            // 所以先占个位置		
            //int pos = buf.Position;
            //buf.PutChar((char)0);

            ByteBuffer MsgBuf = new ByteBuffer();

            // 未知的2字节
            MsgBuf.PutChar((char)1);
            // 分片数
            MsgBuf.Put((byte)TotalFragments);
            // 分片序号
            MsgBuf.Put((byte)FragmentSequence);
            if (MessageId == 0)
            {
                MessageId = (ushort)this.Sequence;
            }
            // 消息id
            MsgBuf.PutUShort(MessageId);
            // 未知4字节
            MsgBuf.PutInt(0);

            // 以0结束的消息，首先我们要根据用户设置的message，解析出一个网络可发送的格式
            // 这一步比较麻烦，暂时想不到好的办法
            MsgBuf.PutInt(0x4D534700);//"MSG"
            MsgBuf.PutInt(0);
            // 发送时间
            int time = (int)(Utils.Util.GetTimeMillis(DateTime.Now) / 1000);
            MsgBuf.PutInt(time);
            MsgBuf.PutInt((MessageId << 16) | MessageId);//maybe a random interger
            MsgBuf.PutInt(0);
            MsgBuf.PutInt(0x09008600);
            byte[] Font_Name = Utils.Util.GetBytes(FontStyle.FontName);
            MsgBuf.PutUShort((ushort)Font_Name.Length);
            MsgBuf.Put(Font_Name);
            MsgBuf.PutUShort(0);
            MsgBuf.Put(0x01);
            MsgBuf.PutUShort((ushort)(MessageBytes.Length + 3));
            MsgBuf.Put(1);
            MsgBuf.PutUShort((ushort)(MessageBytes.Length));

            // 写入消息正文字节数组
            if (MessageBytes != null)
                MsgBuf.Put(MessageBytes);
            //if (FragmentSequence == TotalFragments - 1)
            //{
            //    MsgBuf.Put((byte)0x20);
            //}
            byte[] MsgData = MsgBuf.ToByteArray();
            buf.PutUShort((ushort)MsgData.Length);
            buf.Put(MsgData);
            MsgData = null;
            MsgBuf = null;
            
            //byte[] msgBytes = null;
            //int j, i = 0;
            //while ((j = Message.IndexOf((char)FaceType.DEFAULT, i)) != -1)
            //{
            //    string sub = Message.Substring(i, j);
            //    if (!sub.Equals(""))
            //    {
            //        msgBytes = Utils.Util.GetBytes(sub);
            //        buf.Put(msgBytes);
            //    }
            //    buf.Put((byte)FaceType.DEFAULT);
            //    buf.Put((byte)(Message[j + 1] & 0xFF));
            //    i = j + 2;
            //}
            //if (i < Message.Length)
            //{
            //    string sub = Message.Substring(i);
            //    msgBytes = Utils.Util.GetBytes(sub);
            //    buf.Put(msgBytes);
            //}

            //// 只有最后一个分片有空格和字体属性
            //if (FragmentSequence == TotalFragments - 1)
            //{
            //    buf.Put((byte)0x20);
            //    FontStyle.Write(buf);
            //}
            // 写入长度
            //int cur = buf.Position;
            //buf.Position = pos;
            //buf.PutChar((char)(cur - pos - 2));
            //buf.Position = cur;
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
        }
    }
}
