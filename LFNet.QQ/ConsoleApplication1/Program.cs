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
using LFNet.QQ;
using LFNet.QQ.Packets.In;
using LFNet.QQ.Packets.Out;
using LFNet.QQ.Packets;


using System.Threading;
using System.Diagnostics;
using LFNet.QQ.Entities;
//using System.Windows.Forms
namespace ConsoleApplication1
{
    class Program
    {
        /// <summary>
        /// 聊天的QQ好友
        /// </summary>
        //static int TQQ = 0;
        static LFNet.QQ.Entities.QQBasicInfo TQQ = null;
        static bool AutoReply =false;
        static QQClient Client;
        static FontStyle fontStyle=new FontStyle(System.Drawing.Color.Red,"黑体",10,true,true,true);
        static void Main(string[] args)
        {
           Init: Console.Title = "LFNet.QQ 基于QQ2009协议开发 作者:dobit QQ:156798087";
            StartLogin();
            AddEvents();
            HelpMessage();
            while (Client.LoginStatus==LoginStatus.Login)
            {
                string s=Console.ReadLine();
                if (s == "") continue;
                if (s.StartsWith("-") || s.StartsWith("/") || TQQ==null)
                {
                    s=s.Replace("-","").Replace("/","");
                    if (s.ToLower() == "x")
                    {
                        
                        Client.Logout();
                        Console.WriteLine();
                        Console.WriteLine("退出成功");
                        Client.LoginStatus = LoginStatus.Logout;
                        break; 
                    }
                    ParseCommand(s);
                    
                }
                else
                {
                    if (TQQ != null)
                    {
                        if (AutoReply)
                        {
                            Lynfo.GoogleTranslateApi.GoogleTranslateApi GTA = new Lynfo.GoogleTranslateApi.GoogleTranslateApi();
                            GTA.TranslateCompleted += new EventHandler<Lynfo.GoogleTranslateApi.TranslateCompletedEventArgs>(GTA_TranslateCompleted);
                            GTA.TranslateAsync(Lynfo.GoogleTranslateApi.GoogleTranslateApi.Language.CHINESE_SIMPLIFIED, Lynfo.GoogleTranslateApi.GoogleTranslateApi.Language.ENGLISH, s, TQQ);
                        }
                        else
                        {
                            SendMsg(TQQ, s);
                        
                        }
                        
                    }
                    
                }
            
            }
            //Console.WriteLine();
            Echo("是否重新登录(Y/N):");
            if (Console.ReadLine().ToLower() == "y")
            {
                goto Init;
            }
            
        }

        

        static void GTA_TranslateCompleted(object sender, Lynfo.GoogleTranslateApi.TranslateCompletedEventArgs e)
        {
            //int qq = Convert.ToInt32(e.UserState);
            if (e.GoogleDataInfo.responseData != null)
            {
                //Echo(string.Format("系统自动{0}:{1}", qq, e.GoogleDataInfo.responseData.translatedText));
                SendMsg((QQBasicInfo)e.UserState, e.GoogleDataInfo.responseData.translatedText);
            }
        }

