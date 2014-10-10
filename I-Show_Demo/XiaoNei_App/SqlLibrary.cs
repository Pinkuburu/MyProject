using System;
using System.Data;
using System.Data.SqlClient;

namespace XiaoNei_App
{
    public class SqlLibrary
    {
        public static bool IsOnline = true;

        public static string GetXn_Main()
        {
            if (IsOnline)
                return @"Data Source=61.147.125.218,1595;Initial Catalog=TeamShow;Persist Security Info=True;User ID=Cupid;Password=qweqwe123";
            else
                return @"Data Source=127.0.0.1,1595;Initial Catalog=TeamShow;Persist Security Info=True;User ID=Cupid;Password=qweqwe123";
        }

        #region 添加新用户 Xn_AddNewUser(int intUid, string strPassword, string strName, bool blSex, DateTime dtBirthday, string strProvince, string strCity, string strTinyurl, string strHeadurl, DateTime dtLastActiveTime)
        /// <summary>
        /// 添加新用户
        /// </summary>
        /// <param name="intUid">校内ID</param>
        /// <param name="strPassword">用户自定义密码</param>
        /// <param name="strName">昵称</param>
        /// <param name="blSex">性别</param>
        /// <param name="dtBirthday">生日</param>
        /// <param name="strProvince">省</param>
        /// <param name="strCity">市</param>
        /// <param name="strTinyurl">头像小图标链接</param>
        /// <param name="strHeadurl">头像大图标链接</param>
        /// <param name="dtLastActiveTime">最后登录时间</param>
        /// <returns>0,失败 1成功</returns>
        public static int Xn_AddNewUser(int intUid, string strPassword, string strName, int intSex, DateTime dtBirthday, string strProvince, string strCity, string strTinyurl, string strHeadurl, DateTime dtLastActiveTime)
        {
            //@Uid int,
            //@Password nchar(20),
            //@Name nchar(20),
            //@Sex bit,
            //@Birthday datetime,
            //@Province nchar(20),
            //@City nchar(20),
            //@Tinyurl nchar(100),
            //@Headurl nchar(100),
            //@LastActiveTime datetime

            SqlParameter[] sp = new SqlParameter[10];
            sp[0] = new SqlParameter("@Uid", SqlDbType.Int);
            sp[1] = new SqlParameter("@Password", SqlDbType.NChar, 20);
            sp[2] = new SqlParameter("@Name", SqlDbType.NChar, 20);
            sp[3] = new SqlParameter("@Sex", SqlDbType.Bit);
            sp[4] = new SqlParameter("@Birthday", SqlDbType.DateTime);
            sp[5] = new SqlParameter("@Province", SqlDbType.NChar, 20);
            sp[6] = new SqlParameter("@City", SqlDbType.NChar, 20);
            sp[7] = new SqlParameter("@Tinyurl", SqlDbType.NChar, 100);
            sp[8] = new SqlParameter("@Headurl", SqlDbType.NChar, 100);
            sp[9] = new SqlParameter("@LastActiveTime", SqlDbType.DateTime);

            sp[0].Value = intUid;
            sp[1].Value = strPassword;
            sp[2].Value = strName;
            sp[3].Value = intSex;
            sp[4].Value = dtBirthday;
            sp[5].Value = strProvince;
            sp[6].Value = strCity;
            sp[7].Value = strTinyurl;
            sp[8].Value = strHeadurl;
            sp[9].Value = dtLastActiveTime;

            return SqlHelper.ExecuteIntDataField(GetXn_Main(), CommandType.StoredProcedure, "Xn_AddNewUser", sp);
        }

        #endregion 添加新用户 Xn_AddNewUser(int intUid, string strPassword, string strName, bool blSex, DateTime dtBirthday, string strProvince, string strCity, string strTinyurl, string strHeadurl, DateTime dtLastActiveTime)
    }
}
