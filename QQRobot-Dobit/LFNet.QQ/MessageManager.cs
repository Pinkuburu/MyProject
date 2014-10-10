
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

using LFNet.QQ.Net;
using LFNet.QQ.Events;
using LFNet.QQ.Packets;
using LFNet.QQ.Entities;
using LFNet.QQ.Threading;
using LFNet.QQ.Packets.In;
using LFNet.QQ.Packets.Out;
namespace LFNet.QQ
{
    /// <summary>
    /// 信息管理
    /// <remark>abu 2008-03-10 </remark>
    /// </summary>
    public class MessageManager
    {
        internal MessageManager() { }
        /// <summary>
        /// Gets or sets the QQ client.
        /// </summary>
        /// <value>The QQ client.</value>
        public QQClient QQClient { get; private set; }
        /// <summary>
        /// Gets the QQ user.
        /// </summary>
        /// <value>The QQ user.</value>
        public QQUser QQUser { get { return QQClient.QQUser; } }
        /// <summary>
        /// 	<remark>abu 2008-03-10 </remark>
        /// </summary>
        /// <param name="client">The client.</param>
        internal MessageManager(QQClient client)
        {
            QQClient = client;
        }
        /// <summary>发送普通信息
        /// 	<remark>abu 2008-03-11 </remark>
        /// </summary>
        /// <param name="receiver">The receiver.</param>
        /// <param name="message">The message.</param>
        public void SendIM(int receiver, string message)
        {
            SendIM(receiver, message, new FontStyle());
        }
        /// <summary>
        /// 发送普通信息
        /// 	<remark>abu 2008-03-11 </remark>
        /// </summary>
        /// <param name="receiver">The receiver.</param>
        /// <param name="message">The message.</param>
        /// <param name="fontSytle">The font sytle.</param>
        public void SendIM(int receiver, string message, FontStyle fontSytle)
        {
            int MaxByte = QQGlobal.QQ_MAX_SEND_IM;//取最长长度

            //
            //发送长信息的功能由 @蓝色的风之精灵 补充，http://www.cnblogs.com/lersh/archive/2008/04/22/1165451.html
            //
            if (Encoding.GetEncoding(QQGlobal.QQ_CHARSET_DEFAULT).GetBytes(message).Length > MaxByte)//判断是不是要分段发送
            {
                List<byte> messageBytes = new List<byte>();
                messageBytes.AddRange(Utils.Util.GetBytes(message));
                messageBytes.Add(0x20);//补一个空格，不补似乎也会出问题
                int messageSize = messageBytes.Count;

                int totalFragments = ((messageSize % MaxByte) > 0) ? (messageSize / MaxByte + 1) : (messageSize / MaxByte);//计算分片数
                for (int fragementSequence = 0; fragementSequence < totalFragments; fragementSequence++)
                {
                    int index = fragementSequence * MaxByte;
                    int BytesSize = ((messageSize - index) > MaxByte) ? MaxByte : (messageSize - index);//不能每次都申请最大长度的byte数组，不然字体会出问题
                    byte[] messageFragementBytes = new byte[BytesSize];


                    messageBytes.CopyTo(index, messageFragementBytes, 0, BytesSize);
                    SendIM(receiver, messageFragementBytes, totalFragments, fragementSequence, fontSytle);


                }
            }
            else
            {
                SendIM(receiver, Utils.Util.GetBytes(message), 1, 0, fontSytle);
            }
        }
        /// <summary>
        /// 发送普通信息
        /// </summary>
        /// <param name="receiver">The receiver.</param>
        /// <param name="message">The message.</param>
        /// <param name="totalFragments">The total fragments.总分块数</param>
        /// <param name="fragementSequence">The fragement sequence.当前当块序号</param>
        /// <param name="fontSytle">The font sytle.</param>
        public void SendIM(int receiver, byte[] message, int totalFragments,
            int fragementSequence, FontStyle fontSytle)
        {
            SendIMPacket packet = new SendIMPacket(QQClient);
            packet.Receiver = receiver;
            packet.Message = message;
            packet.TotalFragments = totalFragments;
            packet.FragmentSequence = fragementSequence;
            packet.FontStyle = fontSytle;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }

        /// <summary>
        /// 发送屏闪
        /// </summary>
        /// <param name="receiver"></param>
        public void SendVibration(int receiver)
        {
            //byte[] 
            SendIMPacket packet = new SendIMPacket(QQClient);
            packet.Receiver = receiver;
            packet.MessageType = NormalIMType.Vibration;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }

