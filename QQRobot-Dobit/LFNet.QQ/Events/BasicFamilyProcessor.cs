
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
using LFNet.QQ.Packets;
using LFNet.QQ.Packets.In;
using LFNet.QQ.Packets.Out;

namespace LFNet.QQ.Events
{
    public class BasicFamilyProcessor : IPacketListener
    {
        QQClient client;
        // QQ用户，我们将在收到登陆包后设置user不为null，所以如果user为null意味着尚未登陆
        [Obsolete]
        QQUser user;
        public BasicFamilyProcessor(QQClient client)
        {
            this.client = client;
        }
        #region IPacketListener Members

        public void PacketArrived(InPacket inPacket)
        {
            // BasicInPacket packet = (BasicInPacket)inPacket;
            if (inPacket is UnknownInPacket)
            {
                client.PacketManager.OnReceivedUnknownPacket(new QQEventArgs<UnknownInPacket, OutPacket>(client, (UnknownInPacket)inPacket, null));
                return;
            }
            if (inPacket is ErrorPacket)
            {
                ErrorPacket error = (ErrorPacket)inPacket;
                if (error.ErrorType == ErrorPacketType.ERROR_TIMEOUT)
                {
                    client.PacketManager.OnSendPacketTimeOut(new QQEventArgs<InPacket, OutPacket>(client, null, error.TimeOutPacket));
                }
                else
                {
                    client.OnError(new QQEventArgs<ErrorPacket, OutPacket>(client, (ErrorPacket)inPacket, null));
                }
                return;
            }
            // 检查目前是否已经登录
            if (!client.QQUser.IsLoggedIn)
            {
                //按理说应该把在登陆之前时收到的包缓存起来，但是在实际中观察发现，登录之前收到的
                //东西基本没用，所以在这里不做任何事情，简单的抛弃这个包
                if (!IsPreloginPacket(inPacket))
                {
                    return;
                }
            }
            // 现在开始判断包的类型，作出相应的处理
            ConnectionPolicy policy = client.ConnectionManager.GetConnection(inPacket.PortName).Policy;
            // 根据输入包，检索对应的输出包
            OutPacket outPacket = policy.RetrieveSent(inPacket);
            // 这里检查是不是服务器发回的确认包
            // 为什么要检查这个呢，因为有些包是服务器主动发出的，对于这些包我们是不需要
            // 从超时发送队列中移除的，如果是服务器的确认包，那么我们就需要把超时发送队列
            // 中包移除
            switch (inPacket.Command)
            {
                //// 这三种包是服务器先发出的，我们要确认
                case QQCommand.Recv_IM:
                case QQCommand.Recv_IM_09:
                case QQCommand.BroadCast:
                case QQCommand.Friend_Change_Status:
                    break;
                // 其他情况我们删除超时队列中的包
                default:
                    client.PacketManager.RemoveResendPacket(inPacket);
                    client.PacketManager.OnSendPacketSuccessed(new QQEventArgs<InPacket, OutPacket>(client, inPacket, outPacket));
                    break;
            }
            switch (inPacket.Command)
            {
                case QQCommand.Touch:
                    client.LoginManager.ProcessLoginTouchReply((LoginTouchReplyPacket)inPacket, outPacket as LoginTouchPacket);
                    break;
                case QQCommand.LoginVerify:
                    client.LoginManager.ProcessLoginVerifyReply((LoginVerifyReplyPacket)inPacket, outPacket as LoginVerifyPacket);
                    break;
                case QQCommand.LoginGetInfo:
                    client.LoginManager.ProcessLoginGetInfoReply((LoginGetInfoReplyPacket)inPacket, outPacket as LoginGetInfoPacket);
                    break;
                case QQCommand.Login_A4:
                    client.LoginManager.ProcessLoginA4Reply((LoginA4ReplyPacket)inPacket, outPacket as LoginA4Packet);
                    break;
                case QQCommand.LoginGetList:
                    client.LoginManager.ProcessLoginGetListReply((LoginGetListReplyPacket)inPacket, outPacket as LoginGetListPacket);
                    break;
                case QQCommand.LoginSendInfo:
                    client.LoginManager.ProcessLoginSendInfoReply((LoginSendInfoReplyPacket)inPacket, outPacket as LoginSendInfoPacket);
                    break;
                case QQCommand.Keep_Alive:
                    client.LoginManager.ProcessKeepAliveReply((KeepAliveReplyPacket)inPacket, outPacket as KeepAlivePacket);
                    break;
                case QQCommand.Change_Status:
                    client.PrivateManager.ProcessChangeStatusReply((ChangeStatusReplyPacket)inPacket, outPacket as ChangeStatusPacket);
                    break;
                case QQCommand.GetLevel:
                    client.PrivateManager.ProcessGetLevelReply((GetLevelReplyPacket)inPacket, outPacket as GetLevelPacket);
                    break;
                case QQCommand.GroupLabel:
                    //client.PrivateManager.ProcessGetLevelReply((GetLevelReplyPacket)inPacket, outPacket as GetLevelPacket);
                    break;
                case QQCommand.Get_Friend_List:
                    client.FriendManager.ProcessGetFriendListReply((GetFriendListReplyPacket)inPacket, outPacket as GetFriendListPacket);
                    break;
                case QQCommand.Get_Online_OP:
                    client.FriendManager.ProcessGetFriendOnlineReply((GetOnlineOpReplyPacket)inPacket, outPacket as GetOnlineOpPacket);
                    break;
                case QQCommand.Recv_IM_09:
                case QQCommand.Recv_IM:
                    ProcessReceiveIM((ReceiveIMPacket)inPacket);
                    break;
                case QQCommand.Friend_Change_Status:
                    ProcessFriendChangeStatus((FriendChangeStatusPacket)inPacket);
                    break;
                case QQCommand.BroadCast:
                    ProcessSystemNotification((SystemNotificationPacket)inPacket);
                    break;
                case QQCommand.Add_Friend:
                    ProcessAddFriendReply((AddFriendReplyPacket)inPacket, outPacket as AddFriendPacket);
                    break;
                case QQCommand.AddFriendAuthorize:
                    ProcessAuthorizeReply((AddFriendAuthorizeReplyPacket)inPacket, outPacket as AddFriendAuthorizePacket);
                    break;
                case QQCommand.RequestToken:
                    ProcessRequestTokenReply((RequestTokenReplyPacket)inPacket, outPacket as RequestTokenPacket);
                    break;

                #region QQ2005
                case QQCommand.Request_Login_Token_05:
                    this.user = client.QQUser;
                    ProcessRequestLoginTokenReply((RequestLoginTokenReplyPacket)inPacket, outPacket as RequestLoginTokenPacket);
                    break;
                //case QQCommand.Keep_Alive_05:
                //    ProcessKeepAliveReply((KeepAliveReplyPacket)inPacket, outPacket as KeepAlivePacket);
                //    break;

                //case QQCommand.Get_Friend_List_05:
                //    ProcessGetFriendListReply((GetFriendListReplyPacket)inPacket, outPacket as GetFriendListPacket);
                //    break;
                
                case QQCommand.Get_UserInfo_05:
                    ProcessGetUserInfoReply((GetUserInfoReplyPacket)inPacket, outPacket as GetUserInfoPacket);
                    break;
                //case QQCommand.Change_Status_05:
                //    ProcessChangeStatusReply((ChangeStatusReplyPacket)inPacket, outPacket as ChangeStatusPacket);
                //    break;
                
                
                
                case QQCommand.Add_Friend_Auth_05:
                    ProcessAddFriendAuthReply((AddFriendAuthResponseReplyPacket)inPacket, outPacket as AddFriendAuthResponsePacket);
                    break;
                case QQCommand.Remove_Self_05:
                    ProcessRemoveSelfReply((RemoveSelfReplyPacket)inPacket, outPacket as RemoveSelfPacket);
                    break;
                case QQCommand.Delete_Friend_05:
                    ProcessDeleteFriendReply((DeleteFriendReplyPacket)inPacket, outPacket as DeleteFriendPacket);
                    break;
                
                case QQCommand.Upload_Group_Friend_05:
                    ProcessUploadGroupFriendReply((UploadGroupFriendReplyPacket)inPacket, (UploadGroupFriendPacket)outPacket);
                    break;
                case QQCommand.Modify_Info_05:
                    ProcessModifyInfoReply((ModifyInfoReplyPacket)inPacket, (ModifyInfoPacket)outPacket);
                    break;
                case QQCommand.Signature_OP_05:
                    ProcessSignatureOpReply((SignatureOpReplyPacket)inPacket, (SignatureOpPacket)outPacket);
                    break;
                case QQCommand.Privacy_Data_OP_05:
                    ProcessPrivacyDataOpReply((PrivacyDataOpReplyPacket)inPacket, (PrivacyDataOpPacket)outPacket);
                    break;
                case QQCommand.Friend_Data_OP_05:
                    ProcessFriendDataOpReply((FriendDataOpReplyPacket)inPacket, (FriendDataOpPacket)outPacket);
                    break;
                //case QQCommand.Friend_Level_OP_05:
                //    ProcessFriendLevelOpReply((FriendLevelOpReplyPacket)inPacket, (FriendLevelOpPacket)outPacket);
                //    break;
                case QQCommand.User_Property_OP_05:
                    PocessUserPropertyOpReply((UserPropertyOpReplyPacket)inPacket, (UserPropertyOpPacket)outPacket);
                    break;
                //case QQCommand.Download_Group_Friend_05:
                //    ProcessDownloadGroupFriendReply((DownloadGroupFriendReplyPacket)inPacket, (DownloadGroupFriendPacket)outPacket);
                //    break;
                case QQCommand.Group_Data_OP_05:
                    ProcessGroupNameOpReply((GroupDataOpReplyPacket)inPacket, (GroupDataOpPacket)outPacket);
                    break;
                case QQCommand.Search_User_05:
                    ProcessSearchUserReply((SearchUserReplyPacket)inPacket, (SearchUserPacket)outPacket);
                    break;
                case QQCommand.Weather_OP_05:
                    ProcessWeatherOpReply((WeatherOpReplyPacket)inPacket, (WeatherOpPacket)outPacket);
                    break;
                case QQCommand.Cluster_Cmd:
                    ProcessClusterCommandReply((ClusterCommandReplyPacket)inPacket, (ClusterCommandPacket)outPacket);
                    break;
                #endregion
                //default:

            }
        }

