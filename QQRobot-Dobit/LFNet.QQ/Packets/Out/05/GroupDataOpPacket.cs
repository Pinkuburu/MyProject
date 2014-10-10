
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
    ///  * 上传下载分组名字的消息包，格式为
    /// * 1. 头部
    /// * 2. 操作方式字节，如果为0x2，为上传组名，如果为0x1，为请求下载组名
    /// *    如果为0x2，后面的部分为
    /// * 	  i.   组序号，qq缺省的组，比如我的好友，序号是0，其他我们自己添加的组，从1开始，一个字节。
    /// *         但是要注意的是，这里不包括我的好友组，因为我的好友组是QQ的缺省组，无需上传名称
    /// *    ii.  16个字节的组名，如果组名长度少于16个字节，后面的填0。之所以是16个，是因为QQ的组名长度最多8个汉字
    /// *    iii. 如果有更多组，重复i，ii部分
    /// *    如果为0x1，后面的部分为
    /// *    i.   未知字节0x2
    /// *    ii.  4个未知字节，全0 
    /// * 3. 尾部
    /// * 
    /// * 这个包没有限制添加的组名叫什么，也没有明确规定第一个组必须是
    /// * 我的好友组，这些规范需要在上层程序中实现。当然也可以不一定非要第一个组是
    /// * 我的好友组，这些客户端的trick随便你怎么搞
    /// * 
    /// * 每次上传都必须上传所有组名
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class GroupDataOpPacket : BasicOutPacket
    {
        public List<string> Groups { get; set; }
        public GroupSubCmd Type { get; set; }
        public GroupDataOpPacket(QQClient client)
            : base(QQCommand.Group_Data_OP_05,true,client)
        {
            Type = GroupSubCmd.UPLOAD;
            Groups = new List<string>();
        }
        public GroupDataOpPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            switch (Type)
            {
                case GroupSubCmd.DOWNLOAD:
                    return "Group Data Packet - Download Group";
                case GroupSubCmd.UPLOAD:
                    return "Group Data Packet - Upload Group";
                default:
                    return "Group Data Packet - Unknown Sub Command";
            }
        }
        protected override void PutBody(ByteBuffer buf)
        {
            // 上传操作标志字节
            buf.Put((byte)Type);
            if (Type == GroupSubCmd.UPLOAD)
            {
                // 循环写入各个组
                int size = Groups.Count;
                for (int i = 0; i < size; i++)
                {
                    String name = Groups[i];
                    // 组序号
                    buf.Put((byte)(i + 1));
                    // 组名称
                    byte[] nameBytes = Utils.Util.GetBytes(name);
                    // 超过最大长度的，截短；小于最大长度的，补0
                    if (nameBytes.Length > QQGlobal.QQ_MAX_GROUP_NAME)
                        buf.Put(nameBytes, 0, QQGlobal.QQ_MAX_GROUP_NAME);
                    else
                    {
                        buf.Put(nameBytes);
                        int j = QQGlobal.QQ_MAX_GROUP_NAME - nameBytes.Length;
                        while (j-- > 0)
                            buf.Put((byte)0);
                    }
                }
            }
            else
            {
                // 未知字节0x2
                buf.Put((byte)0x2);
                // 未知4字节，全0
                buf.PutInt(0);
            }
        }
    }
}
