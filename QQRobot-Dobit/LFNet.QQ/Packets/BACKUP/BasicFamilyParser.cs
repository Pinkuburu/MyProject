
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Packets;
using LFNet.QQ.Packets.In;
namespace LFNet.QQ.Packets
{
    /// <summary> 基本协议族解析器   未完，等所有的协议包定义完后再来补充2008-02-19
    /// </summary>
    public class BasicFamilyParser : IParser
    {
        private int offset;
        private int length;
        private PacketHistory history;
        public BasicFamilyParser()
        {
            history = new PacketHistory();
        }
        #region IParser Members

        public bool Accept(ByteBuffer buf)
        {
            //保存偏移
            offset = buf.Position;
            int bufferLength = buf.Remaining();
            if (bufferLength <= 0)
                return false;
            bool accept = CheckTcp(buf);
            if (!accept)
                accept = CheckUdp(buf);
            return accept;
        }

        public int GetLength(ByteBuffer buf)
        {
            return length;
        }

        public InPacket ParseIncoming(ByteBuffer buf, int len, QQClient client)
        {
            try
            {
                QQCommand Command = GetCommand(buf, client.QQUser);
//#if DEBUG
//                client.LogManager.Log("Recv " + Command.ToString() + "(0x" + Command.ToString("X") + ") Packet Data:" + Utils.Util.ToHex(buf.ToByteArray()));
//#endif
                switch (Command)
                {
                    case QQCommand.Touch:
                        return new LoginTouchReplyPacket(buf, len, client);
                    case QQCommand.LoginRequest:
                        return new LoginRequestReplyPacket(buf, len, client);
                    case QQCommand.LoginVerify:
                        return new LoginVerifyReplyPacket(buf, len, client);
                    case QQCommand.LoginGetInfo:
                        return new LoginGetInfoReplyPacket(buf, len, client);
                    case QQCommand.Login_A4:
                        return new LoginA4ReplyPacket(buf, len, client);
                    case QQCommand.LoginGetList:
                        return new LoginGetListReplyPacket(buf, len, client);
                    case QQCommand.LoginSendInfo:
                        return new LoginSendInfoReplyPacket(buf, len, client);
                    case QQCommand.Keep_Alive:
                        return new KeepAliveReplyPacket(buf, len, client);
                    case QQCommand.Change_Status:
                        return new ChangeStatusReplyPacket(buf, len, client);
                    case QQCommand.GetLevel:
                        return new GetLevelReplyPacket(buf, len, client);
                    case QQCommand.GroupLabel:
                        return new DownloadGroupFriendReplyPacket(buf, len, client);
                    case QQCommand.Get_Friend_List:
                        return new GetFriendListReplyPacket(buf, len, client);
                    case QQCommand.Get_Online_OP:
                        return new GetOnlineOpReplyPacket(buf, len, client);
                    case QQCommand.Send_IM:
                        return new SendIMReplyPacket(buf, len, client);
                    case QQCommand.Recv_IM:
                    case QQCommand.Recv_IM_09:
                        return new ReceiveIMPacket(buf, len, client);
                    case QQCommand.Friend_Change_Status:
                        return new FriendChangeStatusPacket(buf, len, client);
                    case QQCommand.BroadCast:
                        return new SystemNotificationPacket(buf, len, client);
                    case QQCommand.Add_Friend:
                        return new AddFriendReplyPacket(buf, len, client);
                    case QQCommand.RequestToken:
                        return new RequestTokenReplyPacket(buf, len, client);
                    case QQCommand.Cluster_Cmd:
                        return new ClusterCommandReplyPacket(buf, len, client);
                    #region 05
                    case QQCommand.Modify_Info_05:
                        return new ModifyInfoReplyPacket(buf, len, client);
                    
                    case QQCommand.Search_User_05:
                        return new SearchUserReplyPacket(buf, len, client);
                    case QQCommand.Delete_Friend_05:
                        return new DeleteFriendReplyPacket(buf, len, client);
                    case QQCommand.Remove_Self_05:
                        return new RemoveSelfReplyPacket(buf, len, client);
                    case QQCommand.Add_Friend_Auth_05:
                        return new AddFriendAuthResponseReplyPacket(buf, len, client);
                    case QQCommand.Get_UserInfo_05:
                        return new GetUserInfoReplyPacket(buf, len, client);
                    
                    

                    //case QQCommand.Login_05:
                    //    return new LoginReplyPacket(buf, len, client);
                    case QQCommand.Get_Friend_List_05:
                        return new GetFriendListReplyPacket(buf, len, client);
                    
                    //case QQCommand.Recv_Msg_Sys_05:
                    //    return new SystemNotificationPacket(buf, len, client);
                    
                    case QQCommand.Upload_Group_Friend_05:
                        return new UploadGroupFriendReplyPacket(buf, len, client);
                    //case QQCommand.Download_Group_Friend_05:
                    //    return new DownloadGroupFriendReplyPacket(buf, len, client);
                    case QQCommand.Group_Data_OP_05:
                        return new GroupDataOpReplyPacket(buf, len, client);
                    case QQCommand.Friend_Data_OP_05:
                        return new FriendDataOpReplyPacket(buf, len, client);
                    
                    case QQCommand.Request_Key_05:
                        return new RequestKeyReplyPacket(buf, len, client);
                    case QQCommand.Advanced_Search_05:
                        return new AdvancedSearchUserReplyPacket(buf, len, client);
                    case QQCommand.Cluster_Data_OP_05:
                        return new GetTempClusterOnlineMemberReplyPacket(buf, len, client);
                    case QQCommand.AddFriendAuthorize:
                        return new AddFriendAuthorizeReplyPacket(buf, len, client);
                    case QQCommand.Signature_OP_05:
                        return new SignatureOpReplyPacket(buf, len, client);
                    case QQCommand.Weather_OP_05:
                        return new WeatherOpReplyPacket(buf, len, client);
                    case QQCommand.User_Property_OP_05:
                        return new UserPropertyOpReplyPacket(buf, len, client);
                    //case QQCommand.Friend_Level_OP_05:
                    //    return new FriendLevelOpReplyPacket(buf, len, client);
                    case QQCommand.Send_SMS_05:
                        return new SendSMSReplyPacket(buf, len, client);
                    case QQCommand.Temp_Session_OP_05:
                        return new TempSessionOpReplyPacket(buf, len, client);
                    case QQCommand.Privacy_Data_OP_05:
                        return new PrivacyDataOpReplyPacket(buf, len, client);
                    #endregion
                    default:
                        client.LogManager.Log("Recieved an unknown Packet!");
                        return new UnknownInPacket(buf, len, client);

                }
            }
            catch (Exception e)
            {
                client.LogManager.Log(e.Message + "\r\n" + e.StackTrace);
                return new ErrorPacket(ErrorPacketType.RUNTIME_ERROR, client, e);
            }
            //// 如果解析失败，返回null
            //buf.Position = offset;
            //return new UnknownInPacket(buf, len, client);
        }

