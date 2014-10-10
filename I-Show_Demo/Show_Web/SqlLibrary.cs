using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Show_Web;


public class SqlLibrary
{
    public static bool IsOnline = true;

    #region 连接数据库方法 GetServer_Main()
    /// <summary>
    /// 连接数据库方法
    /// </summary>
    /// <returns></returns>
    public static string GetServer_Main()
    {
        if (IsOnline)
            return @"Data Source=58.241.133.214,1595\SQL2005;Initial Catalog=TeamShow;Persist Security Info=True;User ID=Cupid;Password=qweqwe123";
        else
            return @"Data Source=127.0.0.1,52328\SQL2005;Initial Catalog=XiaoNei_Login;Persist Security Info=True;User ID=Cupid;Password=qweqwe123";
    }
    #endregion 连接数据库方法 GetServer_Main()

    #region 新消息轮巡检测 CheckNewMessage(int intUserID)
    /// <summary>
    /// 新消息轮巡检测
    /// </summary>
    /// <param name="intUserID"></param>
    /// <returns></returns>
    public static int CheckNewMessage(int intUserID)
    {
        int intNewMsg = 0;

        SqlParameter[] sp = new SqlParameter[1];
        sp[0] = new SqlParameter("@UserID", SqlDbType.Int);
        sp[0].Value = intUserID;

        DataRow dr = SqlHelper.ExecuteDataRow(GetServer_Main(), CommandType.StoredProcedure, "CheckNewMessage", sp);
        if (dr != null)
        {
            intNewMsg = (int)dr["NewMsg"];
        }
        return intNewMsg;
    }

    #endregion 新消息轮巡检测 CheckNewMessage(int intUserID)

    #region 发送消息 SendMessage(int intUserID, int intSendID, string strSender, string strContent)
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="intUserID">收件人ID</param>
    /// <param name="intSendID">发件人ID</param>
    /// <param name="strSender">发件人昵称</param>
    /// <param name="strContent">消息内容</param>
    /// <returns></returns>
    public static DataRow SendMessage(int intUserID, int intSendID, string strSender, string strContent)
    {
        int intType = 0;

        SqlParameter[] sp = new SqlParameter[4];
        sp[0] = new SqlParameter("@UserID", SqlDbType.Int);
        sp[1] = new SqlParameter("@SendID", SqlDbType.Int);
        sp[2] = new SqlParameter("@Sender", SqlDbType.VarChar, 50);
        sp[3] = new SqlParameter("@Content", SqlDbType.VarChar, 2000);
        sp[0].Value = intUserID;
        sp[1].Value = intSendID;
        sp[2].Value = strSender;
        sp[3].Value = strContent;

        DataRow dr = SqlHelper.ExecuteDataRow(GetServer_Main(), CommandType.StoredProcedure, "SendMessage", sp);
        return dr;
    }

    #endregion 发送消息 SendMessage(int intUserID, int intSendID, string strSender, string strContent)

    #region 显示收件箱 ShowMessageBox(int intUserID)
    /// <summary>
    /// 显示收件箱
    /// </summary>
    /// <param name="intUserID"></param>
    /// <returns></returns>
    public static DataTable ShowMessageBox(int intUserID)
    {
        SqlParameter[] sp = new SqlParameter[1];
        sp[0] = new SqlParameter("@UserID", SqlDbType.Int);
        sp[0].Value = intUserID;

        DataTable dt = SqlHelper.ExecuteDataTable(GetServer_Main(), CommandType.StoredProcedure, "ShowMessage", sp);
        return dt;
    }

    #endregion 显示收件箱 ShowMessageBox(int intUserID)

    #region 读取消息 ReadMessage(int intUserID, int intID)
    /// <summary>
    /// 读取消息
    /// </summary>
    /// <param name="UserID"></param>
    /// <param name="intID"></param>
    /// <returns>0读取标记失败，1读取标记成功</returns>
    public static int ReadMessage(int intUserID, int intID)
    {
        int intType = 0;

        SqlParameter[] sp = new SqlParameter[2];
        sp[0] = new SqlParameter("@UserID", SqlDbType.Int);
        sp[1] = new SqlParameter("@ID", SqlDbType.Int);
        sp[0].Value = intUserID;
        sp[1].Value = intID;

        DataRow dr = SqlHelper.ExecuteDataRow(GetServer_Main(), CommandType.StoredProcedure, "ReadMessage", sp);
        if (dr != null)
        {
            intType = (int)dr["Type"];
        }
        return intType;
    }

