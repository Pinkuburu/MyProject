/*
    Copyright � 2002, The KPD-Team
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

#region ��Ȩ����
//=========================================================== 
// ��Ȩ������LFNet.QQ�ǻ���QQ2009�汾��QQЭ�鿪�����ɵģ�Э��
// ������Ҫ�ο���СϺ��MyQQ(C++)Դ���룬���뿪����Ҫ���ڰ���
// ��LumaQQ.NET��C#.NET�����޸Ķ��ɣ��ʼ�������ʹ��LumaQQ�Ŀ�
// ԴЭ�顣
//
// ����û�ж�LumaQQ.NET��C#.NET����Ŀ��������ĸĶ�����
// Ҫ����Ϊ��MyQQ��C++Э����벿�ַ���ɷ���LumaQQ.Net���
// ��C#���룬��������LumaQQ����Luma������Ȩ�Ͱ�Ȩ������
// 
// ���뿪Դ��Ҫ���ڽ�������ѧϰ���о�Э���������������ȱ����������������Լ�����⡣
// ���������ѧϰ����ʹ�ã������ʹ�ô˿�����ǰ������Э���ö෽���ϵ��
// ���������κ���ҵ��;�ͷǷ���;�����˲����ܺͳе��ɴ˲������κ�Ȩ���Լ��κη������Ρ�
// 
// ��Դ�����ͨ��������ַ��ȡ:
// http://QQCode.lynfo.com, http://www.lynfo.com, http://bbs.lynfo.com, http://hi.baidu.com/dobit.
//
// Copyright @ 2009-2010  Lynfo.com.  All Rights Reserved.   
// Framework: 2.0
// Author: Luma(java��) �� Abu(C# QQ2005Э���) �� Dobit(C# QQ2009Э��汾)
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
using System.Net;
using System.Net.Sockets;

namespace Org.Mentalis.Network.ProxySocket.Authentication {
	/// <summary>
	/// Implements a SOCKS authentication scheme.
	/// </summary>
	/// <remarks>This is an abstract class; it must be inherited.</remarks>
	internal abstract class AuthMethod {
		/// <summary>
		/// Initializes an AuthMethod instance.
		/// </summary>
		/// <param name="server">The socket connection with the proxy server.</param>
		public AuthMethod(Socket server) {
			Server = server;
		}
		/// <summary>
		/// Authenticates the user.
		/// </summary>
		/// <exception cref="ProxyException">Authentication with the proxy server failed.</exception>
		/// <exception cref="ProtocolViolationException">The proxy server uses an invalid protocol.</exception>
		/// <exception cref="SocketException">An operating system error occurs while accessing the Socket.</exception>
		/// <exception cref="ObjectDisposedException">The Socket has been closed.</exception>
		public abstract void Authenticate();
		/// <summary>
		/// Authenticates the user asynchronously.
		/// </summary>
		/// <param name="callback">The method to call when the authentication is complete.</param>
		/// <exception cref="ProxyException">Authentication with the proxy server failed.</exception>
		/// <exception cref="ProtocolViolationException">The proxy server uses an invalid protocol.</exception>
		/// <exception cref="SocketException">An operating system error occurs while accessing the Socket.</exception>
		/// <exception cref="ObjectDisposedException">The Socket has been closed.</exception>
		public abstract void BeginAuthenticate(HandShakeComplete callback);
		/// <summary>
		/// Gets or sets the socket connection with the proxy server.
		/// </summary>
		/// <value>The socket connection with the proxy server.</value>
		protected Socket Server {
			get {
				return m_Server;
			}
			set {
				if (value == null)
					throw new ArgumentNullException();
				m_Server = value;
			}
		}
		/// <summary>
		/// Gets or sets a byt array that can be used to store data.
		/// </summary>
		/// <value>A byte array to store data.</value>
		protected byte[] Buffer {
			get {
				return m_Buffer;
			}
			set {
				m_Buffer = value;
			}
		}
		/// <summary>
		/// Gets or sets the number of bytes that have been received from the remote proxy server.
		/// </summary>
		/// <value>An integer that holds the number of bytes that have been received from the remote proxy server.</value>
		protected int Received {
			get {
				return m_Received;
			}
			set {
				m_Received = value;
			}
		}
		// private variables
		/// <summary>Holds the value of the Buffer property.</summary>
		private byte[] m_Buffer;
		/// <summary>Holds the value of the Server property.</summary>
		private Socket m_Server;
		/// <summary>Holds the address of the method to call when the proxy has authenticated the client.</summary>
		protected HandShakeComplete CallBack;
		/// <summary>Holds the value of the Received property.</summary>
		private int m_Received;
	}
}