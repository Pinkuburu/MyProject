using System.Data;
using System.Data.SqlClient;

namespace MaitiamCloud
{
    public class UserManager
    {
        #region 显示用户管理的用户信息 ShowUser(int intAdminID)
        /// <summary>
        /// 显示用户管理的用户信息
        /// </summary>
        /// <param name="intAdminID">AdminID</param>
        /// <returns>[UserName],[Password],[Status],[UserGroup],[Weixin],[QQ],[Email],[CreateTime]</returns>
        public static DataTable ShowUser(int intAdminID)
        {
            SqlParameter[] sp = new SqlParameter[1];

            sp[0] = new SqlParameter("@AdminID", SqlDbType.Int);

            sp[0].Value = intAdminID;

            return SqlHelper.ExecuteDataTable(DBConnection.GetConnString(), CommandType.StoredProcedure, "UAMS_User_ShowUserByAdminID", sp);
        }
        #endregion 显示用户管理的用户信息 ShowUser(int intAdminID)
    }
}