    #endregion 读取消息 ReadMessage(int intUserID, int intID)    

    #region 删除消息 DeleteMessage(int UserID, int intID)
    /// <summary>
    /// 删除消息
    /// </summary>
    /// <param name="UserID"></param>
    /// <param name="intID"></param>
    /// <returns>0删除标记失败，1删除标记成功</returns>
    public static int DeleteMessage(int intUserID, int intID)
    {
        int intType = 0;

        SqlParameter[] sp = new SqlParameter[2];
        sp[0] = new SqlParameter("@UserID", SqlDbType.Int);
        sp[1] = new SqlParameter("@ID", SqlDbType.Int);
        sp[0].Value = intUserID;
        sp[1].Value = intID;

        DataRow dr = SqlHelper.ExecuteDataRow(GetServer_Main(), CommandType.StoredProcedure, "DeleteMessage", sp);
        if (dr != null)
        {
            intType = (int)dr["Type"];
        }
        return intType;
    }

    #endregion 删除消息 DeleteMessage(int UserID, int intID)

    #region 添加关注 AddConcern(int intUserID, int ConcernID)
    /// <summary>
    /// 添加关注
    /// </summary>
    /// <param name="intUserID"></param>
    /// <param name="ConcernID"></param>
    /// <returns></returns>
    public static int AddConcern(int intUserID, int ConcernID)
    {
        int intType = 0;

        SqlParameter[] sp = new SqlParameter[2];
        sp[0] = new SqlParameter("@UserID", SqlDbType.Int);
        sp[1] = new SqlParameter("@ConcernID", SqlDbType.Int);
        sp[0].Value = intUserID;
        sp[1].Value = ConcernID;

        DataRow dr = SqlHelper.ExecuteDataRow(GetServer_Main(), CommandType.StoredProcedure, "AddConcern", sp);
        if (dr != null)
        {
            intType = (int)dr["Type"];
        }
        return intType;
    }

    #endregion 添加关注 AddConcern(int intUserID, int ConcernID)

    #region 删除关注 DeleteConcern(int intUserID, int ConcernID)
    /// <summary>
    /// 删除关注
    /// </summary>
    /// <param name="intUserID"></param>
    /// <param name="ConcernID"></param>
    /// <returns></returns>
    public static int DeleteConcern(int intUserID, int ConcernID)
    {
        int intType = 0;

        SqlParameter[] sp = new SqlParameter[2];
        sp[0] = new SqlParameter("@UserID", SqlDbType.Int);
        sp[1] = new SqlParameter("@ConcernID", SqlDbType.Int);
        sp[0].Value = intUserID;
        sp[1].Value = ConcernID;

        DataRow dr = SqlHelper.ExecuteDataRow(GetServer_Main(), CommandType.StoredProcedure, "DeleteConcern", sp);
        if (dr != null)
        {
            intType = (int)dr["Type"];
        }
        return intType;
    }

    #endregion 删除关注 DeleteConcern(int intUserID, int ConcernID)

    #region 登录验证 LoginAuth(string strUserName, string strPassword)
    /// <summary>
    /// 登录验证
    /// </summary>
    /// <param name="strUserName"></param>
    /// <param name="strPassword"></param>
    /// <returns></returns>
    public static DataRow LoginAuth(string strUserName, string strPassword)
    {
        int intType = 0;

        SqlParameter[] sp = new SqlParameter[2];
        sp[0] = new SqlParameter("@UserName", SqlDbType.VarChar, 50);
        sp[1] = new SqlParameter("@Password", SqlDbType.VarChar, 50);
        sp[0].Value = strUserName;
        sp[1].Value = strPassword;

        DataRow dr = SqlHelper.ExecuteDataRow(GetServer_Main(), CommandType.StoredProcedure, "LoginAuth", sp);
        return dr;
    }

    #endregion 登录验证 LoginAuth(string strUserName, string strPassword)