        private void ProcessRequestTokenReply(RequestTokenReplyPacket inPacket, RequestTokenPacket outPacket)
        {
            QQEventArgs<RequestTokenReplyPacket, RequestTokenPacket> e = new QQEventArgs<RequestTokenReplyPacket, RequestTokenPacket>(client, inPacket, outPacket);
            if (inPacket.NeedVerify)
            {
                if (inPacket.IsWrongVerifyCode) //输入的验证码错误
                { 
                    //events
                    //应该重新下载验证码图片再输入验证码
                    
                }
                string getQQSession = "";
                string VerifyCodeFileName = Utils.Util.MapPath("\\verify\\Add\\" + outPacket.QQ.ToString() + ".jpg");
                Utils.Util.DownLoadFileFromUrl(inPacket.Url, VerifyCodeFileName, out getQQSession);
                client.FriendManager.FriendActionList[outPacket.QQ].ActionProgress = FriendActionProgress.NeedVerify;//重置好友添加状态
                client.FriendManager.FriendActionList[outPacket.QQ].Message = VerifyCodeFileName;
                client.FriendManager.FriendActionList[outPacket.QQ].VCodeSession = getQQSession;
#if DEBUG
                client.LogManager.Log(ToString() + string.Format(" url:{0}, Session:{1},Code File:{2}", inPacket.Url, getQQSession, VerifyCodeFileName));
#endif
                
            }
            else //Token获取成功
            {
                client.FriendManager.FriendActionList[outPacket.QQ].Token = inPacket.Token;
                //client.FriendManager.FriendActionList[outPacket.QQ].ActionProgress = FriendActionProgress.
                switch (client.FriendManager.FriendActionList[outPacket.QQ].AuthType)
                { 
                    case AuthType.No:
                        client.FriendManager.FriendActionList[outPacket.QQ].ActionProgress = FriendActionProgress.SendRequest;
                        client.FriendManager.SendAddFriend(outPacket.QQ);
                        break;
                    case AuthType.NeedAnswer:
                        client.FriendManager.FriendActionList[outPacket.QQ].ActionProgress = FriendActionProgress.NeedAnswer;
                        //client.FriendManager.OnAddFriendNeedAnswer(e); //触发事件回答问题
                        break;
                    case AuthType.NeedAuth:
                        client.FriendManager.FriendActionList[outPacket.QQ].ActionProgress = FriendActionProgress.NeedAuthor;
                        //client.FriendManager.OnAddFriendNeedAuth(e);
                        break;
                    case AuthType.Reject:
                        break;
                    default:
                        throw new Exception("不支持该AuthType类型！");
                
                }
                
            }
        }

        public bool Accept(LFNet.QQ.Packets.InPacket inPacket)
        {
            return inPacket.GetFamily() == ProtocolFamily.Basic;
        }
        #endregion
        /// <summary>判断包是否时登录前可以出现的包，有些包是在登录前可以处理的，如果不是，应该缓存起来等
        /// 到登录成功后再处理，不过但是在实际中观察发现，登录之前收到的东西基本没用，可以不管
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <returns></returns>
        private bool IsPreloginPacket(InPacket inPacket)
        {
            switch (inPacket.Command)
            {
                case QQCommand.Touch:
                case QQCommand.LoginRequest:
                case QQCommand.Login_A4:
                case QQCommand.LoginVerify:
                case QQCommand.LoginGetInfo:
                case QQCommand.LoginGetList:
                case QQCommand.LoginSendInfo:
                case QQCommand.Request_Login_Token_05:
                //case QQCommand.Login_05:
                   return true;
                case QQCommand.Unknown:
                    return inPacket is ErrorPacket;
                default:
                    return false;
            }
        }

