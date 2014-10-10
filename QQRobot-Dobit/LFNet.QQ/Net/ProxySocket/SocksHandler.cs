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
// °æÈ¨ÉùÃ÷£ºLFNet.QQÊÇ»ùÓÚQQ2009°æ±¾µÄQQÐ­Òé¿ª·¢¶ø³ÉµÄ£¬Ð­Òé
// ·ÖÎöÖ÷Òª²Î¿¼×ÔÐ¡ÏºµÄMyQQ(C++)Ô´´úÂë£¬´úÂë¿ª·¢Ö÷Òª»ùÓÚ°¢²¼
// µÄLumaQQ.NETµÄC#.NET´úÂëÐÞ¸Ä¶ø³É£¬¹Ê¼ÌÐø×ñÕÕÊ¹ÓÃLumaQQµÄ¿ª
// Ô´Ð­Òé¡£
//
// ±¾ÈËÃ»ÓÐ¶ÔLumaQQ.NETµÄC#.NET´úÂëµÄ¿ò¼Ü×ö¹ý¶àµÄ¸Ä¶¯£¬Ö÷
// Òª¹¤×÷Îª½«MyQQµÄC++Ð­Òé´úÂë²¿·Ö·­Òë³É·ûºÏLumaQQ.Net¿ò¼Ü
// µÄC#´úÂë£¬¹ÊÇë×ðÖØLumaQQ×÷ÕßLumaµÄÖø×÷È¨ºÍ°æÈ¨ÉùÃ÷¡£
// 
// ´úÂë¿ªÔ´Ö÷ÒªÓÃÓÚ½â¾ö´ó¼ÒÔÚÑ§Ï°ºÍÑÐ¾¿Ð­Òé¹ý³ÌÖÐÓöµ½ÓÉÓÚÈ±·¦´úÂëËù´øÀ´µÄÖÆÔ¼ÐÔÎÊÌâ¡£
// ±¾´úÂë½ö¹©Ñ§Ï°½»Á÷Ê¹ÓÃ£¬´ó¼ÒÔÚÊ¹ÓÃ´Ë¿ª·¢°üÇ°Çë×ÔÐÐÐ­µ÷ºÃ¶à·½Ãæ¹ØÏµ£¬
// ²»µÃÓÃÓÚÈÎºÎÉÌÒµÓÃÍ¾ºÍ·Ç·¨ÓÃÍ¾£¬±¾ÈË²»ÏíÊÜºÍ³Ðµ£ÓÉ´Ë²úÉúµÄÈÎºÎÈ¨ÀûÒÔ¼°ÈÎºÎ·¨ÂÉÔðÈÎ¡£
// 
// ±¾Ô´´úÂë¿ÉÍ¨¹ýÒÔÏÂÍøÖ·»ñÈ¡:
// http://QQCode.lynfo.com, http://www.lynfo.com, http://bbs.lynfo.com, http://hi.baidu.com/dobit.
//
// Copyright @ 2009-2010  Lynfo.com.  All Rights Reserved.   
// Framework: 2.0
// Author: Luma(java°æ) ¡ú Abu(C# QQ2005Ð­Òé°æ) ¡ú Dobit(C# QQ2009Ð­Òé°æ±¾)
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