    #region 通过UserID读取关注列表（判定关注用） ReadConcernList(int intUser)
    /// <summary>
    /// 通过UserID读取关注列表（判定关注用）
    /// </summary>
    /// <param name="intUser"></param>
    /// <returns></returns>
    public static DataTable ReadConcernList(int intUserID)
    {
        SqlParameter[] sp = new SqlParameter[1];
        sp[0] = new SqlParameter("@UserID", SqlDbType.Int);
        sp[0].Value = intUserID;

        DataTable dt = SqlHelper.ExecuteDataTable(GetServer_Main(), CommandType.StoredProcedure, "ReadConcernlist", sp);
        return dt;
    }

    #endregion 通过UserID读取关注列表（判定关注用） ReadConcernList(int intUser)

    #region 读取用户关注数 CountConcern(int intUserID)
    /// <summary>
    /// 读取用户关注数
    /// </summary>
    /// <param name="intUserID"></param>
    /// <returns></returns>
    public static int CountConcern(int intUserID)
    {
        int intCount = 0;

        SqlParameter[] sp = new SqlParameter[1];
        sp[0] = new SqlParameter("@UserID", SqlDbType.Int);
        sp[0].Value = intUserID;

        DataRow dr = SqlHelper.ExecuteDataRow(GetServer_Main(), CommandType.StoredProcedure, "CountConcern", sp);
        if (dr != null)
        {
            intCount = (int)dr["Count"];
        }
        return intCount;
    }
    #endregion 读取用户关注数 CountConcern(int intUserID)

    #region 读取用户粉丝数 CountFans(int intUserID)
    /// <summary>
    /// 读取用户粉丝数
    /// </summary>
    /// <param name="intUserID"></param>
    /// <returns></returns>
    public static int CountFans(int intUserID)
    {
        int intCount = 0;

        SqlParameter[] sp = new SqlParameter[1];
        sp[0] = new SqlParameter("@UserID", SqlDbType.Int);
        sp[0].Value = intUserID;

        DataRow dr = SqlHelper.ExecuteDataRow(GetServer_Main(), CommandType.StoredProcedure, "CountFans", sp);
        if (dr != null)
        {
            intCount = (int)dr["Count"];
        }
        return intCount;
    }
    #endregion 读取用户粉丝数 CountFans(int intUserID)

    #region 得到查询记录行数 SearchUserListCount(string strProvince, string strCity, string strGender, string strCategory)
    /// <summary>
    /// 得到查询记录行数
    /// </summary>
    /// <param name="strProvince"></param>
    /// <param name="strCity"></param>
    /// <param name="strGender"></param>
    /// <param name="strCategory"></param>
    /// <returns></returns>
    public static int SearchUserListCount(string strProvince, string strCity, string strGender, string strCategory)
    {
        int intCount = 0;

        SqlParameter[] sp = new SqlParameter[4];
        sp[0] = new SqlParameter("@Province", SqlDbType.VarChar, 20);
        sp[1] = new SqlParameter("@City", SqlDbType.VarChar, 20);
        sp[2] = new SqlParameter("@Gender", SqlDbType.Int);
        sp[3] = new SqlParameter("@Category", SqlDbType.Int);
        sp[0].Value = strProvince;
        sp[1].Value = strCity;
        sp[2].Value = Convert.ToInt32(strGender);
        sp[3].Value = Convert.ToInt32(strCategory);

        DataRow dr = SqlHelper.ExecuteDataRow(GetServer_Main(), CommandType.StoredProcedure, "SearchUserListCount", sp);
        if (dr != null)
        {
            intCount = (int)dr["Count"];
        }
        return intCount;
    }
    #endregion 得到查询记录行数 SearchUserListCount(string strProvince, string strCity, string strGender, string strCategory)

    #region 显示粉丝列表 ShowFansList(int intPage, int intUserID)
    /// <summary>
    /// 显示粉丝列表
    /// </summary>
    /// <param name="intPage"></param>
    /// <param name="intUserID"></param>
    /// <returns></returns>
    public static DataTable ShowFansList(int intPage, int intUserID)
    {
        SqlParameter[] sp = new SqlParameter[3];
        sp[0] = new SqlParameter("@PageIndex", SqlDbType.Int, 4);
        sp[1] = new SqlParameter("@PageSize", SqlDbType.Int, 4);
        sp[2] = new SqlParameter("@UserID", SqlDbType.Int, 8);
        sp[0].Value = intPage;
        sp[1].Value = 8;
        sp[2].Value = intUserID;

        DataTable dt = SqlHelper.ExecuteDataTable(GetServer_Main(), CommandType.StoredProcedure, "GetShowFansList", sp);
        return dt;
    }
    #endregion 显示粉丝列表 ShowFansList(int intPage, int intUserID)

