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
using System.Threading;

namespace Org.Mentalis.Network.ProxySocket {
	/// <summary>
	/// A class that implements the IAsyncResult interface. Objects from this class are returned by the BeginConnect method of the ProxySocket class.
	/// </summary>
	internal class IAsyncProxyResult : IAsyncResult {
		/// <summary>Initializes the internal variables of this object</summary>
		/// <param name="stateObject">An object that contains state information for this request.</param>
		internal void Init(object stateObject) {
			m_StateObject = stateObject;
			m_Completed = false;
			if (m_WaitHandle != null)
				m_WaitHandle.Reset();
		
		}
		/// <summary>Initializes the internal variables of this object</summary>
		internal void Reset() {
			m_StateObject = null;
			m_Completed = true;
			if (m_WaitHandle != null)
				m_WaitHandle.Set();
		}
		/// <summary>Gets a value that indicates whether the server has completed processing the call. It is illegal for the server to use any client supplied resources outside of the agreed upon sharing semantics after it sets the IsCompleted property to "true". Thus, it is safe for the client to destroy the resources after IsCompleted property returns "true".</summary>
		/// <value>A boolean that indicates whether the server has completed processing the call.</value>
		public bool IsCompleted {
			get {
				return m_Completed;
			}
		}
		/// <summary>Gets a value that indicates whether the BeginXXXX call has been completed synchronously. If this is detected in the AsyncCallback delegate, it is probable that the thread that called BeginInvoke is the current thread.</summary>
		/// <value>Returns false.</value>
		public bool CompletedSynchronously {
			get {
				return false;
			}
		}
		/// <summary>Gets an object that was passed as the state parameter of the BeginXXXX method call.</summary>
		/// <value>The object that was passed as the state parameter of the BeginXXXX method call.</value>
		public object AsyncState {
			get {
				return m_StateObject;
			}
		}
		/// <summary>
		/// The AsyncWaitHandle property returns the WaitHandle that can use to perform a WaitHandle.WaitOne or WaitAny or WaitAll. The object which implements IAsyncResult need not derive from the System.WaitHandle classes directly. The WaitHandle wraps its underlying synchronization primitive and should be signaled after the call is completed. This enables the client to wait for the call to complete instead polling. The Runtime supplies a number of waitable objects that mirror Win32 synchronization primitives e.g. ManualResetEvent, AutoResetEvent and Mutex.
		/// WaitHandle supplies methods that support waiting for such synchronization objects to become signaled with "any" or "all" semantics i.e. WaitHandle.WaitOne, WaitAny and WaitAll. Such methods are context aware to avoid deadlocks. The AsyncWaitHandle can be allocated eagerly or on demand. It is the choice of the IAsyncResult implementer.
		///</summary>
		/// <value>The WaitHandle associated with this asynchronous result.</value>
		public WaitHandle AsyncWaitHandle {
			get {
				if (m_WaitHandle == null)
					m_WaitHandle = new ManualResetEvent(false);
				return m_WaitHandle;
			}
		}
		// private variables
		/// <summary>Used internally to represent the state of the asynchronous request</summary>
		internal bool m_Completed = true;
		/// <summary>Holds the value of the StateObject property.</summary>
		private object m_StateObject;
		/// <summary>Holds the value of the WaitHandle property.</summary>
		private ManualResetEvent m_WaitHandle;
	}
}