namespace Org.Mentalis.Network.ProxySocket {
	/// <summary>
	/// References the callback method to be called when the protocol negotiation is completed.
	/// </summary>
	internal delegate void HandShakeComplete(Exception error);
	/// <summary>
	/// Implements a specific version of the SOCKS protocol. This is an abstract class; it must be inherited.
	/// </summary>
	internal abstract class SocksHandler {
		/// <summary>
		/// Initilizes a new instance of the SocksHandler class.
		/// </summary>
		/// <param name="server">The socket connection with the proxy server.</param>
		/// <param name="user">The username to use when authenticating with the server.</param>
		/// <exception cref="ArgumentNullException"><c>server</c> -or- <c>user</c> is null.</exception>
		public SocksHandler(Socket server, string user) {
			Server = server;
			Username = user;
		}
		/// <summary>
		/// Converts a port number to an array of bytes.
		/// </summary>
		/// <param name="port">The port to convert.</param>
		/// <returns>An array of two bytes that represents the specified port.</returns>
		protected byte[] PortToBytes(int port) {
			byte [] ret = new byte[2];
			ret[0] = (byte)(port / 256);
			ret[1] = (byte)(port % 256);
			return ret;
		}
		/// <summary>
		/// Converts an IP address to an array of bytes.
		/// </summary>
		/// <param name="address">The IP address to convert.</param>
		/// <returns>An array of four bytes that represents the specified IP address.</returns>
		protected byte[] AddressToBytes(long address) {
			byte [] ret = new byte[4];
			ret[0] = (byte)(address % 256);
			ret[1] = (byte)((address / 256) % 256);
			ret[2] = (byte)((address / 65536) % 256);
			ret[3] = (byte)(address / 16777216);
			return ret;
		}
		/// <summary>
		/// Reads a specified number of bytes from the Server socket.
		/// </summary>
		/// <param name="count">The number of bytes to return.</param>
		/// <returns>An array of bytes.</returns>
		/// <exception cref="ArgumentException">The number of bytes to read is invalid.</exception>
		/// <exception cref="SocketException">An operating system error occurs while accessing the Socket.</exception>
		/// <exception cref="ObjectDisposedException">The Socket has been closed.</exception>
		protected byte[] ReadBytes(int count) {
			if (count <= 0)
				throw new ArgumentException();
			byte[] buffer = new byte[count];
			int received = 0;
			while(received != count) {
				received += Server.Receive(buffer, received, count - received, SocketFlags.None);
			}
			return buffer;
		}
		/// <summary>
		/// Gets or sets the socket connection with the proxy server.
		/// </summary>
		/// <value>A Socket object that represents the connection with the proxy server.</value>
		/// <exception cref="ArgumentNullException">The specified value is null.</exception>
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
		/// Gets or sets the username to use when authenticating with the proxy server.
		/// </summary>
		/// <value>A string that holds the username to use when authenticating with the proxy server.</value>
		/// <exception cref="ArgumentNullException">The specified value is null.</exception>
		protected string Username {
			get {
				return m_Username;
			}
			set {
				if (value == null)
					throw new ArgumentNullException();
				m_Username = value;
			}
		}
		/// <summary>
		/// Gets or sets the return value of the BeginConnect call.
		/// </summary>
		/// <value>An IAsyncProxyResult object that is the return value of the BeginConnect call.</value>
		protected IAsyncProxyResult AsyncResult {
			get {
				return m_AsyncResult;
			}
			set {
				m_AsyncResult = value;
			}
		}
		/// <summary>
		/// Gets or sets a byte buffer.
		/// </summary>
		/// <value>An array of bytes.</value>
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
		/// <summary>Holds the value of the Server property.</summary>
		private Socket m_Server;
		/// <summary>Holds the value of the Username property.</summary>
		private string m_Username;
		/// <summary>Holds the value of the AsyncResult property.</summary>
		private IAsyncProxyResult m_AsyncResult;
		/// <summary>Holds the value of the Buffer property.</summary>
		private byte[] m_Buffer;
		/// <summary>Holds the value of the Received property.</summary>
		private int m_Received;
		/// <summary>Holds the address of the method to call when the SOCKS protocol has been completed.</summary>
		protected HandShakeComplete ProtocolComplete;
		/// <summary>
		/// Starts negotiating with a SOCKS proxy server.
		/// </summary>
		/// <param name="host">The remote server to connect to.</param>
		/// <param name="port">The remote port to connect to.</param>
		public abstract void Negotiate(string host, int port);
		/// <summary>
		/// Starts negotiating with a SOCKS proxy server.
		/// </summary>
		/// <param name="remoteEP">The remote endpoint to connect to.</param>
		public abstract void Negotiate(IPEndPoint remoteEP);
		/// <summary>
		/// Starts negotiating asynchronously with a SOCKS proxy server.
		/// </summary>
		/// <param name="remoteEP">An IPEndPoint that represents the remote device. </param>
		/// <param name="callback">The method to call when the connection has been established.</param>
		/// <param name="proxyEndPoint">The IPEndPoint of the SOCKS proxy server.</param>
		/// <returns>An IAsyncProxyResult that references the asynchronous connection.</returns>
		public abstract IAsyncProxyResult BeginNegotiate(IPEndPoint remoteEP, HandShakeComplete callback, IPEndPoint proxyEndPoint);
		/// <summary>
		/// Starts negotiating asynchronously with a SOCKS proxy server.
		/// </summary>
		/// <param name="host">The remote server to connect to.</param>
		/// <param name="port">The remote port to connect to.</param>
		/// <param name="callback">The method to call when the connection has been established.</param>
		/// <param name="proxyEndPoint">The IPEndPoint of the SOCKS proxy server.</param>
		/// <returns>An IAsyncProxyResult that references the asynchronous connection.</returns>
		public abstract IAsyncProxyResult BeginNegotiate(string host, int port, HandShakeComplete callback, IPEndPoint proxyEndPoint);
	}
}