    #region 显示关注列表 ShowConcernList(int intPage, int intUserID)
    /// <summary>
    /// 显示关注列表
    /// </summary>
    /// <param name="intPage"></param>
    /// <param name="intUserID"></param>
    /// <returns></returns>
    public static DataTable ShowConcernList(int intPage, int intUserID)
    {
        SqlParameter[] sp = new SqlParameter[3];
        sp[0] = new SqlParameter("@PageIndex", SqlDbType.Int, 4);
        sp[1] = new SqlParameter("@PageSize", SqlDbType.Int, 4);
        sp[2] = new SqlParameter("@UserID", SqlDbType.Int, 8);
        sp[0].Value = intPage;
        sp[1].Value = 8;
        sp[2].Value = intUserID;

        DataTable dt = SqlHelper.ExecuteDataTable(GetServer_Main(), CommandType.StoredProcedure, "GetShowConcernList", sp);
        return dt;
    }
    #endregion 显示关注列表 ShowConcernList(int intPage, int intUserID)

    #region 搜索用户列表(模糊) SearchUserList(int intPage, string strProvince, string strCity, int intGender, int intCategory)
    /// <summary>
    /// 搜索用户列表(模糊)
    /// </summary>
    /// <param name="intPage"></param>
    /// <param name="strProvince"></param>
    /// <param name="strCity"></param>
    /// <param name="intGender"></param>
    /// <param name="intCategory"></param>
    /// <returns></returns>
    public static DataTable SearchUserList(int intPage, string strProvince, string strCity, int intGender, int intCategory)
    {
        SqlParameter[] sp = new SqlParameter[6];
        sp[0] = new SqlParameter("@PageIndex", SqlDbType.Int, 4);
        sp[1] = new SqlParameter("@PageSize", SqlDbType.Int, 4);
        sp[2] = new SqlParameter("@Province", SqlDbType.VarChar, 20);
        sp[3] = new SqlParameter("@City", SqlDbType.VarChar, 20);
        sp[4] = new SqlParameter("@Gender", SqlDbType.Int, 1);
        sp[5] = new SqlParameter("@Category", SqlDbType.Int, 1);
        sp[0].Value = intPage;
        sp[1].Value = 8;
        sp[2].Value = strProvince;
        sp[3].Value = strCity;
        sp[4].Value = intGender;
        sp[5].Value = intCategory;

        DataTable dt = SqlHelper.ExecuteDataTable(SqlLibrary.GetServer_Main(), CommandType.StoredProcedure, "SearchUserList", sp);
        return dt;
    }
    #endregion 搜索用户列表(模糊) SearchUserList(int intPage, string strProvince, string strCity, int intGender, int intCategory)

    #region 搜索用户列表(精确) SearchUser(string strNickName)
    /// <summary>
    /// 搜索用户列表(精确)
    /// </summary>
    /// <param name="strNickName"></param>
    /// <returns></returns>
    public static DataTable SearchUser(string strNickName)
    {
        SqlParameter[] sp = new SqlParameter[1];
        sp[0] = new SqlParameter("@NickName", SqlDbType.VarChar, 50);
        sp[0].Value = strNickName;

        DataTable dt = SqlHelper.ExecuteDataTable(SqlLibrary.GetServer_Main(), CommandType.StoredProcedure, "SearchUser", sp);
        return dt;
    }
    #endregion 搜索用户列表(精确) SearchUser(string strNickName)

