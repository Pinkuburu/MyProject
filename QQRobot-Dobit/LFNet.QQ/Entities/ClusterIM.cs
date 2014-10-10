
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
using System.Text.RegularExpressions;

namespace LFNet.QQ.Entities
{
    /// <summary> * 群消息的信息封装bean，具体内容可以参见ReceiveIMPacket
    ///* 
    ///* 关于自定义表情的格式，参见NormalIM注释
    /// </summary>
    public class ClusterIM
    {
        public QQClient QQClient;
        public RecvSource Source { get; set; }
        // 这个字段在收到临时群消息时表示父群ID，在固定群消息时表示群外部ID
        public int ExternalId { get; set; }
        public byte Type { get; set; }
        public int Sender { get; set; }
        public char Unknown1 { get; set; }
        public char Sequence { get; set; }
        public long SendTime { get; set; }
        public int VersionId { get; set; }
        public char ContentType { get; set; }
        public int FragmentSequence { get; set; }
        public int FragmentCount { get; set; }
        public int MessageId { get; set; }
        // 下面这些都是消息的属性，比如字体啦颜色啦，都是在fontAttribute里面的
        public bool hasFontAttribute { get; set; }
        public FontStyle FontStyle { get; set; }

        // 临时群内部ID，仅用于临时群消息时
        public int ClusterId { get; set; }

        // 消息内容，在解析的时候只用byte[]，正式要显示到界面上时才会转为String，上层程序
        // 要负责这个事，这个类只负责把内容读入byte[]
        public byte[] MessageBytes { get; set; }
        public string Message
        {
            get
            {
                if (Source==RecvSource.CLUSTER_09)
                {
                    return AnalyseMessage09(MessageBytes);
                }
                else
                    return Utils.Util.AnalyseMessage(MessageBytes);
            }
        }
        /// <summary>
        /// 纯文本消息
        /// </summary>
        public string TextMessage
        {
            get
            {
                return Regex.Replace(Message, @"(\[face(d*)\])|(<(.*) />)", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            }
        }
        // true表示这个消息中的自定义表情已经全部得到
        public bool FaceResolved { get; set; }
        public ClusterIM(QQClient qqClient, RecvSource source)
        {
            this.QQClient = qqClient;
            this.Source = source;
            FaceResolved = false;
            FontStyle = new FontStyle();
        }

        /// <summary>给定一个输入流，解析ClusterIM结构
    /// </summary>
        public void Read(ByteBuffer buf)
        {
//#if DEBUG
//            QQClient.LogManager.Log("ClusterIM Buf: " + Utils.Util.ToHex(buf.));
//#endif
            //if (Source == RecvSource.CLUSTER_09)
            //{
                buf.GetInt(); // 00 00 00 00 09
            //}

            // 群外部ID或者父群ID
            ExternalId = buf.GetInt();
            // 群类型，1字节
            Type = buf.Get();
            // 临时群内部ID
            if (Source == RecvSource.TEMP_CLUSTER)
                ClusterId = buf.GetInt();
            // 发送者
            Sender = buf.GetInt();
            // 未知1
            Unknown1 = buf.GetChar();
            // 消息序号
            Sequence = buf.GetChar();
            // 发送时间，记得乘1000才对
            SendTime = buf.GetInt() * 1000L;
            // Member Version ID
            VersionId = buf.GetInt();
            // 后面的内容长度
            buf.GetChar();
            // 一些扩展信息 
            if (Source != RecvSource.UNKNOWN_CLUSTER)
            {
                // content type
                ContentType = buf.GetChar();
                // 分片数
                FragmentCount = buf.Get() & 0xFF;
                // 分片序号
                FragmentSequence = buf.Get() & 0xFF;
                // 2字节未知
                MessageId = (int)buf.GetChar();
                // 4字节未知
                buf.GetInt();
            }
            if (Source == RecvSource.CLUSTER_09)
            {
                #region 字体属性开始 未处理
                buf.Position += 8;//'M' 'S' 'G' 00 00 00 00 00
                buf.GetInt();//send time
                buf.Position += 12;//5D 69 71 DE 00 80 80 00 0A 00 86 00  参见sendim
                int len = buf.GetUShort();
                buf.GetByteArray(len);//字体 E5 AE 8B E4 BD 93 =宋体
                #endregion
                buf.GetUShort();//00 00
                //IsNormalIM09 = true;//标注09的信息
                MessageBytes = buf.GetByteArray(buf.Remaining());
            }
            else
            {
                // 消息正文，只有最后一个分片有字体属性
                int remain = buf.Remaining();
                int fontAttributeLength = (FragmentSequence == FragmentCount - 1) ? (buf.Get(buf.Position + remain - 1) & 0xFF) : 0;
                MessageBytes = buf.GetByteArray(remain - fontAttributeLength);
                // 只有最后一个分片有字体属性
                hasFontAttribute = FragmentSequence == FragmentCount - 1;
                // 这后面都是字体属性，这个和SendIMPacket里面的是一样的
                if (hasFontAttribute)
                    FontStyle.Read(buf);
            }
        }

        public string AnalyseMessage09(byte[] buffer)
        {
            ByteBuffer buf = new ByteBuffer(buffer);
            string Msg = "";
            while (buf.HasRemaining())
            {
                byte type = buf.Get();
                int len = buf.GetUShort();
                switch (type)
                {
                    case 0x01://pure text
                        //len_str = buf.GetUShort();
                        Msg += new NormalIMText(QQClient, buf.GetByteArray(len)).ToString();
                        break;
                    case 0x02://face
                        Msg += new NormalIMFace(QQClient, buf.GetByteArray(len)).ToString();
                        break;
                    case 0x06://image
                        Msg += new NormalIMImage(QQClient, buf.GetByteArray(len)).ToString();
                        break;
                    default:
                        QQClient.LogManager.Log(ToString() + " Class Parse Unknown Type=0x" + type.ToString("X") + " Data=" + Utils.Util.ToHex(buf.GetByteArray(len)));
                        break;
                }

            }
            return Msg;
        }
    }
}
