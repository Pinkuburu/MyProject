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
    public class LoginVerifyPacket : BasicOutPacket
    {

        public LoginVerifyPacket(QQClient client)
            : base(QQCommand.LoginVerify, true, client)
        {
        }

        public LoginVerifyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "LoginVerify Packet ";
        }

        protected override void PutBody(ByteBuffer buf)
        {
            // 初始密钥
            buf.Put(user.QQKey.InitKey);
            #region 加密用户身份数据
            ByteBuffer verifyBuf = new ByteBuffer();
            verifyBuf.PutInt(Utils.Util.Random.Next());//random??
            verifyBuf.PutChar((char)0x0001);
            verifyBuf.PutInt(Client.QQUser.QQ);
            verifyBuf.Put(VersionData.QQ09_VERSION_SPEC);
            verifyBuf.Put(0x00);
            verifyBuf.PutChar((char)0x0001);
            verifyBuf.Put(Client.QQUser.Password);
            verifyBuf.Put(Client.ServerTime);
            verifyBuf.Position += 13;//13个00
            verifyBuf.Put(Client.ClientIP);
            verifyBuf.PutInt(0);
            verifyBuf.PutInt(0);
            verifyBuf.PutChar((char)0x0010);
            verifyBuf.Put(Client.QQUser.QQKey.Verify_Key1);
            verifyBuf.Put(Client.QQUser.QQKey.Verify_Key2);

            //加密
            byte[] EncodeVerifyBuf = crypter.Encrypt(verifyBuf.ToByteArray(), user.QQKey.PasswordKey);
            
#if DEBUG
            Client.LogManager.Log(ToString() + " User Data:" + Utils.Util.ToHex(verifyBuf.ToByteArray()));
            Client.LogManager.Log(ToString() + " Key:" + Utils.Util.ToHex(user.QQKey.PasswordKey));
            Client.LogManager.Log(ToString() + " User Data:" + "len " + EncodeVerifyBuf.Length.ToString("") + "<--" + Utils.Util.ToHex(EncodeVerifyBuf));
#endif
            #endregion

            ByteBuffer DecodedBuf = new ByteBuffer();
            DecodedBuf.PutChar((char)0x00CA);
            DecodedBuf.Put(new byte[] { 0x00, 0x01 });
            DecodedBuf.Put(VersionData.QQ09_LOCALE);
            DecodedBuf.Put(VersionData.QQ09_VERSION_SPEC);
            DecodedBuf.Put((byte)0x00);
            DecodedBuf.Put((byte)Client.QQUser.QQKey.Answer_Token.Length);
            DecodedBuf.Put(Client.QQUser.QQKey.Answer_Token);

            DecodedBuf.PutChar((char)EncodeVerifyBuf.Length);
            DecodedBuf.Put(EncodeVerifyBuf);

            DecodedBuf.PutChar((char)0x0000);
            DecodedBuf.PutChar((char)0x018B);
            DecodedBuf.Put(0x2E);//length of the following info

            DecodedBuf.Put(0x01);
            DecodedBuf.PutInt(Utils.Util.Random.Next());

            DecodedBuf.PutChar((char)0x0010);
            DecodedBuf.Put(new byte[]{0xE9,0xC4,0xD6,0x5C,0x4D,0x9D,
                0xA0,0x17,0xE5,0x24,0x6B,0x55,0x57,0xD3,0xAB,0xF1});//unknow6

            DecodedBuf.Put(0x02);
            DecodedBuf.PutInt(Utils.Util.Random.Next());

            DecodedBuf.PutChar((char)0x0010);
            DecodedBuf.Put(new byte[]{0xCB,0x8D,0xA4,0xE2,0x61,0xC2,
                0xDD,0x27,0x39,0xEC,0x8A,0xCA,0xA6,0x98,0xF8,0x9B});//unknow7
            DecodedBuf.Position += 0x015B;//395 zeros?  348 -去后面补的一位
            DecodedBuf.Put(0x00);//否则后面不补00
            byte[] EncodedBuf = crypter.Encrypt(DecodedBuf.ToByteArray(), user.QQKey.InitKey);
            buf.Put(EncodedBuf);
            verifyBuf = null;
            
#if DEBUG
            Client.LogManager.Log(ToString() + " Key:" + Utils.Util.ToHex(user.QQKey.InitKey));
            Client.LogManager.Log(ToString() + " Uncode Body:" + Utils.Util.ToHex(DecodedBuf.ToByteArray()));
#endif
        }
    }
}
