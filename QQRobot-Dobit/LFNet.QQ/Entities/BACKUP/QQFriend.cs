
using System;
using System.Collections.Generic;
using System.Text;

using LFNet.QQ.Utils;
namespace LFNet.QQ.Entities
{
    /// <summary>
    /// 好友的信息
    /// </summary>
    public class QQFriend
    {
        public QQFriend()
        {
            this.FriendStatus = new FriendStatus();
            this.QQBasicInfo = new QQBasicInfo(0, QQType.QQ, 0);
        }
        /// <summary>
        /// QQ基本信息 
        /// 包括号码 组id 类型
        /// </summary>
        /// <value></value>
        public QQBasicInfo QQBasicInfo { get; set; }
        public int QQ { get { return QQBasicInfo.QQ; } }
        public FriendStatus FriendStatus { get; set; }
        /// <summary>
        /// 头像，参看ContactInfo的头像注释
        /// </summary>
        /// <value></value>
        public int Header { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        /// <value></value>
        public int Age { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        /// <value></value>
        public Gender Gender { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        /// <value></value>
        public string Nick { get; set; }

        /// <summary>
        /// // 用户属性标志
        /// bit1 => 会员
        /// bit5 => 移动QQ
        /// bit6 => 绑定到手机
        /// bit7 => 是否有摄像头
        /// bit18 => 是否TM登录
        /// </summary>
        /// <value></value>
        public uint UserFlag { get; set; }

        /// <summary>
        /// true如果好友是会员，否则为false
        /// </summary>
        /// <returns></returns>
        public bool IsMember()
        {
            return (UserFlag & QQGlobal.QQ_FLAG_MEMBER) != 0;
        }
        /// <summary>
        /// 是否绑定手机
        /// </summary>
        /// <returns></returns>
        public bool IsBind()
        {
            return (UserFlag & QQGlobal.QQ_FLAG_BIND) != 0;
        }
        /// <summary>是否移动QQ
        /// </summary>
        /// <returns></returns>
        public bool IsMobile()
        {
            return (UserFlag & QQGlobal.QQ_FLAG_MOBILE) != 0;
        }
        /// <summary>
        /// 用户是否有摄像头
        /// </summary>
        /// <returns></returns>
        public bool HasCam()
        {
            return (UserFlag & QQGlobal.QQ_FLAG_CAM) != 0;
        }
        /// <summary>
        /// 用户是否使用TM登录
        /// </summary>
        /// <returns></returns>
        public bool IsTM()
        {
            return (UserFlag & QQGlobal.QQ_FLAG_TM) != 0;
        }
        /// <summary>
        /// 是否是男性
        /// </summary>
        /// <returns></returns>
        public bool IsGG()
        {
            return Gender == Gender.GG;
        }

        /// <summary>
        /// QQ状态
        /// </summary>
        public QQStatus Status
        {
            get
            {
                return FriendStatus.Status == QQStatus.NONE ? QQStatus.OFFLINE : FriendStatus.Status;
            }
        }
        /// <summary>
        /// 给定一个输入流，解析QQFriend结构
        /// </summary>
        /// <param name="buf">The buf.</param>
        public void Read(ByteBuffer buf)
        {
            //// 000-003: 好友QQ号
            //QQ = buf.GetInt();
            // 004-005: 头像
            Header = buf.GetUShort();
            // 006: 年龄
            Age = buf.Get();
            // 007: 性别
            Gender = (Gender)buf.Get();
            // 008: 昵称长度
            int len = (int)buf.Get();
            byte[] b = buf.GetByteArray(len);
            Nick = Util.GetString(b);
            // 用户属性
            UserFlag = buf.GetUInt();
            buf.Position += 23;
        }
        
    }
}