        private static void SendMsg(int qq, string msg )
        {
            Client.MessageManager.SendIM(qq, msg,fontStyle);
            Echo(string.Format("你对 {0}[{1}] 说:{2}", qq, Client.QQUser.Friends[qq].Nick, msg));
        }
        private static void AddEvents()
        {
            Client.PrivateManager.ChangeStatusSuccessed += new EventHandler<LFNet.QQ.Events.QQEventArgs<ChangeStatusReplyPacket, ChangeStatusPacket>>(PrivateManager_ChangeStatusSuccessed);
            Client.MessageManager.ReceiveKickOut += new EventHandler<LFNet.QQ.Events.QQEventArgs<ReceiveIMPacket, OutPacket>>(MessageManager_ReceiveKickOut);
            //client2.PrivateManager.ChangeStatus(QQStatus.HIDDEN);
            Client.PrivateManager.GetLevelSuccessed += new EventHandler<LFNet.QQ.Events.QQEventArgs<GetLevelReplyPacket, GetLevelPacket>>(PrivateManager_GetLevelSuccessed);
            Client.PrivateManager.GetLevel();
            Client.FriendManager.DownloadGroupFriends(0);
            Client.FriendManager.GetFriendListSuccessed += new EventHandler<LFNet.QQ.Events.QQEventArgs<GetFriendListReplyPacket, GetFriendListPacket>>(FriendManager_GetFriendListSuccessed);
            Client.FriendManager.GetOnlineFriendSuccessed += new EventHandler<LFNet.QQ.Events.QQEventArgs<GetOnlineOpReplyPacket, GetOnlineOpPacket>>(FriendManager_GetOnlineFriendSuccessed);
            Client.FriendManager.GetFriendList();
            Client.MessageManager.ReceiveNormalIM += new EventHandler<LFNet.QQ.Events.QQEventArgs<ReceiveIMPacket, OutPacket>>(MessageManager_ReceiveNormalIM);
            Client.FriendManager.FriendChangeStatus += new EventHandler<LFNet.QQ.Events.QQEventArgs<FriendChangeStatusPacket, OutPacket>>(FriendManager_FriendChangeStatus);
            Client.FriendManager.AddFriendNeedAuth += new EventHandler<LFNet.QQ.Events.QQEventArgs<AddFriendReplyPacket, AddFriendPacket>>(FriendManager_AddFriendNeedAuth);
            Client.FriendManager.AddFriendDeny += new EventHandler<LFNet.QQ.Events.QQEventArgs<AddFriendReplyPacket, AddFriendPacket>>(FriendManager_AddFriendDeny);
            Client.FriendManager.AddFriendNeedAnswer += new EventHandler<LFNet.QQ.Events.QQEventArgs<AddFriendReplyPacket, AddFriendPacket>>(FriendManager_AddFriendNeedAnswer);
            Client.FriendManager.AddFriendSuccessed += new EventHandler<LFNet.QQ.Events.QQEventArgs<AddFriendReplyPacket, AddFriendPacket>>(FriendManager_AddFriendSuccessed);
            Client.MessageManager.SysAddedByOthers += new EventHandler<LFNet.QQ.Events.QQEventArgs<SystemNotificationPacket, OutPacket>>(MessageManager_SysAddedByOthers);
            Client.MessageManager.SysAddedByOthersEx += new EventHandler<LFNet.QQ.Events.QQEventArgs<SystemNotificationPacket, OutPacket>>(MessageManager_SysAddedByOthersEx);
            Client.MessageManager.SysAddOtherApproved += new EventHandler<LFNet.QQ.Events.QQEventArgs<SystemNotificationPacket, OutPacket>>(MessageManager_SysAddOtherApproved);
            Client.MessageManager.SysAddOtherRejected += new EventHandler<LFNet.QQ.Events.QQEventArgs<SystemNotificationPacket, OutPacket>>(MessageManager_SysAddOtherRejected);
            Client.MessageManager.SysRequestAddMe += new EventHandler<LFNet.QQ.Events.QQEventArgs<SystemNotificationPacket, OutPacket>>(MessageManager_SysRequestAddMe);
            Client.MessageManager.SysRequestAddMeEx += new EventHandler<LFNet.QQ.Events.QQEventArgs<SystemNotificationPacket, OutPacket>>(MessageManager_SysRequestAddMeEx);
            Client.MessageManager.SysApprovedAddOtherAndAddMe += new EventHandler<LFNet.QQ.Events.QQEventArgs<SystemNotificationPacket, OutPacket>>(MessageManager_SysApprovedAddOtherAndAddMe);
            Client.ClusterManager.ReceiveClusterIM += new EventHandler<LFNet.QQ.Events.QQEventArgs<ReceiveIMPacket, OutPacket>>(ClusterManager_ReceiveClusterIM);
            Client.ClusterManager.GetClusterInfoSuccessed += new EventHandler<LFNet.QQ.Events.QQEventArgs<ClusterCommandReplyPacket, ClusterGetInfoPacket>>(ClusterManager_GetClusterInfoSuccessed);
            Client.MessageManager.ReceiveVibration += new EventHandler<LFNet.QQ.Events.QQEventArgs<ReceiveIMPacket, OutPacket>>(MessageManager_ReceiveVibration);
            Client.MessageManager.ReceiveInputState += new EventHandler<LFNet.QQ.Events.QQEventArgs<ReceiveIMPacket, OutPacket>>(MessageManager_ReceiveInputState);
        }

        static void ClusterManager_GetClusterInfoSuccessed(object sender, LFNet.QQ.Events.QQEventArgs<ClusterCommandReplyPacket, ClusterGetInfoPacket> e)
        {
            ClusterInfo clusterInfo=  Client.QQUser.ClusterList[(int)e.InPacket.ClusterId];
            clusterInfo.Name = e.InPacket.Info.Name;
            clusterInfo.ExternalId = e.InPacket.Info.ExternalId;
            clusterInfo.Description = e.InPacket.Info.Description;
            Echo(string.Format("群：{0} 名称：{1} 内部号：{2} 描述：{3}", clusterInfo.ExternalId,e.InPacket.Info.Name,clusterInfo.QQBasicInfo.QQ,clusterInfo.Description));
        }

        static void MessageManager_ReceiveInputState(object sender, LFNet.QQ.Events.QQEventArgs<ReceiveIMPacket, OutPacket> e)
        {
            Echo(string.Format("好友 {0}[{1}] 正在输入...", e.InPacket.Header.Sender, Client.QQUser.Friends[(int)e.InPacket.Header.Sender].Nick));
        }

