using System.Data;
using System.Data.SqlClient;

namespace MaitiamCloud
{
    public class LoginManager
    {
        #region 用户登录 Login(string strUsername, string strPassword)
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="strUsername">用户名</param>
        /// <param name="strPassword">密码</param>
        /// <returns>AdminID</returns>
        public static int Login(string strUsername, string strPassword)
        {
            SqlParameter[] sp = new SqlParameter[2];

            sp[0] = new SqlParameter("@UserName", SqlDbType.VarChar, 50);
            sp[1] = new SqlParameter("@Password", SqlDbType.VarChar, 50);

            sp[0].Value = strUsername;
            sp[1].Value = strPassword;

            return (int)SqlHelper.ExecuteScalar(DBConnection.GetConnString(), CommandType.StoredProcedure, "UAMS_Login", sp);
        }
        #endregion 用户登录 Login(string strUsername, string strPassword)
    }
}