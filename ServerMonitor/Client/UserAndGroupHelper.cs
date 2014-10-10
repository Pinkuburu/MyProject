using System;
using System.Collections.Generic;
using System.Text;
using System.DirectoryServices;
using System.IO;
using System.Security.AccessControl;

namespace Client
{
    /// <summary>
    /// 计算机用户和组操作类
    /// </summary>
    public class UserAndGroupHelper
    {
        /// <summary> 
        /// 目录权限 
        /// </summary> 
        public enum FloderRights
        {
            FullControl,
            Read,
            Write
        } 

        private static readonly string PATH = "WinNT://" + Environment.MachineName;
        /// <summary>
        /// 添加windows用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="group">所属组</param>
        /// <param name="description">描述</param>
        public static void AddUser(string username, string password, string group, string description)
        {
            using (DirectoryEntry dir = new DirectoryEntry(PATH))
            {
                using (DirectoryEntry user = dir.Children.Add(username, "User")) //增加用户名
                {
                    user.Properties["FullName"].Add(username); //用户全称
                    user.Invoke("SetPassword", password); //用户密码
                    user.Invoke("Put", "Description", description);//用户详细描述
                    //user.Invoke("Put","PasswordExpired",1); //用户下次登录需更改密码
                    user.Invoke("Put", "UserFlags", 66049); //密码永不过期
                    //user.Invoke("Put", "UserFlags", 0x0040);//用户不能更改密码s
                    user.CommitChanges();//保存用户
                    using (DirectoryEntry grp = dir.Children.Find(group, "group"))
                    {
                        if (grp.Name != "")
                        {
                            grp.Invoke("Add", user.Path.ToString());//将用户添加到某组
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 更改windows用户密码
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="newpassword">新密码</param>
        public static string UpdateUserPassword(string username, string newpassword)
        {
            using (DirectoryEntry dir = new DirectoryEntry(PATH))
            {
                try
                {
                    using (DirectoryEntry user = dir.Children.Find(username, "user"))
                    {
                        user.Invoke("SetPassword", new object[] { newpassword });
                        user.CommitChanges();
                    }
                    Log.WriteLog(LogFile.Trace, "ChangeUserPassword: " + username + "|" + newpassword);
                    return "OK";
                }
                catch(Exception ex)
                {
                    Log.WriteLog(LogFile.Trace, ex.Message.Replace("\r\n", ""));
                    return "Error";
                }                
            }
        }
        /// <summary>
        /// 删除windows用户
        /// </summary>
        /// <param name="username">用户名</param>
        public static void RemoveUser(string username)
        {
            using (DirectoryEntry dir = new DirectoryEntry(PATH))
            {
                using (DirectoryEntry user = dir.Children.Find(username, "User"))
                {
                    dir.Children.Remove(user);
                }
            }
        }
        /// <summary>
        /// 添加windows用户组
        /// </summary>
        /// <param name="groupName">组名称</param>
        /// <param name="description">描述</param>
        public static void AddGroup(string groupName, string description)
        {
            using (DirectoryEntry dir = new DirectoryEntry(PATH))
            {
                using (DirectoryEntry group = dir.Children.Add(groupName, "group"))
                {
                    group.Invoke("Put", new object[] { "Description", description });
                    group.CommitChanges();
                }
            }
        }
        /// <summary>
        /// 删除windows用户组
        /// </summary>
        /// <param name="groupName">组名称</param>
        public static void RemoveGroup(string groupName)
        {
            using (DirectoryEntry dir = new DirectoryEntry(PATH))
            {
                using (DirectoryEntry group = dir.Children.Find(groupName, "Group"))
                {
                    dir.Children.Remove(group);
                }
            }
        }

        /// <summary>
        /// 判断Windows用户是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static bool ExistWinUser(string username)
        {
            try
            {
                using (DirectoryEntry dir = new DirectoryEntry(PATH))
                {
                    //删除存在用户
                    var delUser = dir.Children.Find(username, "user");
                    return delUser != null;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 启用/禁用windows帐户
        /// </summary>
        /// <param name="username"></param>
        /// <param name="isDisable"></param>
        public static void DisableUser(string username, bool isDisable)
        {
            var userDn = "WinNT://" + Environment.MachineName + "/" + username + ",user";
            DirectoryEntry user = new DirectoryEntry(userDn);
            user.InvokeSet("AccountDisabled", isDisable);
            user.CommitChanges();
            user.Close();
        }

        /// <summary> 
        /// 给目录添加用户和权限 
        /// </summary> 
        /// <param name="pathname"></param> 
        /// <param name="username"></param> 
        /// <param name="qx"></param> 
        public static void AddPathRule(string pathname, string username, FloderRights qx)
        {
            DirectoryInfo dirinfo = new DirectoryInfo(pathname);
            if ((dirinfo.Attributes & FileAttributes.ReadOnly) != 0)
            {
                dirinfo.Attributes = FileAttributes.Normal;
            }
            //取得访问控制列表 
            DirectorySecurity dirsecurity = dirinfo.GetAccessControl();
            // string strDomain = Dns.GetHostName(); 
            switch (qx)
            {
                case FloderRights.FullControl:
                    dirsecurity.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.FullControl, AccessControlType.Allow));
                    break;
                case FloderRights.Read:
                    dirsecurity.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.Read, AccessControlType.Allow));
                    break;
                case FloderRights.Write:
                    dirsecurity.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.Write, AccessControlType.Allow));
                    break;
                default:
                    dirsecurity.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.FullControl, AccessControlType.Deny));
                    break;
            }

            dirinfo.SetAccessControl(dirsecurity);

            //取消目录从父继承 
            DirectorySecurity dirSecurity = System.IO.Directory.GetAccessControl(pathname);
            dirSecurity.SetAccessRuleProtection(true, false);
            System.IO.Directory.SetAccessControl(pathname, dirSecurity);

            //AccessControlType.Allow允许访问受保护对象//Deny拒绝访问受保护对象 
            //FullControl、Read 和 Write 完全控制,读,写 
            //FileSystemRights.Write写入//Delete删除 //DeleteSubdirectoriesAndFiles删除文件夹和文件//ListDirectory读取 
            //Modify读写删除-修改//只读打开文件和复制// 
        } 

    }
}
