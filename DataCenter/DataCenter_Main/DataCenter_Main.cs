using System;
using System.IO;
using System.Net;
using servermanager;
using System.Data.SqlClient;
using System.Data;

namespace DataCenter_Main
{
    class DataCenter_Main
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)  //有参数调用
            {
                if (args[0].ToLower().ToString() == "del")
                {
                    try
                    {
                        Fx_DelServerInfo(ServerIP(), 1);
                        Console.WriteLine(ServerIP() + " 删除成功！");
                    }
                    catch
                    {
                        Console.WriteLine("删除失败！");
                        Console.ReadKey();
                    }
                }
                //Console.WriteLine("有参数调用");
                //Console.ReadKey();
            }
            else  //无参数调用
            {
                //Console.WriteLine("无参数调用");
                Console.WriteLine(ServerName());
                Console.WriteLine(ServerIP());
                Console.WriteLine(CheckDisk());
                //Console.WriteLine(GetDataCenterConnString(1));
                Console.WriteLine(ServerArea());
                int intOutput = 0;
                try
                {
                    intOutput = Fx_AddServerInfo(ServerName(), ServerIP(), ServerArea(), CheckDisk(), 1, 1);
                    if (intOutput == 1)
                    {
                        Console.WriteLine("添加信息成功！");
                    }
                    else
                    {
                        Console.WriteLine("更新信息成功！");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                //Console.ReadKey();
            }
        }

        #region 获服务器磁盘空间信息 CheckDisk()
        /// <summary>
        /// 获服务器磁盘空间信息
        /// </summary>
        /// <returns></returns>
        private static string CheckDisk()
        {
            string result = "";
            //检测磁盘空间
            DriveInfo[] MyDrives = DriveInfo.GetDrives();

            foreach (DriveInfo MyDrive in MyDrives)
            {
                if (MyDrive.DriveType == DriveType.Fixed)
                {
                    //result += MyDrive.VolumeLabel + "----" + MyDrive.TotalSize / 1024 / 1024 + "----" + MyDrive.TotalFreeSpace / 1024 / 1024 + "----" + MyDrive.TotalFreeSpace * 100 / MyDrive.TotalSize + "|";
                    result += MyDrive.Name + "----" + MyDrive.VolumeLabel + "----" + MyDrive.TotalSize / 1024 / 1024 + "----" + MyDrive.TotalFreeSpace / 1024 / 1024 + "----" + MyDrive.TotalFreeSpace * 100 / MyDrive.TotalSize + "|";
                }
            }
            return result.Substring(0, result.Length - 1);
        }
        #endregion 获服务器磁盘空间信息

        #region 获取服务器IP信息 ServerIP()
        /// <summary>
        /// 获取服务器IP信息
        /// </summary>
        /// <returns></returns>
        private static string ServerIP()
        {
            IPAddress[] ServerIP = Dns.GetHostByName(Dns.GetHostName()).AddressList;
            return ServerIP[0].ToString();
        }
        #endregion 获取服务器IP信息

        #region 获取服务器名称 ServerName()
        /// <summary>
        /// 获取服务器名称
        /// </summary>
        /// <returns></returns>
        private static string ServerName()
        {
            return ServerSelector.strCopartner;
        }
        #endregion 获取服务器名称

        #region 获取服务器所在区 ServerArea()
        /// <summary>
        /// 获取服务器所在区
        /// </summary>
        /// <returns></returns>
        private static string ServerArea()
        {
            return ServerSelector.strCopartner;
        }
        #endregion 获取服务器所在区

        #region 得到数据中心数据库连接字符串 GetDataCenterConnString(int intServerCategory)
        /// <summary>
        /// 得到数据中心数据库连接字符串
        /// </summary>
        /// <param name="intServerCategory"></param>
        /// <returns></returns>
        private static string GetDataCenterConnString(int intServerCategory)
        {
            string strReturn = "";
            switch (intServerCategory)
            {
                case 1://数据中心服务器
                    strReturn = @"Data Source=222.73.57.140,2149\SQL2005;Initial Catalog=Fx_Main;Persist Security Info=True;User ID=Fx_Admin;Password=qweqwe123";
                    break;
                case 2://测试服务器
                    strReturn = @"Data Source=192.168.1.30,2149;Initial Catalog=Fx_Main;Persist Security Info=True;User ID=Fx_Admin;Password=qweqwe123";
                    break;
                default:
                    break;
            }
            return strReturn;
        }
        #endregion 得到数据中心数据库连接字符串

        #region 添加\更新 Fx_AddServerInfo(string ServerName, string IP, string Content, int Category, int intIsOnline)
        /// <summary>
        /// 添加\更新
        /// </summary>
        /// <param name="ServerName">服务器名称</param>
        /// <param name="IP">服务器IP</param>
        /// <param name="Content">信息内容</param>
        /// <param name="Category">分类 1、硬盘信息</param>
        /// <param name="intIsOnline">是不调用测试数据库</param>
        /// <returns>0、更新信息 1、添加信息</returns>
        private static int Fx_AddServerInfo(string ServerName, string IP, string Area, string Content, int Category, int intIsOnline)
        {
            SqlParameter[] sp = new SqlParameter[5];
            sp[0] = new SqlParameter("@ServerName", SqlDbType.VarChar, 50);
            sp[1] = new SqlParameter("@IP", SqlDbType.VarChar, 15);
            sp[2] = new SqlParameter("@Area", SqlDbType.VarChar, 15);
            sp[3] = new SqlParameter("@Content", SqlDbType.VarChar, 400);
            sp[4] = new SqlParameter("@Category", SqlDbType.Int);

            sp[0].Value = ServerName;
            sp[1].Value = IP;
            sp[2].Value = Area;
            sp[3].Value = Content;
            sp[4].Value = Category;
            return SqlHelper.ExecuteIntDataField(GetDataCenterConnString(intIsOnline), CommandType.StoredProcedure, "Fx_AddServerInfo", sp);
        }
        #endregion 添加&更新

        #region 删除信息 Fx_DelServerInfo(string IP, int intIsOnline)
        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="IP">服务器IP</param>
        /// <param name="intIsOnline">是不调用测试数据库</param>
        /// <returns>0、未找到要删除的信息，1、删除成功</returns>
        private static int Fx_DelServerInfo(string IP, int intIsOnline)
        {
            SqlParameter[] sp = new SqlParameter[1];
            sp[0] = new SqlParameter("@IP", SqlDbType.VarChar, 15);

            sp[0].Value = IP;
            return SqlHelper.ExecuteIntDataField(GetDataCenterConnString(intIsOnline), CommandType.StoredProcedure, "Fx_DelServerInfo", sp);
        }
        #endregion 删除信息
    }
}