    #region 用户密码修改 ModifyPassword(int intUserID, string strUserName, string strPassword, string strNewPassword)
    /// <summary>
    /// 用户密码修改
    /// </summary>
    /// <param name="intUserID"></param>
    /// <param name="strUserName"></param>
    /// <param name="strPassword"></param>
    /// <param name="strNewPassword"></param>
    /// <returns>1成功 0失败</returns>
    public static int ModifyPassword(int intUserID, string strUserName, string strPassword, string strNewPassword)
    {
        int intType = -1;

        SqlParameter[] sp = new SqlParameter[4];
        sp[0] = new SqlParameter("@UserID", SqlDbType.Int);
        sp[1] = new SqlParameter("@UserName", SqlDbType.VarChar, 50);
        sp[2] = new SqlParameter("@Password", SqlDbType.VarChar, 50);
        sp[3] = new SqlParameter("@NewPassword", SqlDbType.VarChar, 50);
        sp[0].Value = intUserID;
        sp[1].Value = strUserName;
        sp[2].Value = strPassword;
        sp[3].Value = strNewPassword;

        DataRow dr = SqlHelper.ExecuteDataRow(GetServer_Main(), CommandType.StoredProcedure, "ModifyPassword", sp);
        if (dr != null)
        {
            intType = (int)dr["Type"];
        }
        return intType;
    }
    #endregion 用户密码修改 ModifyPassword(int intUserID, string strUserName, string strPassword, string strNewPassword)

    #region 用户注册信息修改 ModifyUserInfo(int intUserID, string strUserName, string strPassword, string strNickName, bool blSex, DateTime dtBirthDay, string strProvince, string strCity, string strQQ)
    /// <summary>
    /// 用户注册信息修改
    /// </summary>
    /// <param name="intUserID"></param>
    /// <param name="strUserName"></param>
    /// <param name="strPassword"></param>
    /// <param name="strNickName"></param>
    /// <param name="blSex"></param>
    /// <param name="dtBirthDay"></param>
    /// <param name="strProvince"></param>
    /// <param name="strCity"></param>
    /// <param name="strQQ"></param>
    /// <returns>1成功 0失败</returns>
    public static int ModifyUserInfo(int intUserID, string strUserName, string strPassword, DateTime dtBirthDay, string strProvince, string strCity, string strQQ)
    {
        //int intUserID, string strUserName, string strPassword, string strNickName, bool blSex, DateTime dtBirthDay, string strProvince, string strCity, string strQQ
        int intType = -1;

        SqlParameter[] sp = new SqlParameter[7];
        sp[0] = new SqlParameter("@UserID", SqlDbType.Int);
        sp[1] = new SqlParameter("@UserName", SqlDbType.VarChar, 50);
        sp[2] = new SqlParameter("@Password", SqlDbType.VarChar, 50);
        //sp[3] = new SqlParameter("@NickName", SqlDbType.VarChar, 50);
        //sp[4] = new SqlParameter("@Sex", SqlDbType.Bit);
        sp[3] = new SqlParameter("@BirthDay", SqlDbType.DateTime);
        sp[4] = new SqlParameter("@Province", SqlDbType.VarChar, 50);
        sp[5] = new SqlParameter("@City", SqlDbType.VarChar, 50);
        sp[6] = new SqlParameter("@QQ", SqlDbType.VarChar, 15);
        sp[0].Value = intUserID;
        sp[1].Value = strUserName;
        sp[2].Value = strPassword;
        //sp[3].Value = strNickName;
        //sp[4].Value = blSex;
        sp[3].Value = dtBirthDay;
        sp[4].Value = strProvince;
        sp[5].Value = strCity;
        sp[6].Value = strQQ;

        DataRow dr = SqlHelper.ExecuteDataRow(GetServer_Main(), CommandType.StoredProcedure, "ModifyUserInfo", sp);
        if (dr != null)
        {
            intType = (int)dr["Type"];
        }
        return intType;
    }
    #endregion 用户注册信息修改 ModifyUserInfo(int intUserID, string strUserName, string strPassword, string strNickName, bool blSex, DateTime dtBirthDay, string strProvince, string strCity, string strQQ)

