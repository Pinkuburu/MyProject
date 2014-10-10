using System;
using System.Collections.Generic;
using System.Text;


public class SqlLibrary
{
    public static bool IsOnline = false;

    /// <summary>
    /// 连接数据库方法
    /// </summary>
    /// <returns></returns>
    public static string GetBo_Main()
    {
        if (IsOnline)
            return @"Data Source=222.73.57.140,2149\SQL2005;Initial Catalog=Magic_Farm;Persist Security Info=True;User ID=BO_Admin;Password=loveemma++";
        else
            return @"Data Source=127.0.0.1,2149\SQL2005;Initial Catalog=Fx_Main;Persist Security Info=True;User ID=Fx_Admin;Password=qweqwe123";
    }
}