        static void MessageManager_ReceiveVibration(object sender, LFNet.QQ.Events.QQEventArgs<ReceiveIMPacket, OutPacket> e)
        {
            Echo(string.Format("收到好友 {0}[{1}] 振动", e.InPacket.Header.Sender, Client.QQUser.Friends[(int)e.InPacket.Header.Sender].Nick));
        }

        static void ClusterManager_ReceiveClusterIM(object sender, LFNet.QQ.Events.QQEventArgs<ReceiveIMPacket, OutPacket> e)
        {
            Echo(string.Format("{0},{2}在群{1}中说:{3}", LFNet.QQ.Utils.Util.GetDateTimeFromMillis(e.InPacket.ClusterIM.SendTime), e.InPacket.Header.Sender, e.InPacket.ClusterIM.Sender, e.InPacket.ClusterIM.Message));
#if Robot
            string ret = LFNet.Robot.Robot.Parse(e.InPacket.ClusterIM.TextMessage,Guid.Empty,null);
            if (!string.IsNullOrEmpty(ret))
            {
                Echo(string.Format("系统自动回复群{0}:{1}", e.InPacket.ClusterIM.ExternalId, ret));
                e.QQClient.ClusterManager.SendClusterIM((int)e.InPacket.Header.Sender, ret);
            }
            else
            {
                if (AutoReply && e.InPacket.ClusterIM.Sender != e.QQClient.QQUser.QQ)
                {
                    Lynfo.GoogleTranslateApi.GoogleTranslateApi GTA = new Lynfo.GoogleTranslateApi.GoogleTranslateApi();
                    GTA.TranslateCompleted += new EventHandler<Lynfo.GoogleTranslateApi.TranslateCompletedEventArgs>(GTA_TranslateCompleted2);
                    GTA.TranslateAsync(Lynfo.GoogleTranslateApi.GoogleTranslateApi.Language.CHINESE_SIMPLIFIED, Lynfo.GoogleTranslateApi.GoogleTranslateApi.Language.ENGLISH, e.InPacket.ClusterIM.TextMessage, new LFNet.QQ.Entities.QQBasicInfo( e.InPacket.Header.Sender,QQType.Cluster,0));
                }
            
            }
#endif
        }

        static void GTA_TranslateCompleted2(object sender,Lynfo.GoogleTranslateApi.TranslateCompletedEventArgs e)
        {
            //int qq=Convert.ToInt32(e.UserState);
            QQBasicInfo qq = (QQBasicInfo)e.UserState;
            if (e.GoogleDataInfo.responseData != null)
            {
                Echo(string.Format("系统自动回复群{0}:{1}", qq.QQ, e.GoogleDataInfo.responseData.translatedText));
                Client.ClusterManager.SendClusterIM(qq.QQ, e.GoogleDataInfo.responseData.translatedText);
            }
        }
        private static void SendMsg(QQBasicInfo TQQ, string msg)
        {
            switch (TQQ.Type)
            { 
                case QQType.QQ:
                    Client.MessageManager.SendIM(TQQ.QQ, msg,fontStyle);
                    Echo(string.Format(DateTime.Now.ToShortTimeString()+",你对 {0}[{1}] 说:\n\t{2}", TQQ.QQ, Client.QQUser.Friends[TQQ.QQ].Nick, msg));
                    break;
                case QQType.Cluster:
                    Client.ClusterManager.SendClusterIM(TQQ.QQ, msg);
                    Echo(string.Format(DateTime.Now.ToShortTimeString() + "你在群 {0}[{1}] 里说:\n\t{2}", Client.QQUser.ClusterList[TQQ.QQ].ExternalId, Client.QQUser.ClusterList[TQQ.QQ].Name, msg));
                    break;
            
            }
        }
        private static void SendClusterMsg(int p, string msg)
        {
            
        }
        static void MessageManager_SysApprovedAddOtherAndAddMe(object sender, LFNet.QQ.Events.QQEventArgs<SystemNotificationPacket, OutPacket> e)
        {
            Echo(string.Format("{0} 通过你的请求，并加你为好友", e.InPacket.From));
            //LFNet.QQ.Entities.QQFriend friend = new LFNet.QQ.Entities.QQFriend();
            //friend.QQBasicInfo.QQ = e.InPacket.From;
            e.QQClient.QQUser.Friends.Add(e.InPacket.From).FriendStatus.Status = QQStatus.ONLINE;
        }

        static void FriendManager_AddFriendSuccessed(object sender, LFNet.QQ.Events.QQEventArgs<AddFriendReplyPacket, AddFriendPacket> e)
        {
            Echo(string.Format("添加好友 {0} 成功 ", e.InPacket.FriendQQ));
            e.QQClient.FriendManager.AddFriendToList(0, e.InPacket.FriendQQ);
        }