        #region 包处理方法
            /// <summary>处理请求登录令牌的回复包
    /// </summary>
        /// <param name="basicInPacket">The basic in packet.</param>
        private void ProcessRequestLoginTokenReply(RequestLoginTokenReplyPacket inPacket, RequestLoginTokenPacket outPacket)
        {
            QQEventArgs<RequestLoginTokenReplyPacket, RequestLoginTokenPacket> e = new QQEventArgs<RequestLoginTokenReplyPacket, RequestLoginTokenPacket>(client, inPacket, outPacket);
            if (inPacket.ReplyCode == ReplyCode.OK)
            {
                user.QQKey.LoginToken = inPacket.LoginToken;
                client.LoginManager.OnRequestLoginTokenSuccessed(e);
            }
            else
            {
                client.LoginManager.OnRequestLoginTokenFailed(e);
            }
        }

        
        /// <summary>处理保持登录应答
        /// </summary>
        /// <param name="packet">The packet.</param>
        private void ProcessKeepAliveReply(KeepAliveReplyPacket inPacket, KeepAlivePacket outPacket)
        {
            client.ConnectionManager.OnReceivedKeepAlive(new QQEventArgs<KeepAliveReplyPacket, KeepAlivePacket>(client, inPacket, outPacket));
        }

         ///<summary>
         ///处理接收信息
         ///</summary>
         ///<param name="packet">The packet.</param>
        private void ProcessReceiveIM(ReceiveIMPacket packet)
        {
            QQEventArgs<ReceiveIMPacket, OutPacket> e = new QQEventArgs<ReceiveIMPacket, OutPacket>(client, packet, null);
            //先发送接收确认
            this.client.MessageManager.SendReceiveReplyPacket(packet);
            if (packet.IsDuplicated)
            {
                client.MessageManager.OnReceiveDuplicatedIM(e);
                return;
            }
            switch (packet.Header.Type)
            {
                case RecvSource.FRIEND_0801:
                case RecvSource.FRIEND_0802:
                case RecvSource.FRIEND_09:
                case RecvSource.FRIEND_09SP1:
                case RecvSource.STRANGER:
                    switch (packet.NormalHeader.Type)
                    {
                        case NormalIMType.TEXT:
                            client.MessageManager.OnReceiveNormalIM(e);
                            break;
                        case NormalIMType.TCP_REQUEST:
                            break;
                        case NormalIMType.ACCEPT_TCP_REQUEST:
                            break;
                        case NormalIMType.REJECT_TCP_REQUEST:
                            break;
                        case NormalIMType.UDP_REQUEST:
                            break;
                        case NormalIMType.ACCEPT_UDP_REQUEST:
                            break;
                        case NormalIMType.REJECT_UDP_REQUEST:
                            break;
                        case NormalIMType.NOTIFY_IP:
                            break;
                        case NormalIMType.ARE_YOU_BEHIND_FIREWALL:
                            break;
                        case NormalIMType.ARE_YOU_BEHIND_PROXY:
                            break;
                        case NormalIMType.YES_I_AM_BEHIND_PROXY:
                            break;
                        case NormalIMType.NOTIFY_FILE_AGENT_INFO:
                            break;
                        case NormalIMType.REQUEST_CANCELED:
                            break;
                        case NormalIMType.Vibration:
                            if (e.InPacket.Vibration.IsShake)
                            {
                                client.MessageManager.OnReceiveVibration(e);
                            }
                            else
                            {
                                client.MessageManager.OnInputState(e);
                            }
                            break;
                        default:
                            client.MessageManager.OnReceiveUnknownIM(e);
                            break;
                    }
                    break;
                case RecvSource.BIND_USER:
                    break;
                case RecvSource.MOBILE:
                    break;
                case RecvSource.MEMBER_LOGIN_HINT:
                    break;
                case RecvSource.MOBILE_QQ:
                    break;
                case RecvSource.MOBILE_QQ_2:
                    break;
                case RecvSource.QQLIVE:
                    break;
                case RecvSource.PROPERTY_CHANGE:
                    client.FriendManager.OnUserPropertyChanged(e);
                    break;
                case RecvSource.TEMP_SESSION:
                    client.MessageManager.OnReceiveTempSessionIM(e);
                    break;
                case RecvSource.UNKNOWN_CLUSTER:
                    break;
                case RecvSource.CREATE_CLUSTER:
                case RecvSource.ADDED_TO_CLUSTER:
                    client.ClusterManager.OnAddedToCluster(e);
                    break;
                case RecvSource.DELETED_FROM_CLUSTER:
                    client.ClusterManager.OnRemovedFromCluster(e);
                    break;
                case RecvSource.REQUEST_JOIN_CLUSTER:
                    client.ClusterManager.OnHasRequestJoinCluster(e);
                    break;
                case RecvSource.APPROVE_JOIN_CLUSTER:
                    client.ClusterManager.OnApprovedJoinCluster(e);
                    break;
                case RecvSource.REJECT_JOIN_CLUSTER:
                    client.ClusterManager.OnRejectJoinCluster(e);
                    break;
                case RecvSource.TEMP_CLUSTER:
                    break;
                case RecvSource.CLUSTER:
                case RecvSource.CLUSTER_09:
                    client.ClusterManager.OnReceiveClusterIM(e);
                    break;
                case RecvSource.CLUSTER_NOTIFICATION:
                    break;
                case RecvSource.SYS_MESSAGE:
                    if (packet.SystemMessageType == SystemIMType.QQ_RECV_IM_KICK_OUT)
                    {
                        client.MessageManager.OnReceiveKickOut(e);
                    }
                    else
                    {
                        client.MessageManager.OnReceiveSysMessage(e);
                    }
                    break;
                case RecvSource.SIGNATURE_CHANGE:
                    client.FriendManager.OnSignatureChanged(e);
                    break;
                case RecvSource.CUSTOM_HEAD_CHANGE:
                    break;
                case RecvSource.INPUT_STATE_CHANGE:
                    client.MessageManager.OnInputState(e);
                    break;
                default:
                    break;
            }
        }


        

        /// <summary>processGetUserInfoReply
    /// </summary>
        /// <param name="packet">The packet.</param>
        private void ProcessGetUserInfoReply(GetUserInfoReplyPacket inPacket, GetUserInfoPacket outPacket)
        {
            client.FriendManager.OnGetUserInfoSuccessed(new QQEventArgs<GetUserInfoReplyPacket, GetUserInfoPacket>(client, inPacket, outPacket));
        }

