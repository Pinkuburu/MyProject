
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

namespace LFNet.QQ.Packets.Out
{
    /// <summary>
    ///  * 发送临时群消息
    /// * 1. 头部
    /// * 2. 命令类型，1字节，0x35
    /// * 3. 群类型，1字节
    /// * 4. 父群内部ID，4字节
    /// * 5. 群内部ID，4字节
    /// * 6. 后面的数据的总长度，2字节
    /// * 7. Content Type, 2字节，0x0001表示纯文件，0x0002表示有自定义表情
    /// * 8. 消息分片数，1字节
    /// * 9. 分片序号，1字节，从0开始
    /// * 11. 消息id，2字节，同一条消息的不同分片id相同
    /// * 12. 4字节，未知
    /// * 13. 消息内容，最后一个分片追加空格
    /// * Note: 结尾处的空格是必须的，如果不追加空格，会导致有些缺省表情显示为乱码
    /// * 14. 消息的尾部，包含一些消息的参数，比如字体颜色啦，等等等等，顺序是
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
    /// * 15. 1字节，表示14和15部分的字节长度
    /// * 16. 尾部 
    /// * 
    /// * 注意：只有最后一个分片有14, 15, 16部分
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class ClusterSendTempIMPacket : ClusterSendIMExPacket
    {
        public ClusterType Type { get; set; }
        public int ParentClusterId { get; set; }
        public ClusterSendTempIMPacket(QQClient client)
            : base(client)
        {
            SubCommand = ClusterCommand.SEND_TEMP_IM;
        }
        public ClusterSendTempIMPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Cluster Send Temp Cluster IM Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            // 命令类型
            buf.Put((byte)SubCommand);
            // 群类型
            buf.Put((byte)Type);
            // 父群ID
            buf.PutInt(ParentClusterId);
            // 群内部ID
            buf.PutInt(ClusterId);
            // 后面数据的长度，这个长度需要根据消息长度和字体名称长度计算才能知道，
            // 所以先来产生消息和字体名称字节数组，先占个位置		
            int pos = buf.Position;
            buf.PutChar((char)0);
            // 未知2字节
            buf.PutChar((char)1);
            // 分片数
            buf.Put((byte)TotalFragments);
            // 分片序号
            buf.Put((byte)FragmentSequence);
            // 消息id
            buf.PutUShort(MessageId);
            // 未知4字节
            buf.PutInt(0);
            // 以0结束的消息，首先我们要根据用户设置的message，解析出一个网络可发送的格式
            //    这一步比较麻烦，暂时想不到好的办法
            byte[] msgBytes = null;
            int j, i = 0;
            while ((j = Message.IndexOf((char)FaceType.DEFAULT, i)) != -1)
            {
                String sub = Message.Substring(i, j);
                if (!sub.Equals(""))
                {
                    msgBytes = Utils.Util.GetBytes(sub);
                    buf.Put(msgBytes);
                }
                buf.Put((byte)FaceType.DEFAULT);
                buf.Put((byte)(Message[j + 1] & 0xFF));
                i = j + 2;
            }
            if (i < Message.Length)
            {
                String sub = Message.Substring(i);
                msgBytes = Utils.Util.GetBytes(sub);
                buf.Put(msgBytes);
            }
            // 只有最后一个分片有空格和字体属性
            if (FragmentSequence == TotalFragments - 1)
            {
                buf.Put((byte)0x20);
                FontStyle.Write(buf);
            }
            // 写入长度
            int cur = buf.Position;
            buf.Position = pos;
            buf.PutChar((char)(cur - pos - 2));
            buf.Position = cur;
        }
    }
}
