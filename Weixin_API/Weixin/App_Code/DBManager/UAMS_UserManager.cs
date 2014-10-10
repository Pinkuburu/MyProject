using System;
using System.Data;
using Weixin.App_Code.DataBase;
using System.Data.SqlClient;

namespace Weixin.App_Code.DBManager
{
    public class UAMS_UserManager
    {
        #region 用户关注后，添加用户名到表中 AddUser(string strUserName)
        /// <summary>
        /// 用户关注后，添加用户名到表中
        /// </summary>
        /// <param name="strUserName">用户名</param>
        /// <returns>0用户添加成功|1用户已存在</returns>
        public static bool AddUser(string strUserName)
        {
            SqlParameter[] sp = new SqlParameter[1];

            sp[0] = new SqlParameter("@UserName", SqlDbType.VarChar);
            sp[0].Value = strUserName;

            return Convert.ToBoolean(SqlHelper.ExecuteScalar(DBConnection.GetConnString(), CommandType.StoredProcedure, "UAMS_AddUser", sp));
        }
        #endregion 用户关注后，添加用户名到表中 AddUser(string strUserName)

        #region 获取Token GetToken(string strUserName)
        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="strUserName">用户名</param>
        /// <returns>Token</returns>
        public static string GetToken(string strUserName)
        {
            SqlParameter[] sp = new SqlParameter[1];

            sp[0] = new SqlParameter("@UserName", SqlDbType.VarChar);
            sp[0].Value = strUserName;

            return Convert.ToString(SqlHelper.ExecuteScalar(DBConnection.GetConnString(), CommandType.StoredProcedure, "UAMS_GetToken", sp));
        }
        #endregion 获取Token GetToken(string strUserName)

        #region 添加Token AddToken(string strUserName, string strToken)
        /// <summary>
        /// 添加Token
        /// </summary>
        /// <param name="strUserName">用户名</param>
        /// <param name="strToken">Token值</param>
        public static void AddToken(string strUserName, string strToken)
        {
            SqlParameter[] sp = new SqlParameter[2];

            sp[0] = new SqlParameter("@UserName", SqlDbType.VarChar, 50);
            sp[1] = new SqlParameter("@Token", SqlDbType.VarChar, 8);

            sp[0].Value = strUserName;
            sp[1].Value = strToken;

            SqlHelper.ExecuteNonQuery(DBConnection.GetConnString(), CommandType.StoredProcedure, "UAMS_AddToken", sp);
        }
        #endregion 添加Token AddToken(string strUserName, string strToken)

        #region 激活码使用 CheckToken(string strUserName, string strCode)
        /// <summary>
        /// 激活码使用
        /// </summary>
        /// <param name="strUserName">用户名</param>
        /// <param name="strCode">Token值</param>
        /// <returns>1激活码正常并且已被标记激活|2激活码无效|3激活码已经过期</returns>
        public static int CheckToken(string strUserName, string strCode)
        {
            SqlParameter[] sp = new SqlParameter[2];

            sp[0] = new SqlParameter("@UserName", SqlDbType.VarChar, 50);
            sp[1] = new SqlParameter("@Token", SqlDbType.VarChar, 8);
            sp[0].Value = strUserName;
            sp[1].Value = strCode;

            return (int)SqlHelper.ExecuteScalar(DBConnection.GetConnString(), CommandType.StoredProcedure, "UAMS_GetToken", sp);
        }
        #endregion 激活码使用 CheckToken(string strUserName, string strCode)
    }
}