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
    public class LoginRequestPacket : BasicOutPacket
    {
       
        public LoginRequestPacket(QQClient client)
            : base(QQCommand.LoginRequest, true, client)
        {
            this.Token = null;
            this.GetCode = 0;
            this.Png_Data = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client">QQclient</param>
        /// <param name="token">这个包发出去获取的令牌</param>
        /// <param name="getCode">1或0，1和验证码有关的请求</param>
        /// <param name="code">验证码的值</param>
        public LoginRequestPacket(QQClient client, byte[] token, uint getCode, byte png_data)
            : base(QQCommand.LoginRequest, true, client)
        {
            this.Token = token;
            this.GetCode = getCode;
            this.Png_Data = png_data;
        }
        public LoginRequestPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "LoginRequest Packet";
        }
        /// <summary>
        /// Token获取验证码的令牌
        /// </summary>
        public byte[] Token { get; set; }

        /// <summary>
        /// 0,
        /// 1 获取验证码图片或发送验证码信息
        /// </summary>
        public uint GetCode { get; set; }

        /// <summary>
        /// png_data 0 或1
        /// </summary>
        public byte Png_Data { get; set; }

        protected override void PutBody(ByteBuffer buf)
        {
            // 初始密钥
            buf.Put(user.QQKey.InitKey);
            ByteBuffer DecodedBuf = new ByteBuffer();
            DecodedBuf.Put(new byte[] { 0x00, 0x01 });
            DecodedBuf.Put(VersionData.QQ09_LOCALE);
            DecodedBuf.Put(VersionData.QQ09_VERSION_SPEC);
            DecodedBuf.Put((byte)0x00);
            DecodedBuf.Put((byte)Client.QQUser.QQKey.LoginRequestToken.Length);
            DecodedBuf.Put(Client.QQUser.QQKey.LoginRequestToken);
            if (GetCode != 0) DecodedBuf.Put(0x04);
            else DecodedBuf.Put(0x03);//开头写成0x04了，结果后面出现Token不一致了
            DecodedBuf.Put(0x00);
            DecodedBuf.Put(0x05);
            DecodedBuf.PutInt(0);
            DecodedBuf.Put((byte)Png_Data);
            if (GetCode != 0x00 && Token != null)
            {
                DecodedBuf.Put(0x04);
                DecodedBuf.PutInt(GetCode);
                //answer token
                DecodedBuf.PutChar((char)Token.Length);
                DecodedBuf.Put(Token);

            }
            else if (Png_Data == 0x01 && Token != null)
            {
                //png token
                DecodedBuf.PutChar((char)Token.Length);
                DecodedBuf.Put(Token);
            }
            else
            {
                DecodedBuf.Put(0x00);
                DecodedBuf.Put(0x00);
            }
            byte[] EncodedBuf = crypter.Encrypt(DecodedBuf.ToByteArray(), user.QQKey.InitKey);
            buf.Put(EncodedBuf);
#if DEBUG
            Client.LogManager.Log(ToString() + " Key:" + Utils.Util.ToHex(user.QQKey.InitKey));
            Client.LogManager.Log(ToString() + " UnBody:" + Utils.Util.ToHex(DecodedBuf.ToByteArray()));
#endif
        }
    }
}
