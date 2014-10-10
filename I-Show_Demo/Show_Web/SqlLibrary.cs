using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Show_Web;


public class SqlLibrary
{
    public static bool IsOnline = true;

    #region �������ݿⷽ�� GetServer_Main()
    /// <summary>
    /// �������ݿⷽ��
    /// </summary>
    /// <returns></returns>
    public static string GetServer_Main()
    {
        if (IsOnline)
            return @"Data Source=58.241.133.214,1595\SQL2005;Initial Catalog=TeamShow;Persist Security Info=True;User ID=Cupid;Password=qweqwe123";
        else
            return @"Data Source=127.0.0.1,52328\SQL2005;Initial Catalog=XiaoNei_Login;Persist Security Info=True;User ID=Cupid;Password=qweqwe123";
    }
    #endregion �������ݿⷽ�� GetServer_Main()

    #region ����Ϣ��Ѳ��� CheckNewMessage(int intUserID)
    /// <summary>
    /// ����Ϣ��Ѳ���
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

    #endregion ����Ϣ��Ѳ��� CheckNewMessage(int intUserID)

    #region ������Ϣ SendMessage(int intUserID, int intSendID, string strSender, string strContent)
    /// <summary>
    /// ������Ϣ
    /// </summary>
    /// <param name="intUserID">�ռ���ID</param>
    /// <param name="intSendID">������ID</param>
    /// <param name="strSender">�������ǳ�</param>
    /// <param name="strContent">��Ϣ����</param>
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

    #endregion ������Ϣ SendMessage(int intUserID, int intSendID, string strSender, string strContent)

    #region ��ʾ�ռ��� ShowMessageBox(int intUserID)
    /// <summary>
    /// ��ʾ�ռ���
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

    #endregion ��ʾ�ռ��� ShowMessageBox(int intUserID)

    #region ��ȡ��Ϣ ReadMessage(int intUserID, int intID)
    /// <summary>
    /// ��ȡ��Ϣ
    /// </summary>
    /// <param name="UserID"></param>
    /// <param name="intID"></param>
    /// <returns>0��ȡ���ʧ�ܣ�1��ȡ��ǳɹ�</returns>
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

    #endregion ��ȡ��Ϣ ReadMessage(int intUserID, int intID)    

    #region ɾ����Ϣ DeleteMessage(int UserID, int intID)
    /// <summary>
    /// ɾ����Ϣ
    /// </summary>
    /// <param name="UserID"></param>
    /// <param name="intID"></param>
    /// <returns>0ɾ�����ʧ�ܣ�1ɾ����ǳɹ�</returns>
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

    #endregion ɾ����Ϣ DeleteMessage(int UserID, int intID)

    #region ��ӹ�ע AddConcern(int intUserID, int ConcernID)
    /// <summary>
    /// ��ӹ�ע
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

    #endregion ��ӹ�ע AddConcern(int intUserID, int ConcernID)

    #region ɾ����ע DeleteConcern(int intUserID, int ConcernID)
    /// <summary>
    /// ɾ����ע
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

    #endregion ɾ����ע DeleteConcern(int intUserID, int ConcernID)

    #region ��¼��֤ LoginAuth(string strUserName, string strPassword)
    /// <summary>
    /// ��¼��֤
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

    #endregion ��¼��֤ LoginAuth(string strUserName, string strPassword)

    #region ͨ��UserID��ȡ��ע�б��ж���ע�ã� ReadConcernList(int intUser)
    /// <summary>
    /// ͨ��UserID��ȡ��ע�б��ж���ע�ã�
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

    #endregion ͨ��UserID��ȡ��ע�б��ж���ע�ã� ReadConcernList(int intUser)

    #region ��ȡ�û���ע�� CountConcern(int intUserID)
    /// <summary>
    /// ��ȡ�û���ע��
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
    #endregion ��ȡ�û���ע�� CountConcern(int intUserID)

    #region ��ȡ�û���˿�� CountFans(int intUserID)
    /// <summary>
    /// ��ȡ�û���˿��
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
    #endregion ��ȡ�û���˿�� CountFans(int intUserID)

    #region �õ���ѯ��¼���� SearchUserListCount(string strProvince, string strCity, string strGender, string strCategory)
    /// <summary>
    /// �õ���ѯ��¼����
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
    #endregion �õ���ѯ��¼���� SearchUserListCount(string strProvince, string strCity, string strGender, string strCategory)

    #region ��ʾ��˿�б� ShowFansList(int intPage, int intUserID)
    /// <summary>
    /// ��ʾ��˿�б�
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
    #endregion ��ʾ��˿�б� ShowFansList(int intPage, int intUserID)

    #region ��ʾ��ע�б� ShowConcernList(int intPage, int intUserID)
    /// <summary>
    /// ��ʾ��ע�б�
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
    #endregion ��ʾ��ע�б� ShowConcernList(int intPage, int intUserID)

