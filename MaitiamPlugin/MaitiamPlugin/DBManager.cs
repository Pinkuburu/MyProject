using System;
using System.Data;
using System.Data.SqlClient;

namespace MaitiamPlugin
{
    public class DBManager
    {
        /// <summary>
        /// 添加Token
        /// </summary>
        /// <param name="strUserName">用户名</param>
        public static void UAMS_AddToken(string strUserName)
        {
            SqlParameter[] sp = new SqlParameter[2];

            sp[0] = new SqlParameter("@UserName", SqlDbType.VarChar, 50);
            sp[1] = new SqlParameter("@Token", SqlDbType.VarChar, 50);

            sp[0].Value = strUserName;
            sp[1].Value = rndToken();

            SqlHelper.ExecuteNonQuery(DBConnection.GetConnString(), CommandType.StoredProcedure, "UAMS_AddToken", sp);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="strUserName">用户名</param>
        /// <returns>0添加用户，1用户已存在</returns>
        public static int UAMS_AddUser(string strUserName)
        {
            SqlParameter[] sp = new SqlParameter[1];

            sp[0] = new SqlParameter("@UserName", SqlDbType.VarChar, 50);

            sp[0].Value = strUserName;

            return (int)SqlHelper.ExecuteScalar(DBConnection.GetConnString(), CommandType.StoredProcedure, "UAMS_AddUser", sp);
        }

        /// <summary>
        /// 激活码使用
        /// </summary>
        /// <param name="strUserName">用户名</param>
        /// <param name="strToken">Token</param>
        /// <returns>1激活码正常并且已被标记激活，2激活码无效，3激活码已经过期</returns>
        public static int UAMS_GetToken(string strUserName, string strToken)
        {
            SqlParameter[] sp = new SqlParameter[2];

            sp[0] = new SqlParameter("@UserName", SqlDbType.VarChar, 50);
            sp[1] = new SqlParameter("@Token", SqlDbType.VarChar, 50);

            sp[0].Value = strUserName;
            sp[1].Value = strToken;

            return (int)SqlHelper.ExecuteScalar(DBConnection.GetConnString(), CommandType.StoredProcedure, "UAMS_GetToken", sp);
        }

        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="strUserName">用户名</param>
        /// <param name="strPassword">密码</param>
        /// <returns>返回一行数据</returns>
        public static DataRow UAMS_CheckUser(string strUserName, string strPassword)
        {
            SqlParameter[] sp = new SqlParameter[2];

            sp[0] = new SqlParameter("@UserName", SqlDbType.VarChar, 50);
            sp[1] = new SqlParameter("@Password", SqlDbType.VarChar, 50);

            sp[0].Value = strUserName;
            sp[1].Value = strPassword;

            return SqlHelper.ExecuteDataRow(DBConnection.GetConnString(), CommandType.StoredProcedure, "UAMS_CheckUser", sp);
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns>6位随机数</returns>
        private static string rndToken()
        {
            Random rnd = new Random();
            string strToken = rnd.Next(000000, 999999).ToString();
            return strToken;
        }
    }
}
