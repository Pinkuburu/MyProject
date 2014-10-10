
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

namespace LFNet.QQ.Packets
{
    /// <summary>
    /// 包解析器
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// 判断此parser是否可以处理这个包，判断不能影响到buf的指针位置
    /// </summary>
        /// <param name="buf">The buf.</param>
        /// <returns>true表示这个parser可以处理这个包</returns>
        bool Accept(ByteBuffer buf);
        /// <summary>包的总长度
    /// </summary>
        /// <param name="buf">The buf.</param>
        /// <returns>包的总长度</returns>
        int GetLength(ByteBuffer buf);
        /// <summary>从buf当前位置解析出一个输入包对象，解析完毕后指针位于length之后
    /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="length">包长度.</param>
        /// <param name="user">The user.</param>
        /// <returns>InPacket子类，如果解析不了返回null</returns>
        InPacket ParseIncoming(ByteBuffer buf, int length, QQClient client);
        /// <summary>从buf当前位置解析出一个输出包对象，解析完毕后指针位于length之后
    /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="length">包长度.</param>
        /// <param name="user">QQ用户对象.</param>
        /// <returns>OutPacket子类，如果解析不了，返回null</returns>
        OutPacket ParseOutcoming(ByteBuffer buf, int length, QQClient client);
        /// <summary>
        /// 检查这个输入包是否重复
    /// </summary>
        /// <param name="packet">The packet.</param>
        /// <returns>true表示重复</returns>
        bool IsDuplicate(InPacket packet);
        /// <summary>检查这个包是重复包是否也要回复
    /// </summary>
        /// <param name="packet">The packet.</param>
        /// <returns>true表示即使这个包是重复包也要回复</returns>
        bool IsDuplicatedNeedReply(InPacket packet);
        /// <summary>假设buf的当前位置处是一个包，返回下一个包的起始位置。这个方法
        /// 用来重新调整buf指针。如果无法重定位，返回当前位置
    /// </summary>
        /// <param name="buf">The buf.</param>
        /// <returns>下一个包的起始位置</returns>
        int Relocate(ByteBuffer buf);

        /// <summary>
        /// PacketHistory类
    /// </summary>
        /// <returns></returns>
        PacketHistory GetHistory();
    }
}