    #region 拍摄状态更新 ModifyShootStatus(int intUserID, string strUserName, string strPassword, bool blShootStatus)
    /// <summary>
    /// 拍摄状态更新
    /// </summary>
    /// <param name="intUserID"></param>
    /// <param name="strUserName"></param>
    /// <param name="strPassword"></param>
    /// <param name="blShootStatus"></param>
    /// <returns></returns>
    public static int ModifyShootStatus(int intUserID, string strUserName, string strPassword, bool blShootStatus)
    {
        int intType = -1;

        SqlParameter[] sp = new SqlParameter[4];
        sp[0] = new SqlParameter("@UserID", SqlDbType.Int);
        sp[1] = new SqlParameter("@UserName", SqlDbType.VarChar, 50);
        sp[2] = new SqlParameter("@Password", SqlDbType.VarChar, 50);
        sp[3] = new SqlParameter("@ShootStatus", SqlDbType.Bit);
        sp[0].Value = intUserID;
        sp[1].Value = strUserName;
        sp[2].Value = strPassword;
        sp[3].Value = blShootStatus;

        DataRow dr = SqlHelper.ExecuteDataRow(GetServer_Main(), CommandType.StoredProcedure, "ModifyShootStatus", sp);
        if (dr != null)
        {
            intType = (int)dr["Type"];
        }
        return intType;
    }
    #endregion 拍摄状态更新 ModifyShootStatus(int intUserID, string strUserName, string strPassword, bool blShootStatus)

    #region 读取用户信息和图片信息 ReadUserInfoAndImg(int intUserID)
    /// <summary>
    /// 读取用户信息和图片信息
    /// </summary>
    /// <param name="intUserID"></param>
    /// <returns></returns>
    public static DataRow ReadUserInfoAndImg(int intUserID)
    {
        SqlParameter[] sp = new SqlParameter[1];
        sp[0] = new SqlParameter("@UserID", SqlDbType.Int);
        sp[0].Value = intUserID;

        DataRow dr = SqlHelper.ExecuteDataRow(SqlLibrary.GetServer_Main(), CommandType.StoredProcedure, "ReadUserInfoAndImg", sp);
        return dr;
    }
    #endregion 读取用户信息和图片信息 ReadUserInfoAndImg(int intUserID)

    #region 发送QQ消息 SendQQMsg(string strQQ, string strMsg)
    /// <summary>
    /// 发送QQ消息
    /// </summary>
    /// <param name="strQQ"></param>
    /// <param name="strMsg"></param>
    /// <returns></returns>
    public static void SendQQMsg(string strQQ, string strMsg)
    {
        WebClient HTTPproc = new WebClient();
        try
        {
            HTTPproc.OpenRead("Http://61.147.125.204:8848/Api?Key=Cupid&utf=1&SendType=SendMessage&ID=" + strQQ + "&Message=" + UrlEncode(strMsg, "UTF-8") + "&Time=1000");
        }
        catch
        {
        	
        }        
    }
    #endregion 发送QQ消息 SendQQMsg(string strQQ, string strMsg)

    #region URL编码 UrlEncode(string str, string encode)
    /// <summary>
    /// URL编码
    /// </summary>
    /// <param name="str"></param>
    /// <param name="encode"></param>
    /// <returns></returns>
    private static string UrlEncode(string str, string encode)
    {
        int factor = 0;
        if (encode == "UTF-8")
            factor = 3;
        if (encode == "GB2312")
            factor = 2;
        //不需要编码的字符

        string okChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.";
        System.Text.Encoder encoder = System.Text.Encoding.GetEncoding(encode).GetEncoder();
        char[] c1 = str.ToCharArray();
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //一个字符一个字符的编码

        for (int i = 0; i < c1.Length; i++)
        {
            //不需要编码

            if (okChar.IndexOf(c1[i]) > -1)
                sb.Append(c1[i]);
            else
            {
                byte[] c2 = new byte[factor];
                int charUsed, byteUsed; bool completed;

                encoder.Convert(c1, i, 1, c2, 0, factor, true, out charUsed, out byteUsed, out completed);

                foreach (byte b in c2)
                {
                    if (b != 0)
                        sb.AppendFormat("%{0:X}", b);
                }
            }
        }
        return sb.ToString().Trim();
    }
    #endregion

    #region 更新在线拍摄时间 UpdateOnlineTime(int intUserID, string strUserName, string strPassword)
    /// <summary>
    /// 更新在线拍摄时间
    /// </summary>
    /// <param name="intUserID"></param>
    /// <param name="strUserName"></param>
    /// <param name="strPassword"></param>
    /// <returns>1成功 2失败</returns>
    public static int UpdateOnlineTime(int intUserID, string strUserName, string strPassword)
    {
        int intType = -1;

        SqlParameter[] sp = new SqlParameter[3];
        sp[0] = new SqlParameter("@UserID", SqlDbType.Int);
        sp[1] = new SqlParameter("@UserName", SqlDbType.VarChar, 50);
        sp[2] = new SqlParameter("@Password", SqlDbType.VarChar, 50);
        sp[0].Value = intUserID;
        sp[1].Value = strUserName;
        sp[2].Value = strPassword;

        DataRow dr = SqlHelper.ExecuteDataRow(GetServer_Main(), CommandType.StoredProcedure, "UpdateOnlineTime", sp);
        if (dr != null)
        {
            intType = (int)dr["Type"];
        }
        return intType;
    }
    #endregion 更新在线拍摄时间 UpdateOnlineTime(int intUserID, string strUserName, string strPassword)

