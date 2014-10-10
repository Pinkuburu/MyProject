
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
    /// <summary>
    /// * 普通消息的本体，其在NormalIMHeader之后
    /// * 
    /// * 普通消息中可能内嵌一些图片信息，除了普通的文本之外，图片的信息格式为：
    /// * 一. 缺省表情，缺省表情的前导字节是0x14，0x14之后的一个字节表示缺省表情的索引值
    /// * 二. 自定义表情，自定义表情的前导字节是0x15，0x15之后的格式为:
    /// * 	  1. 存在性字节，如果这个表情第一次出现，则为0x33，如果已经出现过，则为0x34，当为0x33时，后面的内容是
    /// * 		 i.   扩展名长度，1字节，以'0'为基准，'2'则表示长度为3
    /// *       ii.  表情图片的文件名，其文件名由md5的字符串形式和扩展名构成，因此这个长度应该是32 + 1 + 3(一般是GIF)
    /// *       iii. 表情的shortcut长度，以'A'为基准，如果长度是2，则这个字节是'C'
    /// *       iv.  表情的shortcut
    /// *    2. 如果为0x34时，则后面的内容为：
    /// * 		 i.   1字节索引值，假如这个自定义表情出现在第一个位置，则这个字节为'A'   
    /// *    3. 如果为0x36时，群内自定义表情
    /// * 		 i. 自定义表情协议块的长度的10进制字符串形式，3字节，不足者前部填为空格，比如为了表示这个自定义表情用了
    /// *          88个字节，那么这个字段就是" 88"，呵呵，晕吧，注意这个长度是从0x15开始算起，一直到结束。注意这个长度
    /// *          是字节长度
    /// *       ii. 表情标识，1字节，标识这个表情是新的，还是已经出现过的，如果是新的，用'e'表示。如果是已经出现过的，
    /// *           用一个大写字母表示，第一个新表情代号是A，第二个是B，以此类推
    /// *       iii. 表情的快捷键字节长度，1字节，用一个大写字母表示，比如A表示长度为0，依次类推
    /// *       iv. 后面的内容开始一直到agent key之前的内容的长度，2字节，用16进制的字符串表示
    /// *       v. session id的16进制字符串形式，8字节，不足者前面是空格
    /// *       vi. 中转服务器IP的16进制字符串形式，注意是little-endian，那么ipv4的话自然就是8个字节了
    /// *       vii. 中转服务器端口号的16进制字符串形式，8个字节
    /// *       viii. file agent key，16字节
    /// *       ix. 图片的文件名，文件名的形式是MD5的字符串形式加上点加上后缀名而成，所以一般是36个字节，但是
    /// *           我想最好还是根据前面的长度减去其他字段的长度来判断好些
    /// *       x.  快捷键，长度前面已经说了
    /// *       xi. 一个字节，'A'，可能是用来分界用的
    /// *    4. 如果为0x37时，群内自定义表情
    /// *       0x37表示这个表情已经在前面出现过，参见0x36时的格式，0x37缺少0x36的iv, v, vii, viii, ix部分，
    /// *       其他部分均相同
    /// </summary>
    public class NormalIM 
    {
        public QQClient QQClient { get; set; }
        public bool HasFontAttribute { get; set; }
        public int TotalFragments { get; set; }
        public int FragmentSequence { get; set; }
        public int MessageId { get; set; }
        public ReplyType ReplyType { get; set; }
        public FontStyle FontStyle { get; set; }
        public bool IsNormalIM09 { get; set; }

        public NormalIM(QQClient qqClient)
        {
            QQClient = qqClient;
        }
        /// <summary>消息内容，在解析的时候只用byte[]，正式要显示到界面上时才会转为String，上层程序
        ///  要负责这个事，这个类只负责把内容读入byte[]
        /// </summary>
        /// <value></value>
        public byte[] MessageBytes { get; set; }
        /// <summary>
        /// 消息已经解析内部图片
        /// </summary>
        /// <value></value>
        public string Message
        {
            get
            {
                if (IsNormalIM09)
                {
                    return AnalyseMessage09(MessageBytes);
                }
                else
                    return Utils.Util.AnalyseMessage(MessageBytes);
            }
        }
        public string TextMessage
        {
            get {

                return Regex.Replace(Message, @"(\[face(d*)\])|(<(.*) />)", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            }
        }
        /// <summary>给定一个输入流，解析NormalIM结构
        /// </summary>
        /// <param name="buf">The buf.</param>
        public void Read(ByteBuffer buf)
        {
            FontStyle = new FontStyle();
            // 是否有字体属性
            HasFontAttribute = buf.GetInt() != 0;
            // 分片数
            TotalFragments = (int)buf.Get();
            // 分片序号
            FragmentSequence = (int)buf.Get();
            // 消息id 两个字节
            MessageId = (int)buf.GetUShort();
            // 消息类型，这里的类型表示是正常回复还是自动回复之类的信息
            ReplyType = (ReplyType)buf.Get();
            // 消息正文，长度=剩余字节数 - 包尾字体属性长度
            int remain = buf.Remaining();
            int fontAttributeLength = HasFontAttribute ? (buf.Get(buf.Position + remain - 1) & 0xFF) : 0;
            MessageBytes = buf.GetByteArray(remain - fontAttributeLength);
            // 这后面都是字体属性，这个和SendIMPacket里面的是一样的
            if (HasFontAttribute)
            {
                if (buf.HasRemaining())
                    FontStyle.Read(buf);
                else
                    HasFontAttribute = false;
            }

        }
        /// <summary>
        /// 分析09的流
        /// </summary>
        /// <param name="buf"></param>
        public void Read09(ByteBuffer buf)
        {
            FontStyle = new FontStyle();
            // 是否有字体属性
            HasFontAttribute = buf.GetInt() != 0;
            // 分片数
            TotalFragments = (int)buf.Get();
            // 分片序号
            FragmentSequence = (int)buf.Get();
            // 消息id 两个字节
            MessageId = (int)buf.GetUShort();
            // 消息类型，这里的类型表示是正常回复还是自动回复之类的信息
            ReplyType = (ReplyType)buf.Get();
            // 消息正文
            #region 字体属性开始 未处理
            buf.Position += 8;//'M' 'S' 'G' 00 00 00 00 00
            buf.GetInt();//send time
            buf.Position += 12;//5D 69 71 DE 00 80 80 00 0A 00 86 00  参见sendim
            int len = buf.GetUShort();
            buf.GetByteArray(len);//字体 E5 AE 8B E4 BD 93 =宋体
            #endregion
            buf.GetUShort();//00 00
            IsNormalIM09 = true;//标注09的信息
            MessageBytes = buf.GetByteArray(buf.Remaining());
            //_Message = "";
            //while (buf.HasRemaining())
            //{
            //    byte type = buf.Get();
            //    len = buf.GetUShort();
            //    switch (type)
            //    {
            //        case 0x01://pure text
            //            //len_str = buf.GetUShort();
            //            _Message +=new NormalIMText(QQClient,buf.GetByteArray(len)).ToString();
            //            break;
            //        case 0x02://face
            //            _Message += new NormalIMFace(QQClient, buf.GetByteArray(len)).ToString();
            //            break;
            //        case 0x06://image
            //            _Message += new NormalIMImage(QQClient, buf.GetByteArray(len)).ToString();
            //            break;
            //        default:
            //            QQClient.LogManager.Log(ToString() + " Class Parse Unknown Type=0x" + type.ToString("X") + " Data=" + Utils.Util.ToHex(buf.GetByteArray(len)));
            //            break;


            //    }

            //}
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