        /// <summary>发送临时信息
        /// Sends the temp IM.
        /// </summary>
        /// <param name="receiver">The receiver.</param>
        /// <param name="message">The message.</param>
        /// <param name="myNick">My nick.</param>
        public void SendTempIM(int receiver, string message, string myNick)
        {
            SendTempIM(receiver, message, myNick, new FontStyle());
        }
        /// <summary>
        /// 发送临时信息
        /// <remark>abu 2008-03-11 </remark>
        /// </summary>
        /// <param name="receiver">The receiver.</param>
        /// <param name="message">The message.</param>
        /// <param name="myNick">My nick.你的昵称</param>
        /// <param name="fontSytle">The font sytle.</param>
        public void SendTempIM(int receiver, string message, string myNick, FontStyle fontSytle)
        {
            TempSessionOpPacket packet = new TempSessionOpPacket(QQClient);
            packet.SubCommand = TempSessionSubCmd.SendIM;
            packet.Receiver = receiver;
            packet.Message = message;
            packet.Nick = myNick;
            packet.FontStyle = fontSytle;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }
        /// <summary>发送接收信息回复包
        /// 	<remark>abu 2008-03-10 </remark>
        /// </summary>
        /// <param name="imPacket">The im packet.</param>
        internal void SendReceiveReplyPacket(ReceiveIMPacket inPacket)
        {
            ReceiveIMReplyPacket reply = new ReceiveIMReplyPacket(inPacket.Reply, QQClient);
            reply.Sequence = inPacket.Sequence;
            reply.Command = inPacket.Command;//修正支持2009发来的消息
            QQClient.PacketManager.SendPacket(reply);
        }
        #region events

        /// <summary>
        /// 收到一条重复信息
        /// 	<remark>abu 2008-03-10 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<ReceiveIMPacket, OutPacket>> ReceiveDuplicatedIM;
        /// <summary>
        /// Raises the <see cref="E:ReceiveDuplicatedIM"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ReceiveIMPacket&gt;"/> instance containing the event data.</param>
        internal void OnReceiveDuplicatedIM(QQEventArgs<ReceiveIMPacket, OutPacket> e)
        {
            if (ReceiveDuplicatedIM != null)
            {
                ReceiveDuplicatedIM(this, e);
            }
        }

        /// <summary>
        /// 存放临时分片包
        /// </summary>
        Dictionary<int, Dictionary<int, byte[]>> fragments = new Dictionary<int, Dictionary<int, byte[]>>();
        /// <summary>
        /// 收到一条普通信息
        /// 	<remark>abu 2008-03-10 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<ReceiveIMPacket, OutPacket>> ReceiveNormalIM;
        /// <summary>
        /// Raises the <see cref="E:ReceiveNormalIM"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ReceiveIMPacket&gt;"/> instance containing the event data.</param>
        internal void OnReceiveNormalIM(QQEventArgs<ReceiveIMPacket, OutPacket> e)
        {
            if (e.InPacket.NormalIM.TotalFragments > 1)
            {
                if (!fragments.ContainsKey(e.InPacket.NormalIM.MessageId))
                {
                    fragments.Add(e.InPacket.NormalIM.MessageId, new Dictionary<int, byte[]>());
                }
                Dictionary<int, byte[]> messageFragments = fragments[e.InPacket.NormalIM.MessageId];
                if (!messageFragments.ContainsKey(e.InPacket.NormalIM.FragmentSequence))
                {
                    messageFragments.Add(e.InPacket.NormalIM.FragmentSequence, e.InPacket.NormalIM.MessageBytes);
                }
                //如果消息分片还没有接收完
                if (messageFragments.Count < e.InPacket.NormalIM.TotalFragments)
                {
                    return;
                }
                //已经接收到了所有分片包，要注意，包的接收顺序可能不是顺序接收到的。
                //合成包
                List<byte> messageBytes = new List<byte>();
                for (int i = 0; i < e.InPacket.NormalIM.TotalFragments; i++)
                {
                    messageBytes.AddRange(messageFragments[i]);
                }
                fragments.Remove(e.InPacket.NormalIM.MessageId);
                e.InPacket.NormalIM.MessageBytes = messageBytes.ToArray();
            }
            QQClient.LogManager.Log(e.InPacket.NormalIM.Message);
            if (ReceiveNormalIM != null)
            {
                ReceiveNormalIM(this, e);
            }
        }