        static void FriendManager_AddFriendNeedAnswer(object sender, LFNet.QQ.Events.QQEventArgs<AddFriendReplyPacket, AddFriendPacket> e)
        {
            Echo(string.Format("添加QQ:{0} 需要回答问题 \n系统暂不支持回答问题的方式加好友", e.InPacket.FriendQQ)); 
        }

        static void FriendManager_AddFriendDeny(object sender, LFNet.QQ.Events.QQEventArgs<AddFriendReplyPacket, AddFriendPacket> e)
        {
            Echo(string.Format("QQ:{0} 拒绝加为好友", e.InPacket.FriendQQ)); 
        }

        static void FriendManager_AddFriendNeedAuth(object sender, LFNet.QQ.Events.QQEventArgs<AddFriendReplyPacket, AddFriendPacket> e)
        {
            Echo(string.Format("添加QQ:{0} 需要输入验证信息 \n请使用 -add {0} [验证信息] 的命令", e.InPacket.FriendQQ)); 
        }

        static void MessageManager_SysRequestAddMeEx(object sender, LFNet.QQ.Events.QQEventArgs<SystemNotificationPacket, OutPacket> e)
        {
            Echo(string.Format("{0} 加你为好友,附加消息:{1}\n系统已经自动同意加对方为好友", e.InPacket.From, e.InPacket.Message));//输入 -add {0} 命令加{0}为好友
            //e.QQClient.FriendManager.ApprovedAddMe(e.InPacket.From);
            if (e.InPacket.ReverseAdd == RevenseAdd.Allow)//对方允许添加好友
            {
                e.QQClient.FriendManager.ApprovedAndAdd(e.InPacket.From);
                e.QQClient.FriendManager.AddFriendToList(0, e.InPacket.From);
                //LFNet.QQ.Entities.QQFriend friend=new LFNet.QQ.Entities.QQFriend();
                //friend.QQBasicInfo=e.InPacket.From;
                e.QQClient.QQUser.Friends.Add(e.InPacket.From).FriendStatus.Status = QQStatus.ONLINE;
            }
            else //走其它添加好友的方式
            {
                e.QQClient.FriendManager.ApprovedAdd(e.InPacket.From);
                //e.QQClient.FriendManager.AddFriendToList(0, e.InPacket.From);
                //e.QQClient.FriendManager.AddFriend(e.InPacket.From);
            }
        }

        static void MessageManager_SysRequestAddMe(object sender, LFNet.QQ.Events.QQEventArgs<SystemNotificationPacket, OutPacket> e)
        {
            Echo(string.Format("{0} 加你为好友,附加消息:{1}\n输入 -add {0} 命令加{0}为好友", e.InPacket.From, e.InPacket.Message));
        }

        static void MessageManager_SysAddOtherRejected(object sender, LFNet.QQ.Events.QQEventArgs<SystemNotificationPacket, OutPacket> e)
        {
            Echo(string.Format("{0} 拒绝加你为好友,附加消息:{1}", e.InPacket.From,e.InPacket.Message));
        }

        static void MessageManager_SysAddOtherApproved(object sender, LFNet.QQ.Events.QQEventArgs<SystemNotificationPacket, OutPacket> e)
        {
            Echo(string.Format("{0} 通过您的认证", e.InPacket.From));
        }

        static void MessageManager_SysAddedByOthersEx(object sender, LFNet.QQ.Events.QQEventArgs<SystemNotificationPacket, OutPacket> e)
        {
            Echo(string.Format("{0} 加你为好友,附加消息:{1}\n 输入 -add {0} 命令加{0}为好友", e.InPacket.From, e.InPacket.Message));
        }

        static void MessageManager_SysAddedByOthers(object sender, LFNet.QQ.Events.QQEventArgs<SystemNotificationPacket, OutPacket> e)
        {
            Echo(string.Format("{0} 加你为好友.",e.InPacket.From));
        }

        

        static void MessageManager_ReceiveKickOut(object sender, LFNet.QQ.Events.QQEventArgs<ReceiveIMPacket, OutPacket> e)
        {
            Client.LoginStatus = LoginStatus.Logout;
            Echo(e.InPacket.SysMessage);
            
        }

        static void FriendManager_FriendChangeStatus(object sender, LFNet.QQ.Events.QQEventArgs<FriendChangeStatusPacket, OutPacket> e)
        {
            Echo(string.Format("好友{0}[{1}],更改状态为 {2}", e.InPacket.FriendQQ.ToString(),e.QQClient.QQUser.Friends[(int)e.InPacket.FriendQQ].Nick,  e.InPacket.Status.ToString()));
        }

       

