﻿
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
    /// <summary> * 提交成员分组情况到服务器
    /// * 1. 头部
    /// * 2. 命令，1字节，0x13
    /// * 3. 群内部id，4字节
    /// * 4. 未知1字节，0x00
    /// * 5. 成员QQ号，4字节
    /// * 6. 成员所属组织序号，1字节，没有组织时是0x00
    /// * 7. 如果有更多成员，重复5-6部分
    /// * 8. 尾部
    /// * 
    /// * 注意：不需要一次提交所有成员分组情况，如果只有个别成员的分组变动了（比如拖动操作），
    /// * 那么只需要提交改变的成员。所以这个操作不象修改临时群成员那样，又有添加又有删除的，
    /// * 可以一个包搞定了
    /// 	<remark>abu 2008-02-28 </remark>
    /// </summary>
    public class ClusterCommitMemberOrganizationPacket : ClusterCommandPacket
    {
        public List<Member> Members { get; set; }
        public ClusterCommitMemberOrganizationPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public ClusterCommitMemberOrganizationPacket(QQClient client)
            : base(client)
        {
            SubCommand = ClusterCommand.COMMIT_MEMBER_ORGANIZATION;
        }
        public override string GetPacketName()
        {
            return "Cluster Commit Member Organization Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put((byte)SubCommand);
            buf.PutInt(ClusterId);
            buf.Put((byte)0);
            foreach (Member m in Members)
            {
                buf.PutInt(m.QQ);
                buf.Put((byte)m.Organization);
            }
        }
    }
}
