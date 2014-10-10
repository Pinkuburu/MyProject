
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
namespace LFNet.QQ.Packets.In
{
    /// <summary>
    ///  * 系统消息包，系统消息和ReceiveIMPacket里面的系统通知有什么区别呢？
    /// * 系统消息是表示你被别人加为好友了之类的消息，所以有源有目的，其他人
    /// * 收不到的，系统通知是系统发给大家的消息。好了，废话这么多，系统消息的
    /// * 格式是:
    /// * 1. 头部，头部说明了系统消息的类型，目前已知的有四种
    /// * 2. 对于一般的系统通知，其格式为:
    /// * 	  以0x1F相隔的多个字段，对于已知的类型，分别是消息类型，源，目的，附加内容，对于未知的
    /// *    消息类型，前面三个是一样的，后面的就未知了
    /// *    
    /// *    对于QQ_SYS_BEING_ADDED_EX，消息正文的格式为：
    /// *    i. 后面内容的字节长度
    /// *    ii. 未知内容，不知用什么才能解密
    /// *    
    /// *    对于QQ_SYS_ADD_FRIEND_REQUEST_EX，其消息正文的格式为
    /// *    i. 消息正文字节长度
    /// *    ii. 消息正文
    /// *    iii. 是否允许对方加自己为好友，0x01表示允许，0x02表示不允许
    /// *    
    /// *    对于QQ_SYS_ADD_FRIEND_APPROVED_AND_ADD，其消息正文的格式为
    /// *    i. 未知的1字节，0x00
    /// * 3. 尾部
    /// * </pre>
    /// *
    /// * Note: 只有使用2005的0x00A8发送认证消息，才会收到QQ_SYS_ADD_FRIEND_REQUEST_EX消息
    /// 	<remark>abu 2008-02-26 </remark>
    /// </summary>
    public class SystemNotificationPacket : BasicInPacket
    {
        // 分隔符
        static string DIVIDER = System.Text.Encoding.Default.GetString(new byte[] { (byte)0x1F });
        /// <summary>
        /// 消息类型
        /// 	<remark>abu 2008-02-26 </remark>
        /// </summary>
        /// <value></value>
        public SystemMessageType Type { get; set; }
        /// <summary>
        /// 从哪里来，是源的QQ号
        /// 	<remark>abu 2008-02-26 </remark>
        /// </summary>
        /// <value></value>
        public int From { get; set; }
        /// <summary>
        /// 到哪里去，目的的QQ号
        /// 	<remark>abu 2008-02-26 </remark>
        /// </summary>
        /// <value></value>
        public int To { get; set; }
        /// <summary>
        /// 附加的消息，比如如果别人拒绝了你加他为好友，并说了理由，那就在这里了
        /// 	<remark>abu 2008-02-26 </remark>
        /// </summary>
        /// <value></value>
        public string Message { get; set; }
        /// <summary>
        /// only for QQ_SYS_ADD_FRIEND_REQUEST_EX
        /// </summary>
        /// <value></value>
        public RevenseAdd ReverseAdd { get; set; }

        public SystemNotificationPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "System Notification Packet";
        }
        protected override void ParseBody(ByteBuffer buf)
        {
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif

            byte[] b = buf.ToByteArray();
            String s = null;

            s = Utils.Util.GetString(b);

            String[] fields = s.Split(DIVIDER.ToCharArray());
            Type = (SystemMessageType)Utils.Util.GetInt(fields[0], 0);
            From = Utils.Util.GetInt(fields[1], 0);
            To = Utils.Util.GetInt(fields[2], 0);
            if (fields.Length > 3)
            {
                switch (Type)
                {
                    case SystemMessageType.ADD_FRIEND_REQUEST_EX:
                        byte[] fByte = Utils.Util.GetBytes(fields[3]);
                        int len = fByte[0] & 0xFF;
                        Message = Utils.Util.GetString(fByte, 1, len);
                        ReverseAdd = (RevenseAdd)fByte[len + 1];//后面还有个token
                        //int 
                        //ReverseAdd = (RevenseAdd)fByte[fByte.Length - 1];
                        break;
                    case SystemMessageType.BEING_ADDED_EX:
                    case SystemMessageType.ADD_FRIEND_APPROVED_AND_ADD:
                        Message = "";
                        break;
                    default:
                        Message = fields[3];
                        break;
                }
            }
            else
                Message = "";
            if (From == 0 || To == 0)
                throw new PacketParseException("系统通知字段解析出错");
        }
    }
}