    //    /// <summary>处理改变状态回复事件
    ///// </summary>
    //    /// <param name="packet">The packet.</param>
    //    private void ProcessChangeStatusReply(ChangeStatusReplyPacket inPacket, ChangeStatusPacket outPacket)
    //    {
    //        QQEventArgs<ChangeStatusReplyPacket, ChangeStatusPacket> e = new QQEventArgs<ChangeStatusReplyPacket, ChangeStatusPacket>(client, inPacket, outPacket);
    //        if (inPacket.ReplyCode == ReplyCode.CHANGE_STATUS_OK)
    //        {
    //            client.FriendManager.OnChangeStatusSuccessed(e);
    //        }
    //        else
    //        {
    //            client.FriendManager.OnChangeStatusFailed(e);
    //        }
    //    }

        /// <summary>处理好友状态改变事件
        /// </summary>
        /// <param name="packet">The packet.</param>
        private void ProcessFriendChangeStatus(FriendChangeStatusPacket packet)
        {
            QQEventArgs<FriendChangeStatusPacket, OutPacket> e = new QQEventArgs<FriendChangeStatusPacket, OutPacket>(client, packet, null);
            client.FriendManager.OnFriendChangeStatus(e);
        }

        /// <summary>
        /// 处理系统消息，比如谁谁谁添加你为好友
        /// </summary>
        /// <param name="packet">The packet.</param>
        private void ProcessSystemNotification(SystemNotificationPacket packet)
        {
            QQEventArgs<SystemNotificationPacket, OutPacket> e = new QQEventArgs<SystemNotificationPacket, OutPacket>(client, packet, null);
            switch (packet.Type)
            {
                case SystemMessageType.BEING_ADDED:
                    client.MessageManager.OnSysAddedByOthers(e);
                    break;
                case SystemMessageType.ADD_FRIEND_REQUEST:
                    client.MessageManager.OnSysRequestAddMe(e);
                    break;
                case SystemMessageType.ADD_FRIEND_APPROVED:
                    client.MessageManager.OnSysAddOtherApproved(e);
                    break;
                case SystemMessageType.ADD_FRIEND_REJECTED:
                    client.MessageManager.OnSysAddOtherRejected(e);
                    break;
                case SystemMessageType.ADVERTISEMENT:
                    client.MessageManager.OnSysAdvertisment(e);
                    break;
                case SystemMessageType.BEING_ADDED_EX:
                    client.MessageManager.OnAddedByOthersEx(e);
                    break;
                case SystemMessageType.ADD_FRIEND_REQUEST_EX:
                    client.MessageManager.OnSysRequestAddMeEx(e);
                    break;
                case SystemMessageType.ADD_FRIEND_APPROVED_AND_ADD:
                    client.MessageManager.OnSysApprovedAddOtherAndAddMe(e);
                    break;
                case SystemMessageType.UPDATE_HINT:
                default:
                    break;
            }
        }

        /// <summary>
        /// 处理请求加一个好友的回复包
        /// </summary>
        /// <param name="packet">The packet.</param>
        private void ProcessAddFriendReply(AddFriendReplyPacket inPacket, AddFriendPacket outPacket)
        {
            QQEventArgs<AddFriendReplyPacket, AddFriendPacket> e = new QQEventArgs<AddFriendReplyPacket, AddFriendPacket>(client, inPacket, outPacket);
            if (inPacket.ReplyCode == ReplyCode.OK)
            {
                client.FriendManager.FriendActionList[inPacket.FriendQQ].AuthType = inPacket.AuthCode;
                switch (inPacket.AuthCode)
                {
                    case AuthType.No:
                        //client.FriendManager.OnAddFriendSuccessed(e); //dobit 不能触发该事件 还没添加成功呢
                        break;
                    case AuthType.NeedAuth:
                        //if (string.IsNullOrEmpty(client.FriendManager.FriendActionList[inPacket.FriendQQ].AuthorMessage))//如果没有填写加好友的验证信息
                        //{
                        //    client.FriendManager.OnAddFriendNeedAuth(e);//触发该事件 提前让用户填写验证信息 这个事件就不该出发
                        //}
                        break;
                    case AuthType.Reject:
                        client.FriendManager.FriendActionList[inPacket.FriendQQ].ActionProgress = FriendActionProgress.Finished;
                        client.FriendManager.FriendActionList[inPacket.FriendQQ].Message = string.Format("{0} 拒绝加为好友！", e.InPacket.FriendQQ);
                        client.FriendManager.OnAddFriendDeny(e);
                        return; //操作已经完成跳出
                        break;
                    case AuthType.NeedAnswer://暂时不支持
                        //if (string.IsNullOrEmpty(client.FriendManager.FriendActionList[inPacket.FriendQQ].AnswerMessage)) //如果没有填写回答问题的答案
                        //{
                        //    client.FriendManager.OnAddFriendNeedAnswer(e);//触发该事件
                        //}
                        break;
                    default:
                        client.LogManager.Log("unknown AuthType=0x" + inPacket.AuthCode.ToString("X"));
                        break;
                }
                client.FriendManager.FriendActionRequestToken(inPacket.FriendQQ);//请求令牌
            }
            else
            {
                if (inPacket.ReplyCode == ReplyCode.ADD_FRIEND_ALREADY)
                {
                    //client.FriendManager.FriendActionList.Remove(e.InPacket.FriendQQ);//从好友操作列表中移除
                    client.FriendManager.FriendActionList[inPacket.FriendQQ].ActionProgress = FriendActionProgress.Finished;
                    client.FriendManager.FriendActionList[inPacket.FriendQQ].Message = string.Format("{0} 已经是你的QQ好友", e.InPacket.FriendQQ);
                    client.LogManager.Log(string.Format("{0} is already in your QQ friends", e.InPacket.FriendQQ));
                    client.FriendManager.OnFriendAlready(e);
                }
                else
                {
                    client.FriendManager.FriendActionList[inPacket.FriendQQ].ActionProgress = FriendActionProgress.Finished;
                    client.FriendManager.FriendActionList[inPacket.FriendQQ].Message = string.Format("添加 {0} 失败! 服务器消息:ReplyCode=0x{1:X} ", inPacket.FriendQQ, inPacket.ReplyCode);
                    client.LogManager.Log(string.Format("add {0} failed! Server ReplyCode is 0x{1:x} ", inPacket.FriendQQ, inPacket.ReplyCode));
                    //client.FriendManager.FriendActionList.Remove(e.InPacket.FriendQQ);//从添加操作列表中移除
                    client.FriendManager.OnAddFriendFailed(e);
                }
            }
        }

