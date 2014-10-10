
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
    ///  * 发送短消息的请求包，格式为：
    /// * 1. 包头
    /// * 2. 消息序号，2字节，从0开始，用在拆分发送中
    /// * 3. 未知2字节，全0
    /// * 4. 未知4字节，全0
    /// * 5. 发送者昵称，最长13个字节，如果不足，则后面为0
    /// * 6. 未知的1字节，0x01
    /// * 7. 如果是免提短信，0x20，其他情况，0x00
    /// * 8. 短消息内容类型，1字节
    /// * 9. 短消息内容类型编号，4字节
    /// * 10. 未知的1字节，0x01
    /// * 11. 接受者中的手机号码个数，1字节
    /// * 12. 手机号码，18字节，不足的为0
    /// * 13. 未知的2字节
    /// * 14. 未知的1字节
    /// * 15. 如果有更多手机号，重复12-14部分
    /// * 注：12-15部分只在11部分不为0时存在
    /// * 16. 接受者中的QQ号码个数，1字节
    /// * 17. QQ号码，4字节
    /// * 18. 如果有更多QQ号码，重复17部分
    /// * 注：17-18部分只有在16部分不为0时存在
    /// * 19. 未知1字节，一般是0x03
    /// * 20. 短消息字节长度，2字节，如果8部分不为0，则此部分0x0000
    /// * 注：QQ的短信和发送者昵称加起来不能超过58个字符（英文和汉字都算是一个字符），
    /// * 昵称最长是13字节，所以最短也应该能发43个字符，所以可以考虑不按照QQ的做法，
    /// * 我们可以尽量发满86个字节。
    /// * 21. 短消息字节数组，消息的格式如下：
    /// * 		如果是普通的消息，则就是平常的字节数组而已
    /// *      如果有些字符有闪烁，则那些字节要用0x01括起来
    /// *      如果这条消息是一条长消息拆分而成的部分，则在消息字节数组前面要加一部分内容，这部分内容是
    /// *      [消息序号的字符串形式，从1开始] [0x2F] [总消息条数的字符串形式] [0x0A]
    /// * 注：21部分只有当20部分部位0时存在
    /// * 22. 尾部
    /// * 
    /// * 调用这个包时，message的内容必须是已经组装好的
    /// 	<remark>abu 2008-02-29 </remark>
    /// </summary>
    public class SendSMSPacket : BasicOutPacket
    {
        public ushort MessageSequence { get; set; }
        public byte[] Message { get; set; }
        public SMSContentType ContentType { get; set; }
        public int ContentId { get; set; }
        public SMSSendMode SendMode { get; set; }
        public string SenderName { get; set; }
        public List<string> ReceiverMobile { get; set; }
        public List<int> ReceiverQQ { get; set; }

        public SendSMSPacket(QQClient client)
            : base(QQCommand.Send_SMS_05,true,client)
        {
            MessageSequence = 0;
            ContentType = SMSContentType.NORMAL;
            SendMode = SMSSendMode.NORMAL;
        }
        public SendSMSPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Send SMS Packet";
        }
        protected override void PutBody(ByteBuffer buf)
        {
            // 短消息序号
            buf.PutUShort(MessageSequence);
            // 未知2字节
            buf.PutChar((char)0);
            // 未知4字节
            buf.PutInt(0);
            // 发送者昵称
            byte[] b = Utils.Util.GetBytes(SenderName);
            if (b.Length > QQGlobal.QQ_MAX_SMS_SENDER_NAME)
            {
                buf.Put(b, 0, QQGlobal.QQ_MAX_SMS_SENDER_NAME);
            }
            else
            {
                buf.Put(b);
                int stuff = QQGlobal.QQ_MAX_SMS_SENDER_NAME - b.Length;
                while (stuff-- > 0)
                    buf.Put((byte)0);
            }
            // 未知1字节
            buf.Put((byte)0x01);
            // 发送模式
            buf.Put((byte)SendMode);
            // 内容类型
            buf.Put((byte)ContentType);
            // 内容编号
            buf.PutInt(ContentId);
            // 未知1字节
            buf.Put((byte)0x01);
            // 手机个数
            if (ReceiverMobile == null)
                buf.Put((byte)0);
            else
            {
                buf.Put((byte)ReceiverMobile.Count);
                foreach (String mobile in ReceiverMobile)
                {
                    b = Utils.Util.GetBytes(mobile);
                    if (b.Length > QQGlobal.QQ_MAX_SMS_MOBILE_LENGTH)
                    {
                        buf.Put(b, 0, QQGlobal.QQ_MAX_SMS_MOBILE_LENGTH);
                    }
                    else
                    {
                        buf.Put(b);
                        int stuff = QQGlobal.QQ_MAX_SMS_MOBILE_LENGTH - b.Length;
                        while (stuff-- > 0)
                            buf.Put((byte)0);
                    }
                    // 未知的2字节
                    buf.PutChar((char)0);
                    // 未知的1字节
                    buf.Put((byte)0);
                }
            }
            // QQ号码个数
            if (ReceiverQQ == null)
                buf.Put((byte)0);
            else
            {
                buf.Put((byte)ReceiverQQ.Count);
                foreach (int qq in ReceiverQQ)
                    buf.PutInt(qq);
            }
            // 未知1字节
            buf.Put((byte)0x03);
            // 消息
            if (Message == null)
                buf.PutChar((char)0);
            else
            {
                buf.PutChar((char)Message.Length);
                // 消息
                buf.Put(Message);
            }
        }
    }
}
