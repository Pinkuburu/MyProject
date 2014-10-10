using System;
using System.Collections.Generic;
using System.Text;


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
            return @"Data Source=222.73.57.140,2149\SQL2005;Initial Catalog=LittleWar_Main;Persist Security Info=True;User ID=LW_Admin;Password=loveemma++";
        else
            return @"Data Source=127.0.0.1,2149\SQL2005;Initial Catalog=Fx_Main;Persist Security Info=True;User ID=LW_Admin;Password=qweqwe123";
    }
}
