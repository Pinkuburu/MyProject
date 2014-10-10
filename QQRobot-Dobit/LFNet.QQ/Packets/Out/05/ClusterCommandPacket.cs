
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
    public class ClusterCommandPacket : BasicOutPacket
    {
        public ClusterCommand SubCommand { get; set; }
        public int ClusterId { get; set; }

        /** 字体属性 */
        protected const byte NONE = 0x00;
        /// <summary>
        /// 
        /// </summary>
        protected const byte BOLD = 0x20;
        /// <summary>
        /// 
        /// </summary>
        protected const byte ITALIC = 0x40;
        /// <summary>
        /// 
        /// </summary>
        protected const byte UNDERLINE = (byte)0x80;

        public ClusterCommandPacket(QQClient client) : base(QQCommand.Cluster_Cmd,true,client) { }
        public ClusterCommandPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        protected override void ParseBody(ByteBuffer buf)
        {
            SubCommand = (ClusterCommand)buf.Get();
        }
        public override string GetPacketName()
        {
            switch (SubCommand)
            {
                case ClusterCommand.CREATE_CLUSTER:
                    return "Cluster Create Packet";
                case ClusterCommand.MODIFY_MEMBER:
                    return "Cluster Modify Member Packet";
                case ClusterCommand.MODIFY_CLUSTER_INFO:
                    return "Cluster Modify Info Packet";
                case ClusterCommand.GET_CLUSTER_INFO:
                    return "Cluster Get Info Packet";
                case ClusterCommand.ACTIVATE_CLUSTER:
                    return "Cluster Activate Packet";
                case ClusterCommand.SEARCH_CLUSTER:
                    return "Cluster Search Packet";
                case ClusterCommand.JOIN_CLUSTER:
                    return "Cluster Join Packet";
                case ClusterCommand.JOIN_CLUSTER_AUTH:
                    return "Cluster Auth Packet";
                case ClusterCommand.EXIT_CLUSTER:
                    return "Cluster Exit Packet";
                case ClusterCommand.GET_ONLINE_MEMBER:
                    return "Cluster Get Online Member Packet";
                case ClusterCommand.GET_MEMBER_INFO:
                    return "Cluster Get Member Info Packet";

                case ClusterCommand.SEND_IM_EX:
                case ClusterCommand.SEND_IM_EX09:
                    return "Cluster Send IM Ex Packet";
                case ClusterCommand.CREATE_TEMP:
                    return "Cluster Create Temp Cluster Packet";
                case ClusterCommand.MODIFY_TEMP_MEMBER:
                    return "Cluster Modify Temp Cluster Member Packet";
                case ClusterCommand.EXIT_TEMP:
                    return "Cluster Exit Temp Cluster Packet";
                case ClusterCommand.GET_TEMP_INFO:
                    return "Cluster Get Temp Cluster Info Packet";
                case ClusterCommand.ACTIVATE_TEMP:
                    return "Cluster Get Temp Cluster Member Packet";
                case ClusterCommand.MODIFY_CARD:

                case ClusterCommand.GET_CARD_BATCH:

                case ClusterCommand.GET_CARD:

                case ClusterCommand.COMMIT_ORGANIZATION:

                case ClusterCommand.UPDATE_ORGANIZATION:

                case ClusterCommand.COMMIT_MEMBER_ORGANIZATION:

                case ClusterCommand.GET_VERSION_ID:

                case ClusterCommand.SET_ROLE:

                case ClusterCommand.TRANSFER_ROLE:

                case ClusterCommand.DISMISS_CLUSTER:

                case ClusterCommand.MODIFY_TEMP_INFO:

                case ClusterCommand.SEND_TEMP_IM:

                case ClusterCommand.SUB_CLUSTER_OP:

                default:
                    return "Unknown Cluster Command Packet";
            }
        }
        protected override void PutBody(ByteBuffer buf)
        {
            
        }
    }
}
