
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
    /// 个人资料管理
    /// </summary>
    public class PrivateManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateManager"/> class.
        /// </summary>
        internal PrivateManager() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateManager"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        internal PrivateManager(QQClient client)
        {
            this.QQClient = client;
        }
        /// <summary>
        /// Gets or sets the QQ client.
        /// </summary>
        /// <value>The QQ client.</value>
        public QQClient QQClient { get; private set; }
        /// <summary>
        /// Gets or sets the QQ user.
        /// </summary>
        /// <value>The QQ user.</value>
        public QQUser QQUser { get { return QQClient.QQUser; } }

        /// <summary>
        /// 改变QQ状态为当前QQ状态,默认摄像头等
        /// </summary>
        public void ChangeStatus(QQStatus status)
        {
            ChangeStatus(status, QQClient.QQUser.IsShowFakeCam);
        }
        /// <summary>
        /// 改变QQ状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="showFakeCam">是否有摄像头</param>
        public void ChangeStatus(QQStatus status, bool showFakeCam)
        {
            OutPacket outPacket = new ChangeStatusPacket(QQClient,status, showFakeCam);
            QQClient.PacketManager.SendPacket(outPacket);
        }

        /// <summary>
        /// 获取当前QQ等级信息
        /// </summary>
        public void GetLevel()
        {
            OutPacket outPacket = new GetLevelPacket(QQClient);
            QQClient.PacketManager.SendPacket(outPacket);
        }


        /// <summary>请求自己这里的天气预报
        /// Gets the weather.
        /// </summary>
        public void GetWeather()
        {
            WeatherOpPacket packet = new WeatherOpPacket(QQClient);
            packet.IP = QQUser.IP;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }
        /// <summary>修改QQ密码
        /// Modifies the password.
        /// </summary>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="newPassword">The new password.</param>
        public void ModifyPassword(string oldPassword, string newPassword)
        {
            ModifyInfo(oldPassword, newPassword, QQUser.ContactInfo);
        }

        /// <summary>修改个人信息
        /// Modifies the info.
        /// </summary>
        /// <param name="contactInfo">The contact info.</param>
        public void ModifyInfo(ContactInfo contactInfo)
        {
            ModifyInfo(null, null, contactInfo);
        }

        /// <summary>修改个人信息或密码
        /// Modifies the info.
        /// </summary>
        /// <param name="oldPassword">The old password.老密码，如果不修改密码，设成null</param>
        /// <param name="newPassword">The new password.新密码，如果不修改密码，设成null</param>
        /// <param name="contactInfo">The contact info.</param>
        private void ModifyInfo(string oldPassword, string newPassword, ContactInfo contactInfo)
        {
            ModifyInfoPacket packet = new ModifyInfoPacket(QQClient);
            packet.OldPassword = oldPassword;
            packet.NewPassword = newPassword;
            string[] infos = contactInfo.GetInfoArray();
            for (int i = 0; i < QQGlobal.QQ_COUNT_MODIFY_USER_INFO_FIELD; i++)
            {
                if (infos[i] == "-")
                {
                    infos[i] = "";
                }
            }
            packet.ContactInfo = contactInfo;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }

        /// <summary>修改个性签名
        /// Modifies the signature.
        /// </summary>
        /// <param name="sig">The sig.</param>
        public void ModifySignature(string sig)
        {
            SignatureOpPacket packet = new SignatureOpPacket(QQClient);
            packet.SubCommand = SignatureSubCmd.MODIFY;
            packet.Signature = sig;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }

        /// <summary>删除个性签名
        /// Deletes the signature.
        /// </summary>
        public void DeleteSignature()
        {
            SignatureOpPacket packet = new SignatureOpPacket(QQClient);
            packet.SubCommand = SignatureSubCmd.DELETE;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }

        /// <summary>设置是否只能通过QQ号码找到我
        /// Searches me by QQ only.
        /// </summary>
        /// <param name="only">if set to <c>true</c> [only].</param>
        public void SetSearchMeByQQOnly(bool only)
        {
            PrivacyDataOpPacket packet = new PrivacyDataOpPacket(QQClient);
            if (only)
            {
                packet.OpCode = ValueSet.Set;
            }
            else
                packet.OpCode = ValueSet.UnSet;
            packet.SubCommand = PrivacySubCmd.SearchMeByOnly;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }
        /// <summary>共享我的地址位置
        /// Shares the geography.
        /// </summary>
        /// <param name="shar">if set to <c>true</c> [shar].</param>
        public void ShareGeography(bool share)
        {
            PrivacyDataOpPacket packet = new PrivacyDataOpPacket(QQClient);
            if (share)
            {
                packet.OpCode = ValueSet.Set;
            }
            else
                packet.OpCode = ValueSet.UnSet;
            packet.SubCommand = PrivacySubCmd.ShareGeography;
            QQClient.PacketManager.SendPacket(packet, QQPort.Main.Name);
        }
        #region events

        #region 天气预报
        /// <summary>请求天气预报成功
        /// Occurs when [get weather successed].
        /// </summary>
        public event EventHandler<QQEventArgs<WeatherOpReplyPacket, WeatherOpPacket>> GetWeatherSuccessed;
        /// <summary>
        /// Raises the <see cref="E:WeatherSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.WeatherOpReplyPacket,LFNet.QQ.Packets.Out.WeatherOpPacket&gt;"/> instance containing the event data.</param>
        internal void OnGetWeatherSuccessed(QQEventArgs<WeatherOpReplyPacket, WeatherOpPacket> e)
        {
            if (GetWeatherSuccessed != null)
            {
                GetWeatherSuccessed(this, e);
            }
        }
        /// <summary>请求天气预报失败
        /// Occurs when [get weather failed].
        /// </summary>
        public event EventHandler<QQEventArgs<WeatherOpReplyPacket, WeatherOpPacket>> GetWeatherFailed;
        /// <summary>
        /// Raises the <see cref="E:GetWeatherFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.WeatherOpReplyPacket,LFNet.QQ.Packets.Out.WeatherOpPacket&gt;"/> instance containing the event data.</param>
        internal void OnGetWeatherFailed(QQEventArgs<WeatherOpReplyPacket, WeatherOpPacket> e)
        {
            if (GetWeatherFailed != null)
            {
                GetWeatherFailed(this, e);
            }
        }
        #endregion

        /// <summary>个人信息修改成功事件
        /// Occurs when [modify info successed].
        /// </summary>
        public event EventHandler<QQEventArgs<ModifyInfoReplyPacket, ModifyInfoPacket>> ModifyInfoSuccessed;
        /// <summary>
        /// Raises the <see cref="E:ModifyInfoSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ModifyInfoReplyPacket,LFNet.QQ.Packets.Out.ModifyInfoPacket&gt;"/> instance containing the event data.</param>
        internal void OnModifyInfoSuccessed(QQEventArgs<ModifyInfoReplyPacket, ModifyInfoPacket> e)
        {
            //个人信息修改成功后更新QQUser对象
            QQUser.ContactInfo = e.OutPacket.ContactInfo;
            if (ModifyInfoSuccessed != null)
            {
                ModifyInfoSuccessed(this, e);
            }
        }

        /// <summary>个人信息修改失败事件
        /// Occurs when [modify info failed].
        /// </summary>
        public event EventHandler<QQEventArgs<ModifyInfoReplyPacket, ModifyInfoPacket>> ModifyInfoFailed;
        /// <summary>
        /// Raises the <see cref="E:ModifyInfoFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.ModifyInfoReplyPacket,LFNet.QQ.Packets.Out.ModifyInfoPacket&gt;"/> instance containing the event data.</param>
        internal void OnModifyInfoFailed(QQEventArgs<ModifyInfoReplyPacket, ModifyInfoPacket> e)
        {
            if (ModifyInfoFailed != null)
            {
                ModifyInfoFailed(this, e);
            }
        }

        /// <summary>修改个性签名成功
        /// Occurs when [modify signature successed].
        /// </summary>
        public event EventHandler<QQEventArgs<SignatureOpReplyPacket, SignatureOpPacket>> ModifySignatureSuccessed;
        /// <summary>
        /// Raises the <see cref="E:ModifySignatureSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.SignatureOpReplyPacket,LFNet.QQ.Packets.Out.SignatureOpPacket&gt;"/> instance containing the event data.</param>
        internal void OnModifySignatureSuccessed(QQEventArgs<SignatureOpReplyPacket, SignatureOpPacket> e)
        {
            //个性签名修改成功修改对象
            QQUser.Signature = e.OutPacket.Signature;

            if (ModifySignatureSuccessed != null)
            {
                ModifySignatureSuccessed(this, e);
            }
        }

        /// <summary>修改个性签名失败
        /// Occurs when [modify signature failed].
        /// </summary>
        public event EventHandler<QQEventArgs<SignatureOpReplyPacket, SignatureOpPacket>> ModifySignatureFailed;
        /// <summary>
        /// Raises the <see cref="E:ModifySignatureFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.SignatureOpReplyPacket,LFNet.QQ.Packets.Out.SignatureOpPacket&gt;"/> instance containing the event data.</param>
        internal void OnModifySignatureFailed(QQEventArgs<SignatureOpReplyPacket, SignatureOpPacket> e)
        {
            if (ModifySignatureFailed != null)
            {
                ModifySignatureFailed(this, e);
            }
        }
        /// <summary>删除个性签名成功
        /// Occurs when [delete signature successed].
        /// </summary>
        public event EventHandler<QQEventArgs<SignatureOpReplyPacket, SignatureOpPacket>> DeleteSignatureSuccessed;
        /// <summary>
        /// Raises the <see cref="E:DeleteSignatureSuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.SignatureOpReplyPacket,LFNet.QQ.Packets.Out.SignatureOpPacket&gt;"/> instance containing the event data.</param>
        internal void OnDeleteSignatureSuccessed(QQEventArgs<SignatureOpReplyPacket, SignatureOpPacket> e)
        {
            if (DeleteSignatureSuccessed != null)
            {
                DeleteSignatureSuccessed(this, e);
            }
        }

        /// <summary>删除个性签名失败
        /// Occurs when [delete signature failed].
        /// </summary>
        public event EventHandler<QQEventArgs<SignatureOpReplyPacket, SignatureOpPacket>> DeleteSignatureFailed;
        /// <summary>
        /// Raises the <see cref="E:DeleteSignatureFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.SignatureOpReplyPacket,LFNet.QQ.Packets.Out.SignatureOpPacket&gt;"/> instance containing the event data.</param>
        internal void OnDeleteSignatureFailed(QQEventArgs<SignatureOpReplyPacket, SignatureOpPacket> e)
        {
            if (DeleteSignatureFailed != null)
            {
                DeleteSignatureFailed(this, e);
            }
        }


        /// <summary>成功设置只能通过好友找到选项
        /// Occurs when [set search me by QQ only successed].
        /// </summary>
        public event EventHandler<QQEventArgs<PrivacyDataOpReplyPacket, PrivacyDataOpPacket>> SetSearchMeByQQOnlySuccessed;
        /// <summary>
        /// Raises the <see cref="E:SetSearchMeByQQOnlySuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.PrivacyDataOpReplyPacket,LFNet.QQ.Packets.In.PrivacyDataOpReplyPacket&gt;"/> instance containing the event data.</param>
        internal void OnSetSearchMeByQQOnlySuccessed(QQEventArgs<PrivacyDataOpReplyPacket, PrivacyDataOpPacket> e)
        {
            if (SetSearchMeByQQOnlySuccessed != null)
            {
                SetSearchMeByQQOnlySuccessed(this, e);
            }
        }
        /// <summary>设置只能通过好友找到选项不成功
        /// Occurs when [set search me by QQ only failed].
        /// </summary>
        public event EventHandler<QQEventArgs<PrivacyDataOpReplyPacket, PrivacyDataOpPacket>> SetSearchMeByQQOnlyFailed;
        /// <summary>
        /// Raises the <see cref="E:SetSearchMeByQQOnlyFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.PrivacyDataOpReplyPacket,LFNet.QQ.Packets.In.PrivacyDataOpReplyPacket&gt;"/> instance containing the event data.</param>
        internal void OnSetSearchMeByQQOnlyFailed(QQEventArgs<PrivacyDataOpReplyPacket, PrivacyDataOpPacket> e)
        {
            if (SetSearchMeByQQOnlyFailed != null)
            {
                SetSearchMeByQQOnlyFailed(this, e);
            }
        }
        /// <summary>设置共享地理位置选项成功
        /// Occurs when [set share geography successed].
        /// </summary>
        public event EventHandler<QQEventArgs<PrivacyDataOpReplyPacket, PrivacyDataOpPacket>> SetShareGeographySuccessed;
        /// <summary>
        /// Raises the <see cref="E:SetShareGeographySuccessed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.PrivacyDataOpReplyPacket,LFNet.QQ.Packets.In.PrivacyDataOpReplyPacket&gt;"/> instance containing the event data.</param>
        internal void OnSetShareGeographySuccessed(QQEventArgs<PrivacyDataOpReplyPacket, PrivacyDataOpPacket> e)
        {
            if (SetShareGeographySuccessed != null)
            {
                SetShareGeographySuccessed(this, e);
            }
        }
        /// <summary>设置共享地理位置选项失败
        /// Occurs when [set share geography failed].
        /// </summary>
        public event EventHandler<QQEventArgs<PrivacyDataOpReplyPacket, PrivacyDataOpPacket>> SetShareGeographyFailed;
        /// <summary>
        /// Raises the <see cref="E:SetShareGeographyFailed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="LFNet.QQ.Events.QQEventArgs&lt;LFNet.QQ.Packets.In.PrivacyDataOpReplyPacket,LFNet.QQ.Packets.In.PrivacyDataOpReplyPacket&gt;"/> instance containing the event data.</param>
        internal void OnSetShareGeographyFailed(QQEventArgs<PrivacyDataOpReplyPacket, PrivacyDataOpPacket> e)
        {
            if (SetShareGeographyFailed != null)
            {
                SetShareGeographyFailed(this, e);
            }
        }
        #region 改变QQ状态事件
        /// <summary>
        /// QQ改变状态成功
        /// </summary>
        public event EventHandler<QQEventArgs<ChangeStatusReplyPacket, ChangeStatusPacket>> ChangeStatusSuccessed;
        internal void OnChangeStatusSuccessed(QQEventArgs<ChangeStatusReplyPacket, ChangeStatusPacket> e)
        {
            if (ChangeStatusSuccessed != null)
            {
                ChangeStatusSuccessed(this, e);
            }
        }
        /// <summary>
        /// QQ改变状态失败
        /// </summary>
        public event EventHandler<QQEventArgs<ChangeStatusReplyPacket, ChangeStatusPacket>> ChangeStatusFailed;
        internal void OnChangeStatusFailed(QQEventArgs<ChangeStatusReplyPacket, ChangeStatusPacket> e)
        {
            if (ChangeStatusFailed != null)
            {
                ChangeStatusFailed(this, e);
            }
        }
        #endregion

        #region GetLevel Events
        public event EventHandler<QQEventArgs<GetLevelReplyPacket, GetLevelPacket>> GetLevelSuccessed;
        internal void OnGetLevelSuccessed(QQEventArgs<GetLevelReplyPacket, GetLevelPacket> e)
        {
            if (GetLevelSuccessed != null)
            {
                GetLevelSuccessed(this, e);
            }
        }
        public event EventHandler<QQEventArgs<GetLevelReplyPacket, GetLevelPacket>> GetLevelFailed;
        internal void OnGetLevelFailed(QQEventArgs<GetLevelReplyPacket, GetLevelPacket> e)
        {
            if (GetLevelFailed != null)
            {
                GetLevelFailed(this, e);
            }
        }

        #endregion


        #endregion


        #region Process
        internal void ProcessChangeStatusReply(ChangeStatusReplyPacket changeStatusReplyPacket, ChangeStatusPacket changeStatusPacket)
        {
            QQEventArgs<ChangeStatusReplyPacket, ChangeStatusPacket> e = new QQEventArgs<ChangeStatusReplyPacket, ChangeStatusPacket>(QQClient, changeStatusReplyPacket, changeStatusPacket);
            if (changeStatusReplyPacket.ReplyCode == ReplyCode.CHANGE_STATUS_OK)//状态改变成功
            {
                QQClient.QQUser.Status = changeStatusPacket.Status;
                QQClient.PrivateManager.OnChangeStatusSuccessed(e);
            }
            else //状态改变失败
            {

                QQClient.PrivateManager.OnChangeStatusFailed(e);
            }
            //throw new NotImplementedException();
        }
        
        /// <summary>
        /// 获取用户等级
        /// </summary>
        /// <param name="getLevelReplyPacket"></param>
        /// <param name="getLevelPacket"></param>
        internal void ProcessGetLevelReply(GetLevelReplyPacket getLevelReplyPacket, GetLevelPacket getLevelPacket)
        {
            QQEventArgs<GetLevelReplyPacket, GetLevelPacket> e=new QQEventArgs<GetLevelReplyPacket,GetLevelPacket>(QQClient,getLevelReplyPacket,getLevelPacket);
            if (getLevelReplyPacket.ReplyCode == 0x88)
            {
                OnGetLevelSuccessed(e);
            }
            else
            { OnGetLevelFailed(e); }
        }

        #endregion
    }
}