        static void MessageManager_ReceiveNormalIM(object sender, LFNet.QQ.Events.QQEventArgs<ReceiveIMPacket, OutPacket> e)
        {
            Echo(string.Format("{0},{1}[{2}] 说:{3}",LFNet.QQ.Utils.Util.GetDateTimeFromMillis( e.InPacket.NormalHeader.SendTime), e.InPacket.Header.Sender,e.QQClient.QQUser.Friends[(int)e.InPacket.Header.Sender].Nick, e.InPacket.NormalIM.Message));
#if Robot
            string ret = LFNet.Robot.Robot.Parse(e.InPacket.NormalIM.TextMessage,Guid.Empty,null);
            if (!string.IsNullOrEmpty(ret))
            {
                Echo(string.Format("系统自动回复{0}说:{1}", e.InPacket.NormalHeader.Sender, ret));
                e.QQClient.MessageManager.SendIM(e.InPacket.NormalHeader.Sender, ret);
            }
            else
            {
                if (AutoReply)
                {
                    Lynfo.GoogleTranslateApi.GoogleTranslateApi GTA = new Lynfo.GoogleTranslateApi.GoogleTranslateApi();
                    GTA.TranslateCompleted += new EventHandler<Lynfo.GoogleTranslateApi.TranslateCompletedEventArgs>(GTA_TranslateCompleted);
                    GTA.TranslateAsync(Lynfo.GoogleTranslateApi.GoogleTranslateApi.Language.CHINESE_SIMPLIFIED, Lynfo.GoogleTranslateApi.GoogleTranslateApi.Language.ENGLISH, e.InPacket.NormalIM.TextMessage,new QQBasicInfo( e.InPacket.Header.Sender,QQType.QQ,0));
                }
            }
#endif
        }


        static void FriendManager_GetOnlineFriendSuccessed(object sender, LFNet.QQ.Events.QQEventArgs<GetOnlineOpReplyPacket, GetOnlineOpPacket> e)
        {
            //Console.WriteLine("获取在线好友列表成功！");
            
        }

        static void FriendManager_GetFriendListSuccessed(object sender, LFNet.QQ.Events.QQEventArgs<GetFriendListReplyPacket, GetFriendListPacket> e)
        {
            //Console.WriteLine("获取好友列表成功！");
            //Console.WriteLine("好友\t\t\t昵称\t头像\tAge\tsex");
            //foreach (KeyValuePair<int,FriendInfo> Item in e.QQClient.QQUser.Friends)
            //{
            //    Console.WriteLine(string.Format("{0,-12}\t{1,-15}\t{2}\t{3}\t{4}", Item.Value.BasicInfo.QQ, Item.Value.BasicInfo.Nick, Item.Value.BasicInfo.Header.ToString(), Item.Value.BasicInfo.Age, Item.Value.BasicInfo.Gender));
            //}
            e.QQClient.FriendManager.GetOnlineFriend();
        }

        static void PrivateManager_GetLevelSuccessed(object sender, LFNet.QQ.Events.QQEventArgs<GetLevelReplyPacket, GetLevelPacket> e)
        {
            Echo(string.Format("Level:{0}  ActiveDays:{1} UpgradeDays:{2}", e.InPacket.Level, e.InPacket.ActiveDays, e.InPacket.UpgradeDays));
            
        }

        static void PrivateManager_ChangeStatusSuccessed(object sender, LFNet.QQ.Events.QQEventArgs<ChangeStatusReplyPacket, ChangeStatusPacket> e)
        {
            Echo("改变状态成功：" + e.QQClient.QQUser.Status.ToString());
        }

        static void LoginManager_LoginStatusChanged(object sender, LFNet.QQ.Events.LoginStatusChangedEventArgs<LoginStatus> e)
        {
            if (e.LoginStatus == LoginStatus.ChangeServer)
            {
                Echo(e.LoginStatus.ToString()+":"+Client.LoginServerHost);
            }else
            Echo(e.LoginStatus.ToString());
        }

        static void LoginManager_LoginNeedVerifyCode(object sender, LFNet.QQ.Events.QQEventArgs<LFNet.QQ.Packets.In.LoginRequestReplyPacket, LFNet.QQ.Packets.Out.LoginRequestPacket> e)
        {
            Console.Write("请输入验证码("+e.InPacket.CodeFileName+")：");
            string code=Console.ReadLine();
            e.QQClient.LoginManager.LoginSendVerifyCode(code);
        }

