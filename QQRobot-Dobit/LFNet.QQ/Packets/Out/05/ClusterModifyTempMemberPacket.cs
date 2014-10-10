
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
    ///  * 修改临时群成员列表：
    /// * 1. 头部
    /// * 2. 群命令类型，1字节，0x31
    /// * 3. 临时群类型，1字节，0x01是多人对话，0x02是讨论组
    /// * 4. 父群内部ID，4字节
    /// * 5. 临时群内部ID，4字节
    /// * 6. 操作类型，0x01是添加，0x02是删除，常量定义在QQ.java中
    /// * 7. 要操作的QQ号，4字节
    /// * 8. 如果有更多成员要操作，重复7部分
    /// * 9. 尾部
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class ClusterModifyTempMemberPacket : ClusterCommandPacket
    {
        public ClusterType Type { get; set; }
        public int ParentClusterId { get; set; }
        public byte Operation { get; set; }
        public List<int> Members { get; set; }
        public ClusterModifyTempMemberPacket(QQClient client)
            : base(client)
        {
            SubCommand = ClusterCommand.MODIFY_TEMP_MEMBER;
        }
        public ClusterModifyTempMemberPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Modify Temp Cluster Member Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            // 命令类型
            buf.Put((byte)SubCommand);
            // 临时群类型
            buf.Put((byte)Type);
            // 父群ID
            buf.PutInt(ParentClusterId);
            // 临时群ID
            buf.PutInt(ClusterId);
            // 操作方式
            buf.Put(Operation);
            // 成员QQ号
            foreach (int i in Members)
                buf.PutInt(i);
        }
    }
}
