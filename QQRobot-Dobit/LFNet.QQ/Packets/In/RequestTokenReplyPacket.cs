
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
    /// <summary>
    /// * 请求Token回复包
    /// </summary>
    public class RequestTokenReplyPacket : BasicInPacket
    {
        public byte[] Key { get; set; }
        /// <summary>
        /// 得到的Token
        /// </summary>
        public byte[] Token { get; set; }
        /// <summary>
        /// 01 add
        /// 02 delete
        /// </summary>
        public byte SubCommand { get; set; }

        /// <summary>
        /// 是否需要输入验证码
        /// </summary>
        public bool NeedVerify { get; set; }

        /// <summary>
        /// 输入的验证码是否错误
        /// </summary>
        public bool IsWrongVerifyCode { get; set; }
        /// <summary>
        /// 验证码下载地址
        /// </summary>
        public string Url { get; set; }
        //public ReplyCode ReplyCode { get; set; }
        public RequestTokenReplyPacket(ByteBuffer buf, int length, QQClient client) : base(buf, length, client) { }
        public override string GetPacketName()
        {
            return "Request Token Reply Packet";
        }
        protected override void ParseBody(ByteBuffer buf)
        {
#if DEBUG
            Client.LogManager.Log(ToString() + " " + Utils.Util.ToHex(buf.ToByteArray()));
#endif
            SubCommand = buf.Get();
            buf.GetChar();//0006
            byte verify = buf.Get();
            if (verify != 0)
            {
                if (buf.Position == buf.Length)//输入的验证码不正确
                {
                    //应该返回验证码不正确的消息
                    IsWrongVerifyCode = true;
                    Client.LogManager.Log(ToString() + " 验证码不正确！");
                    return;
                }
                NeedVerify = true;//需要输入验证码
                int len = buf.GetUShort();
                Url = Utils.Util.GetString(buf.GetByteArray(len));
//                string getQQSession = "";
//                VerifyCodeFileName=Utils.Util.MapPath("\\verify\\" + Client.QQUser.QQ + ".jpg");
//                Utils.Util.DownLoadFileFromUrl(url, VerifyCodeFileName, out getQQSession);
//#if DEBUG
//                Client.LogManager.Log(ToString() + string.Format(" url:{0}, Session:{1},Code File:{2}", url, getQQSession, VerifyCodeFileName));
//#endif
//                Client.QQUser.QQKey.QQSessionToken = Utils.Util.GetBytes(getQQSession);
            }
            else
            {
                int len = buf.GetUShort();
                Token = buf.GetByteArray(len);
            }

            
        }
    }
}
