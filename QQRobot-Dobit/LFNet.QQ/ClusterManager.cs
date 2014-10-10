

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
    /// <summary>群功能
    /// 得到我加入的所有群,需要通过FriendManager.DownloadGroupFriends来得到
    /// 在返回的数据包当中,有标识该好友是普通好友,还是群好友.
    /// 	<remark>abu 2008-03-29 </remark>
    /// </summary>
    public class ClusterManager
    {
        /// <summary>
        /// 	<remark>abu 2008-03-07 </remark>
        /// </summary>
        /// <value></value>
        public QQClient QQClient { get; private set; }
        /// <summary>
        /// 	<remark>abu 2008-03-29 </remark>
        /// </summary>
        /// <value></value>
        public QQUser QQUser { get { return QQClient.QQUser; } }
        /// <summary>
        /// 	<remark>abu 2008-03-29 </remark>
        /// </summary>
        /// <param name="client">The client.</param>
        public ClusterManager(QQClient client)
        {
            QQClient = client;
        }

        /// <summary>加入群
        /// Joins the cluster.
        /// </summary>
        /// <param name="clusterId">The cluster id.</param>
        public void JoinCluster(int clusterId)
        {
            ClusterJoinPacket packet = new ClusterJoinPacket(QQClient);
            packet.ClusterId = clusterId;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }
        /// <summary>发送加入群请求信息
        /// Requests the join cluster.
        /// </summary>
        /// <param name="clusterId">The cluster id.</param>
        /// <param name="message">The message.</param>
        public void RequestJoinCluster(int clusterId, string message)
        {
            ClusterAuthPacket packet = new ClusterAuthPacket(QQClient);
            packet.Type = (int)ClusterAuth.Request;
            packet.ClusterId = clusterId;
            packet.Message = message;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }
        /// <summary>查找群
        /// Searches the cluster by id.
        /// </summary>
        /// <param name="externalId">The external id.</param>
        public void SearchClusterById(int externalId)
        {
            ClusterSearchPacket packet = new ClusterSearchPacket(QQClient);
            packet.ExternalId = externalId;
            packet.SearchType = ClusterSearchType.By_ID;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }
        /// <summary>退出群
        /// Exits the cluster.
        /// </summary>
        /// <param name="externalId">The external id.</param>
        public void ExitCluster(int externalId)
        {
            ClusterExitPacket packet = new ClusterExitPacket(QQClient);
            packet.ClusterId = externalId;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }
        /// <summary>
        /// Sends the cluster IM.
        /// </summary>
        /// <param name="clusterId">The cluster id.</param>
        /// <param name="message">The message.</param>
        public void SendClusterIM(int clusterId, string message)
        {
            int MaxByte = QQGlobal.QQ_MAX_SEND_IM;//取最长长度
            if (Encoding.GetEncoding(QQGlobal.QQ_CHARSET_DEFAULT).GetBytes(message).Length > MaxByte)//判断是不是要分段发送
            {
                List<byte> messageBytes = new List<byte>();
                messageBytes.AddRange(Utils.Util.GetBytes(message));
                //messageBytes.Add(0x20);//补一个空格，不补似乎也会出问题
                int messageSize = messageBytes.Count;

                int totalFragments = ((messageSize % MaxByte) > 0) ? (messageSize / MaxByte + 1) : (messageSize / MaxByte);//计算分片数
                for (int fragementSequence = 0; fragementSequence < totalFragments; fragementSequence++)
                {
                    int index = fragementSequence * MaxByte;
                    int BytesSize = ((messageSize - index) > MaxByte) ? MaxByte : (messageSize - index);//不能每次都申请最大长度的byte数组，不然字体会出问题
                    byte[] messageFragementBytes = new byte[BytesSize];


                    messageBytes.CopyTo(index, messageFragementBytes, 0, BytesSize);
                    SendClusterIM(clusterId, messageFragementBytes, totalFragments, fragementSequence,(ushort) messageBytes.Count,new FontStyle());


                }
            }
            else
            {
                SendClusterIM(clusterId, Utils.Util.GetBytes(message), 1, 0);
            }
        }
        /// <summary>发送群信息
        /// Sends the cluster IM.
        /// </summary>
        /// <param name="clusterId">The cluster id.</param>
        /// <param name="message">The message.</param>
        /// <param name="totalFragments">The total fragments.</param>
        /// <param name="fragmentSequence">The fragment sequence.</param>
        public void SendClusterIM(int clusterId, byte[] message, int totalFragments, int fragmentSequence)
        {
            SendClusterIM(clusterId, message, totalFragments, fragmentSequence,0, new FontStyle());
        }
        /// <summary>发送群信息
        /// Sends the cluster IM.
        /// </summary>
        /// <param name="clusterId">The cluster id.</param>
        /// <param name="message">The message.</param>
        /// <param name="totalFragments">The total fragments.</param>
        /// <param name="fragmentSequence">The fragment sequence.</param>
        /// <param name="fontStyle">The font style.</param>
        public void SendClusterIM(int clusterId, byte[] message, int totalFragments, int fragmentSequence,ushort messageId, FontStyle fontStyle)
        {
            ClusterSendIMExPacket packet = new ClusterSendIMExPacket(QQClient);
            packet.ClusterId = clusterId;
            packet.MessageBytes = message;
            packet.MessageId = messageId;
            packet.TotalFragments = totalFragments;
            packet.FragmentSequence = fragmentSequence;
            packet.FontStyle = fontStyle;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }
        public void SendClusterIM(int clusterId, string message, int totalFragments, int fragmentSequence, FontStyle fontStyle)
        {
            ClusterSendIMExPacket packet = new ClusterSendIMExPacket(QQClient);
            packet.ClusterId = clusterId;
            packet.Message = message;
            packet.TotalFragments = totalFragments;
            packet.FragmentSequence = fragmentSequence;
            packet.FontStyle = fontStyle;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }
        /// <summary>请求群信息
        /// <remarks>
        /// 返回的ClusterCommandReplyPacket字段包括:
        /// Info
        /// Members
        /// </remarks>
        /// Gets the cluster info.
        /// </summary>
        /// <param name="clusterId">The cluster id.群内部ID</param>
        public void GetClusterInfo(int clusterId)
        {
            ClusterGetInfoPacket packet = new ClusterGetInfoPacket(QQClient);
            packet.ClusterId = clusterId;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }

        /// <summary>得到群中成员的信息
        /// Gets the cluster member info.
        /// <remarks>
        /// 返回的ClusterCommandReplyPacket字段包括:
        /// MemberInfos
        /// </remarks>
        /// </summary>
        /// <param name="clusterId">The cluster id.群的内部ID</param>
        /// <param name="members">The members.成员的QQ号列表，元素类型是Integer或者Member</param>
        public void GetClusterMemberInfo(int clusterId, int[] members)
        {
            // 由于一次最多只能得到61个成员的信息，所以这里按照30个成员一组进行拆分
            // 因为QQ是一次拆这么多
            int times = (members.Length + 29) / 30;
            for (int i = 0; i < times; i++)
            {
                ClusterGetMemberInfoPacket packet = new ClusterGetMemberInfoPacket(QQClient);
                packet.ClusterId = clusterId;
                for (int j = 30 * i; j < 30 * i + 30 && j < members.Length; j++)
                {
                    packet.Members.Add(members[j]);
                }
                QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
            }
        }
        /// <summary>修改群名片
        /// Modifies the card.
        /// </summary>
        /// <param name="clusterId">The cluster id.</param>
        /// <param name="card">The card.</param>
        public void ModifyCard(int clusterId, Card card)
        {
            ClusterModifyCardPacket packet = new ClusterModifyCardPacket(QQClient);
            packet.ClusterId = clusterId;
            packet.Card = card;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }
        /// <summary>更新群组织结构
        /// Updates the organization.
        /// </summary>
        /// <param name="clusterId">The cluster id.</param>
        public void UpdateOrganization(int clusterId)
        {
            ClusterUpdateOrganizationPacket packet = new ClusterUpdateOrganizationPacket(QQClient);
            packet.ClusterId = clusterId;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }

        /// <summary>获取群在线成员
        /// Gets the cluster online member.
        /// </summary>
        /// <param name="clusterId">The cluster id.</param>
        public void GetClusterOnlineMember(int clusterId)
        {
            ClusterGetOnlineMemberPacket packet = new ClusterGetOnlineMemberPacket(QQClient);
            packet.ClusterId = clusterId;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }

        /// <summary>得到群成员名片
        /// Gets the card.
        /// </summary>
        /// <param name="clusterId">The cluster id.</param>
        /// <param name="qq">The qq.</param>
        public void GetCard(int clusterId, int qq)
        {
            ClusterGetCardPacket packet = new ClusterGetCardPacket(QQClient);
            packet.ClusterId = clusterId;
            packet.QQ = qq;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }
        /// <summary>成批得到群成员名片
        /// Gets the card batch.
        /// <remarks>注意,当ClusterCommandReplyPacket的NextStart值为0,表示已经得到所有的群成员名片,否则它的值就是下一个成员的开始下标
        /// </remarks>
        /// </summary>
        /// <param name="clusterId">The cluster id.</param>
        /// <param name="start">The start.</param>
        public void GetCardBatch(int clusterId, int start)
        {
            ClusterGetCardBatchPacket packet = new ClusterGetCardBatchPacket(QQClient);
            packet.ClusterId = clusterId;
            packet.Start = start;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }
        /// <summary>请求得到多人对话列表
        /// Gets the dialog list.
        /// </summary>
        public void GetDialogList()
        {
            ClusterSubClusterOpPacket packet = new ClusterSubClusterOpPacket(QQClient);
            packet.OpByte = ClusterSubCmd.GET_DIALOG_LIST;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }

        /// <summary>创建一个临时群
        /// Creates the temporary cluster.
        /// </summary>
        /// <param name="name">The name.群名称</param>
        /// <param name="type">The type.临时群类型</param>
        /// <param name="parentClusterId">The parent cluster id.父群内部ID</param>
        /// <param name="members">The members.成员QQ号数组</param>
        public void CreateTemporaryCluster(string name, ClusterType type, int parentClusterId, List<int> members)
        {
            ClusterCreateTempPacket packet = new ClusterCreateTempPacket(QQClient);
            packet.Type = type;
            packet.Name = name;
            packet.Members = members;
            packet.ParentClusterId = parentClusterId;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }
        /// <summary>创建一个固定群
        /// Creates the permanent cluster.
        /// </summary>
        /// <param name="name">The name.群名称</param>
        /// <param name="notice">The notice.群声明</param>
        /// <param name="desription">The desription.群描述</param>
        /// <param name="members">The members.群成员</param>
        /// <param name="category">The category.群的分类</param>
        /// <param name="authType">Type of the auth.群认证类型</param>
        public void CreatePermanentCluster(string name, string notice, string desription, List<int> members, int category, AuthType authType)
        {
            ClusterCreatePacket packet = new ClusterCreatePacket(QQClient);
            packet.Type = ClusterType.PERMANENT;
            packet.AuthType = authType;
            packet.Category = category;
            packet.Name = name;
            packet.Notice = notice;
            packet.Description = desription;
            packet.Members = members;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }
        #region events
        /// <summary>加入群成功
        /// Occurs when [cluster join successed].
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterJoinPacket>> JoinClusterSuccessed;
        /// <summary>
        /// Raises the <see cref="E:JoinClusterSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ClusterCommandReplyPacket,LFNet.QQ.Packets.Out.ClusterJoinPacket&gt;"/> instance containing the event data.</param>
        internal void OnClusterJoinSuccessed(QQEventArgs<ClusterCommandReplyPacket, ClusterJoinPacket> e)
        {
            if (JoinClusterSuccessed != null)
            {
                JoinClusterSuccessed(this, e);
            }
        }
        /// <summary>加入群需要验证信息
        /// Occurs when [join cluster need auth].
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterJoinPacket>> JoinClusterNeedAuth;
        /// <summary>
        /// Raises the <see cref="E:JoinClusterNeedAuth"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ClusterCommandReplyPacket,LFNet.QQ.Packets.Out.ClusterJoinPacket&gt;"/> instance containing the event data.</param>
        internal void OnJoinClusterNeedAuth(QQEventArgs<ClusterCommandReplyPacket, ClusterJoinPacket> e)
        {
            if (JoinClusterNeedAuth != null)
            {
                JoinClusterNeedAuth(this, e);
            }
        }
        /// <summary>被别人拒绝加入群  (发送验证消息的结果返回事件请使用RejectJoinCluster事件)
        /// Occurs when [join cluster denied].
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterJoinPacket>> JoinClusterDenied;
        /// <summary>
        /// Raises the <see cref="E:JoinClusterDenied"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.Out.ClusterCommandPacket,LFNet.QQ.Packets.Out.ClusterJoinPacket&gt;"/> instance containing the event data.</param>
        internal void OnJoinClusterDenied(QQEventArgs<ClusterCommandReplyPacket, ClusterJoinPacket> e)
        {
            if (JoinClusterDenied != null)
            {
                JoinClusterDenied(this, e);
            }
        }
        /// <summary>加入群失败
        /// Occurs when [join cluster failed].
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterJoinPacket>> JoinClusterFailed;
        /// <summary>
        /// Raises the <see cref="E:JoinClusterFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ClusterCommandReplyPacket,LFNet.QQ.Packets.Out.ClusterJoinPacket&gt;"/> instance containing the event data.</param>
        internal void OnJoinClusterFailed(QQEventArgs<ClusterCommandReplyPacket, ClusterJoinPacket> e)
        {
            if (JoinClusterFailed != null)
            {
                JoinClusterFailed(this, e);
            }
        }

        /// <summary>发送加入群验证信息成功
        /// Occurs when [send join cluster auth successed].
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterAuthPacket>> SendJoinClusterAuthSuccessed;
        /// <summary>
        /// Raises the <see cref="E:SendJoinClusterAuthSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ClusterCommandReplyPacket,LFNet.QQ.Packets.Out.ClusterAuthPacket&gt;"/> instance containing the event data.</param>
        internal void OnSendJoinClusterAuthSuccessed(QQEventArgs<ClusterCommandReplyPacket, ClusterAuthPacket> e)
        {
            if (SendJoinClusterAuthSuccessed != null)
            {
                SendJoinClusterAuthSuccessed(this, e);
            }
        }
        /// <summary>发送加入群验证信息失败
        /// Occurs when [send join cluster auth failed].
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterAuthPacket>> SendJoinClusterAuthFailed;
        /// <summary>
        /// Raises the <see cref="E:SendJoinClusterAuthFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ClusterCommandReplyPacket,LFNet.QQ.Packets.Out.ClusterAuthPacket&gt;"/> instance containing the event data.</param>
        internal void OnSendJoinClusterAuthFailed(QQEventArgs<ClusterCommandReplyPacket, ClusterAuthPacket> e)
        {
            if (SendJoinClusterAuthFailed != null)
            {
                SendJoinClusterAuthFailed(this, e);
            }
        }

        /// <summary>
        /// Occurs when [search cluster successed].
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterSearchPacket>> SearchClusterSuccessed;
        /// <summary>
        /// Raises the <see cref="E:SearchClusterSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ClusterCommandReplyPacket,LFNet.QQ.Packets.Out.ClusterSearchPacket&gt;"/> instance containing the event data.</param>
        internal void OnSearchClusterSuccessed(QQEventArgs<ClusterCommandReplyPacket, ClusterSearchPacket> e)
        {
            if (SearchClusterSuccessed != null)
            {
                SearchClusterSuccessed(this, e);
            }
        }
        /// <summary>
        /// Occurs when [search cluster failed].
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterSearchPacket>> SearchClusterFailed;
        /// <summary>
        /// Raises the <see cref="E:SearchClusterFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ClusterCommandReplyPacket,LFNet.QQ.Packets.Out.ClusterSearchPacket&gt;"/> instance containing the event data.</param>
        internal void OnSearchClusterFailed(QQEventArgs<ClusterCommandReplyPacket, ClusterSearchPacket> e)
        {
            if (SearchClusterFailed != null)
            {
                SearchClusterFailed(this, e);
            }
        }

        /// <summary>
        /// Occurs when [exit cluster successed].
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterExitPacket>> ExitClusterSuccessed;
        /// <summary>
        /// Raises the <see cref="E:ExitClusterSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ClusterCommandReplyPacket,LFNet.QQ.Packets.Out.ClusterExitPacket&gt;"/> instance containing the event data.</param>
        internal void OnExitClusterSuccessed(QQEventArgs<ClusterCommandReplyPacket, ClusterExitPacket> e)
        {
            if (ExitClusterSuccessed != null)
            {
                ExitClusterSuccessed(this, e);
            }
        }
        /// <summary>
        /// Occurs when [exit cluster failed].
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterExitPacket>> ExitClusterFailed;
        /// <summary>
        /// Raises the <see cref="E:ExitClusterFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ClusterCommandReplyPacket,LFNet.QQ.Packets.Out.ClusterExitPacket&gt;"/> instance containing the event data.</param>
        internal void OnExitClusterFailed(QQEventArgs<ClusterCommandReplyPacket, ClusterExitPacket> e)
        {
            if (ExitClusterFailed != null)
            {
                ExitClusterFailed(this, e);
            }
        }

        /// <summary>
        /// Occurs when [send cluster IM successed].
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterSendIMExPacket>> SendClusterIMExSuccessed;
        /// <summary>
        /// Raises the <see cref="E:SendClusterIMExSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ClusterCommandReplyPacket,LFNet.QQ.Packets.Out.ClusterSendIMExPacket&gt;"/> instance containing the event data.</param>
        internal void OnSendClusterIMExSuccessed(QQEventArgs<ClusterCommandReplyPacket, ClusterSendIMExPacket> e)
        {
            if (SendClusterIMExSuccessed != null)
            {
                SendClusterIMExSuccessed(this, e);
            }
        }

        /// <summary>
        /// Occurs when [send cluster IM failed].
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterSendIMExPacket>> SendClusterIMExFailed;
        /// <summary>
        /// Raises the <see cref="E:SendClusterIMExFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ClusterCommandReplyPacket,LFNet.QQ.Packets.Out.ClusterSendIMExPacket&gt;"/> instance containing the event data.</param>
        internal void OnSendClusterIMExFailed(QQEventArgs<ClusterCommandReplyPacket, ClusterSendIMExPacket> e)
        {
            if (SendClusterIMExFailed != null)
            {
                SendClusterIMExFailed(this, e);
            }
        }
        /// <summary>
        /// 存放临时分片包
        /// </summary>
        Dictionary<int, Dictionary<int, byte[]>> fragments = new Dictionary<int, Dictionary<int, byte[]>>();
        /// <summary>接收到普通群消息
        /// 注意ReceiveIMPacket中的属性并不是都有用。
        /// 并且event args中的OutPacket始终为空
        /// Occurs when [receive cluster IM].
        /// </summary>
        public event EventHandler<QQEventArgs<ReceiveIMPacket, OutPacket>> ReceiveClusterIM;
        /// <summary>
        /// Raises the <see cref="E:ReceiveClusterIM"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ReceiveIMPacket,LFNet.QQ.Packets.OutPacket&gt;"/> instance containing the event data.</param>
        internal void OnReceiveClusterIM(QQEventArgs<ReceiveIMPacket, OutPacket> e)
        {
            if (e.InPacket.ClusterIM.FragmentCount > 1)
            {
                if (!fragments.ContainsKey(e.InPacket.ClusterIM.MessageId))
                {
                    fragments.Add(e.InPacket.ClusterIM.MessageId, new Dictionary<int, byte[]>());
                }
                Dictionary<int, byte[]> messageFragments = fragments[e.InPacket.ClusterIM.MessageId];
                if (!messageFragments.ContainsKey(e.InPacket.ClusterIM.FragmentSequence))
                {
                    messageFragments.Add(e.InPacket.ClusterIM.FragmentSequence, e.InPacket.ClusterIM.MessageBytes);
                }
                //如果消息分片还没有接收完
                if (messageFragments.Count < e.InPacket.ClusterIM.FragmentCount)
                {
                    return;
                }
                //已经接收到了所有分片包，要注意，包的接收顺序可能不是顺序接收到的。
                //合成包
                List<byte> messageBytes = new List<byte>();
                for (int i = 0; i < e.InPacket.ClusterIM.FragmentCount; i++)
                {
                    messageBytes.AddRange(messageFragments[i]);
                }
                fragments.Remove(e.InPacket.ClusterIM.MessageId);
                e.InPacket.ClusterIM.MessageBytes = messageBytes.ToArray();
            }
            QQClient.LogManager.Log(e.InPacket.ClusterIM.Message);
            if (ReceiveClusterIM != null)
            {
                ReceiveClusterIM(this, e);
            }
        }

        /// <summary>被加入到群
        /// Occurs when [added to cluster].
        /// </summary>
        public event EventHandler<QQEventArgs<ReceiveIMPacket, OutPacket>> AddedToCluster;
        /// <summary>
        /// Raises the <see cref="E:AddedToCluster"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ReceiveIMPacket,LFNet.QQ.Packets.OutPacket&gt;"/> instance containing the event data.</param>
        internal void OnAddedToCluster(QQEventArgs<ReceiveIMPacket, OutPacket> e)
        {
            if (AddedToCluster != null)
            {
                AddedToCluster(this, e);
            }
        }

        /// <summary>从一个群中被移除
        /// Occurs when [removed from cluster].
        /// </summary>
        public event EventHandler<QQEventArgs<ReceiveIMPacket, OutPacket>> RemovedFromCluster;
        /// <summary>
        /// Raises the <see cref="E:RemovedFromCluster"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ReceiveIMPacket,LFNet.QQ.Packets.OutPacket&gt;"/> instance containing the event data.</param>
        internal void OnRemovedFromCluster(QQEventArgs<ReceiveIMPacket, OutPacket> e)
        {
            if (RemovedFromCluster != null)
            {
                RemovedFromCluster(this, e);
            }
        }

        /// <summary>对方同意我加入群，带附加消息属性Packet.Message
        /// Occurs when [approved join cluster].
        /// </summary>
        public event EventHandler<QQEventArgs<ReceiveIMPacket, OutPacket>> ApprovedJoinCluster;
        /// <summary>
        /// Raises the <see cref="E:ApprovedJoinCluster"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ReceiveIMPacket,LFNet.QQ.Packets.OutPacket&gt;"/> instance containing the event data.</param>
        internal void OnApprovedJoinCluster(QQEventArgs<ReceiveIMPacket, OutPacket> e)
        {
            if (ApprovedJoinCluster != null)
            {
                ApprovedJoinCluster(this, e);
            }
        }

        /// <summary>被拒绝加入群，带附加消息
        /// Occurs when [reject join cluster].
        /// </summary>
        public event EventHandler<QQEventArgs<ReceiveIMPacket, OutPacket>> RejectJoinCluster;
        /// <summary>
        /// Raises the <see cref="E:RejectJoinCluster"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ReceiveIMPacket,LFNet.QQ.Packets.OutPacket&gt;"/> instance containing the event data.</param>
        internal void OnRejectJoinCluster(QQEventArgs<ReceiveIMPacket, OutPacket> e)
        {
            if (RejectJoinCluster != null)
            {
                RejectJoinCluster(this, e);
            }
        }

        /// <summary>有人请求加入群，带附加消息
        /// Occurs when [has requested join cluster].
        /// </summary>
        public event EventHandler<QQEventArgs<ReceiveIMPacket, OutPacket>> HasRequestedJoinCluster;
        /// <summary>
        /// Raises the <see cref="E:HasRequestedJoinCluster"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ReceiveIMPacket,LFNet.QQ.Packets.OutPacket&gt;"/> instance containing the event data.</param>
        internal void OnHasRequestJoinCluster(QQEventArgs<ReceiveIMPacket, OutPacket> e)
        {
            if (HasRequestedJoinCluster != null)
            {
                HasRequestedJoinCluster(this, e);
            }
        }

        /// <summary>
        /// Occurs when [modify card successed].
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterModifyCardPacket>> ModifyCardSuccessed;
        /// <summary>
        /// Raises the <see cref="E:ModifyCardSuccssed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ClusterCommandReplyPacket,LFNet.QQ.Packets.Out.ClusterModifyCardPacket&gt;"/> instance containing the event data.</param>
        internal void OnModifyCardSuccssed(QQEventArgs<ClusterCommandReplyPacket, ClusterModifyCardPacket> e)
        {
            if (ModifyCardSuccessed != null)
            {
                ModifyCardSuccessed(this, e);
            }
        }

        /// <summary>
        /// Occurs when [modify card failed].
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterModifyCardPacket>> ModifyCardFailed;
        /// <summary>
        /// Raises the <see cref="E:ModifyCardFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ClusterCommandReplyPacket,LFNet.QQ.Packets.Out.ClusterModifyCardPacket&gt;"/> instance containing the event data.</param>
        internal void OnModifyCardFailed(QQEventArgs<ClusterCommandReplyPacket, ClusterModifyCardPacket> e)
        {
            if (ModifyCardFailed != null)
            {
                ModifyCardFailed(this, e);
            }
        }

        /// <summary>
        /// Occurs when [update organization successed].
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterUpdateOrganizationPacket>> UpdateOrganizationSuccessed;
        /// <summary>
        /// Raises the <see cref="E:UpdateOrganizationSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.ClusterCommand,LFNet.QQ.Packets.Out.ClusterUpdateOrganizationPacket&gt;"/> instance containing the event data.</param>
        internal void OnUpdateOrganizationSuccessed(QQEventArgs<ClusterCommandReplyPacket, ClusterUpdateOrganizationPacket> e)
        {
            if (UpdateOrganizationSuccessed != null)
            {
                UpdateOrganizationSuccessed(this, e);
            }
        }

        /// <summary>
        /// Occurs when [update organization failed].
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterUpdateOrganizationPacket>> UpdateOrganizationFailed;
        /// <summary>
        /// Raises the <see cref="E:UpdateOrganizationFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ClusterCommandReplyPacket,LFNet.QQ.Packets.Out.ClusterUpdateOrganizationPacket&gt;"/> instance containing the event data.</param>
        internal void OnUpdateOrganizationFailed(QQEventArgs<ClusterCommandReplyPacket, ClusterUpdateOrganizationPacket> e)
        {
            if (UpdateOrganizationFailed != null)
            {
                UpdateOrganizationFailed(this, e);
            }
        }

        /// <summary>
        /// Occurs when [get online member successed].
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterGetOnlineMemberPacket>> GetOnlineMemberSuccessed;
        /// <summary>
        /// Raises the <see cref="E:GetOnlineMemberSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ClusterCommandReplyPacket,LFNet.QQ.Packets.Out.ClusterGetOnlineMemberPacket&gt;"/> instance containing the event data.</param>
        internal void OnGetOnlineMemberSuccessed(QQEventArgs<ClusterCommandReplyPacket, ClusterGetOnlineMemberPacket> e)
        {
            if (GetOnlineMemberSuccessed != null)
            {
                GetOnlineMemberSuccessed(this, e);
            }
        }

        /// <summary>
        /// Occurs when [get online member failed].
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterGetOnlineMemberPacket>> GetOnlineMemberFailed;
        /// <summary>
        /// Raises the <see cref="E:GetOnlineMemberFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ClusterCommandReplyPacket,LFNet.QQ.Packets.Out.ClusterGetOnlineMemberPacket&gt;"/> instance containing the event data.</param>
        internal void OnGetOnlineMemberFailed(QQEventArgs<ClusterCommandReplyPacket, ClusterGetOnlineMemberPacket> e)
        {
            if (GetOnlineMemberFailed != null)
            {
                GetOnlineMemberSuccessed(this, e);
            }
        }

        /// <summary>
        /// Occurs when [get cluster info successed].
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterGetInfoPacket>> GetClusterInfoSuccessed;
        /// <summary>
        /// Raises the <see cref="E:GetClusterInfoSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ClusterCommandReplyPacket,LFNet.QQ.Packets.Out.ClusterGetInfoPacket&gt;"/> instance containing the event data.</param>
        internal void OnGetClusterInfoSuccessed(QQEventArgs<ClusterCommandReplyPacket, ClusterGetInfoPacket> e)
        {
            if (GetClusterInfoSuccessed != null)
            {
                GetClusterInfoSuccessed(this, e);
            }
        }

        /// <summary>
        /// Occurs when [get cluster info failed].
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterGetInfoPacket>> GetClusterInfoFailed;
        /// <summary>
        /// Raises the <see cref="E:GetClusterInfoFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ClusterCommandReplyPacket,LFNet.QQ.Packets.Out.ClusterGetInfoPacket&gt;"/> instance containing the event data.</param>
        internal void OnGetClusterInfoFailed(QQEventArgs<ClusterCommandReplyPacket, ClusterGetInfoPacket> e)
        {
            if (GetClusterInfoFailed != null)
            {
                GetClusterInfoFailed(this, e);
            }
        }

        /// <summary>
        /// Occurs when [ge member info successed].
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterGetMemberInfoPacket>> GetMemberInfoSuccessed;
        /// <summary>
        /// Raises the <see cref="E:GetMemberInfoSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ClusterCommandReplyPacket,LFNet.QQ.Packets.Out.ClusterGetMemberInfoPacket&gt;"/> instance containing the event data.</param>
        internal void OnGetMemberInfoSuccessed(QQEventArgs<ClusterCommandReplyPacket, ClusterGetMemberInfoPacket> e)
        {
            if (GetMemberInfoSuccessed != null)
            {
                GetMemberInfoSuccessed(this, e);
            }
        }

        /// <summary>
        /// Occurs when [get member info failed].
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterGetMemberInfoPacket>> GetMemberInfoFailed;
        /// <summary>
        /// Raises the <see cref="E:GetMemberInfoFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ClusterCommandReplyPacket,LFNet.QQ.Packets.Out.ClusterGetMemberInfoPacket&gt;"/> instance containing the event data.</param>
        internal void OnGetMemberInfoFailed(QQEventArgs<ClusterCommandReplyPacket, ClusterGetMemberInfoPacket> e)
        {
            if (GetMemberInfoFailed != null)
            {
                GetMemberInfoFailed(this, e);
            }
        }

        /// <summary>
        /// Occurs when [get card successed].
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterGetCardPacket>> GetCardSuccessed;
        /// <summary>
        /// Raises the <see cref="E:GetCardSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ClusterCommandReplyPacket,LFNet.QQ.Packets.Out.ClusterGetCardPacket&gt;"/> instance containing the event data.</param>
        internal void OnGetCardSuccessed(QQEventArgs<ClusterCommandReplyPacket, ClusterGetCardPacket> e)
        {
            if (GetCardSuccessed != null)
            {
                GetCardSuccessed(this, e);
            }
        }

        /// <summary>
        /// Occurs when [get card failed].
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterGetCardPacket>> GetCardFailed;
        /// <summary>
        /// Raises the <see cref="E:GetCardFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ClusterCommandReplyPacket,LFNet.QQ.Packets.Out.ClusterGetCardPacket&gt;"/> instance containing the event data.</param>
        internal void OnGetCardFailed(QQEventArgs<ClusterCommandReplyPacket, ClusterGetCardPacket> e)
        {
            if (GetCardFailed != null)
            {
                GetCardFailed(this, e);
            }
        }

        /// <summary>
        /// Occurs when [batch get card successed].
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterGetCardBatchPacket>> BatchGetCardSuccessed;
        /// <summary>
        /// Raises the <see cref="E:BatchGetCardSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ClusterCommandReplyPacket,LFNet.QQ.Packets.Out.ClusterGetCardBatchPacket&gt;"/> instance containing the event data.</param>
        internal void OnBatchGetCardSuccessed(QQEventArgs<ClusterCommandReplyPacket, ClusterGetCardBatchPacket> e)
        {
            if (BatchGetCardSuccessed != null)
            {
                BatchGetCardSuccessed(this, e);
            }
        }

        /// <summary>
        /// Occurs when [batch get card failed].
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterGetCardBatchPacket>> BatchGetCardFailed;
        /// <summary>
        /// Raises the <see cref="E:BatchGetCardFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ClusterCommandReplyPacket,LFNet.QQ.Packets.Out.ClusterGetCardBatchPacket&gt;"/> instance containing the event data.</param>
        internal void OnBatchGetCardFailed(QQEventArgs<ClusterCommandReplyPacket, ClusterGetCardBatchPacket> e)
        {
            if (BatchGetCardFailed != null)
            {
                BatchGetCardFailed(this, e);
            }
        }

        /// <summary>
        /// Occurs when [get dialog subject successed].
        /// <remarks>获取对话列表(GetDialog)和获取讨论组(GetSubject)公用事件</remarks>
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterSubClusterOpPacket>> GetDialogSubjectSuccessed;
        /// <summary>
        /// Raises the <see cref="E:GetDialogSubjectSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ClusterCommandReplyPacket,LFNet.QQ.Packets.Out.ClusterSubClusterOpPacket&gt;"/> instance containing the event data.</param>
        internal void OnGetDialogSubjectSuccessed(QQEventArgs<ClusterCommandReplyPacket, ClusterSubClusterOpPacket> e)
        {
            if (GetDialogSubjectSuccessed != null)
            {
                GetDialogSubjectSuccessed(this, e);
            }
        }

        /// <summary>
        /// Occurs when [get dialog subject failed].
        /// <remarks>获取对话列表(GetDialog)和获取讨论组(GetSubject)公用事件</remarks>
        /// </summary>
        public event EventHandler<QQEventArgs<ClusterCommandReplyPacket, ClusterSubClusterOpPacket>> GetDialogSubjectFailed;
        /// <summary>
        /// Raises the <see cref="E:GetDialogSubjectFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ClusterCommandReplyPacket,LFNet.QQ.Packets.Out.ClusterSubClusterOpPacket&gt;"/> instance containing the event data.</param>
        internal void OnGetDialogSubjectFailed(QQEventArgs<ClusterCommandReplyPacket, ClusterSubClusterOpPacket> e)
        {
            if (GetDialogSubjectFailed != null)
            {
                GetDialogSubjectFailed(this, e);
            }
        }
        #endregion
    }
}
