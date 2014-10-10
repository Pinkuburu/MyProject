using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace XiaoNei_Login_Web
{
    public class DataBaseitem
    {
        public static bool IsOnline = false;

        public static string GetLoginConn()
        {
            if (IsOnline)
            {
                return @"Data Source=222.73.57.140,2149\SQL2005;Initial Catalog=XiaoNei_Login;Persist Security Info=True;User ID=XN_Admin;Password=qweqwe123";
            }
            else
            {
                return @"Data Source=127.0.0.1,2149\SQL2005;Initial Catalog=XiaoNei_Login;Persist Security Info=True;User ID=XN_Admin;Password=qweqwe123";
            }
        }
    }
}
