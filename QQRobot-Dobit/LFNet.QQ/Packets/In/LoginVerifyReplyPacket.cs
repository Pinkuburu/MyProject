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

namespace LFNet.QQ.Packets.In
{
   public class LoginVerifyReplyPacket:BasicInPacket
    {
       public LoginVerifyReplyPacket(ByteBuffer buf, int length, QQClient client)
            : base(buf, length, client)
        { }
       
       public byte ReplyCode { get; private set; }
       /// <summary>
       /// 错误信息,当出现错误时才能看到该信息
       /// </summary>
       public string ReplyMessage { get; private set; }
        public override string GetPacketName()
        {
            return "Login Verify Reply Packet";
        }

        protected override void ParseBody(ByteBuffer buf)
        {

#if DEBUG
            Client.LogManager.Log(ToString() + " Decoded Data:" + Utils.Util.ToHex(buf.ToByteArray()));
#endif

            buf.GetChar();//length or sth..
            ReplyCode = buf.Get();
            int len = 0;
            switch (ReplyCode)
            { 
                case 0x00://success!
                    Client.LogManager.Log(ToString() + ":0x" + ReplyCode.ToString("X2") + " Login Success!");
                    len = buf.GetChar();//0x0020
                    Client.QQUser.QQKey.LoginInfo_Token = buf.GetByteArray(len);
                    Client.QQUser.QQKey.LoginInfo_UnknowData = buf.GetByteArray(4); //buf.GetInt()
                    Client.ServerTime = buf.GetByteArray(4);

                    len = buf.GetChar();
                    Client.QQUser.QQKey.LoginInfo_Data_Token = buf.GetByteArray(len);
                    len = buf.GetChar();
                    Client.QQUser.QQKey.LoginInfo_Magic_Token = buf.GetByteArray(len);
                    Client.QQUser.QQKey.LoginInfo_Key1 = buf.GetByteArray(16);
                    buf.GetChar();//0x00 00
                    if (buf.Position + 3 < buf.Length)//判断来的包是否包含LoginInfo_Key3 因为有的版本没这个key 应该说本人用的正式版本没这个
                    {
                        Client.QQUser.QQKey.LoginInfo_Key3 = buf.GetByteArray(16);
#if DEBUG
                        Client.LogManager.Log(ToString() + "Client.QQUser.QQKey.LoginInfo_Key3:" + Utils.Util.ToHex(Client.QQUser.QQKey.LoginInfo_Key3));
#endif
                    }
                    buf.GetChar();//0x00 00
                    return;
                case 0x33:
                case 0x51://denied!
                    Client.LogManager.Log(ToString() + ":0x" + ReplyCode.ToString("X2") + " Denied!");
                    break;
                case 0xBF:
                    Client.LogManager.Log(ToString() + ":0x" + ReplyCode.ToString("X2") + " No this QQ number!");
                    break;
                case 0x34:
                    Client.LogManager.Log(ToString() + ":0x" + ReplyCode.ToString("X2") + " Wrong password!");
                    break;
                default:
                    Client.LogManager.Log(ToString() + ":0x" + ReplyCode.ToString("X2") + " Unknow ReplyCode!");
                    break;
                
            }
            buf.Position = 11;
            len =(int) buf.GetChar();
            byte[] data = buf.GetByteArray(len);
            ReplyMessage = Utils.Util.GetString(data);

            Client.LogManager.Log(ToString() + ":0x" + ReplyCode.ToString("X2") + " Message Data(UTF-8): "+Utils.Util.ToHex(data)+"-->" + ReplyMessage);
        }
    }
}