        /// <summary>收到一条未知类型的信息，暂时无法处理
        /// 	<remark>abu 2008-03-10 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<ReceiveIMPacket, OutPacket>> ReceiveUnknownIM;
        /// <summary>
        /// Raises the <see cref="E:ReceiveUnknownIM"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ReceiveIMPacket&gt;"/> instance containing the event data.</param>
        internal void OnReceiveUnknownIM(QQEventArgs<ReceiveIMPacket, OutPacket> e)
        {
            if (ReceiveUnknownIM != null)
            {
                ReceiveUnknownIM(this, e);
            }
        }

        /// <summary>事件在收到你的QQ号在其他地方登陆导致你被系统踢出时发生，
        /// * source是SystemNotificationPacket。系统通知和系统消息是不同的两种事件，系统通知是对你一个人发
        /// * 出的（或者是和你相关的），系统消息是一种广播式的，每个人都会收到，要分清楚这两种事件。此外
        /// * 系统通知的载体是SystemNotificationPacket，而系统消息是ReceiveIMPacket，ReceiveIMPacket的功
        /// * 能和格式很多。这也是一个区别。注意其后的我被其他人加为好友，验证被通过被拒绝等等，都是系统
        /// * 通知范畴
        /// 	<remark>abu 2008-03-10 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<ReceiveIMPacket, OutPacket>> ReceiveKickOut;
        /// <summary>
        /// Raises the <see cref="E:ReceiveKickOut"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ReceiveIMPacket&gt;"/> instance containing the event data.</param>
        internal void OnReceiveKickOut(QQEventArgs<ReceiveIMPacket, OutPacket> e)
        {
            //设置为未登录
            QQClient.LoginManager.SetLogout();
            if (ReceiveKickOut != null)
            {
                ReceiveKickOut(this, e);
            }
        }

        /// <summary>
        /// 收到系统消息
        /// 	<remark>abu 2008-03-10 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<ReceiveIMPacket, OutPacket>> ReceiveSysMessage;
        /// <summary>
        /// Raises the <see cref="E:ReceiveSysMessage"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ReceiveIMPacket&gt;"/> instance containing the event data.</param>
        internal void OnReceiveSysMessage(QQEventArgs<ReceiveIMPacket, OutPacket> e)
        {
            if (ReceiveSysMessage != null)
            {
                ReceiveSysMessage(this, e);
            }
        }

        /// <summary>事件发生在有人将我加为好友时
        /// 	<remark>abu 2008-03-12 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<SystemNotificationPacket, OutPacket>> SysAddedByOthers;
        /// <summary>
        /// Raises the <see cref="E:SysAddedByOthers"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.SystemNotificationPacket&gt;"/> instance containing the event data.</param>
        internal void OnSysAddedByOthers(QQEventArgs<SystemNotificationPacket, OutPacket> e)
        {
            if (SysAddedByOthers != null)
            {
                SysAddedByOthers(this, e);
            }
        }

        /// <summary> 事件发生在有人将我加为好友时
        /// 当对方使用0x00A8命令
        /// 	<remark>abu 2008-03-12 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<SystemNotificationPacket, OutPacket>> SysAddedByOthersEx;
        /// <summary>
        /// Raises the <see cref="E:AddedByOthersEx"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.SystemNotificationPacket&gt;"/> instance containing the event data.</param>
        internal void OnAddedByOthersEx(QQEventArgs<SystemNotificationPacket, OutPacket> e)
        {
            if (SysAddedByOthersEx != null)
            {
                SysAddedByOthersEx(this, e);
            }
        }

        /// <summary>事件发生在有人请求加我为好友时，SysAddedByOthers是我没有设置验证
        /// 是发生的，这个事件是我如果设了验证时发生的，两者不会都发生。
        /// 当对方不使用0x00A8命令发送认证消息，才会收到此系统通知
        /// 	<remark>abu 2008-03-12 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<SystemNotificationPacket, OutPacket>> SysRequestAddMe;
        /// <summary>
        /// Raises the <see cref="E:SysRequestAddMe"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.SystemNotificationPacket&gt;"/> instance containing the event data.</param>
        internal void OnSysRequestAddMe(QQEventArgs<SystemNotificationPacket, OutPacket> e)
        {
            if (SysRequestAddMe != null)
            {
                SysRequestAddMe(this, e);
            }
        }
        /// <summary>事件发生在有人请求加我为好友时
        /// 这是SysRequestAddMe的扩展事件，在2005中使用 当对方使用0x00A8命令发送认证消息，才会收到此系统通知
        /// 	<remark>abu 2008-03-12 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<SystemNotificationPacket, OutPacket>> SysRequestAddMeEx;
        /// <summary>
        /// Raises the <see cref="E:SysRequestAddMeEx"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.SystemNotificationPacket&gt;"/> instance containing the event data.</param>
        internal void OnSysRequestAddMeEx(QQEventArgs<SystemNotificationPacket, OutPacket> e)
        {
            if (SysRequestAddMeEx != null)
            {
                SysRequestAddMeEx(this, e);
            }
        }

