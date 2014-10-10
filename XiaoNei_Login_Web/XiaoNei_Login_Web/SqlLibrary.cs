using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using FxLibrary;

public class SqlLibrary
{
    public static bool IsOnline = false;
    //public static string strKeyWords = "";

    public static string GetFx_Main()
    {
        if (IsOnline)
            return @"Data Source=222.73.57.140,2149\SQL2005;Initial Catalog=Fx_Main;Persist Security Info=True;User ID=Fx_Admin;Password=qweqwe123";
        else
            return @"Data Source=127.0.0.1,2149\SQL2005;Initial Catalog=Fx_Main;Persist Security Info=True;User ID=Fx_Admin;Password=qweqwe123";
    }

    #region ����û�
    /// <summary>
    /// ����û�
    /// </summary>
    /// <param name="intSID"></param>
    /// <returns></returns>
    public static int Fx_AddNewUser(int intSID)
    {
        SqlParameter[] sp = new SqlParameter[1];
        sp[0] = new SqlParameter("@SID", SqlDbType.Int);

        sp[0].Value = intSID;
        return SqlHelper.ExecuteIntDataField(GetFx_Main(), CommandType.StoredProcedure, "Fx_AddNewUser", sp);
    }
    #endregion

    #region �����Ϣ
    /// <summary>
    /// �����Ϣ
    /// </summary>
    /// <param name="intSID"></param>
    /// <param name="strSMSContent"></param>
    /// <returns></returns>
    public static int Fx_SaveMessage(int intSID, string strSMSContent)
    {
        SqlParameter[] sp = new SqlParameter[2];
        sp[0] = new SqlParameter("@SID", SqlDbType.Int);
        sp[1] = new SqlParameter("@SMSContent", SqlDbType.VarChar, 200);

        sp[0].Value = intSID;
        sp[1].Value = strSMSContent;
        return SqlHelper.ExecuteIntDataField(GetFx_Main(), CommandType.StoredProcedure, "Fx_SaveMessage", sp);
    }
    #endregion

    #region ��Ƿ�����Ϣ
    /// <summary>
    /// ��Ƿ�����Ϣ
    /// </summary>
    /// <param name="intID"></param>
    public static int Fx_UpdateMessage(int intID)
    {
        SqlParameter[] sp = new SqlParameter[1];
        sp[0] = new SqlParameter("@ID", SqlDbType.Int);

        sp[0].Value = intID;
        return SqlHelper.ExecuteIntDataField(GetFx_Main(), CommandType.StoredProcedure, "Fx_UpdateMessage", sp);
    }
    #endregion

    #region ��ӷ�����Ԥ����¼
    /// <summary>
    /// ��ӷ�����Ԥ����¼
    /// </summary>
    /// <param name="ServerName"></param>
    /// <param name="ServerStatus"></param>
    public static void Fx_AddServerRec(string ServerName, string ServerStatus)
    {
        SqlParameter[] sp = new SqlParameter[2];
        sp[0] = new SqlParameter("@ServerName", SqlDbType.VarChar,50);
        sp[1] = new SqlParameter("@ServerStatus", SqlDbType.VarChar,50);

        sp[0].Value = ServerName;
        sp[1].Value = ServerStatus;
        SqlHelper.ExecuteNonQuery(GetFx_Main(), CommandType.StoredProcedure, "Fx_AddServerRec", sp);
    }
    #endregion

    #region ���¼ƻ�����ʱ��
    /// <summary>
    /// ���¼ƻ�����ʱ��
    /// </summary>
    /// <param name="intID"></param>
    /// <returns></returns>
    public static int Fx_UpdateTaskRuntime(int intID)
    {
        SqlParameter[] sp = new SqlParameter[1];
        sp[0] = new SqlParameter("@ID", SqlDbType.Int);

        sp[0].Value = intID;
        return SqlHelper.ExecuteIntDataField(GetFx_Main(), CommandType.StoredProcedure, "Fx_UpdateTaskRuntime", sp);
    }
    #endregion

}

