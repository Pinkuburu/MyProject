/*
    Copyright © 2002, The KPD-Team
    All rights reserved.
    http://www.mentalis.org/

  Redistribution and use in source and binary forms, with or without
  modification, are permitted provided that the following conditions
  are met:

    - Redistributions of source code must retain the above copyright
       notice, this list of conditions and the following disclaimer. 

    - Neither the name of the KPD-Team, nor the names of its contributors
       may be used to endorse or promote products derived from this
       software without specific prior written permission. 

  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
  FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL
  THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
  INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
  (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
  SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
  HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
  STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
  ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
  OF THE POSSIBILITY OF SUCH DAMAGE.
*/

#region °æÈ¨ÉùÃ÷
//=========================================================== 
// °æÈ¨ÉùÃ÷£ºLFNet.QQÊÇ»ùÓÚQQ2009°æ±¾µÄQQĞ­Òé¿ª·¢¶ø³ÉµÄ£¬Ğ­Òé
// ·ÖÎöÖ÷Òª²Î¿¼×ÔĞ¡ÏºµÄMyQQ(C++)Ô´´úÂë£¬´úÂë¿ª·¢Ö÷Òª»ùÓÚ°¢²¼
// µÄLumaQQ.NETµÄC#.NET´úÂëĞŞ¸Ä¶ø³É£¬¹Ê¼ÌĞø×ñÕÕÊ¹ÓÃLumaQQµÄ¿ª
// Ô´Ğ­Òé¡£
//
// ±¾ÈËÃ»ÓĞ¶ÔLumaQQ.NETµÄC#.NET´úÂëµÄ¿ò¼Ü×ö¹ı¶àµÄ¸Ä¶¯£¬Ö÷
// Òª¹¤×÷Îª½«MyQQµÄC++Ğ­Òé´úÂë²¿·Ö·­Òë³É·ûºÏLumaQQ.Net¿ò¼Ü
// µÄC#´úÂë£¬¹ÊÇë×ğÖØLumaQQ×÷ÕßLumaµÄÖø×÷È¨ºÍ°æÈ¨ÉùÃ÷¡£
// 
// ´úÂë¿ªÔ´Ö÷ÒªÓÃÓÚ½â¾ö´ó¼ÒÔÚÑ§Ï°ºÍÑĞ¾¿Ğ­Òé¹ı³ÌÖĞÓöµ½ÓÉÓÚÈ±·¦´úÂëËù´øÀ´µÄÖÆÔ¼ĞÔÎÊÌâ¡£
// ±¾´úÂë½ö¹©Ñ§Ï°½»Á÷Ê¹ÓÃ£¬´ó¼ÒÔÚÊ¹ÓÃ´Ë¿ª·¢°üÇ°Çë×ÔĞĞĞ­µ÷ºÃ¶à·½Ãæ¹ØÏµ£¬
// ²»µÃÓÃÓÚÈÎºÎÉÌÒµÓÃÍ¾ºÍ·Ç·¨ÓÃÍ¾£¬±¾ÈË²»ÏíÊÜºÍ³Ğµ£ÓÉ´Ë²úÉúµÄÈÎºÎÈ¨ÀûÒÔ¼°ÈÎºÎ·¨ÂÉÔğÈÎ¡£
// 
// ±¾Ô´´úÂë¿ÉÍ¨¹ıÒÔÏÂÍøÖ·»ñÈ¡:
// http://QQCode.lynfo.com, http://www.lynfo.com, http://bbs.lynfo.com, http://hi.baidu.com/dobit.
//
// Copyright @ 2009-2010  Lynfo.com.  All Rights Reserved.   
// Framework: 2.0
// Author: Luma(java°æ) ¡ú Abu(C# QQ2005Ğ­Òé°æ) ¡ú Dobit(C# QQ2009Ğ­Òé°æ±¾)
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

namespace Org.Mentalis.Network.ProxySocket {
	/// <summary>
	/// The exception that is thrown when a proxy error occurs.
	/// </summary>
	public class ProxyException : Exception {
		/// <summary>
		/// Initializes a new instance of the ProxyException class.
		/// </summary>
		public ProxyException() : this("An error Occurred while talking to the proxy server.") {}
		/// <summary>
		/// Initializes a new instance of the ProxyException class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public ProxyException(string message) : base(message) {}
		/// <summary>
		/// Initializes a new instance of the ProxyException class.
		/// </summary>
		/// <param name="socks5Error">The error number returned by a SOCKS5 server.</param>
		public ProxyException(int socks5Error) : this(ProxyException.Socks5ToString(socks5Error)) {}
		/// <summary>
		/// Converts a SOCKS5 error number to a human readable string.
		/// </summary>
		/// <param name="socks5Error">The error number returned by a SOCKS5 server.</param>
		/// <returns>A string representation of the specified SOCKS5 error number.</returns>
		public static string Socks5ToString(int socks5Error) {
			switch(socks5Error) {
				case 0:
					return "Connection succeeded.";
				case 1:
					return "General SOCKS server failure.";
				case 2:
					return "Connection not allowed by ruleset.";
				case 3:
					return "Network unreachable.";
				case 4:
					return "Host unreachable.";
				case 5:
					return "Connection refused.";
				case 6:
					return "TTL expired.";
				case 7:
					return "Command not supported.";
				case 8:
					return "Address type not supported.";
				default:
					return "Unspecified SOCKS error.";
			}
		}
	}
}