        private static void ParseCommand(string s)
        {
            string[] args = s.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (args.Length > 0)
            {
                switch (args[0].ToLower())
                {
                    case "t":
                    case "to":
                        if (args.Length > 1)
                            ChatToQQ(args[1]);
                        break;
                    case "l":
                    case "list":
                    case "view":
                        List();
                        break;
                    case "o":
                    case "ol":
                    case "onlines":
                    case "listonlines":
                        OnLineList();
                        break;
                    case "add":
                    case "+":
                    case "a"://应该不要吧
                        int q = 0;
                        try
                        {
                            q = int.Parse(args[1]);
                        }
                        catch (Exception e)
                        {
                            Echo("添加失败，请输入正确的QQ号码！");
                            break;
                        }
                        if (args.Length > 2)
                        {
                            Client.FriendManager.SendAddFriendAuth(q, args[2]);
                            Echo("添加好友:" + q.ToString() + ",附加消息" + args[2]);
                        }
                        else
                        {
                            Client.FriendManager.AddFriend(q);
                            Echo("添加好友:" + q.ToString());
                        }
                        FriendAction();
                        break;
                    case "v":
                    case "vibration":
                        if (args.Length > 1)
                        {
                            int qqnum = 0;
                            if (int.TryParse(args[1], out qqnum))
                            {
                                if (Client.QQUser.Friends.ContainsKey(qqnum))
                                {
                                    int t = 1;
                                    if (args.Length > 2)
                                        int.TryParse(args[2], out t);
                                    for (int i = 0; i < t; i++)
                                    {
                                        Client.MessageManager.SendVibration(qqnum);
                                        Echo(string.Format("你给 {1}[{0}] 发送振动", qqnum, Client.QQUser.Friends[qqnum].Nick));
                                        Thread.Sleep(10000);//休眠 防止发送过快
                                    }


                                }
                                else
                                    Echo("未找到该QQ好友！");
                            }
                            else
                            {
                                Echo("QQ号错误，命令格式为: -v 123456");
                            }
                        }
                        else
                        {
                            if (TQQ != null)
                            {
                                Client.MessageManager.SendVibration(TQQ.QQ);
                                Echo(string.Format("你给 {1}[{0}] 发送振动", TQQ, Client.QQUser.Friends[TQQ.QQ].Nick));
                            }
                        }
                        break;

                    case "h":
                    case "help":
                    case "?":
                        HelpMessage();
                        break;
                    case "e":
                    case "en":
                    case "english":
                        if (AutoReply)
                        {
                            AutoReply = false;
                            Echo("自动英文回复关闭");
                        }
                        else
                        {
                            AutoReply = true;
                            Echo("自动英文回复开启");
                        }
                        break;

                    case "c":
                    case "cls":
                        Console.Clear();
                        break;

                }

            }
        }
        private static void StartLogin()
        {
        Init:
#if RELEASE
            int qq=0;
            bool IsQQNum = false;
            
            while (!IsQQNum)
            {
                try
                {
                    Console.Write("QQ账号:");
                    string s = Console.ReadLine();
                    Console.WriteLine("您输入的QQ号是:{0}", s);
                    qq = int.Parse(s);
                    IsQQNum = true;
                }
                catch
                {
                    Console.WriteLine("QQ号码错误,请重新输入:");
                    IsQQNum = false;
                }
            }
            Console.Write("密码:");
            string p = "";
            p = Console.ReadLine();

            Console.Write("是否隐身登陆(Y/N):");
            string st=Console.ReadLine();
            Console.Clear();
            QQUser user2 = new QQUser(qq, p);
            if(st=="Y"||st=="y")
            {
                user2.LoginMode = QQStatus.HIDDEN;
            }
#else
            QQUser user2 = new QQUser(1667100016, "qweqwe123++");
#endif
            Client =new QQClient(user2);
            Client.LoginManager.LoginStatusChanged += new EventHandler<LFNet.QQ.Events.LoginStatusChangedEventArgs<LoginStatus>>(LoginManager_LoginStatusChanged);
            //user2.IsUdp = true;//设置登录模式 udp 或tcp 
            Client.LoginServerHost = "219.133.49.171"; //指定默认的服务器IP地址，可以不指定服务器IP，系统自动搜索可用的IP
            //UDP登录服务器【sz.tencent.com】域名解析成功,解析为【219.133.49.171】 
            //UDP登录服务器【sz2.tencent.com】域名解析成功,解析为【219.133.60.31】 
            //UDP登录服务器【sz3.tencent.com】域名解析成功,解析为【219.133.48.87】 
            //UDP登录服务器【sz4.tencent.com】域名解析成功,解析为【219.133.40.138】 
            //UDP登录服务器【sz5.tencent.com】域名解析成功,解析为【219.133.49.169】 
            //UDP登录服务器【sz6.tencent.com】域名解析成功,解析为【219.133.60.37】 
            //UDP登录服务器【sz7.tencent.com】域名解析成功,解析为【219.133.48.89】 
            //UDP登录服务器【sz8.tencent.com】域名解析成功,解析为【219.133.40.37】 
            //UDP登录服务器【sz9.tencent.com】域名解析成功,解析为【219.133.60.39】 
            Client.Login();
            while (Client.LoginStatus != LoginStatus.Login)//如果没有登录成功则
            {
                if (Client.LoginStatus == LoginStatus.NeedVerifyCode)
                {
                    Console.Write("请输入验证码：");
                    string code = Console.ReadLine();
                    Client.LoginStatus = LoginStatus.SendVerifyCode;
                    Client.LoginManager.LoginSendVerifyCode(code);

                }
                else if (Client.LoginStatus == LoginStatus.WrongPassword)
                {
                    goto Init;
                }
                //else if(Client.LoginStatus ==LoginStatus.ChangeServer)
                //{
                //    Console.WriteLine("尝试连接："+Client.LoginServerHost);
                //}
                Thread.Sleep(100);
            }
            foreach (ClusterInfo clusterInfo in Client.QQUser.ClusterList.Values)
            {
                if (clusterInfo.ExternalId == 0)
                {
                    Client.ClusterManager.GetClusterInfo(clusterInfo.QQBasicInfo.QQ);
                }
            }
        }
        /// <summary>
        /// 和谁聊天
        /// </summary>
        /// <param name="args"></param>
        private static void ChatToQQ(string To)
        {
            int qqnum = 0;
            if (int.TryParse(To, out qqnum))
            {
                if (Client.QQUser.Friends.ContainsKey(qqnum))
                {
                    TQQ = Client.QQUser.QQList[qqnum];
                    Echo("你将和 " + TQQ.QQ.ToString() + "[" + Client.QQUser.Friends[TQQ.QQ].Nick + "] 聊天。");


                }
                else if (Client.QQUser.ClusterList.ContainsKey(qqnum))
                {
                    TQQ = Client.QQUser.ClusterList[qqnum].QQBasicInfo;
                    Echo("你将在群 " + TQQ.QQ.ToString() + "[" + Client.QQUser.ClusterList[TQQ.QQ].Name + "] 中聊天。");
                }
                else
                {
                    Echo("输入的QQ号错误，当前好友列表中没有该好友！请使用/view命令查看好友列表");
                }

            }
            else
            {
                //var q=from fi in Client.QQUser.Friends.Values where  fi.
                List<QQFriend> fl = new List<QQFriend>();
                foreach (QQFriend Friend in Client.QQUser.Friends.Values)
                {
                    if (Friend.Nick != null && Friend.Nick.Contains(To))
                    {
                        fl.Add(Friend);
                    }
                }
                if (fl.Count == 0)
                {
                    Echo("好友列表中没有该好友！请使用/view命令查看好友列表");
                }
                else if (fl.Count == 1)
                {
                    TQQ = fl[0].QQBasicInfo;
                    Echo("你将和 " + TQQ.QQ.ToString() + "[" + Client.QQUser.Friends[TQQ.QQ].Nick + "] 聊天。");
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("共找到 " + fl.Count.ToString() + " 位好友,请使用-To QQ号命令从下面选择一位好友");
                    sb.AppendLine(string.Format("{0,-12}{1,-20}\t{2,-5}\t{3,-5}\t{4,-5}\t{5,5}", "好友", "昵称", "头像", "年龄", "性别", "状态"));
                    foreach (QQFriend Friend in fl)
                    {
                        sb.AppendLine(string.Format("{0,-12}{1,-20}\t{2,7}\t{3,7}\t{4,7}\t{5,7}", Friend.QQ, Friend.Nick, Friend.Header.ToString(), Friend.Age, Friend.Gender, Friend.FriendStatus.Status.ToString()));
                    }
                    Echo(sb.ToString());
                }
            }
        }
        static void HelpMessage()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("==========================================================");
            sb.AppendLine("本软件基于QQ2009协议开发，仅用于学习研究，不得用于商业用途");
            sb.AppendLine("作者:dobit QQ:156798087");
            sb.AppendLine("Email:dobit@msn.cn");
            sb.AppendLine("代码开发过程参阅了LumaQQ.net(C#.net)、MYQQ(C++)等源代码");
            sb.AppendLine("在这里对LumaQQ.net(C#.net)、MYQQ(C++)作品的开发人员表示感谢！");
            sb.AppendLine("==========================================================");
            sb.AppendLine("帮助信息:");
            sb.AppendLine("\t-h help ? 显示帮助信息;");
            sb.AppendLine("\t-t to 与好友对话;");
            sb.AppendLine("\t\t-t to 123456 与QQ号为123456的好友对话;");
            sb.AppendLine("\t\t-t to 张三 与昵称为张三的好友对话;");
            sb.AppendLine("\t-a add 添加好友;");
            sb.AppendLine("\t\t-add 12345 添加12345为好友;");
            sb.AppendLine("\t-l list view 显示全部好友信息;");
            sb.AppendLine("\t-0 lo onlines listOnlines 显示全部在线好友信息;");
            sb.AppendLine("\t-v vibration 给好友发送振动;");
            sb.AppendLine("\t\t-v 给当前聊天好友发送振动;");
            sb.AppendLine("\t\t-v 123456 10  给好友123456发送10次振动;");
            sb.AppendLine("\t-c cls 清屏幕;");

