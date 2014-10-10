using System;
using System.Collections.Generic;
using System.Web;

namespace MaitiamCloud
{
    public class DBConnection
    {
        public static string GetConnString()
        {
            return @"Data Source=ishow.xba.com.cn,1595;Initial Catalog=Maitiam_UAMS;Persist Security Info=True;User ID=cupid;Password=qweqwe123";
        }
    }
}