        /// <summary>事件发生在我请求加一个人，
        /// 那个人同意我加的时候 
        /// 	<remark>abu 2008-03-12 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<SystemNotificationPacket, OutPacket>> SysAddOtherApproved;
        /// <summary>
        /// Raises the <see cref="E:SysAddOtherApproved"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.SystemNotificationPacket&gt;"/> instance containing the event data.</param>
        internal void OnSysAddOtherApproved(QQEventArgs<SystemNotificationPacket, OutPacket> e)
        {
            if (SysAddOtherApproved != null)
            {
                SysAddOtherApproved(this, e);
            }
        }

        /// <summary> 事件发生在我请求加一个人，那个人拒绝时
        /// 	<remark>abu 2008-03-12 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<SystemNotificationPacket, OutPacket>> SysAddOtherRejected;
        /// <summary>
        /// Raises the <see cref="E:SysAddOtherRejected"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.SystemNotificationPacket&gt;"/> instance containing the event data.</param>
        internal void OnSysAddOtherRejected(QQEventArgs<SystemNotificationPacket, OutPacket> e)
        {
            if (SysAddOtherRejected != null)
            {
                SysAddOtherRejected(this, e);
            }
        }

        /// <summary>广告
        /// 	<remark>abu 2008-03-12 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<SystemNotificationPacket, OutPacket>> SysAdvertisment;
        /// <summary>
        /// Raises the <see cref="E:SysAdvertisment"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.SystemNotificationPacket&gt;"/> instance containing the event data.</param>
        internal void OnSysAdvertisment(QQEventArgs<SystemNotificationPacket, OutPacket> e)
        {
            if (SysAdvertisment != null)
            {
                SysAdvertisment(this, e);
            }
        }

        /// <summary>对方同意加你为好友，并且把你加为好友
        /// 	<remark>abu 2008-03-12 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<SystemNotificationPacket, OutPacket>> SysApprovedAddOtherAndAddMe;
        /// <summary>
        /// Raises the <see cref="E:SysApprovedAddOtherAndAddMe"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.SystemNotificationPacket&gt;"/> instance containing the event data.</param>
        internal void OnSysApprovedAddOtherAndAddMe(QQEventArgs<SystemNotificationPacket, OutPacket> e)
        {
            if (SysApprovedAddOtherAndAddMe != null)
            {
                SysApprovedAddOtherAndAddMe(this, e);
            }
        }

        /// <summary>
        /// 收到一条临时会话信息
        /// 	<remark>abu 2008-03-15 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<ReceiveIMPacket, OutPacket>> ReceiveTempSessionIM;
        /// <summary>
        /// Raises the <see cref="E:ReceiveTempSessionIM"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ReceiveIMPacket,LFNet.QQ.Packets.OutPacket&gt;"/> instance containing the event data.</param>
        internal void OnReceiveTempSessionIM(QQEventArgs<ReceiveIMPacket, OutPacket> e)
        {
            if (ReceiveTempSessionIM != null)
            {
                ReceiveTempSessionIM(this, e);
            }
        }

        /// <summary>
        /// 收到振动事件
        /// </summary>
        public event EventHandler<QQEventArgs<ReceiveIMPacket, OutPacket>> ReceiveVibration;
        internal void OnReceiveVibration(QQEventArgs<ReceiveIMPacket, OutPacket> e)
        {
            if (ReceiveVibration != null)
            {
                ReceiveVibration(this, e);
            }
        }
        /// <summary>
        /// 正在输入事件
        /// </summary>
        public event EventHandler<QQEventArgs<ReceiveIMPacket, OutPacket>> ReceiveInputState;
        internal void OnInputState(QQEventArgs<ReceiveIMPacket, OutPacket> e)
        {
            if (ReceiveInputState != null)
            {
                ReceiveInputState(this, e);
            }
            
        }
        #endregion



        
    }
}