            sb.AppendLine("\t-x exit 退出;");
            sb.AppendLine("\t好友表情发送格式：/wx (微笑) /ll(流泪) /kuk (酷)...如：你好/wx  ");
            Echo(sb.ToString());
        }

        static void List()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("QQ好友:{0}",Client.QQUser.Friends.Count));
            sb.AppendLine(string.Format("{0,-12}{1,-18}\t{2,-5}\t{3,-5}\t{4,-5}\t{5,5}", "好友", "昵称", "头像", "年龄", "性别", "状态"));
            foreach (QQFriend Friend in Client.QQUser.Friends.Values)
            {
                sb.AppendLine(string.Format("{0,-12}{1,-16}\t{2,7}\t{3,7}\t{4,7}\t{5,7}", Friend.QQBasicInfo.QQ, Friend.Nick, Friend.Header.ToString(), Friend.Age, Friend.Gender, Friend.Status.ToString()));
            }
            sb.AppendLine(string.Format("QQ群:{0} 个", Client.QQUser.ClusterList.Count));
            sb.AppendLine(string.Format("{0}\t\t{1}\t{2}", "UIN","群号","群名"));
            foreach(LFNet.QQ.Entities.ClusterInfo clusterInfo in Client.QQUser.ClusterList.Values)
            {
                //if(clusterInfo.ExternalId==0)
                //{
                //    Client.ClusterManager.GetClusterInfo(clusterInfo.QQBasicInfo.QQ);
                //}
                sb.AppendLine(string.Format("{0}\t{1}\t{2}", clusterInfo.QQBasicInfo.QQ, clusterInfo.ExternalId, clusterInfo.Name));
           
            }
            Echo(sb.ToString());
        }

        static void OnLineList()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("{0,-12}{1,-18}\t{2,-5}\t{3,-5}\t{4,-5}\t{5,5}", "好友", "昵称", "头像", "年龄", "性别", "状态"));
            foreach (QQFriend Friend in Client.QQUser.Friends.Values)
            {
                if (Friend.FriendStatus.Status != QQStatus.NONE && Friend.FriendStatus.Status != QQStatus.OFFLINE)
                {
                    sb.AppendLine(string.Format("{0,-12}{1,-16}\t{2,7}\t{3,7}\t{4,7}\t{5,7}", Friend.QQBasicInfo.QQ, Friend.Nick, Friend.Header.ToString(), Friend.Age, Friend.Gender, Friend.Status.ToString()));
                }
            }
            Echo(sb.ToString());
        }
        static void Echo(string s)
        {
            if (Client.LoginStatus == LoginStatus.Login)
            {

                Console.WriteLine();
                //Console.Write(s);
                s = s.Replace("\r\n", "\n").Replace("\r","\n");
                string[] ss = s.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s1 in ss)
                {
                    Console.WriteLine(s1);
                }
                if (TQQ != null)
                {
                    if (TQQ.Type == QQType.QQ)
                    {
                        Console.Write("\nQQ>好友:" + TQQ.QQ.ToString() + "[" + Client.QQUser.Friends[TQQ.QQ].Nick + "]>");
                    }else
                        Console.Write("\nQQ>群:" + TQQ.QQ.ToString() + "[" + Client.QQUser.ClusterList[TQQ.QQ].Name + "]>");
                }
                else
                {
                    Console.Write("\nQQ>");
                }
            }else
                Console.WriteLine(s);
            //if(Console.
        }

        static void FriendAction()
        {
            do
            {
                List<int> RemoveItems =new List<int>();
                foreach (FriendActionInfo FAI in Client.FriendManager.FriendActionList.Values)
                {
                    if (FAI.ActionProgress == FriendActionProgress.NeedVerify)
                    {
                        Console.WriteLine();
                        Console.Write(string.Format("验证码保存在：{0}\n 请输入验证码：", FAI.Message));
                        string code = Console.ReadLine();
                        FAI.VCode = code;
                    }
                    else if (FAI.ActionProgress == FriendActionProgress.NeedAuthor)
                    {
                        Console.Write(string.Format("添加好友 {0} 需要验证信息：", FAI.QQ));
                        FAI.AuthorMessage = Console.ReadLine();
                    }
                    else if (FAI.ActionProgress == FriendActionProgress.NeedAnswer)
                    {
                        Console.Write(string.Format("添加好友 {0} 请回答下面的问题：\n", FAI.QQ));
                        Console.Write(string.Format("问题:{0} 回答:", FAI.Message));
                        FAI.AnswerMessage = Console.ReadLine();
                    }
                    else if (FAI.ActionProgress == FriendActionProgress.Finished)
                    {

                        Echo(string.Format("添加好友 {0} 完成 :{1}", FAI.QQ, FAI.Message));
                        RemoveItems.Add(FAI.QQ);
                    }
                    
                }
                if (RemoveItems != null && RemoveItems.Count>0)
                { 
                    foreach(int item in RemoveItems)
                    {
                        Client.FriendManager.FriendActionList.Remove(item);
                    }
                }
                Thread.Sleep(100);//休眠
            } while (Client.FriendManager.FriendActionList.UnFinished()!=0);
        }
    }
}
