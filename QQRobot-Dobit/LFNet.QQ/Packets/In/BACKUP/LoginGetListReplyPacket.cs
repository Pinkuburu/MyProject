using System;
using System.Collections.Generic;
using System.Text;
using LFNet.QQ.Entities;

namespace LFNet.QQ.Packets.In
{
    /// <summary>
    /// 返回用户好友群列表
    /// </summary>
    public class LoginGetListReplyPacket : BasicInPacket
    {
        public LoginGetListReplyPacket(ByteBuffer buf, int length, QQClient client)
            : base(buf, length, client)
        {
        }

        public override string GetPacketName()
        {
            return "Login GetList Reply Packet";
        }
        /// <summary>
        /// 当为0x038A时表示后面还有数据
        /// </summary>
        public char ReplyCode { get; private set; }
        public bool Finished { get; private set; }
        /// <summary>
        /// 请求下一个包
        /// </summary>
        public ushort NextPos { get; set; }
        ///// <summary>
        ///// 本次获取的QQ好友列表
        ///// </summary>
        //public List<QQFriend> QQFriendList { get; set; }
        public List<QQBasicInfo> QQList { get; set; }
        ///// <summary>
        ///// QQ群列表
        ///// </summary>
        //public List<ClusterInfo> ClusterList { get; set; }
        protected override void ParseBody(ByteBuffer buf)
        {
#if DEBUG
            Client.LogManager.Log(ToString() + " Decoded Data:" + Utils.Util.ToHex(buf.ToByteArray()));
#endif

            ReplyCode = buf.GetChar();//00 9C
            buf.GetInt();//00 00 00 00
            NextPos = buf.GetUShort();
            Finished = !(ReplyCode == 0x038A && NextPos > 0);
            //this.ClusterList = new List<ClusterInfo>();
            //this.QQFriendList = new List<QQFriend>();
            this.QQList = new List<QQBasicInfo>();
            while (buf.Position + 2 < buf.Length)
            {
                int number = buf.GetInt();
                QQType type =(QQType) buf.Get();
                byte gid = buf.Get();

                QQBasicInfo qq = new QQBasicInfo(number, type, ((int)gid) / 4);
                //qq.UIN = number;
                //qq.GroupId = ((int)gid) / 4;
                //qq.Type = (QQType)type;
                this.QQList.Add(qq);
                //if (type == 0x04)
                //{
                //    ClusterInfo ci = new ClusterInfo();
                //    ci.ClusterId =(uint) number;//群内部号码
                //    this.ClusterList.Add(ci);
                //}
                //else if (type == 0x01)
                //{
                //    QQFriend friend = new QQFriend();
                    
                //    friend.QQ = number;
                //    friend.GroupId = ((int)gid)/4;
                //    this.QQFriendList.Add(friend);
                //}
                //else
                //{
                //    Client.LogManager.Log("unknown type: type=0x"+type.ToString("X2")+" number="+number.ToString() +" gid=0x"+gid.ToString("X2"));
                //}
            
            }


            
        }


    }
}
