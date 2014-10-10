﻿#region 版权声明
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

namespace LFNet.QQ
{
    /// <summary>
    /// QQ2009P
    /// </summary>
    public class VersionData
    {
        public static byte[] QQ09_LOCALE = new byte[] { 0x00, 0x00, 0x08, 0x04, 0x01, 0xE0 };
        public static byte[] QQ09_VERSION_SPEC = new byte[] { 0x00, 0x00, 0x02, 0x20, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x08, 0xFB };
        public static byte[] QQ09_EXE_HASH = new byte[] { 0xD5, 0xCB, 0x09, 0x9A, 0x61, 0x63, 0x16, 0x7E, 0xEA, 0xF8, 0x05, 0x6E, 0xFF, 0x50, 0xF3, 0x12 };
       public static byte[] QQ09_Support_Data = new byte[] { 0x0A, 0x28, 0xE8, 0xAF, 0xA5, 0xE8, 0xBD, 0xAF, 0xE4, 0xBB, 0xB6, 0xE5, 0x9F, 0xBA, 0xE4, 0xBA, 0x8E, 0x4C, 0x46, 0x4E, 0x65, 0x74, 0x2E, 0x51, 0x51, 0xE3, 0x80, 0x90, 0xE4, 0xBD, 0x9C, 0xE8, 0x80, 0x85, 0xEF, 0xBC, 0x9A, 0x64, 0x6F, 0x62, 0x69, 0x74, 0xEF, 0xBC, 0x8C, 0x51, 0x51, 0x3A, 0x31, 0x35, 0x36, 0x37, 0x39, 0x38, 0x30, 0x38, 0x37, 0xE3, 0x80, 0x91, 0xE5, 0xBC, 0x80, 0xE5, 0x8F, 0x91, 0x29 };
    }
   ///// <summary>
   ///// QQ2009正式
   ///// </summary>
   //public class VersionData
   //{
   //    public static byte[] QQ09_LOCALE = new byte[] { 0x00, 0x00, 0x08, 0x04, 0x01, 0xE0 };
   //    public static byte[] QQ09_VERSION_SPEC = new byte[] { 0x00, 0x00, 0x02, 0x10, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x08, 0xDD };
   //    //public static byte[] QQ09_EXE_HASH = new byte[] { 0xD5, 0xCB, 0x09, 0x9A, 0x61, 0x63, 0x16, 0x7E, 0xEA, 0xF8, 0x05, 0x6E, 0xFF, 0x50, 0xF3, 0x12 };
   //}
}