        public OutPacket ParseOutcoming(ByteBuffer buf, int length, QQClient client)
        {
            throw new NotImplementedException();
        }

        public bool IsDuplicate(InPacket packet)
        {
            return history.Check(packet, true);
        }

        public bool IsDuplicatedNeedReply(InPacket packet)
        {
            return packet.Command == QQCommand.Recv_IM_09;
        }

        public int Relocate(ByteBuffer buf)
        {
            int offset = buf.Position;
            if (buf.Remaining() < 2)
                return offset;
            int len = buf.GetUShort(offset);
            if (len <= 0 || offset + len > buf.Length)
                return offset;
            else
                return offset + len;
        }

        public PacketHistory GetHistory()
        {
            return history;
        }

        #endregion

        /// <summary>
        /// 得到包的命令和序号  
    /// </summary>
        /// <param name="buf">The buf.</param>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        private QQCommand GetCommand(ByteBuffer buf, QQUser user)
        {
            if (!user.IsUdp)
            {
                return (QQCommand)buf.GetUShort(offset + 5);
            }
            else
            {
                return (QQCommand)buf.GetUShort(offset + 3);
            }
        }
        /// <summary>检查一个包是否是tcp包
    /// </summary>
        /// <param name="buf">The buf.</param>
        /// <returns>true表示是</returns>
        private bool CheckTcp(ByteBuffer buf)
        {
            //buffer length不大于2则连个长度字段都没有
            int bufferLength = buf.Length - buf.Position;
            if (bufferLength < 2) return false;
            // 如果可读内容小于包长，则这个包还没收完
            length = buf.GetChar(offset);
            if (length <= 0 || length > bufferLength)
                return false;
            // 再检查包头包尾
            if (buf.Get(offset + 2) == QQGlobal.QQ_HEADER_BASIC_FAMILY)
                if (buf.Get(offset + length - 1) == QQGlobal.QQ_TAIL_BASIC_FAMILY)
                    return true;
            return false;
        }
        private bool CheckUdp(ByteBuffer buf)
        {
            if (buf.Get(offset) == QQGlobal.QQ_HEADER_BASIC_FAMILY)
            {
                //首先检查是否UDP方式
                length = buf.Length - buf.Position;
                if (buf.Get(offset + length - 1) == QQGlobal.QQ_TAIL_BASIC_FAMILY)
                    return true;
            }
            return false;
        }

        #region IParser 成员


        
        #endregion
    }
}