        /// <summary> 处理认证信息的回复包
        /// </summary>
        /// <param name="packet">The packet.</param>
        private void ProcessAddFriendAuthReply(AddFriendAuthResponseReplyPacket inPacket, AddFriendAuthResponsePacket outPacket)
        {
            QQEventArgs<AddFriendAuthResponseReplyPacket, AddFriendAuthResponsePacket> e = new QQEventArgs<AddFriendAuthResponseReplyPacket, AddFriendAuthResponsePacket>(client, inPacket, outPacket);
            if (inPacket.ReplyCode != ReplyCode.ADD_FRIEND_AUTH_OK)
            {
                client.FriendManager.OnResponseAuthFailed(e);
            }
            else
            {
                client.FriendManager.OnResponseAuthSuccessed(e);
            }
        }

        /// <summary>处理删除自己的回复包
    /// </summary>
        /// <param name="packet">The packet.</param>
        private void ProcessRemoveSelfReply(RemoveSelfReplyPacket inPacket, RemoveSelfPacket outPacket)
        {
            QQEventArgs<RemoveSelfReplyPacket, RemoveSelfPacket> e = new QQEventArgs<RemoveSelfReplyPacket, RemoveSelfPacket>(client, inPacket, outPacket);
            if (inPacket.ReplyCode == ReplyCode.OK)
            {
                client.FriendManager.OnRemoveSelfSuccessed(e);
            }
            else
            {
                client.FriendManager.OnRemoveSelfFailed(e);
            }
        }

        /// <summary>处理删除好友的回复包
    /// </summary>
        /// <param name="packet">The packet.</param>
        private void ProcessDeleteFriendReply(DeleteFriendReplyPacket inPacket, DeleteFriendPacket outPacket)
        {
            QQEventArgs<DeleteFriendReplyPacket, DeleteFriendPacket> e = new QQEventArgs<DeleteFriendReplyPacket, DeleteFriendPacket>(client, inPacket, outPacket);
            if (inPacket.ReplyCode != ReplyCode.OK)
            {
                client.FriendManager.OnDeleteFriendFailed(e);
            }
            else
            {
                client.FriendManager.OnDeleteFriendSuccessed(e);
            }
        }

        /// <summary>处理认证消息发送包
    /// </summary>
        /// <param name="packet">The packet.</param>
        private void ProcessAuthorizeReply(AddFriendAuthorizeReplyPacket inPacket, AddFriendAuthorizePacket outPacket)
        {
            QQEventArgs<AddFriendAuthorizeReplyPacket, AddFriendAuthorizePacket> e = new QQEventArgs<AddFriendAuthorizeReplyPacket, AddFriendAuthorizePacket>(client, inPacket, outPacket);
            switch (inPacket.ReplyCode)
            {
                case ReplyCode.OK:
                    client.FriendManager.OnSendAuthSuccessed(e);
                    if (client.FriendManager.FriendActionList.ContainsKey((int)inPacket.To))
                    {
                        client.FriendManager.FriendActionList[inPacket.To].ActionProgress = FriendActionProgress.Finished;
                        client.FriendManager.FriendActionList[inPacket.To].Message = "发送添加好友信息成功";
                    }
                    break;
                default:
                    if (client.FriendManager.FriendActionList.ContainsKey((int)inPacket.To))
                    {
                        client.FriendManager.FriendActionList[inPacket.To].ActionProgress = FriendActionProgress.Finished;
                        client.FriendManager.FriendActionList[inPacket.To].Message = "发送添加好友信息失败！";
                    }
                    client.FriendManager.OnSendAuthFailed(e);
                    break;
            }
        }

        /// <summary>
        /// Processes the upload group friend reply.处理上传分组好友列表回复包
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="outPacket">The out packet.</param>
        private void ProcessUploadGroupFriendReply(UploadGroupFriendReplyPacket inPacket, UploadGroupFriendPacket outPacket)
        {
            QQEventArgs<UploadGroupFriendReplyPacket, UploadGroupFriendPacket> e = new QQEventArgs<UploadGroupFriendReplyPacket, UploadGroupFriendPacket>(client, inPacket, outPacket);
            if (inPacket.ReplyCode == ReplyCode.OK)
            {
                client.FriendManager.OnUploadGroupFriendSuccessed(e);
            }
            else
            {
                client.FriendManager.OnUploadGroupFriendFailed(e);
            }
        }

        /// <summary>处理修改个人信息的回复包
        /// Processes the modify info reply.
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="outPacket">The out packet.</param>
        private void ProcessModifyInfoReply(ModifyInfoReplyPacket inPacket, ModifyInfoPacket outPacket)
        {
            QQEventArgs<ModifyInfoReplyPacket, ModifyInfoPacket> e = new QQEventArgs<ModifyInfoReplyPacket, ModifyInfoPacket>(client, inPacket, outPacket);
            if (inPacket.Success)
            {
                client.PrivateManager.OnModifyInfoSuccessed(e);
            }
            else
            {
                client.PrivateManager.OnModifyInfoFailed(e);
            }
        }