    #region �����û��б�(ģ��) SearchUserList(int intPage, string strProvince, string strCity, int intGender, int intCategory)
    /// <summary>
    /// �����û��б�(ģ��)
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
    #endregion �����û��б�(ģ��) SearchUserList(int intPage, string strProvince, string strCity, int intGender, int intCategory)

    #region �����û��б�(��ȷ) SearchUser(string strNickName)
    /// <summary>
    /// �����û��б�(��ȷ)
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
    #endregion �����û��б�(��ȷ) SearchUser(string strNickName)

    #region �û������޸� ModifyPassword(int intUserID, string strUserName, string strPassword, string strNewPassword)
    /// <summary>
    /// �û������޸�
    /// </summary>
    /// <param name="intUserID"></param>
    /// <param name="strUserName"></param>
    /// <param name="strPassword"></param>
    /// <param name="strNewPassword"></param>
    /// <returns>1�ɹ� 0ʧ��</returns>
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
    #endregion �û������޸� ModifyPassword(int intUserID, string strUserName, string strPassword, string strNewPassword)

    #region �û�ע����Ϣ�޸� ModifyUserInfo(int intUserID, string strUserName, string strPassword, string strNickName, bool blSex, DateTime dtBirthDay, string strProvince, string strCity, string strQQ)
    /// <summary>
    /// �û�ע����Ϣ�޸�
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
    /// <returns>1�ɹ� 0ʧ��</returns>
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
    #endregion �û�ע����Ϣ�޸� ModifyUserInfo(int intUserID, string strUserName, string strPassword, string strNickName, bool blSex, DateTime dtBirthDay, string strProvince, string strCity, string strQQ)

    #region ����״̬���� ModifyShootStatus(int intUserID, string strUserName, string strPassword, bool blShootStatus)
    /// <summary>
    /// ����״̬����
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
    #endregion ����״̬���� ModifyShootStatus(int intUserID, string strUserName, string strPassword, bool blShootStatus)

    #region ��ȡ�û���Ϣ��ͼƬ��Ϣ ReadUserInfoAndImg(int intUserID)
    /// <summary>
    /// ��ȡ�û���Ϣ��ͼƬ��Ϣ
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
    #endregion ��ȡ�û���Ϣ��ͼƬ��Ϣ ReadUserInfoAndImg(int intUserID)

    #region ����QQ��Ϣ SendQQMsg(string strQQ, string strMsg)
    /// <summary>
    /// ����QQ��Ϣ
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
    #endregion ����QQ��Ϣ SendQQMsg(string strQQ, string strMsg)

    #region URL���� UrlEncode(string str, string encode)
    /// <summary>
    /// URL����
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
        //����Ҫ������ַ�

        string okChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.";
        System.Text.Encoder encoder = System.Text.Encoding.GetEncoding(encode).GetEncoder();
        char[] c1 = str.ToCharArray();
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //һ���ַ�һ���ַ��ı���

        for (int i = 0; i < c1.Length; i++)
        {
            //����Ҫ����

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

    #region ������������ʱ�� UpdateOnlineTime(int intUserID, string strUserName, string strPassword)
    /// <summary>
    /// ������������ʱ��
    /// </summary>
    /// <param name="intUserID"></param>
    /// <param name="strUserName"></param>
    /// <param name="strPassword"></param>
    /// <returns>1�ɹ� 2ʧ��</returns>
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
    #endregion ������������ʱ�� UpdateOnlineTime(int intUserID, string strUserName, string strPassword)

    #region ��ѯ�û��Ƿ�ΪVIP�û� CheckVIP(int intUserID)
    /// <summary>
    /// ��ѯ�û��Ƿ�ΪVIP�û�
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
    #endregion ��ѯ�û��Ƿ�ΪVIP�û� CheckVIP(int intUserID)

    #region �ύ���� SendPropose(int intUserID, string strContent)
    /// <summary>
    /// ������Ϣ
    /// </summary>
    /// <param name="intUserID">�ռ���ID</param>
    /// <param name="intSendID">������ID</param>
    /// <param name="strSender">�������ǳ�</param>
    /// <param name="strContent">��Ϣ����</param>
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

    #endregion �ύ���� SendPropose(int intUserID, string strContent)

    #region ��¼��֤�������ã�CheckUser(string strUserName, string strPassword)
    /// <summary>
    /// ��¼��֤�������ã�
    /// </summary>
    /// <param name="strUserName"></param>
    /// <param name="strPasswor"></param>
    /// <returns>0û�д��ʺ�,-1�ʺ��������,1��֤�ɹ�</returns>
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
    #endregion ��¼��֤�������ã�CheckUser(string strUserName, string strPassword)

    #region ��ȡ�û�����ʱ�� ReadOnlineTime(int intUserID)
    /// <summary>
    /// ��ȡ�û�����ʱ��
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
    #endregion ��ȡ�û�����ʱ�� ReadOnlineTime(int intUserID)
}
