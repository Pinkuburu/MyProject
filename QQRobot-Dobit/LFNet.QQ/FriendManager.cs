

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
    /// 好友操作进度
    /// 1.A7命令获取添加好友验证信息 直接添加 需要验证.....
    /// 2.AE命令获取令牌
    /// 3.B7回答问题
    /// 4.A8发送添加命令 完成这步就可以把权限还给线程做其它事了
    /// </summary>
    public enum FriendActionProgress
    {
        
        /// <summary>
        /// 获取用户验证信息
        /// </summary>
        RequestFriendAuthInfo,
        /// <summary>
        /// 请求操作令牌
        /// </summary>
        RequestToken,
        /// <summary>
        /// 需要验证码
        /// </summary>
        NeedVerify,
        /// <summary>
        /// 需要验证
        /// </summary>
        NeedAuthor,
        /// <summary>
        /// 需要回答问题
        /// </summary>
        NeedAnswer,
        /// <summary>
        /// 发送问题答案
        /// </summary>
        SendAnswer,
        /// <summary>
        /// 发送添加请求
        /// </summary>
        SendRequest,
        /// <summary>
        /// 服务器返回信息后
        /// </summary>
        Finished
    }

    public enum FriendAction:byte
    {
        /// <summary>
        /// 添加一个好友
        /// </summary>
        Add=0x01,
        /// <summary>
        /// 删除一个好友
        /// </summary>
        Delete=0x02
    }
    /// <summary>
    /// 要操作的好友列表
    /// </summary>
    public class FriendActionList : Dictionary<int, FriendActionInfo>
    {
        public QQClient QQClient;
        public FriendActionList(QQClient client)
        {
            this.QQClient = client;
        }
        /// <summary>
        /// 指示所有好友操作是否结束
        /// </summary>
        public bool AllFinished
        {
            get { return UnFinished() == 0; }
        }
        /// <summary>
        /// 添加一个操作QQ
        /// </summary>
        /// <param name="qq"></param>
        public void Add(int qq)
        {
            if (!this.ContainsKey(qq))
            {
                this.Add(qq, new FriendActionInfo(qq));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qq"></param>
        /// <param name="action"></param>
        public void Add(int qq, FriendAction action)
        {
            if (!this.ContainsKey(qq))
            {
                this.Add(qq, new FriendActionInfo(qq, action));
            }
        }
        public void RemoveFinished()
        {
            List<int> RemoveItems = new List<int>();
            foreach (FriendActionInfo FAI in this.Values)
            {
                if (FAI.ActionProgress == FriendActionProgress.Finished)
                {
                    RemoveItems.Add(FAI.QQ);
                }
            }

            foreach (int item in RemoveItems)
            {
                this.Remove(item);
            }

        }
        /// <summary>
        /// 等待处理的QQ数
        /// </summary>
        public int UnFinished()
        {

            int i = 0;
            foreach (FriendActionInfo FAI in this.Values)
            {
                if (FAI.ActionProgress != FriendActionProgress.Finished)
                {
                    //this.Remove(FAI.QQ);//移除操作完成的QQ号
                    if (FAI.ActionProgress == FriendActionProgress.NeedVerify && !string.IsNullOrEmpty(FAI.VCode))
                    {
                        FAI.ActionProgress = FriendActionProgress.RequestToken;
                        QQClient.FriendManager.FriendActionRequestToken(FAI.QQ, FAI.VCode);
                        FAI.VCode = null; //防止重复发包
                    }
                    else if (FAI.ActionProgress == FriendActionProgress.NeedAuthor && !string.IsNullOrEmpty(FAI.AuthorMessage))
                    {
                        if (FAI.Token != null)//说明得到了令牌
                        {
                            FAI.ActionProgress = FriendActionProgress.SendRequest;
                            QQClient.FriendManager.SendAddFriend(FAI.QQ, FAI.AuthorMessage);
                            FAI.AuthorMessage = null;
                        }
                    }
                    else if (FAI.ActionProgress == FriendActionProgress.NeedAnswer && !string.IsNullOrEmpty(FAI.AnswerMessage))
                    {
                        FAI.ActionProgress = FriendActionProgress.SendAnswer;
                        QQClient.FriendManager.SendAddFriend(FAI.QQ, FAI.AnswerMessage);
                        FAI.AnswerMessage = null;
                    }
                    i++;

                }
            }
            return i;


        }
    }

    /// <summary>
    /// 好友操作信息
    /// </summary>
    public class FriendActionInfo
    {
        public int QQ { get; private set; }
        /// <summary>
        /// 默认为添加操作
        /// </summary>
        /// <param name="qq"></param>
        public FriendActionInfo(int qq)
        {
            QQ = qq;
            Action = FriendAction.Add;
            ActionProgress = FriendActionProgress.RequestFriendAuthInfo;
        }

        public FriendActionInfo(int qq, FriendAction action)
        {
            QQ = qq;
            Action = action;
            ActionProgress = FriendActionProgress.RequestFriendAuthInfo;
            if(action==FriendAction.Delete)
                ActionProgress = FriendActionProgress.RequestToken;
            
        }
        /// <summary>
        /// 添加还是删除
        /// </summary>
        public FriendAction Action;
        /// <summary>
        /// 操作进度
        /// </summary>
        public FriendActionProgress ActionProgress;
        /// <summary>
        /// 保存当前一些信息
        /// 需要验证码：存放的是验证码存放地址
        /// 需要回答问题：存放的是回答的问题
        /// </summary>
        public string Message;
        /// <summary>
        /// 认真类型
        /// </summary>
        public AuthType AuthType;
        /// <summary>
        /// 0xAE得到的Token
        /// </summary>
        public byte[] Token;
        /// <summary>
        /// 回答问题得到的Token
        /// </summary>
        public byte[] AnswerToken;
        /// <summary>
        /// 回答问题答案
        /// </summary>
        public string AnswerMessage { get; set; }

        /// <summary>
        /// 验证信息
        /// </summary>
        public string AuthorMessage { get; set; }

        /// <summary>
        /// 获取验证码时得到的会话
        /// </summary>
        public string VCodeSession { get; set; }
        /// <summary>
        /// 存放验证码；
        /// *当有值不为空时 调用unfinished 或AllFinished 方法时会自动发送带验证码的请求Token的包；
        /// *也可以调用QQClient.FriendManager.FriendActionRequestToken发送包
        /// </summary>
        public string VCode { get; set; }
    }
    /// <summary>
    /// 好友管理类
    /// </summary>
    public class FriendManager
    {
        /// <summary>
        /// 锁
        /// </summary>
        public object lockobject = new object();
        /// <summary>
        /// Initializes a new instance of the <see cref="FriendManager"/> class.
        /// </summary>
        private FriendManager() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="FriendManager"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        internal FriendManager(QQClient client)
        {
            this.QQClient = client;
            FriendActionList = new FriendActionList(QQClient);//初始化好友列表
        }
        /// <summary>
        /// </summary>
        /// <value></value>
        public QQClient QQClient { get; private set; }
        /// <summary>
        /// </summary>
        /// <value></value>
        public QQUser QQUser { get { return QQClient.QQUser; } }

        public FriendActionList FriendActionList;

        /// <summary>搜索所有的在线用户
        /// Searches the user.
        /// </summary>
        /// <param name="page">The page.</param>
        public void SearchUser(int page)
        {
            SearchUser(page, "", "", "");
        }
        /// <summary>自定义搜索用户
        /// Searches the user.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="qq">The qq.</param>
        /// <param name="nick">The nick.</param>
        /// <param name="email">The email.</param>
        public void SearchUser(int page, string qq, string nick, string email)
        {
            SearchUserPacket packet = new SearchUserPacket(QQClient);
            packet.Page = page.ToString();
            packet.QQStr = qq;
            packet.Nick = nick;
            packet.Email = email;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }

        /// <summary>下载好友分组
        /// Downloads the group friend.
        /// </summary>
        /// <param name="beginFrom">The begin from.起始好友号 如果是第一个包，起始好友号为0</param>
        public void DownloadGroupFriends(int beginFrom)
        {
            DownloadGroupFriendPacket packet = new DownloadGroupFriendPacket(QQClient);
            packet.BeginFrom = beginFrom;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }
        /// <summary>上传分组名称
        /// Uploads the group.
        /// </summary>
        /// <param name="groups">The groups.</param>
        public void UploadGroupName(List<string> groups)
        {
            GroupDataOpPacket packet = new GroupDataOpPacket(QQClient);
            packet.Type = GroupSubCmd.UPLOAD;
            packet.Groups = groups;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }

        /// <summary>下载分组名称
        /// Downloads the group.
        /// </summary>
        public void DownloadGroupName()
        {
            GroupDataOpPacket packet = new GroupDataOpPacket(QQClient);
            packet.Type = GroupSubCmd.DOWNLOAD;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }

        /// <summary>得到用户属性
        /// Gets the user property.
        /// </summary>
        /// <param name="startPosition">The start position.</param>
        public void GetUserProperty(ushort startPosition)
        {
            UserPropertyOpPacket packet = new UserPropertyOpPacket(QQClient);
            packet.StartPosition = startPosition;
            packet.SubCommand = UserPropertySubCmd.GET;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }

        /// <summary>得到好友等级
        /// Gets the friend level.
        /// </summary>
        /// <param name="friends">The friends.</param>
        public void GetFriendLevel(List<int> friends)
        {
            FriendLevelOpPacket packet = new FriendLevelOpPacket(QQClient);
            packet.Friends = friends;
            packet.SubCommand = FriendLevelSubCmd.GET;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }

        /// <summary>根据QQ号码读取个性签名
        /// Gets the signature.
        /// </summary>
        /// <param name="qq">The qq.</param>
        public void GetSignature(int qq)
        {
            List<Signature> list = new List<Signature>();
            Signature sig = new Signature();
            sig.QQ = qq;
            list.Add(sig);
            GetSignature(list);
        }
        /// <summary>读取个性签名
        /// <remarks>在得到好友的个性签名时，QQ的做法是对所有的QQ号排个序，每次最多请求33个</remarks>
        /// Gets the signature.
        /// </summary>
        /// <param name="sigs">The sigs.</param>
        public void GetSignature(List<Signature> sigs)
        {
            SignatureOpPacket packet = new SignatureOpPacket(QQClient);
            packet.SubCommand = SignatureSubCmd.GET;
            packet.Signatures = sigs;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }

        /// <summary>下载好友备注信息
        /// Downloads the friend remark.
        /// </summary>
        /// <param name="qq">The qq.</param>
        public void DownloadFriendRemark(int qq)
        {
            FriendDataOpPacket packet = new FriendDataOpPacket(QQClient);
            packet.SubCommand = FriendOpSubCmd.DOWNLOAD_FRIEND_REMARK;
            packet.QQ = qq;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }
        /// <summary>批量下载好友备注信息
        /// Batches the download friend remark.
        /// </summary>
        /// <param name="page">The page.页号</param>
        public void BatchDownloadFriendRemark(int page)
        {
            FriendDataOpPacket packet = new FriendDataOpPacket(QQClient);
            packet.SubCommand = FriendOpSubCmd.BATCH_DOWNLOAD_FRIEND_REMARK;
            packet.Page = page;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }
        /// <summary>上传好友备信息
        /// Uploads the friend remark.
        /// </summary>
        /// <param name="qq">The qq.</param>
        /// <param name="remark">The remark.</param>
        public void UploadFriendRemark(int qq, FriendRemark remark)
        {
            FriendDataOpPacket packet = new FriendDataOpPacket(QQClient);
            packet.Remark = remark;
            packet.QQ = qq;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }
        #region 添加、删除好友
        /// <summary>
        /// 添加一个好友
        /// 不需要验证会返回添加成功的信息
        /// 第一步 发送A7命令 获取要添加的QQ的添加好友设置
        /// </summary>
        /// <param name="qq">The qq.</param>
        public void AddFriend(int qq)
        {
            AddFriendPacket packet = new AddFriendPacket(QQClient);
            packet.To = qq;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name, false);
            FriendActionList.Add(qq);//把添加好友加到操作列表
        }
        /// <summary>
        /// 好友操作
        /// 第二步 0xAE请求令牌
        /// </summary>
        /// <param name="qq"></param>
        public void FriendActionRequestToken(int qq)
        {
            FriendActionRequestToken(qq, null);
        }
        /// <summary>
        /// 好友操作 输入验证码
        /// 第二步 0xAE请求令牌
        /// </summary>
        /// <param name="qq">qq</param>
        /// <param name="verifyCode">验证码</param>
        public void FriendActionRequestToken(int qq, string verifyCode)
        {
            FriendActionList[qq].ActionProgress = FriendActionProgress.RequestToken;
            RequestTokenPacket requestTokenPacket = new RequestTokenPacket(QQClient);
            requestTokenPacket.QQ = qq;
            requestTokenPacket.Type = (ushort)FriendActionList[qq].Action;
            if (!string.IsNullOrEmpty(verifyCode))
            {
                byte[] bytes = Utils.Util.GetBytes(verifyCode);
                ByteBuffer buf = new ByteBuffer(bytes);
                int vCode = buf.GetInt();
                requestTokenPacket.Code = vCode;
                requestTokenPacket.VCodeSession = Utils.Util.GetBytes(FriendActionList[qq].VCodeSession);//Utils.Util.HexStrToBytes(FriendActionList[qq].VCodeSession);
            }
            QQClient.PacketManager.SendPacket(requestTokenPacket);
        }
        /// <summary>
        /// 0xA8命令添加一个不需要验证的QQ好友
        /// 第三步 用户设置为不需要验证时
        /// </summary>
        /// <param name="qq"></param>
        public void SendAddFriend(int qq)
        {
            AddFriendAuthorizePacket outPacket = new AddFriendAuthorizePacket(QQClient);
            outPacket.To = qq;
            outPacket.SubCommand = AddFriendAuthSubCmd.Add;
            outPacket.AddFriendToken = FriendActionList[qq].Token;
            FriendActionList[qq].ActionProgress = FriendActionProgress.SendRequest;
            QQClient.PacketManager.SendPacket(outPacket);
        }
        /// <summary>
        /// 0xA8命令添加一个需要验证的QQ好友
        /// 第三步 用户设置为需要验证时
        /// </summary>
        /// <param name="qq"></param>
        public void SendAddFriend(int qq, string message)
        {
            AddFriendAuthorizePacket outPacket = new AddFriendAuthorizePacket(QQClient);
            outPacket.To = qq;
            outPacket.SubCommand = AddFriendAuthSubCmd.NeedAuthor;
            outPacket.AddFriendToken = FriendActionList[qq].Token;
            outPacket.Message = message;
            FriendActionList[qq].ActionProgress = FriendActionProgress.SendRequest;
            QQClient.PacketManager.SendPacket(outPacket);
        }

        /// <summary>
        /// 添加好友
        /// 0xA8
        /// 第三步 用户设置为要回答问题时
        /// </summary>
        /// <param name="QQ"></param>
        /// <param name="answer"></param>
        public void SendAddFriendAnswer(int QQ, string answer)
        {
            //未实现 用B7包获取问题再回答问题

        }
        

        /// <summary> 把好友从服务器端的好友列表中删除
        /// Removes the friend from list.
        /// </summary>
        /// <param name="qq">The qq.</param>
        public void RemoveFriendFromList(int qq)
        {
            FriendDataOpPacket packet = new FriendDataOpPacket(QQClient);
            packet.SubCommand = FriendOpSubCmd.REMOVE_FRIEND_FROM_LIST;
            packet.QQ = qq;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }
        /// <summary>
        /// 添加好友到服务器端的好友列表中
        /// </summary>
        /// <param name="group">The group.好友的组号，我的好友组是0，然后是1，2，...</param>
        /// <param name="qq">The qq.</param>
        public void AddFriendToList(int group, int qq)
        {
            UploadGroupFriendPacket packet = new UploadGroupFriendPacket(QQClient);
            packet.addFriend(group, qq);
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }
       
        /// <summary>
        /// 删除一个好友
        /// </summary>
        /// <param name="qq">The qq.</param>
        public void DeleteFriend(int qq)
        {
            DeleteFriendPacket packet = new DeleteFriendPacket(QQClient);
            packet.To = qq;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }
        /// <summary> 把某人的好友列表中的自己删除
        /// </summary>
        /// <param name="qq">The qq.</param>
        public void RemoveSelfFrom(int qq)
        {
            RemoveSelfPacket packet = new RemoveSelfPacket(QQClient);
            packet.RemoveFrom = qq;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }

        #region 对方请求加好友的方法 20090902
        /// <summary>
        /// 通过验证并加对方为好友
        /// </summary>
        /// <param name="qq"></param>
        public void ApprovedAndAdd(int qq)
        {
            AddFriendAuthorizePacket packet = new AddFriendAuthorizePacket(QQClient);
            packet.To = qq;
            packet.SubCommand = AddFriendAuthSubCmd.ApproveAndAdd;//.ApproveAndAdd;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }

        /// <summary>
        /// 通过验证 
        /// A8命令
        /// new
        /// </summary>
        /// <param name="qq"></param>
        public void ApprovedAdd(int qq)
        {
            AddFriendAuthorizePacket packet = new AddFriendAuthorizePacket(QQClient);
            packet.To = qq;
            packet.SubCommand = AddFriendAuthSubCmd.Approve;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }
        #endregion
        /// <summary> 
        /// 如果要加的人需要认证，用这个方法发送验证请求
        /// </summary>
        /// <param name="qq">The qq.</param>
        /// <param name="message">The message.</param>
        public void SendAddFriendAuth(int qq, string message)
        {
            AddFriendAuthorizePacket packet = new AddFriendAuthorizePacket(QQClient);
            packet.To = qq;
            packet.SubCommand = AddFriendAuthSubCmd.NeedAuthor; //add 20090902
            packet.Message = message;

            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }

        /// <summary>
        /// 如果我要同意一个人加我为好友的请求，用这个方法发送同意消息
        /// 20090902 验证有效
        /// </summary>
        /// <param name="qq">The qq.</param>
        [Obsolete]
        public void ApprovedAddMe(int qq)
        {
            AddFriendAuthResponsePacket packet = new AddFriendAuthResponsePacket(QQClient);
            packet.To = qq;
            packet.Action = AuthAction.Approve;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }
        /// <summary>
        /// 如果我要拒绝一个人加我为好友的请求，用这个方法发送拒绝消息
        /// </summary>
        /// <param name="qq">The qq.</param>
        /// <param name="message">The message.</param>
        public void RejectAddMe(int qq, string message)
        {
            AddFriendAuthResponsePacket packet = new AddFriendAuthResponsePacket(QQClient);
            packet.To = qq;
            packet.Message = message;
            packet.Action = AuthAction.Reject;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }
        #endregion
        /// <summary>得到一个用户的详细信息
        /// 	<remark>abu 2008-03-11 </remark>
        /// </summary>
        /// <param name="qq">The qq.</param>
        public void GetUserInfo(int qq)
        {
            GetUserInfoPacket packet = new GetUserInfoPacket(QQClient);
            packet.QQ = qq;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }

        /// <summary>请求在线好友列表
        /// 	<remark>abu 2008-03-11 </remark>
        /// </summary>
        public void GetOnlineFriend()
        {
            GetOnlineFriend(0);
        }

        /// <summary>请求在线好友列表
        /// 	<remark>abu 2008-03-11 </remark>
        /// </summary>
        /// <param name="startPosition">The start position.</param>
        private void GetOnlineFriend(int startPosition)
        {
            GetOnlineOpPacket packet = new GetOnlineOpPacket(QQClient);
            packet.StartPosition = startPosition;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }

        /// <summary>
        /// 请求取得好友名单
        /// </summary>
        public void GetFriendList()
        {
            
            GetFriendList(0);
        }

        /// <summary>
        /// 请求取得好友名单
        /// </summary>
        /// <param name="position">The position.</param>
        private void GetFriendList(int startPosition)
        {
            GetFriendListPacket packet = new GetFriendListPacket(QQClient);
            packet.StartPosition = (ushort)startPosition;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }
        #region events
        #region 搜索QQ用户事件

        /// <summary>搜索好友回复事件
        /// Occurs when [search user successed].
        /// </summary>
        public event EventHandler<QQEventArgs<SearchUserReplyPacket, SearchUserPacket>> SearchUserSuccessed;
        /// <summary>
        /// Raises the <see cref="E:SearchUserSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.SearchUserReplyPacket,LFNet.QQ.Packets.Out.SearchUserPacket&gt;"/> instance containing the event data.</param>
        internal void OnSearchUserSuccessed(QQEventArgs<SearchUserReplyPacket, SearchUserPacket> e)
        {
            if (SearchUserSuccessed != null)
            {
                SearchUserSuccessed(this, e);
            }
        }

        #endregion

        #region 分组名称事件
        /// <summary>上传分组名称成功
        /// Occurs when [upload group names successed].
        /// </summary>
        public event EventHandler<QQEventArgs<GroupDataOpReplyPacket, GroupDataOpPacket>> UploadGroupNamesSuccessed;
        /// <summary>
        /// Raises the <see cref="E:UploadGroupNamesSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.GroupDataOpReplyPacket,LFNet.QQ.Packets.Out.GroupDataOpPacket&gt;"/> instance containing the event data.</param>
        internal void OnUploadGroupNamesSuccessed(QQEventArgs<GroupDataOpReplyPacket, GroupDataOpPacket> e)
        {
            if (UploadGroupNamesSuccessed != null)
            {
                UploadGroupNamesSuccessed(this, e);
            }
        }

        /// <summary>上传分组名称失败
        /// Occurs when [upload group names failed].
        /// </summary>
        public event EventHandler<QQEventArgs<GroupDataOpReplyPacket, GroupDataOpPacket>> UploadGroupNamesFailed;
        internal void OnUploadGroupNamesFailed(QQEventArgs<GroupDataOpReplyPacket, GroupDataOpPacket> e)
        {
            if (UploadGroupNamesFailed != null)
            {
                UploadGroupNamesFailed(this, e);
            }
        }

        /// <summary>下载分组名称成功
        /// Occurs when [download group names successed].
        /// </summary>
        public event EventHandler<QQEventArgs<GroupDataOpReplyPacket, GroupDataOpPacket>> DownloadGroupNamesSuccessed;
        /// <summary>
        /// Raises the <see cref="E:DownloadGroupNamesSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.GroupDataOpReplyPacket,LFNet.QQ.Packets.Out.GroupDataOpPacket&gt;"/> instance containing the event data.</param>
        internal void OnDownloadGroupNamesSuccessed(QQEventArgs<GroupDataOpReplyPacket, GroupDataOpPacket> e)
        {
            if (DownloadGroupNamesSuccessed != null)
            {
                DownloadGroupNamesSuccessed(this, e);
            }
        }

        /// <summary>下载分名称失败
        /// Occurs when [download group names failed].
        /// </summary>
        public event EventHandler<QQEventArgs<GroupDataOpReplyPacket, GroupDataOpPacket>> DownloadGroupNamesFailed;
        internal void OnDownloadGroupNamesFailed(QQEventArgs<GroupDataOpReplyPacket, GroupDataOpPacket> e)
        {
            if (DownloadGroupNamesFailed != null)
            {
                DownloadGroupNamesFailed(this, e);
            }
        }
        #endregion

        #region 下载分组好友
        /// <summary>下载分组好友成功
        /// Occurs when [download group friend successed].
        /// </summary>
        public event EventHandler<QQEventArgs<DownloadGroupFriendReplyPacket, DownloadGroupFriendPacket>> DownloadGroupFriendSuccessed;
        /// <summary>
        /// Raises the <see cref="E:DownloadGroupFriendSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.DownloadGroupFriendReplyPacket,LFNet.QQ.Packets.Out.DownloadGroupFriendPacket&gt;"/> instance containing the event data.</param>
        internal void OnDownloadGroupFriendSuccessed(QQEventArgs<DownloadGroupFriendReplyPacket, DownloadGroupFriendPacket> e)
        {
            if (DownloadGroupFriendSuccessed != null)
            {
                DownloadGroupFriendSuccessed(this, e);
            }
        }
        /// <summary>下载分组好友失败
        /// Occurs when [download group friend failed].
        /// </summary>
        public event EventHandler<QQEventArgs<DownloadGroupFriendReplyPacket, DownloadGroupFriendPacket>> DownloadGroupFriendFailed;
        /// <summary>
        /// Raises the <see cref="E:DownloadGroupFriendFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.DownloadGroupFriendReplyPacket,LFNet.QQ.Packets.Out.DownloadGroupFriendPacket&gt;"/> instance containing the event data.</param>
        internal void OnDownloadGroupFriendFailed(QQEventArgs<DownloadGroupFriendReplyPacket, DownloadGroupFriendPacket> e)
        {
            if (DownloadGroupFriendFailed != null)
            {
                DownloadGroupFriendFailed(this, e);
            }
        }
        #endregion

        #region 读取用户属性
        /// <summary>读取用户属性成功
        /// Occurs when [get user property successed].
        /// </summary>
        public event EventHandler<QQEventArgs<UserPropertyOpReplyPacket, UserPropertyOpPacket>> GetUserPropertySuccessed;
        /// <summary>
        /// Raises the <see cref="E:GetUserPropertySuccess"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.UserPropertyOpReplyPacket,LFNet.QQ.Packets.Out.UserPropertyOpPacket&gt;"/> instance containing the event data.</param>
        internal void OnGetUserPropertySuccess(QQEventArgs<UserPropertyOpReplyPacket, UserPropertyOpPacket> e)
        {
            if (GetUserPropertySuccessed != null)
            {
                GetUserPropertySuccessed(this, e);
            }
        }
        #endregion

        #region 读取好友等级
        /// <summary>读取好友等级成功
        /// Occurs when [get friend level successed].
        /// </summary>
        public event EventHandler<QQEventArgs<FriendLevelOpReplyPacket, FriendLevelOpPacket>> GetFriendLevelSuccessed;
        /// <summary>
        /// Raises the <see cref="E:GetFriendLevelSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.FriendLevelOpReplyPacket,LFNet.QQ.Packets.Out.FriendLevelOpPacket&gt;"/> instance containing the event data.</param>
        internal void OnGetFriendLevelSuccessed(QQEventArgs<FriendLevelOpReplyPacket, FriendLevelOpPacket> e)
        {
            if (GetFriendLevelSuccessed != null)
            {
                GetFriendLevelSuccessed(this, e);
            }
        }

        #endregion

        #region 好友个性签名信息
        /// <summary>读取个性签名成功
        /// Occurs when [get signature successed].
        /// </summary>
        public event EventHandler<QQEventArgs<SignatureOpReplyPacket, SignatureOpPacket>> GetSignatureSuccessed;
        /// <summary>
        /// Raises the <see cref="E:GetSignatureSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.SignatureOpReplyPacket,LFNet.QQ.Packets.Out.SignatureOpPacket&gt;"/> instance containing the event data.</param>
        internal void OnGetSignatureSuccessed(QQEventArgs<SignatureOpReplyPacket, SignatureOpPacket> e)
        {
            if (GetSignatureSuccessed != null)
            {
                GetSignatureSuccessed(this, e);
            }
        }

        /// <summary>读取个性签名失败
        /// Occurs when [get signature failed].
        /// </summary>
        public event EventHandler<QQEventArgs<SignatureOpReplyPacket, SignatureOpPacket>> GetSignatureFailed;
        /// <summary>
        /// Raises the <see cref="E:GetSignatureFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.SignatureOpReplyPacket,LFNet.QQ.Packets.Out.SignatureOpPacket&gt;"/> instance containing the event data.</param>
        internal void OnGetSignatureFailed(QQEventArgs<SignatureOpReplyPacket, SignatureOpPacket> e)
        {
            if (GetSignatureFailed != null)
            {
                GetSignatureFailed(this, e);
            }
        }
        #endregion

        #region 好友备注信息操作
        /// <summary>上传好友备注信息成功
        /// Occurs when [upload friend remark successed].
        /// </summary>
        public event EventHandler<QQEventArgs<FriendDataOpReplyPacket, FriendDataOpPacket>> UploadFriendRemarkSuccessed;
        /// <summary>
        /// Raises the <see cref="E:UploadFriendRemarkSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.FriendDataOpReplyPacket,LFNet.QQ.Packets.Out.FriendDataOpPacket&gt;"/> instance containing the event data.</param>
        internal void OnUploadFriendRemarkSuccessed(QQEventArgs<FriendDataOpReplyPacket, FriendDataOpPacket> e)
        {
            if (UploadFriendRemarkSuccessed != null)
            {
                UploadFriendRemarkSuccessed(this, e);
            }
        }

        /// <summary>上传好友备注信息失败
        /// Occurs when [upload friend remark failed].
        /// </summary>
        public event EventHandler<QQEventArgs<FriendDataOpReplyPacket, FriendDataOpPacket>> UploadFriendRemarkFailed;
        /// <summary>
        /// Raises the <see cref="E:UploadFriendRemarkFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.FriendDataOpReplyPacket,LFNet.QQ.Packets.Out.FriendDataOpPacket&gt;"/> instance containing the event data.</param>
        internal void OnUploadFriendRemarkFailed(QQEventArgs<FriendDataOpReplyPacket, FriendDataOpPacket> e)
        {
            if (UploadFriendRemarkFailed != null)
            {
                UploadFriendRemarkFailed(this, e);
            }
        }

        /// <summary>下载好友备注信息成功
        /// Occurs when [download friend remark successed].
        /// </summary>
        public event EventHandler<QQEventArgs<FriendDataOpReplyPacket, FriendDataOpPacket>> DownloadFriendRemarkSuccessed;
        /// <summary>
        /// Raises the <see cref="E:DownloadFriendRemarkSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.FriendDataOpReplyPacket,LFNet.QQ.Packets.Out.FriendDataOpPacket&gt;"/> instance containing the event data.</param>
        internal void OnDownloadFriendRemarkSuccessed(QQEventArgs<FriendDataOpReplyPacket, FriendDataOpPacket> e)
        {
            if (DownloadFriendRemarkSuccessed != null)
            {
                DownloadFriendRemarkSuccessed(this, e);
            }
        }

        /// <summary>下载好友备注信息失败
        /// Occurs when [download friend remark failed].
        /// </summary>
        public event EventHandler<QQEventArgs<FriendDataOpReplyPacket, FriendDataOpPacket>> DownloadFriendRemarkFailed;
        /// <summary>
        /// Raises the <see cref="E:DownloadFriendRemarkFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.FriendDataOpReplyPacket,LFNet.QQ.Packets.Out.FriendDataOpPacket&gt;"/> instance containing the event data.</param>
        internal void OnDownloadFriendRemarkFailed(QQEventArgs<FriendDataOpReplyPacket, FriendDataOpPacket> e)
        {
            if (DownloadFriendRemarkFailed != null)
            {
                DownloadFriendRemarkFailed(this, e);
            }
        }

        /// <summary>成批下载好友信息成功
        /// Occurs when [batch download friend remark successed].
        /// </summary>
        public event EventHandler<QQEventArgs<FriendDataOpReplyPacket, FriendDataOpPacket>> BatchDownloadFriendRemarkSuccessed;
        /// <summary>
        /// Raises the <see cref="E:BatchDownloadFriendRemarkSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.FriendDataOpReplyPacket,LFNet.QQ.Packets.Out.FriendDataOpPacket&gt;"/> instance containing the event data.</param>
        internal void OnBatchDownloadFriendRemarkSuccessed(QQEventArgs<FriendDataOpReplyPacket, FriendDataOpPacket> e)
        {
            if (BatchDownloadFriendRemarkSuccessed != null)
            {
                BatchDownloadFriendRemarkSuccessed(this, e);
            }
        }

        /// <summary>成批下载好友信息失败
        /// Occurs when [batch download friend remark failed].
        /// </summary>
        public event EventHandler<QQEventArgs<FriendDataOpReplyPacket, FriendDataOpPacket>> BatchDownloadFriendRemarkFailed;
        /// <summary>
        /// Raises the <see cref="E:BatchDownloadFriendRemarkFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.FriendDataOpReplyPacket,LFNet.QQ.Packets.Out.FriendDataOpPacket&gt;"/> instance containing the event data.</param>
        internal void OnBatchDownloadFriendRemarkFailed(QQEventArgs<FriendDataOpReplyPacket, FriendDataOpPacket> e)
        {
            if (BatchDownloadFriendRemarkFailed != null)
            {
                BatchDownloadFriendRemarkFailed(this, e);
            }
        }

        /// <summary>从服务器端好友列表中移除好友成功
        /// Occurs when [remove friend from list successed].
        /// </summary>
        public event EventHandler<QQEventArgs<FriendDataOpReplyPacket, FriendDataOpPacket>> RemoveFriendFromListSuccessed;
        /// <summary>
        /// Raises the <see cref="E:RemoveFriendFromListSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.FriendDataOpReplyPacket,LFNet.QQ.Packets.Out.FriendDataOpPacket&gt;"/> instance containing the event data.</param>
        internal void OnRemoveFriendFromListSuccessed(QQEventArgs<FriendDataOpReplyPacket, FriendDataOpPacket> e)
        {
            if (RemoveFriendFromListSuccessed != null)
            {
                RemoveFriendFromListSuccessed(this, e);
            }
        }

        /// <summary>从服务器端好友列表中移除好友失败
        /// Occurs when [remove friend from list failed].
        /// </summary>
        public event EventHandler<QQEventArgs<FriendDataOpReplyPacket, FriendDataOpPacket>> RemoveFriendFromListFailed;
        /// <summary>
        /// Raises the <see cref="E:RemoveFriendFromListFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.FriendDataOpReplyPacket,LFNet.QQ.Packets.Out.FriendDataOpPacket&gt;"/> instance containing the event data.</param>
        internal void OnRemoveFriendFromListFailed(QQEventArgs<FriendDataOpReplyPacket, FriendDataOpPacket> e)
        {
            if (RemoveFriendFromListFailed != null)
            {
                RemoveFriendFromListFailed(this, e);
            }
        }

        #endregion

        /// <summary>
        /// 取得在线好友列表
        /// </summary>
        public event EventHandler<QQEventArgs<GetOnlineOpReplyPacket, GetOnlineOpPacket>> GetOnlineFriendSuccessed;
        /// <summary>
        /// Called when [get online friend successed].
        /// </summary>
        /// <param name="e">The e.</param>
        internal void OnGetOnlineFriendSuccessed(QQEventArgs<GetOnlineOpReplyPacket, GetOnlineOpPacket> e)
        {
            foreach (FriendStatus online in e.InPacket.OnlineFriends)
            {
                QQUser.Friends.SetFriendOnline(online.QQ, online);
            }
            if (!e.InPacket.Finished)
            {
                GetOnlineFriend(e.InPacket.Position + 1);
            }
            else
            {
                if (GetOnlineFriendSuccessed != null)
                {
                    GetOnlineFriendSuccessed(this, e);
                }
            }
        }

        /// <summary>得到好友列表事件
        /// <remarks>获得好友列表及在线状态的顺序是：
        /// 先得到所有的好友列表，根据TheEnd判断是否已经得到所有的好友。
        /// 得到所有的好友列表后，才能去获取在线好友列表。</remarks>
        /// 	<remark>abu 2008-03-11 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<GetFriendListReplyPacket, GetFriendListPacket>> GetFriendListSuccessed;
        /// <summary>
        /// Raises the <see cref="E:GetFriendListSuccess"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.GetFriendListReplyPacket&gt;"/> instance containing the event data.</param>
        internal void OnGetFriendListSuccessed(QQEventArgs<GetFriendListReplyPacket, GetFriendListPacket> e)
        {
            //foreach (QQFriend friend in e.InPacket.Friends)
            //{
            //    if (QQUser.Friends.ContainsKey((int)friend.QQ))
            //    {
            //        friend.GroupId = QQUser.Friends[(int)friend.QQ].BasicInfo.GroupId;
            //        QQUser.Friends[(int)friend.QQ]=new FriendInfo(friend);
            //    }
            //    else
            //    {
            //        QQUser.Friends.Add((int)friend.QQ, new FriendInfo(friend));
            //    }
            //}
            if (!e.InPacket.Finished)
            {
                GetFriendList(e.InPacket.Position + 1);
            }
            else
            {
                if (GetFriendListSuccessed != null)
                {
                    GetFriendListSuccessed(this, e);
                }
            }
        }

        /// <summary>得到用户详细信息回复事件 
        /// 	<remark>abu 2008-03-11 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<GetUserInfoReplyPacket, GetUserInfoPacket>> GetUserInfoSuccessed;
        /// <summary>
        /// Raises the <see cref="E:GetUserInfoSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.GetUserInfoReplyPacket&gt;"/> instance containing the event data.</param>
        internal void OnGetUserInfoSuccessed(QQEventArgs<GetUserInfoReplyPacket, GetUserInfoPacket> e)
        {
            //如果QQ号码等于自己则更新自己的详细信息
            if (e.QQClient.QQUser.QQ == e.InPacket.ContactInfo.QQ)
            {
                e.QQClient.QQUser.ContactInfo = e.InPacket.ContactInfo;
            }
            if (GetUserInfoSuccessed != null)
            {
                GetUserInfoSuccessed(this, e);
            }
        }

        /// <summary>收到好友的状态发生变化
        /// 	<remark>abu 2008-03-12 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<FriendChangeStatusPacket, OutPacket>> FriendChangeStatus;
        /// <summary>
        /// Raises the <see cref="E:FriendChangeStatus"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.FriendChangeStatusPacket&gt;"/> instance containing the event data.</param>
        internal void OnFriendChangeStatus(QQEventArgs<FriendChangeStatusPacket, OutPacket> e)
        {
            if (FriendChangeStatus != null)
            {
                FriendChangeStatus(this, e);
            }
        }


        #region 添加好友的回复事件

        /// <summary>
        /// 好友添加成功
        /// </summary>
        public event EventHandler<QQEventArgs<AddFriendReplyPacket, AddFriendPacket>> AddFriendSuccessed;
        /// <summary>
        /// Raises the <see cref="E:AddFriendSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.AddFriendExReplyPacket&gt;"/> instance containing the event data.</param>
        internal void OnAddFriendSuccessed(QQEventArgs<AddFriendReplyPacket, AddFriendPacket> e)
        {
            if (AddFriendSuccessed != null)
            {
                AddFriendSuccessed(this, e);
            }
        }
        /// <summary>
        /// 添加好友时，需要发送验证信息
        /// </summary>
        public event EventHandler<QQEventArgs<AddFriendReplyPacket, AddFriendPacket>> AddFriendNeedAuth;
        /// <summary>
        /// Raises the <see cref="E:AddFriendNeedAuth"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.AddFriendExReplyPacket&gt;"/> instance containing the event data.</param>
        internal void OnAddFriendNeedAuth(QQEventArgs<AddFriendReplyPacket, AddFriendPacket> e)
        {
            if (AddFriendNeedAuth != null)
            {
                AddFriendNeedAuth(this, e);
            }
        }

        /// <summary>
        /// 对方拒绝让你加为好友
        /// </summary>
        public event EventHandler<QQEventArgs<AddFriendReplyPacket, AddFriendPacket>> AddFriendDeny;
        /// <summary>
        /// Raises the <see cref="E:AddFriendDeny"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.AddFriendExReplyPacket&gt;"/> instance containing the event data.</param>
        internal void OnAddFriendDeny(QQEventArgs<AddFriendReplyPacket, AddFriendPacket> e)
        {
            if (AddFriendDeny != null)
            {
                AddFriendDeny(this, e);
            }
        }
        /// <summary>
        /// 需要回答问题才能加为好友
        /// </summary>
        public event EventHandler<QQEventArgs<AddFriendReplyPacket, AddFriendPacket>> AddFriendNeedAnswer;
        /// <summary>
        /// Raises the <see cref="E:AddFriendNeedAnswer"/> event.
        /// </summary>
        /// <param name="e"></param>
        internal void OnAddFriendNeedAnswer(QQEventArgs<AddFriendReplyPacket, AddFriendPacket> e)
        {
            if (AddFriendNeedAnswer != null)
            {
                AddFriendNeedAnswer(this, e);
            }
        }
        /// <summary>
        /// 对方已经是好友
        /// </summary>
        public event EventHandler<QQEventArgs<AddFriendReplyPacket, AddFriendPacket>> FriendAlready;
        /// <summary>
        /// Raises the <see cref="E:FriendAlready"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.AddFriendExReplyPacket&gt;"/> instance containing the event data.</param>
        internal void OnFriendAlready(QQEventArgs<AddFriendReplyPacket, AddFriendPacket> e)
        {
            if (FriendAlready != null)
            {
                FriendAlready(this, e);
            }
        }

        /// <summary>
        /// 添加好友失败
        /// </summary>
        public event EventHandler<QQEventArgs<AddFriendReplyPacket, AddFriendPacket>> AddFriendFailed;
        /// <summary>
        /// Raises the <see cref="E:AddFriendFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.AddFriendExReplyPacket&gt;"/> instance containing the event data.</param>
        internal void OnAddFriendFailed(QQEventArgs<AddFriendReplyPacket, AddFriendPacket> e)
        {
            if (AddFriendFailed != null)
            {
                AddFriendFailed(this, e);
            }
        }

        /// <summary>认证信息发送成功
        /// 	<remark>abu 2008-03-12 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<AddFriendAuthorizeReplyPacket, AddFriendAuthorizePacket>> SendAuthSuccessed;
        /// <summary>
        /// Raises the <see cref="E:SendAuthSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.AddFriendAuthResponseReplyPacket&gt;"/> instance containing the event data.</param>
        internal void OnSendAuthSuccessed(QQEventArgs<AddFriendAuthorizeReplyPacket, AddFriendAuthorizePacket> e)
        {
            if (SendAuthSuccessed != null)
            {
                SendAuthSuccessed(this, e);
            }
        }

        /// <summary>发送认证信息失败
        /// 	<remark>abu 2008-03-12 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<AddFriendAuthorizeReplyPacket, AddFriendAuthorizePacket>> SendAuthFailed;
        /// <summary>
        /// Raises the <see cref="E:SendAuthFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.AddFriendAuthResponseReplyPacket&gt;"/> instance containing the event data.</param>
        internal void OnSendAuthFailed(QQEventArgs<AddFriendAuthorizeReplyPacket, AddFriendAuthorizePacket> e)
        {
            if (SendAuthFailed != null)
            {
                SendAuthFailed(this, e);
            }
        }
        #endregion

        #region 把自己从好友的好友列表中删除的回复事件
        /// <summary>把自己从好友的好友列表中删除成功
        /// 	<remark>abu 2008-03-12 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<RemoveSelfReplyPacket, RemoveSelfPacket>> RemoveSelfSuccessed;
        /// <summary>
        /// Raises the <see cref="E:RemoveSelfSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.RemoveSelfReplyPacket&gt;"/> instance containing the event data.</param>
        internal void OnRemoveSelfSuccessed(QQEventArgs<RemoveSelfReplyPacket, RemoveSelfPacket> e)
        {
            if (RemoveSelfSuccessed != null)
            {
                RemoveSelfSuccessed(this, e);
            }
        }

        /// <summary>把自己从好友的好友列表中删除失败
        /// 	<remark>abu 2008-03-12 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<RemoveSelfReplyPacket, RemoveSelfPacket>> RemoveSelfFailed;
        /// <summary>
        /// Raises the <see cref="E:RemoveSelfFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.RemoveSelfReplyPacket&gt;"/> instance containing the event data.</param>
        internal void OnRemoveSelfFailed(QQEventArgs<RemoveSelfReplyPacket, RemoveSelfPacket> e)
        {
            if (RemoveSelfFailed != null)
            {
                RemoveSelfFailed(this, e);
            }
        }

        #endregion

        #region 删除好友回复事件
        /// <summary>删除好友成功
        /// 	<remark>abu 2008-03-12 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<DeleteFriendReplyPacket, DeleteFriendPacket>> DeleteFriendSuccessed;
        /// <summary>
        /// Raises the <see cref="E:DeleteFriendSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.DeleteFriendReplyPacket&gt;"/> instance containing the event data.</param>
        internal void OnDeleteFriendSuccessed(QQEventArgs<DeleteFriendReplyPacket, DeleteFriendPacket> e)
        {
            if (DeleteFriendSuccessed != null)
            {
                DeleteFriendSuccessed(this, e);
            }
        }
        /// <summary>删除好友失败
        /// 	<remark>abu 2008-03-12 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<DeleteFriendReplyPacket, DeleteFriendPacket>> DeleteFriendFailed;
        /// <summary>
        /// Raises the <see cref="E:DeleteFriendFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.DeleteFriendReplyPacket&gt;"/> instance containing the event data.</param>
        internal void OnDeleteFriendFailed(QQEventArgs<DeleteFriendReplyPacket, DeleteFriendPacket> e)
        {
            if (DeleteFriendFailed != null)
            {
                DeleteFriendFailed(this, e);
            }
        }
        #endregion

        #region 处理对方发送过来的认证信息回复事件
        /// <summary>处理认证信息成功
        /// 	<remark>abu 2008-03-12 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<AddFriendAuthResponseReplyPacket, AddFriendAuthResponsePacket>> ResponseAuthSuccessed;
        /// <summary>
        /// Raises the <see cref="E:ResponseAuthSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.Out.AddFriendAuthResponsePacket&gt;"/> instance containing the event data.</param>
        internal void OnResponseAuthSuccessed(QQEventArgs<AddFriendAuthResponseReplyPacket, AddFriendAuthResponsePacket> e)
        {
            if (ResponseAuthSuccessed != null)
            {
                ResponseAuthSuccessed(this, e);
            }
        }

        /// <summary>处理认证信息失败
        /// 	<remark>abu 2008-03-12 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<AddFriendAuthResponseReplyPacket, AddFriendAuthResponsePacket>> ResponseAuthFailed;
        /// <summary>
        /// Raises the <see cref="E:ResponseAuthFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.AddFriendAuthResponseReplyPacket&gt;"/> instance containing the event data.</param>
        internal void OnResponseAuthFailed(QQEventArgs<AddFriendAuthResponseReplyPacket, AddFriendAuthResponsePacket> e)
        {
            if (ResponseAuthFailed != null)
            {
                ResponseAuthFailed(this, e);
            }
        }
        #endregion

        #region 处理上传分组好友列表回复事件
        /// <summary>
        /// Occurs when [upload group friend successed].事件在上传分组中的好友列表成功时发生
        /// </summary>
        public event EventHandler<QQEventArgs<UploadGroupFriendReplyPacket, UploadGroupFriendPacket>> UploadGroupFriendSuccessed;
        /// <summary>
        /// Raises the <see cref="E:UploadGroupFriendSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.UploadGroupFriendReplyPacket,LFNet.QQ.Packets.Out.UploadGroupFriendPacket&gt;"/> instance containing the event data.</param>
        internal void OnUploadGroupFriendSuccessed(QQEventArgs<UploadGroupFriendReplyPacket, UploadGroupFriendPacket> e)
        {
            if (UploadGroupFriendSuccessed != null)
            {
                UploadGroupFriendSuccessed(this, e);
            }
        }

        /// <summary>
        /// Occurs when [upload group friend failed].事件在下载分组中的好友列表成功时发生
        /// </summary>
        public event EventHandler<QQEventArgs<UploadGroupFriendReplyPacket, UploadGroupFriendPacket>> UploadGroupFriendFailed;
        /// <summary>
        /// Raises the <see cref="E:UploadGroupFriendFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.UploadGroupFriendReplyPacket,LFNet.QQ.Packets.Out.UploadGroupFriendPacket&gt;"/> instance containing the event data.</param>
        internal void OnUploadGroupFriendFailed(QQEventArgs<UploadGroupFriendReplyPacket, UploadGroupFriendPacket> e)
        {
            if (UploadGroupFriendFailed != null)
            {
                UploadGroupFriendFailed(this, e);
            }
        }
        #endregion

        /// <summary>
        /// 好友个性签名改变
        /// 	<remark>abu 2008-03-15 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<ReceiveIMPacket, OutPacket>> SignatureChanged;
        /// <summary>
        /// Raises the <see cref="E:SignatureChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ReceiveIMPacket,LFNet.QQ.Packets.OutPacket&gt;"/> instance containing the event data.</param>
        internal void OnSignatureChanged(QQEventArgs<ReceiveIMPacket, OutPacket> e)
        {
            if (SignatureChanged != null)
            {
                SignatureChanged(this, e);
            }
        }

        /// <summary>
        /// 收到好友属性变化通知
        /// 	<remark>abu 2008-03-15 </remark>
        /// </summary>
        public event EventHandler<QQEventArgs<ReceiveIMPacket, OutPacket>> UserPropertyChanged;
        /// <summary>
        /// Raises the <see cref="E:UserPropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ReceiveIMPacket,LFNet.QQ.Packets.OutPacket&gt;"/> instance containing the event data.</param>
        internal void OnUserPropertyChanged(QQEventArgs<ReceiveIMPacket, OutPacket> e)
        {
            if (UserPropertyChanged != null)
            {
                UserPropertyChanged(this, e);
            }
        }
        #endregion

        internal void ProcessGetFriendListReply(GetFriendListReplyPacket getFriendListReplyPacket, GetFriendListPacket getFriendListPacket)
        {
            QQEventArgs<GetFriendListReplyPacket, GetFriendListPacket> e = new QQEventArgs<GetFriendListReplyPacket, GetFriendListPacket>(QQClient, getFriendListReplyPacket, getFriendListPacket);
            QQClient.LogManager.Log(string.Format("本次得到{0}位好友信息.", getFriendListReplyPacket.Friends.Count));
            OnGetFriendListSuccessed(e);//获取好友列表成功
            
        }

        /// <summary>
        /// 处理得到在线好友应答
        /// </summary>
        /// <param name="packet">The packet.</param>
        internal void ProcessGetFriendOnlineReply(GetOnlineOpReplyPacket inPacket, GetOnlineOpPacket outPacket)
        {
            OnGetOnlineFriendSuccessed(new QQEventArgs<GetOnlineOpReplyPacket, GetOnlineOpPacket>(QQClient, inPacket, outPacket));
        }


        
    }
}
