
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
    /// <summary>
    ///  * 上传下载好友备注的消息包，格式为
    /// * 1. 头部
    /// * 2. 子命令，1字节
    /// * 3. 页号，1字节，从1开始，如果为0，表示此字段未用
    /// * 4. 操作对象的QQ号，4字节
    /// * 5. 未知1字节，0x00
    /// * 6. 以下为备注信息，一共7个域，域的顺序依次次是
    /// *    姓名、手机、电话、地址、邮箱、邮编、备注
    /// *    每个域都有一个前导字节，这个字节表示了这个域的字节长度
    /// * 7. 尾部
    /// * 
    /// * Note: 如果子命令是0x00(批量下载备注)，只有2，3部分
    /// * 		 如果子命令是0x01(上传备注)，所有部分都要，3部分未用
    /// *       如果子命令是0x02(删除好友)，仅保留1,2,4,7部分
    /// *       如果子命令是0x03(下载备注)，仅保留1,2,4,7部分
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class FriendDataOpPacket : BasicOutPacket
    {
        /// <summary>操作类型，上传还是下载
        /// 	<remark>abu 2008-02-29 </remark>
        /// </summary>
        /// <value></value>
        public FriendOpSubCmd SubCommand { get; set; }
        /// <summary> 操作的对象的QQ号
        /// 	<remark>abu 2008-02-29 </remark>
        /// </summary>
        /// <value></value>
        public int QQ { get; set; }
        /// <summary>好友备注对象
        /// 	<remark>abu 2008-02-29 </remark>
        /// </summary>
        /// <value></value>
        public FriendRemark Remark { get; set; }
        /// <summary>页号
        /// 	<remark>abu 2008-02-29 </remark>
        /// </summary>
        /// <value></value>
        public int Page { get; set; }

        public FriendDataOpPacket(QQClient client)
            : base(QQCommand.Friend_Data_OP_05,true,client)
        {
            SubCommand = FriendOpSubCmd.UPLOAD_FRIEND_REMARK;
            Remark = new FriendRemark();
        }
        public FriendDataOpPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            switch (SubCommand)
            {
                case FriendOpSubCmd.BATCH_DOWNLOAD_FRIEND_REMARK:
                    return "Friend Data Packet - Batch Download Remark";
                case FriendOpSubCmd.UPLOAD_FRIEND_REMARK:
                    return "Friend Data Packet - Upload Remark";
                case FriendOpSubCmd.REMOVE_FRIEND_FROM_LIST:
                    return "Friend Data Packet - Remove Friend From List";
                case FriendOpSubCmd.DOWNLOAD_FRIEND_REMARK:
                    return "Friend Data Packet - Download Remark";
                default:
                    return "Friend Data Packet - Unknown Sub Command";
            }
        }
        protected override void PutBody(ByteBuffer buf)
        {
            // 操作类型
            buf.Put((byte)SubCommand);
            // 未知字节0x0，仅在上传时
            if (SubCommand == FriendOpSubCmd.UPLOAD_FRIEND_REMARK || SubCommand == FriendOpSubCmd.BATCH_DOWNLOAD_FRIEND_REMARK)
                buf.Put((byte)Page);
            // 操作对象的QQ号
            if (SubCommand != FriendOpSubCmd.BATCH_DOWNLOAD_FRIEND_REMARK)
                buf.PutInt(QQ);
            // 后面的内容为一个未知字节0和备注信息，仅在上传时
            if (SubCommand == FriendOpSubCmd.UPLOAD_FRIEND_REMARK)
            {
                buf.Put((byte)0);
                // 备注信息
                // 姓名
                if (string.IsNullOrEmpty(Remark.Name))
                    buf.Put((byte)0);
                else
                {
                    byte[] b = Utils.Util.GetBytes(Remark.Name);
                    buf.Put((byte)b.Length);
                    buf.Put(b);
                }
                // 手机
                if (string.IsNullOrEmpty(Remark.Mobile))
                    buf.Put((byte)0);
                else
                {
                    byte[] b = Utils.Util.GetBytes(Remark.Mobile);
                    buf.Put((byte)b.Length);
                    buf.Put(b);
                }
                // 电话
                if (string.IsNullOrEmpty(Remark.Telephone))
                    buf.Put((byte)0);
                else
                {
                    byte[] b = Utils.Util.GetBytes(Remark.Telephone);
                    buf.Put((byte)b.Length);
                    buf.Put(b);
                }
                // 地址
                if (string.IsNullOrEmpty(Remark.Address))
                    buf.Put((byte)0);
                else
                {
                    byte[] b = Utils.Util.GetBytes(Remark.Address);
                    buf.Put((byte)b.Length);
                    buf.Put(b);
                }
                // 邮箱
                if (string.IsNullOrEmpty(Remark.Email))
                    buf.Put((byte)0);
                else
                {
                    byte[] b = Utils.Util.GetBytes(Remark.Email);
                    buf.Put((byte)b.Length);
                    buf.Put(b);
                }
                // 邮编
                if (string.IsNullOrEmpty(Remark.Zipcode))
                    buf.Put((byte)0);
                else
                {
                    byte[] b = Utils.Util.GetBytes(Remark.Zipcode);
                    buf.Put((byte)b.Length);
                    buf.Put(b);
                }
                // 备注
                if (string.IsNullOrEmpty(Remark.Note))
                    buf.Put((byte)0);
                else
                {
                    byte[] b = Utils.Util.GetBytes(Remark.Note);
                    buf.Put((byte)b.Length);
                    buf.Put(b);
                }
            }
        }
    }
}
