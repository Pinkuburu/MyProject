/*
    Copyright ?2002, The KPD-Team
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
using System.Net;
using System.Net.Sockets;

// Implements a number of classes to allow Sockets to connect trough a firewall.
namespace Org.Mentalis.Network.ProxySocket {
	/// <summary>
	/// Specifies the type of proxy servers that an instance of the ProxySocket class can use.
	/// </summary>
	public enum ProxyTypes {
		/// <summary>No proxy server; the ProxySocket object behaves exactly like an ordinary Socket object.</summary>
		None,
		/// <summary>A SOCKS4[A] proxy server.</summary>
		Socks4,
		/// <summary>A SOCKS5 proxy server.</summary>
		Socks5
	}
	/// <summary>
	/// Implements a Socket class that can connect trough a SOCKS proxy server.
	/// </summary>
	/// <remarks>This class implements SOCKS4[A] and SOCKS5.<br>It does not, however, implement the BIND commands, so you cannot .</br></remarks>
	public class ProxySocket : Socket {
		/// <summary>
		/// Initializes a new instance of the ProxySocket class.
		/// </summary>
		/// <param name="addressFamily">One of the AddressFamily values.</param>
		/// <param name="socketType">One of the SocketType values.</param>
		/// <param name="protocolType">One of the ProtocolType values.</param>
		/// <exception cref="SocketException">The combination of addressFamily, socketType, and protocolType results in an invalid socket.</exception>
		public ProxySocket (AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType) : this(addressFamily, socketType, protocolType, "") {}
		/// <summary>
		/// Initializes a new instance of the ProxySocket class.
		/// </summary>
		/// <param name="addressFamily">One of the AddressFamily values.</param>
		/// <param name="socketType">One of the SocketType values.</param>
		/// <param name="protocolType">One of the ProtocolType values.</param>
		/// <param name="proxyUsername">The username to use when authenticating with the proxy server.</param>
		/// <exception cref="SocketException">The combination of addressFamily, socketType, and protocolType results in an invalid socket.</exception>
		/// <exception cref="ArgumentNullException"><c>proxyUsername</c> is null.</exception>
		public ProxySocket (AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType, string proxyUsername) : this(addressFamily, socketType, protocolType, proxyUsername, "") {}
		/// <summary>
		/// Initializes a new instance of the ProxySocket class.
		/// </summary>
		/// <param name="addressFamily">One of the AddressFamily values.</param>
		/// <param name="socketType">One of the SocketType values.</param>
		/// <param name="protocolType">One of the ProtocolType values.</param>
		/// <param name="proxyUsername">The username to use when authenticating with the proxy server.</param>
		/// <param name="proxyPassword">The password to use when authenticating with the proxy server.</param>
		/// <exception cref="SocketException">The combination of addressFamily, socketType, and protocolType results in an invalid socket.</exception>
		/// <exception cref="ArgumentNullException"><c>proxyUsername</c> -or- <c>proxyPassword</c> is null.</exception>
		public ProxySocket (AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType, string proxyUsername, string proxyPassword) : base(addressFamily, socketType, protocolType) {
			ProxyUser = proxyUsername;
			ProxyPass = proxyPassword;
			ToThrow = new InvalidOperationException();
		}
		/// <summary>
		/// Establishes a connection to a remote device.
		/// </summary>
		/// <param name="remoteEP">An EndPoint that represents the remote device.</param>
		/// <exception cref="ArgumentNullException">The remoteEP parameter is a null reference (Nothing in Visual Basic).</exception>
		/// <exception cref="SocketException">An operating system error occurs while accessing the Socket.</exception>
		/// <exception cref="ObjectDisposedException">The Socket has been closed.</exception>
		/// <exception cref="ProxyException">An error Occurred while talking to the proxy server.</exception>
		public new void Connect(EndPoint remoteEP) {
			if (remoteEP == null)
				throw new ArgumentNullException("<remoteEP> cannot be null.");
			if (this.ProtocolType != ProtocolType.Tcp || ProxyType == ProxyTypes.None || ProxyEndPoint == null)
				base.Connect(remoteEP);
			else {
				base.Connect(ProxyEndPoint);
				if (ProxyType == ProxyTypes.Socks4)
					(new Socks4Handler(this, ProxyUser)).Negotiate((IPEndPoint)remoteEP);
				else if (ProxyType == ProxyTypes.Socks5)
					(new Socks5Handler(this, ProxyUser, ProxyPass)).Negotiate((IPEndPoint)remoteEP);
			}
		}
		/// <summary>
		/// Establishes a connection to a remote device.
		/// </summary>
		/// <param name="host">The remote host to connect to.</param>
		/// <param name="port">The remote port to connect to.</param>
		/// <exception cref="ArgumentNullException">The host parameter is a null reference (Nothing in Visual Basic).</exception>
		/// <exception cref="ArgumentException">The port parameter is invalid.</exception>
		/// <exception cref="SocketException">An operating system error occurs while accessing the Socket.</exception>
		/// <exception cref="ObjectDisposedException">The Socket has been closed.</exception>
		/// <exception cref="ProxyException">An error Occurred while talking to the proxy server.</exception>
		/// <remarks>If you use this method with a SOCKS4 server, it will let the server resolve the hostname. Not all SOCKS4 servers support this 'remote DNS' though.</remarks>
		new public void Connect(string host, int port) {
			if (host == null)
				throw new ArgumentNullException("<host> cannot be null.");
			if (port <= 0 || port > 65535)
				throw new ArgumentException("Invalid port.");
			if (this.ProtocolType != ProtocolType.Tcp || ProxyType == ProxyTypes.None || ProxyEndPoint == null)
				base.Connect(new IPEndPoint(Dns.GetHostEntry(host).AddressList[0], port));
			else {
				base.Connect(ProxyEndPoint);
				if (ProxyType == ProxyTypes.Socks4)
					(new Socks4Handler(this, ProxyUser)).Negotiate(host, port);
				else if (ProxyType == ProxyTypes.Socks5)
					(new Socks5Handler(this, ProxyUser, ProxyPass)).Negotiate(host, port);
			}
		}
		/// <summary>
		/// Begins an asynchronous request for a connection to a network device.
		/// </summary>
		/// <param name="remoteEP">An EndPoint that represents the remote device.</param>
		/// <param name="callback">The AsyncCallback delegate.</param>
		/// <param name="state">An object that contains state information for this request.</param>
		/// <returns>An IAsyncResult that references the asynchronous connection.</returns>
		/// <exception cref="ArgumentNullException">The remoteEP parameter is a null reference (Nothing in Visual Basic).</exception>
		/// <exception cref="SocketException">An operating system error occurs while creating the Socket.</exception>
		/// <exception cref="ObjectDisposedException">The Socket has been closed.</exception>
		public new IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state) {
			if (remoteEP == null || callback == null)
				throw new ArgumentNullException();
            
			if (this.ProtocolType != ProtocolType.Tcp || ProxyType == ProxyTypes.None || ProxyEndPoint == null) {
                ToThrow = null;
				return base.BeginConnect(remoteEP, callback, state);
			} else {
                CallBack = callback;
				if (ProxyType == ProxyTypes.Socks4) {
					AsyncResult = (new Socks4Handler(this, ProxyUser)).BeginNegotiate((IPEndPoint)remoteEP, new HandShakeComplete(this.OnHandShakeComplete), ProxyEndPoint);
					return AsyncResult;
				} else if(ProxyType == ProxyTypes.Socks5) {
					AsyncResult = (new Socks5Handler(this, ProxyUser, ProxyPass)).BeginNegotiate((IPEndPoint)remoteEP, new HandShakeComplete(this.OnHandShakeComplete), ProxyEndPoint);
					return AsyncResult;
				}
				return null;
			}
		}
		/// <summary>
		/// Begins an asynchronous request for a connection to a network device.
		/// </summary>
		/// <param name="host">The host to connect to.</param>
		/// <param name="port">The port on the remote host to connect to.</param>
		/// <param name="callback">The AsyncCallback delegate.</param>
		/// <param name="state">An object that contains state information for this request.</param>
		/// <returns>An IAsyncResult that references the asynchronous connection.</returns>
		/// <exception cref="ArgumentNullException">The host parameter is a null reference (Nothing in Visual Basic).</exception>
		/// <exception cref="ArgumentException">The port parameter is invalid.</exception>
		/// <exception cref="SocketException">An operating system error occurs while creating the Socket.</exception>
		/// <exception cref="ObjectDisposedException">The Socket has been closed.</exception>
		new public IAsyncResult BeginConnect(string host, int port, AsyncCallback callback, object state) {
			if (host == null || callback == null)
				throw new ArgumentNullException();
			if (port <= 0 || port >  65535)
				throw new ArgumentException();
			CallBack = callback;
			//State = state;
			if (this.ProtocolType != ProtocolType.Tcp || ProxyType == ProxyTypes.None || ProxyEndPoint == null) {
				RemotePort = port;
				AsyncResult = BeginDns(host, new HandShakeComplete(this.OnHandShakeComplete));
				return AsyncResult;
			} else {
				if (ProxyType == ProxyTypes.Socks4) {
					AsyncResult = (new Socks4Handler(this, ProxyUser)).BeginNegotiate(host, port, new HandShakeComplete(this.OnHandShakeComplete), ProxyEndPoint);
					return AsyncResult;
				} else if(ProxyType == ProxyTypes.Socks5) {
					AsyncResult = (new Socks5Handler(this, ProxyUser, ProxyPass)).BeginNegotiate(host, port, new HandShakeComplete(this.OnHandShakeComplete), ProxyEndPoint);
					return AsyncResult;
				}
				return null;
			}
		}

		/// <summary>
		/// Ends a pending asynchronous connection request.
		/// </summary>
		/// <param name="asyncResult">Stores state information for this asynchronous operation as well as any user-defined data.</param>
		/// <exception cref="ArgumentNullException">The asyncResult parameter is a null reference (Nothing in Visual Basic).</exception>
		/// <exception cref="ArgumentException">The asyncResult parameter was not returned by a call to the BeginConnect method.</exception>
		/// <exception cref="SocketException">An operating system error occurs while accessing the Socket.</exception>
		/// <exception cref="ObjectDisposedException">The Socket has been closed.</exception>
		/// <exception cref="InvalidOperationException">EndConnect was previously called for the asynchronous connection.</exception>
		/// <exception cref="ProxyException">The proxy server refused the connection.</exception>
		public new void EndConnect(IAsyncResult asyncResult) {
			if (asyncResult == null)
				throw new ArgumentNullException();
			if (!asyncResult.IsCompleted)
				throw new ArgumentException();
			if (ToThrow != null)
				throw ToThrow;
			return;
		}
		/// <summary>
		/// Begins an asynchronous request to resolve a DNS host name or IP address in dotted-quad notation to an IPAddress instance.
		/// </summary>
		/// <param name="host">The host to resolve.</param>
		/// <param name="callback">The method to call when the hostname has been resolved.</param>
		/// <returns>An IAsyncResult instance that references the asynchronous request.</returns>
		/// <exception cref="SocketException">There was an error while trying to resolve the host.</exception>
		internal IAsyncProxyResult BeginDns(string host, HandShakeComplete callback) {
			try {
				Dns.BeginGetHostEntry(host, new AsyncCallback(this.OnResolved), this);
				return new IAsyncProxyResult();
			} catch {
				throw new SocketException();
			}
		}
		/// <summary>
		/// Called when the specified hostname has been resolved.
		/// </summary>
		/// <param name="asyncResult">The result of the asynchronous operation.</param>
		private void OnResolved(IAsyncResult asyncResult) {
			try {
				IPHostEntry dns = Dns.EndGetHostEntry(asyncResult);
				base.BeginConnect(new IPEndPoint(dns.AddressList[0], RemotePort), new AsyncCallback(this.OnConnect), State);
			} catch (Exception e) {
				OnHandShakeComplete(e);
			}
		}
		/// <summary>
		/// Called when the Socket is connected to the remote host.
		/// </summary>
		/// <param name="asyncResult">The result of the asynchronous operation.</param>
		private void OnConnect(IAsyncResult asyncResult) {
			try {
				base.EndConnect(asyncResult);
				OnHandShakeComplete(null);
			} catch (Exception e) {
				OnHandShakeComplete(e);
			}
		}
		/// <summary>
		/// Called when the Socket has finished talking to the proxy server and is ready to relay data.
		/// </summary>
		/// <param name="error">The error to throw when the EndConnect method is called.</param>
		private void OnHandShakeComplete(Exception error) {
			if (error != null)
				this.Close();
			ToThrow = error;
			AsyncResult.Reset();			
			if (CallBack != null)
				CallBack(AsyncResult);
		}
		/// <summary>
		/// Gets or sets the EndPoint of the proxy server.
		/// </summary>
		/// <value>An IPEndPoint object that holds the IP address and the port of the proxy server.</value>
		public IPEndPoint ProxyEndPoint {
			get {
				return m_ProxyEndPoint;
			}
			set {
				m_ProxyEndPoint = value;
			}
		}
		/// <summary>
		/// Gets or sets the type of proxy server to use.
		/// </summary>
		/// <value>One of the ProxyTypes values.</value>
		public ProxyTypes ProxyType {
			get {
				return m_ProxyType;
			}
			set {
				m_ProxyType = value;
			}
		}
		/// <summary>
		/// Gets or sets a user-defined object.
		/// </summary>
		/// <value>The user-defined object.</value>
		private object State {
			get {
				return m_State;
			}
			set {
				m_State = value;
			}
		}
		/// <summary>
		/// Gets or sets the username to use when authenticating with the proxy.
		/// </summary>
		/// <value>A string that holds the username that's used when authenticating with the proxy.</value>
		/// <exception cref="ArgumentNullException">The specified value is null.</exception>
		public string ProxyUser {
			get {
				return m_ProxyUser;
			}
			set {
				if (value == null)
					throw new ArgumentNullException();
				m_ProxyUser = value;
			}
		}
		/// <summary>
		/// Gets or sets the password to use when authenticating with the proxy.
		/// </summary>
		/// <value>A string that holds the password that's used when authenticating with the proxy.</value>
		/// <exception cref="ArgumentNullException">The specified value is null.</exception>
		public string ProxyPass {
			get {
				return m_ProxyPass;
			}
			set {
				if (value == null)
					throw new ArgumentNullException();
				m_ProxyPass = value;
			}
		}
		/// <summary>
		/// Gets or sets the asynchronous result object.
		/// </summary>
		/// <value>An instance of the IAsyncProxyResult class.</value>
		private IAsyncProxyResult AsyncResult {
			get {
				return m_AsyncResult;
			}
			set {
				m_AsyncResult = value;
			}
		}
		/// <summary>
		/// Gets or sets the exception to throw when the EndConnect method is called.
		/// </summary>
		/// <value>An instance of the Exception class (or subclasses of Exception).</value>
		private Exception ToThrow {
			get {
				return m_ToThrow;
			}
			set {
				m_ToThrow = value;
			}
		}
		/// <summary>
		/// Gets or sets the remote port the user wants to connect to.
		/// </summary>
		/// <value>An integer that specifies the port the user wants to connect to.</value>
		private int RemotePort {
			get {
				return m_RemotePort;
			}
			set {
				m_RemotePort = value;
			}
		}
		// private variables
		/// <summary>Holds the value of the State property.</summary>
		private object m_State;
		/// <summary>Holds the value of the ProxyEndPoint property.</summary>
		private IPEndPoint m_ProxyEndPoint = null;
		/// <summary>Holds the value of the ProxyType property.</summary>
		private ProxyTypes m_ProxyType = ProxyTypes.None;
		/// <summary>Holds the value of the ProxyUser property.</summary>
		private string m_ProxyUser = null;
		/// <summary>Holds the value of the ProxyPass property.</summary>
		private string m_ProxyPass = null;
		/// <summary>Holds a pointer to the method that should be called when the Socket is connected to the remote device.</summary>
		private AsyncCallback CallBack = null;
		/// <summary>Holds the value of the AsyncResult property.</summary>
		private IAsyncProxyResult m_AsyncResult;
		/// <summary>Holds the value of the ToThrow property.</summary>
		private Exception m_ToThrow = null;
		/// <summary>Holds the value of the RemotePort property.</summary>
		private int m_RemotePort;
	}
}