    #region 查询用户是否为VIP用户 CheckVIP(int intUserID)
    /// <summary>
    /// 查询用户是否为VIP用户
    /// </summary>
    /// <param name="intUserID"></param>
    /// <returns></returns>
    public static bool CheckVIP(int intUserID)
    {
        bool blVIP = false;

        SqlParameter[] sp = new SqlParameter[1];
        sp[0] = new SqlParameter("@UserID", SqlDbType.Int);
        sp[0].Value = intUserID;

        DataRow dr = SqlHelper.ExecuteDataRow(GetServer_Main(), CommandType.StoredProcedure, "CheckVIP", sp);
        if (dr != null)
        {
            blVIP = (bool)dr["VIP"];
        }
        return blVIP;
    }
    #endregion 查询用户是否为VIP用户 CheckVIP(int intUserID)

    #region 提交建议 SendPropose(int intUserID, string strContent)
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="intUserID">收件人ID</param>
    /// <param name="intSendID">发件人ID</param>
    /// <param name="strSender">发件人昵称</param>
    /// <param name="strContent">消息内容</param>
    /// <returns></returns>
    public static int SendPropose(int intUserID, string strContent)
    {
        int intType = 0;

        SqlParameter[] sp = new SqlParameter[2];
        sp[0] = new SqlParameter("@UserID", SqlDbType.Int);
        sp[1] = new SqlParameter("@Content", SqlDbType.VarChar, 1000);
        sp[0].Value = intUserID;
        sp[1].Value = strContent;

        DataRow dr = SqlHelper.ExecuteDataRow(GetServer_Main(), CommandType.StoredProcedure, "SendPropose", sp);
        if (dr != null)
        {
            intType = (int)dr["Type"];
        }
        return intType;
    }

    #endregion 提交建议 SendPropose(int intUserID, string strContent)

    #region 登录验证（合作用）CheckUser(string strUserName, string strPassword)
    /// <summary>
    /// 登录验证（合作用）
    /// </summary>
    /// <param name="strUserName"></param>
    /// <param name="strPasswor"></param>
    /// <returns>0没有此帐号,-1帐号密码错误,1验证成功</returns>
    public static int CheckUser(string strUserName, string strPassword)
    {
        int intType = 0;

        SqlParameter[] sp = new SqlParameter[2];
        sp[0] = new SqlParameter("@UserName", SqlDbType.VarChar);
        sp[1] = new SqlParameter("@Password", SqlDbType.VarChar);
        sp[0].Value = strUserName;
        sp[1].Value = strPassword;

        DataRow dr = SqlHelper.ExecuteDataRow(GetServer_Main(), CommandType.StoredProcedure, "CheckUser", sp);
        if (dr != null)
        {
            intType = (int)dr["Type"];
        }
        return intType;
    }
    #endregion 登录验证（合作用）CheckUser(string strUserName, string strPassword)

    #region 读取用户在线时长 ReadOnlineTime(int intUserID)
    /// <summary>
    /// 读取用户在线时长
    /// </summary>
    /// <param name="intUserID"></param>
    /// <returns></returns>
    public static long ReadOnlineTime(int intUserID)
    {
        long longCount = 0;

        SqlParameter[] sp = new SqlParameter[1];
        sp[0] = new SqlParameter("@UserID", SqlDbType.Int);
        sp[0].Value = intUserID;

        DataRow dr = SqlHelper.ExecuteDataRow(GetServer_Main(), CommandType.StoredProcedure, "GetOnlinetime", sp);
        if (dr != null)
        {
            longCount = (long)dr["OnlineTime"];
        }
        return longCount;
    }
    #endregion 读取用户在线时长 ReadOnlineTime(int intUserID)
}