        /// <summary>处理个性签名操作回复包
        /// Processes the signature op reply.
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="outPacket">The out packet.</param>
        private void ProcessSignatureOpReply(SignatureOpReplyPacket inPacket, SignatureOpPacket outPacket)
        {
            QQEventArgs<SignatureOpReplyPacket, SignatureOpPacket> e = new QQEventArgs<SignatureOpReplyPacket, SignatureOpPacket>(client, inPacket, outPacket);
            if (inPacket.ReplyCode == ReplyCode.OK)
            {
                switch (inPacket.SubCommand)
                {
                    case SignatureSubCmd.MODIFY:
                        client.PrivateManager.OnModifySignatureSuccessed(e);
                        break;
                    case SignatureSubCmd.DELETE:
                        client.PrivateManager.OnDeleteSignatureSuccessed(e);
                        break;
                    case SignatureSubCmd.GET:
                        client.FriendManager.OnGetSignatureSuccessed(e);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (inPacket.SubCommand)
                {
                    case SignatureSubCmd.MODIFY:
                        client.PrivateManager.OnModifySignatureFailed(e);
                        break;
                    case SignatureSubCmd.DELETE:
                        client.PrivateManager.OnDeleteSignatureFailed(e);
                        break;
                    case SignatureSubCmd.GET:
                        client.FriendManager.OnGetSignatureFailed(e);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>处理隐私选项操作回复包
        /// Processes the privacy data op reply.
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="outPacket">The out packet.</param>
        private void ProcessPrivacyDataOpReply(PrivacyDataOpReplyPacket inPacket, PrivacyDataOpPacket outPacket)
        {
            QQEventArgs<PrivacyDataOpReplyPacket, PrivacyDataOpPacket> e = new QQEventArgs<PrivacyDataOpReplyPacket, PrivacyDataOpPacket>(client, inPacket, outPacket);
            if (inPacket.ReplyCode == ReplyCode.OK)
            {
                switch (inPacket.SubCommand)
                {
                    case PrivacySubCmd.SearchMeByOnly:
                        client.PrivateManager.OnSetSearchMeByQQOnlySuccessed(e);
                        break;
                    case PrivacySubCmd.ShareGeography:
                        client.PrivateManager.OnSetShareGeographySuccessed(e);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (inPacket.SubCommand)
                {
                    case PrivacySubCmd.SearchMeByOnly:
                        client.PrivateManager.OnSetSearchMeByQQOnlyFailed(e);
                        break;
                    case PrivacySubCmd.ShareGeography:
                        client.PrivateManager.OnSetShareGeographyFailed(e);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>处理上传下载备注的回复包
        /// Processes the friend data op reply.
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="outPacket">The out packet.</param>
        private void ProcessFriendDataOpReply(FriendDataOpReplyPacket inPacket, FriendDataOpPacket outPacket)
        {
            QQEventArgs<FriendDataOpReplyPacket, FriendDataOpPacket> e = new QQEventArgs<FriendDataOpReplyPacket, FriendDataOpPacket>(client, inPacket, outPacket);
            if (inPacket.ReplyCode == ReplyCode.OK)
            {
                switch (inPacket.SubCommand)
                {
                    case FriendOpSubCmd.BATCH_DOWNLOAD_FRIEND_REMARK:
                        client.FriendManager.OnBatchDownloadFriendRemarkSuccessed(e);
                        break;
                    case FriendOpSubCmd.UPLOAD_FRIEND_REMARK:
                        client.FriendManager.OnUploadFriendRemarkSuccessed(e);
                        break;
                    case FriendOpSubCmd.REMOVE_FRIEND_FROM_LIST:
                        client.FriendManager.OnRemoveFriendFromListSuccessed(e);
                        break;
                    case FriendOpSubCmd.DOWNLOAD_FRIEND_REMARK:
                        client.FriendManager.OnDownloadFriendRemarkSuccessed(e);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (inPacket.SubCommand)
                {
                    case FriendOpSubCmd.BATCH_DOWNLOAD_FRIEND_REMARK:
                        client.FriendManager.OnBatchDownloadFriendRemarkFailed(e);
                        break;
                    case FriendOpSubCmd.UPLOAD_FRIEND_REMARK:
                        client.FriendManager.OnUploadFriendRemarkFailed(e);
                        break;
                    case FriendOpSubCmd.REMOVE_FRIEND_FROM_LIST:
                        client.FriendManager.OnRemoveFriendFromListFailed(e);
                        break;
                    case FriendOpSubCmd.DOWNLOAD_FRIEND_REMARK:
                        client.FriendManager.OnDownloadFriendRemarkFailed(e);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>处理好友等级回复包
        /// Processes the friend level op reply.
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="outPacket">The out packet.</param>
        private void ProcessFriendLevelOpReply(FriendLevelOpReplyPacket inPacket, FriendLevelOpPacket outPacket)
        {
            QQEventArgs<FriendLevelOpReplyPacket, FriendLevelOpPacket> e = new QQEventArgs<FriendLevelOpReplyPacket, FriendLevelOpPacket>(client, inPacket, outPacket);
            client.FriendManager.OnGetFriendLevelSuccessed(e);
        }

        /// <summary>处理用户属性回复包
        /// Pocesses the user property op reply.
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="outPacket">The out packet.</param>
        private void PocessUserPropertyOpReply(UserPropertyOpReplyPacket inPacket, UserPropertyOpPacket outPacket)
        {
            QQEventArgs<UserPropertyOpReplyPacket, UserPropertyOpPacket> e = new QQEventArgs<UserPropertyOpReplyPacket, UserPropertyOpPacket>(client, inPacket, outPacket);
            client.FriendManager.OnGetUserPropertySuccess(e);
        }

        /// <summary>处理下载分组好友列表回复包
        /// Processes the download group friend reply.
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="outPacket">The out packet.</param>
        private void ProcessDownloadGroupFriendReply(DownloadGroupFriendReplyPacket inPacket, DownloadGroupFriendPacket outPacket)
        {
            QQEventArgs<DownloadGroupFriendReplyPacket, DownloadGroupFriendPacket> e = new QQEventArgs<DownloadGroupFriendReplyPacket, DownloadGroupFriendPacket>(client, inPacket, outPacket);
            if (inPacket.ReplyCode == ReplyCode.OK)
            {
                client.FriendManager.OnDownloadGroupFriendSuccessed(e);
            }
            else
            {
                client.FriendManager.OnDownloadGroupFriendFailed(e);
            }
        }

        /// <summary>处理分组名称回复包
        /// Processes the group name op reply.
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="outPacket">The out packet.</param>
        private void ProcessGroupNameOpReply(GroupDataOpReplyPacket inPacket, GroupDataOpPacket outPacket)
        {
            QQEventArgs<GroupDataOpReplyPacket, GroupDataOpPacket> e = new QQEventArgs<GroupDataOpReplyPacket, GroupDataOpPacket>(client, inPacket, outPacket);
            if (inPacket.ReplyCode == ReplyCode.OK)
            {
                switch (inPacket.SubCommand)
                {
                    case GroupSubCmd.DOWNLOAD:
                        client.FriendManager.OnDownloadGroupNamesSuccessed(e);
                        break;
                    case GroupSubCmd.UPLOAD:
                        client.FriendManager.OnUploadGroupNamesSuccessed(e);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (inPacket.SubCommand)
                {
                    case GroupSubCmd.DOWNLOAD:
                        client.FriendManager.OnDownloadGroupNamesFailed(e);
                        break;
                    case GroupSubCmd.UPLOAD:
                        client.FriendManager.OnUploadGroupNamesFailed(e);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>处理搜索用户的回复包
        /// Processes the search user reply.
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="outPacket">The out packet.</param>
        private void ProcessSearchUserReply(SearchUserReplyPacket inPacket, SearchUserPacket outPacket)
        {
            QQEventArgs<SearchUserReplyPacket, SearchUserPacket> e = new QQEventArgs<SearchUserReplyPacket, SearchUserPacket>(client, inPacket, outPacket);
            client.FriendManager.OnSearchUserSuccessed(e);
        }

        /// <summary>处理天气预报操作回复包
        /// Processes the weather op reply.
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="outPacket">The out packet.</param>
        private void ProcessWeatherOpReply(WeatherOpReplyPacket inPacket, WeatherOpPacket outPacket)
        {
            QQEventArgs<WeatherOpReplyPacket, WeatherOpPacket> e = new QQEventArgs<WeatherOpReplyPacket, WeatherOpPacket>(client, inPacket, outPacket);
            if (inPacket.ReplyCode == ReplyCode.OK)
            {
                if (!string.IsNullOrEmpty(inPacket.Province))
                {
                    client.PrivateManager.OnGetWeatherSuccessed(e);
                }
                else
                {
                    client.PrivateManager.OnGetWeatherFailed(e);
                }
            }
            else
            {
                client.PrivateManager.OnGetWeatherFailed(e);
            }
        }

        /// <summary>处理群命令回复包
        /// Processes the cluster command reply.
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="outPacket">The out packet.</param>
        private void ProcessClusterCommandReply(ClusterCommandReplyPacket inPacket, ClusterCommandPacket outPacket)
        {
            switch (inPacket.SubCommand)
            {
                case ClusterCommand.CREATE_CLUSTER:
                    break;
                case ClusterCommand.MODIFY_MEMBER:
                    break;
                case ClusterCommand.MODIFY_CLUSTER_INFO:
                    break;
                case ClusterCommand.GET_CLUSTER_INFO:
                    ProcessClusterGetInfoReply(inPacket, (ClusterGetInfoPacket)outPacket);
                    break;
                case ClusterCommand.ACTIVATE_CLUSTER:
                    break;
                case ClusterCommand.SEARCH_CLUSTER:
                    ProcessClusterSearchReply(inPacket, (ClusterSearchPacket)outPacket);
                    break;
                case ClusterCommand.JOIN_CLUSTER:
                    ProcessClusterJoinReply(inPacket, (ClusterJoinPacket)outPacket);
                    break;
                case ClusterCommand.JOIN_CLUSTER_AUTH:
                    ProcessClusterJoinAuthReply(inPacket, (ClusterAuthPacket)outPacket);
                    break;
                case ClusterCommand.EXIT_CLUSTER:
                    ProcessClusterExitReply(inPacket, (ClusterExitPacket)outPacket);
                    break;
                case ClusterCommand.GET_ONLINE_MEMBER:
                    ProcessClusterGetOnlineMemberReply(inPacket, (ClusterGetOnlineMemberPacket)outPacket);
                    break;
                case ClusterCommand.GET_MEMBER_INFO:
                    ProccessClusterGetMemberInfoReply(inPacket, (ClusterGetMemberInfoPacket)outPacket);
                    break;
                case ClusterCommand.MODIFY_CARD:
                    ProcessModifyCardReply(inPacket, (ClusterModifyCardPacket)outPacket);
                    break;
                case ClusterCommand.GET_CARD_BATCH:
                    ProcessClusterGetCardBatchReply(inPacket, (ClusterGetCardBatchPacket)outPacket);
                    break;
                case ClusterCommand.GET_CARD:
                    ProcessClusterGetCardReply(inPacket, (ClusterGetCardPacket)outPacket);
                    break;
                case ClusterCommand.COMMIT_ORGANIZATION:
                    break;
                case ClusterCommand.UPDATE_ORGANIZATION:
                    ProcessUpdateOrganizationReply(inPacket, (ClusterUpdateOrganizationPacket)outPacket);
                    break;
                case ClusterCommand.COMMIT_MEMBER_ORGANIZATION:
                    break;
                case ClusterCommand.GET_VERSION_ID:
                    break;
                case ClusterCommand.SEND_IM_EX:
                    break;
                case ClusterCommand.SET_ROLE:
                    break;
                case ClusterCommand.TRANSFER_ROLE:
                    break;
                case ClusterCommand.DISMISS_CLUSTER:
                    break;
                case ClusterCommand.CREATE_TEMP:
                    break;
                case ClusterCommand.MODIFY_TEMP_MEMBER:
                    break;
                case ClusterCommand.EXIT_TEMP:
                    break;
                case ClusterCommand.GET_TEMP_INFO:
                    break;
                case ClusterCommand.MODIFY_TEMP_INFO:
                    break;
                case ClusterCommand.SEND_TEMP_IM:
                    break;
                case ClusterCommand.SUB_CLUSTER_OP:
                    ProcessClusterSubClusterOpReply(inPacket, (ClusterSubClusterOpPacket)outPacket);
                    break;
                case ClusterCommand.ACTIVATE_TEMP:
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Processes the cluster join reply.
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="outPacket">The out packet.</param>
        private void ProcessClusterJoinReply(ClusterCommandReplyPacket inPacket, ClusterJoinPacket outPacket)
        {
            QQEventArgs<ClusterCommandReplyPacket, ClusterJoinPacket> e = new QQEventArgs<ClusterCommandReplyPacket, ClusterJoinPacket>(client, inPacket, outPacket);
            if (inPacket.ReplyCode == ReplyCode.OK)
            {
                switch (inPacket.JoinReply)
                {
                    case ClusterJoinReply.OK:
                        client.ClusterManager.OnClusterJoinSuccessed(e);
                        break;
                    case ClusterJoinReply.NeedAuth:
                        client.ClusterManager.OnJoinClusterNeedAuth(e);
                        break;
                    case ClusterJoinReply.Denied:
                        client.ClusterManager.OnJoinClusterDenied(e);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                client.ClusterManager.OnJoinClusterFailed(e);
            }
        }
        /// <summary>处理认证信息发送的回复包
        /// Processes the cluster join auth reply.
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="outPacket">The out packet.</param>
        private void ProcessClusterJoinAuthReply(ClusterCommandReplyPacket inPacket, ClusterAuthPacket outPacket)
        {
            QQEventArgs<ClusterCommandReplyPacket, ClusterAuthPacket> e = new QQEventArgs<ClusterCommandReplyPacket, ClusterAuthPacket>(client, inPacket, outPacket);
            if (inPacket.ReplyCode == ReplyCode.OK)
            {
                client.ClusterManager.OnSendJoinClusterAuthSuccessed(e);
            }
            else
            {
                client.ClusterManager.OnSendJoinClusterAuthFailed(e);
            }
        }
        /// <summary>
        /// Processes the cluster search reply.
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="outPacket">The out packet.</param>
        private void ProcessClusterSearchReply(ClusterCommandReplyPacket inPacket, ClusterSearchPacket outPacket)
        {
            QQEventArgs<ClusterCommandReplyPacket, ClusterSearchPacket> e = new QQEventArgs<ClusterCommandReplyPacket, ClusterSearchPacket>(client, inPacket, outPacket);
            if (inPacket.ReplyCode == ReplyCode.OK)
            {
                client.ClusterManager.OnSearchClusterSuccessed(e);
            }
            else
                client.ClusterManager.OnSearchClusterFailed(e);
        }
        /// <summary>
        /// Processes the cluster exit reply.
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="outPacket">The out packet.</param>
        private void ProcessClusterExitReply(ClusterCommandReplyPacket inPacket, ClusterExitPacket outPacket)
        {
            QQEventArgs<ClusterCommandReplyPacket, ClusterExitPacket> e = new QQEventArgs<ClusterCommandReplyPacket, ClusterExitPacket>(client, inPacket, outPacket);
            if (inPacket.ReplyCode == ReplyCode.OK)
            {
                client.ClusterManager.OnExitClusterSuccessed(e);
            }
            else
                client.ClusterManager.OnExitClusterFailed(e);
        }
        /// <summary>
        /// Processes the cluster send IM ex reply.
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="outPacket">The out packet.</param>
        private void ProcessClusterSendIMExReply(ClusterCommandReplyPacket inPacket, ClusterSendIMExPacket outPacket)
        {
            QQEventArgs<ClusterCommandReplyPacket, ClusterSendIMExPacket> e = new QQEventArgs<ClusterCommandReplyPacket, ClusterSendIMExPacket>(client, inPacket, outPacket);
            if (inPacket.ReplyCode == ReplyCode.OK)
            {
                client.ClusterManager.OnSendClusterIMExSuccessed(e);
            }
            else
                client.ClusterManager.OnSendClusterIMExFailed(e);
        }

        /// <summary>
        /// Processes the modify card reply.
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="outPacket">The out packet.</param>
        private void ProcessModifyCardReply(ClusterCommandReplyPacket inPacket, ClusterModifyCardPacket outPacket)
        {
            QQEventArgs<ClusterCommandReplyPacket, ClusterModifyCardPacket> e = new QQEventArgs<ClusterCommandReplyPacket, ClusterModifyCardPacket>(client, inPacket, outPacket);
            if (inPacket.ReplyCode == ReplyCode.OK)
            {
                client.ClusterManager.OnModifyCardSuccssed(e);
            }
            else
            {
                client.ClusterManager.OnModifyCardFailed(e);
            }
        }

        /// <summary>
        /// Processes the update organization reply.
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="outPacket">The out packet.</param>
        private void ProcessUpdateOrganizationReply(ClusterCommandReplyPacket inPacket, ClusterUpdateOrganizationPacket outPacket)
        {
            QQEventArgs<ClusterCommandReplyPacket, ClusterUpdateOrganizationPacket> e = new QQEventArgs<ClusterCommandReplyPacket, ClusterUpdateOrganizationPacket>(client, inPacket, outPacket);
            if (inPacket.ReplyCode == ReplyCode.OK)
            {
                client.ClusterManager.OnUpdateOrganizationSuccessed(e);
            }
            else
            {
                client.ClusterManager.OnUpdateOrganizationFailed(e);
            }
        }

        /// <summary>
        /// Processes the cluster get online member reply.
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="outPacket">The out packet.</param>
        private void ProcessClusterGetOnlineMemberReply(ClusterCommandReplyPacket inPacket, ClusterGetOnlineMemberPacket outPacket)
        {
            QQEventArgs<ClusterCommandReplyPacket, ClusterGetOnlineMemberPacket> e = new QQEventArgs<ClusterCommandReplyPacket, ClusterGetOnlineMemberPacket>(client, inPacket, outPacket);
            if (inPacket.ReplyCode == ReplyCode.OK)
            {
                client.ClusterManager.OnGetOnlineMemberSuccessed(e);
            }
            else
            {
                client.ClusterManager.OnGetOnlineMemberFailed(e);
            }
        }
        /// <summary>
        /// Processes the cluster get info reply.
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="outPacket">The out packet.</param>
        private void ProcessClusterGetInfoReply(ClusterCommandReplyPacket inPacket, ClusterGetInfoPacket outPacket)
        {
            QQEventArgs<ClusterCommandReplyPacket, ClusterGetInfoPacket> e = new QQEventArgs<ClusterCommandReplyPacket, ClusterGetInfoPacket>(client, inPacket, outPacket);
            if (inPacket.ReplyCode == ReplyCode.OK)
            {
                client.ClusterManager.OnGetClusterInfoSuccessed(e);
            }
            else
            {
                client.ClusterManager.OnGetClusterInfoFailed(e);
            }
        }
        /// <summary>
        /// Proccesses the cluster get member info reply.
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="outPacket">The out packet.</param>
        private void ProccessClusterGetMemberInfoReply(ClusterCommandReplyPacket inPacket, ClusterGetMemberInfoPacket outPacket)
        {
            QQEventArgs<ClusterCommandReplyPacket, ClusterGetMemberInfoPacket> e = new QQEventArgs<ClusterCommandReplyPacket, ClusterGetMemberInfoPacket>(client, inPacket, outPacket);
            if (inPacket.ReplyCode == ReplyCode.OK)
            {
                client.ClusterManager.OnGetMemberInfoSuccessed(e);
            }
            else
            {
                client.ClusterManager.OnGetMemberInfoFailed(e);
            }
        }

        /// <summary>
        /// Processes the cluster get card reply.
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="outPacket">The out packet.</param>
        private void ProcessClusterGetCardReply(ClusterCommandReplyPacket inPacket, ClusterGetCardPacket outPacket)
        {
            QQEventArgs<ClusterCommandReplyPacket, ClusterGetCardPacket> e = new QQEventArgs<ClusterCommandReplyPacket, ClusterGetCardPacket>(client, inPacket, outPacket);
            if (inPacket.ReplyCode == ReplyCode.OK)
            {
                client.ClusterManager.OnGetCardSuccessed(e);
            }
            else
            {
                client.ClusterManager.OnGetCardFailed(e);
            }
        }
        /// <summary>
        /// Processes the cluster get card batch reply.
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="outPacket">The out packet.</param>
        private void ProcessClusterGetCardBatchReply(ClusterCommandReplyPacket inPacket, ClusterGetCardBatchPacket outPacket)
        {
            QQEventArgs<ClusterCommandReplyPacket, ClusterGetCardBatchPacket> e = new QQEventArgs<ClusterCommandReplyPacket, ClusterGetCardBatchPacket>(client, inPacket, outPacket);
            if (inPacket.ReplyCode == ReplyCode.OK)
            {
                client.ClusterManager.OnBatchGetCardSuccessed(e);
            }
            else
            {
                client.ClusterManager.OnBatchGetCardFailed(e);
            }
        }

        /// <summary>
        /// Processes the cluster sub cluster op reply.
        /// </summary>
        /// <param name="inPacket">The in packet.</param>
        /// <param name="outPacket">The out packet.</param>
        private void ProcessClusterSubClusterOpReply(ClusterCommandReplyPacket inPacket, ClusterSubClusterOpPacket outPacket)
        {
            QQEventArgs<ClusterCommandReplyPacket, ClusterSubClusterOpPacket> e = new QQEventArgs<ClusterCommandReplyPacket, ClusterSubClusterOpPacket>(client, inPacket, outPacket);
            if (inPacket.ReplyCode == ReplyCode.OK)
            {
                client.ClusterManager.OnGetDialogSubjectSuccessed(e);
            }
            else
            {
                client.ClusterManager.OnGetDialogSubjectFailed(e);
            }
            }
        #endregion
    }
}
