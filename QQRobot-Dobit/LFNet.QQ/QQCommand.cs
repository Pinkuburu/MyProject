
namespace LFNet.QQ
{
    /// <summary>
    /// QQ命令2009
    /// </summary>
    public enum QQCommand:ushort
    {
        /// <summary>
        /// 连接服务器
        /// </summary>
        Touch = 0x0091,
        /// <summary>
        /// 登出
        /// </summary>
        Logout = 0x0062,
        LoginRequest = 0x00ba,
        LoginGetInfo = 0x00e5,
        Login_A4 = 0x00a4,
        LoginGetList = 0x0018,
        LoginSendInfo = 0x0030,
        /// <summary>
        /// 保持在线
        /// </summary>
        Keep_Alive = 0x0058,
        Get_UserInfo = 0x0006,
        /// <summary>
        /// 改变自己的在线状态
        /// </summary>
        Change_Status = 0x000d,
        /// <summary>
        /// 发送消息
        /// </summary>
        Send_IM = 0x00cd,
        /// <summary>
        /// 接收消息
        /// </summary>
        Recv_IM = 0x0017,
        Recv_IM_09 = 0x00ce,
        Request_Key = 0x001d,
        /// <summary>
        /// 获取好友列表基本信息 昵称等
        /// </summary>
        Get_Friend_List = 0x0126,
        /// <summary>
        /// 群相关命令
        /// </summary>
        Cluster_Cmd = 0x0002,
        BuddyAlias = 0x003e,
        /// <summary>
        /// 分组信息
        /// </summary>
        GroupLabel = 0x0001,
        GetLevel = 0x005C,
        GetBuddySign = 0x0067,
        /// <summary>
        /// 系统消息
        /// </summary>
        BroadCast = 0x0080,
        /// <summary>
        /// 好友改变状态
        /// </summary>
        Friend_Change_Status = 0x0081,
        /// <summary>
        /// 请求好友设置得问题
        /// </summary>
        AddBuddyQuestion = 0x00B7,
        Account = 0x00b5,
        GetNotice = 0x00d4,
        CheckIP = 0x00da,
        LoginVerify = 0x00dd,
        /// <summary>
        /// 添加好友时的验证命令
        /// </summary>
        RequestToken = 0x00ae,
        Del_Buddy = 0x000a,

        /// <summary>
        /// 添加好友命令,直接请求添加,相当于请求对方添加好友设置
        /// </summary>
        Add_Friend = 0x00A7,
        /// <summary>
        /// 发送验证消息
        /// /AddBuddyVerify/
        /// </summary>
        AddFriendAuthorize = 0x00A8,
        /// <summary>
        /// 请求添加好友的时候会有个Token
        /// 需要把这个token发送到服务器去才能通过验证并加对方为好友
        /// </summary>
        AddFriendSendToken=0x00B5,

        ///// 保持在线状态
        ///// </summary>
        //Keep_Alive_05 = 0x0002,
        /// <summary>
        /// 修改自己的信息
        /// </summary>
        Modify_Info_05 = 0x0004,
        /// <summary>
        /// 查找用户
        /// </summary>
        Search_User_05 = 0x0005,
        /// <summary>
        ///  得到好友信息 
        /// </summary>
        Get_UserInfo_05 = 0x0006,

        /// <summary>
        /// 删除一个好友
        /// </summary>
        Delete_Friend_05 = 0x000A,
        /// <summary>
        /// 发送验证信息
        /// </summary>
        Add_Friend_Auth_05 = 0x000B,
        /// <summary>
        /// 确认收到了系统消息
        /// </summary>
        Ack_Sys_Msg_05 = 0x0012,
        /// <summary>
        /// 发送消息
        /// </summary>
        Send_IM_05 = 0x0016,

        /// <summary>
        /// 把自己从对方好友名单中删除
        /// </summary>
        Remove_Self_05 = 0x001c,
        /// <summary>
        /// 请求一些操作需要的密钥，比如文件中转，视频也有可能 
        /// </summary>
        Request_Key_05 = 0x001d,
        /// <summary>
        /// 得到好友列表 
        /// </summary>
        Get_Friend_List_05 = 0x0026,
        /// <summary>
        /// 得到在线好友列表
        /// </summary>
        Get_Online_OP = 0x0027,
        /// <summary>
        /// 发送短消息
        /// </summary>
        Send_SMS_05 = 0x002d,
        ///// <summary>
        ///// 群相关命令
        ///// </summary>
        //Cluster_Cmd_05 = 0x0030,
        /// <summary>
        /// 测试连接
        /// </summary>
        Test_05 = 0x0031,
        /// <summary>
        /// 分组数组操作
        /// </summary>
        Group_Data_OP_05 = 0x003C,
        /// <summary>
        /// 上传分组中的好友QQ号列表 
        /// </summary>
        Upload_Group_Friend_05 = 0x003D,
        /// <summary>
        /// 好友相关数据操作
        /// </summary>
        Friend_Data_OP_05 = 0x003E,
        ///// <summary>
        ///// 下载分组中的好友QQ号列表 
        ///// </summary>
        //Download_Group_Friend_05 = 0x0058,
        /// <summary>
        /// 好友等级信息相关操作
        /// </summary>
        Friend_Level_OP_05 = 0x005C,
        /// <summary>
        /// 隐私数据操作 
        /// </summary>
        Privacy_Data_OP_05 = 0x005E,
        /// <summary>
        /// 群数据操作命令
        /// </summary>
        Cluster_Data_OP_05 = 0x005F,
        /// <summary>
        /// 好友高级查找 
        /// </summary>
        Advanced_Search_05 = 0x0061,
        /// <summary>
        /// 请求登录令牌
        /// </summary>
        Request_Login_Token_05 = 0x0062,
        /// <summary>
        /// 用户属性操作 
        /// </summary>
        User_Property_OP_05 = 0x0065,
        /// <summary>
        /// 临时会话操作
        /// </summary>
        Temp_Session_OP_05 = 0x00E1,   //尝试0x0066 改为 0x00E1
        /// <summary>
        /// 个性签名的操作 
        /// </summary>
        Signature_OP_05 = 0x0067,
        /// <summary>
        /// 天气操作
        /// </summary>
        Weather_OP_05 = 0x00A6,
        
        
       
        /// <summary>
        /// 未知命令，调试用途 
        /// </summary>
        Unknown = 0xFFFF,
    }
    
}
