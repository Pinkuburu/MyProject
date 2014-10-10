
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
    ///  * 获取在线好友列表的请求包，格式为
    /// * 1. 头部
    /// * 2. 1个字节，只有值为0x02或者0x03时服务器才有反应，不然都是返回0xFF
    /// *    经过初步的试验，发现3得到的好友都是一些系统服务，号码比如72000001到72000013，
    /// *    就是那些移动QQ，会员服务之类的；而2是用来得到好友的
    /// * 3. 起始位置，4字节。这个起始位置的含义与得到好友列表中的字段完全不同。估计是两拨人
    /// *    设计的，-_-!...
    /// *    这个起始位置需要有回复包得到，我们已经知道，在线好友的回复包一次最多返回30个好友，
    /// *    那么如果你的在线好友超过30个，就需要计算这个值。第一个请求包，这个字段肯定是0，后面
    /// *    的请求包和前一个回复包就是相关的了。具体的规则是这样的，在前一个回复包中的30个好友里面，
    /// *    找到QQ号最大的那个，然后把他的QQ号加1，就是下一个请求包的起始位置了！
    /// * 6. 尾部
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class GetOnlineOpPacket : BasicOutPacket
    {
        public int StartPosition { get; set; }
        public GetOnlineSubCmd SubCommand { get; set; }
        public GetOnlineOpPacket(QQClient client)
            : base(QQCommand.Get_Online_OP,true,client)
        {
            StartPosition = QQGlobal.QQ_POSITION_ONLINE_LIST_START;
            SubCommand = GetOnlineSubCmd.GET_ONLINE_FRIEND;
        }
        public GetOnlineOpPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Get Friend Online Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put((byte)SubCommand);
            buf.PutInt(StartPosition);
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
        }
    }
}
