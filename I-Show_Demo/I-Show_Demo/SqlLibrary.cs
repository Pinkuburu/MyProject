using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace I_Show_Demo
{
    public class SqlLibrary
    {
        public static bool IsOnline = true;

        /// <summary>
        /// 连接数据库方法
        /// </summary>
        /// <returns></returns>
        public static string GetServer_Main()
        {
            if (IsOnline)
                return @"Data Source=222.73.165.106,1595\SQL2005;Initial Catalog=xintest;Persist Security Info=True;User ID=Cupid;Password=qweqwe123";
            else
                return @"Data Source=127.0.0.1,2149\SQL2005;Initial Catalog=Fx_Main;Persist Security Info=True;User ID=LW_Admin;Password=qweqwe123";
        }
    }
}
