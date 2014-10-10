using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Show_Web
{
    public class Classlibrary
    {
        /// <summary>
        /// 格式化请求
        /// </summary>
        /// <param name="strIndex">请求的索引</param>
        /// <param name="intType">请求的类型0:整型 1:字符串 2:短整型 3:长整型</param>
        /// <returns>返回值</returns>
        public static object GetRequest(string strIndex, int intType)
        {
            HttpContext hc = HttpContext.Current;
            object objOut = hc.Request.QueryString[strIndex];
            if (objOut == null)
            {
                //hc.Response.Redirect("Report.aspx?Parameter=3");
                if (intType == 0)
                    return 0;
                else if (intType == 1)
                    return "";
                else if (intType == 2)
                    return Convert.ToInt16(0);
                else
                    return Convert.ToInt64(0);
            }
            else
            {
                try
                {
                    if (intType == 0)
                        return Convert.ToInt32(objOut.ToString().Trim());
                    else if (intType == 1)
                        return objOut.ToString().Trim();
                    else if (intType == 2)
                        return Convert.ToInt16(objOut.ToString().Trim());
                    else
                        return Convert.ToInt64(objOut.ToString().Trim());
                }
                catch
                {
                    //hc.Response.Redirect("Report.aspx?Parameter=3");
                    if (intType == 0)
                        return 0;
                    else if (intType == 1)
                        return "";
                    else if (intType == 2)
                        return Convert.ToInt16(0);
                    else
                        return Convert.ToInt64(0);
                }
            }
        }

        public static bool HasNickName(string strNickName)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("NickName", SqlDbType.VarChar) };
            commandParameters[0].Value = strNickName;
            return Convert.ToBoolean(SqlHelper.ExecuteScalar(SqlLibrary.GetServer_Main(), CommandType.StoredProcedure, "HasNickName", commandParameters));
        }

        public static bool HasUserName(string strUserName)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("UserName", SqlDbType.VarChar) };
            commandParameters[0].Value = strUserName;
            return Convert.ToBoolean(SqlHelper.ExecuteScalar(SqlLibrary.GetServer_Main(), CommandType.StoredProcedure, "HasUserName", commandParameters));
        }

        public static SqlDataReader RegisterUser(string strUserName, string strPassword, string strNickName, int intSex, string strBirthDay, string strPro, string strCity, string strQQ)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("UserName", SqlDbType.VarChar), new SqlParameter("Password", SqlDbType.VarChar), new SqlParameter("NickName", SqlDbType.VarChar), new SqlParameter("Sex", SqlDbType.Bit), new SqlParameter("BirthDay", SqlDbType.DateTime), new SqlParameter("Province", SqlDbType.VarChar), new SqlParameter("City", SqlDbType.VarChar), new SqlParameter("QQ", SqlDbType.VarChar) };
            commandParameters[0].Value = strUserName;
            commandParameters[1].Value = strPassword;
            commandParameters[2].Value = strNickName;
            commandParameters[3].Value = intSex;
            commandParameters[4].Value = strBirthDay;
            commandParameters[5].Value = strPro;
            commandParameters[6].Value = strCity;
            commandParameters[7].Value = strQQ;
            return SqlHelper.ExecuteReader(SqlLibrary.GetServer_Main(), CommandType.StoredProcedure, "RegisterUser", commandParameters);
        }

        public static DataRow GetUserRowByUserNamePWD(string strUserName, string strPassword)
        {
            SqlParameter[] commandParameters = new SqlParameter[] { new SqlParameter("UserName", SqlDbType.VarChar), new SqlParameter("Password", SqlDbType.VarChar) };
            commandParameters[0].Value = strUserName;
            commandParameters[1].Value = strPassword;
            return SqlHelper.ExecuteDataRow(SqlLibrary.GetServer_Main(), CommandType.StoredProcedure, "GetUserRowByUserNamePWD", commandParameters);
        }

        public static DataRow GetMainParameterRow()
        {
            return SqlHelper.ExecuteDataRow(SqlLibrary.GetServer_Main(), CommandType.StoredProcedure, "GetMainParameterRow");
        }

        
    }
}
