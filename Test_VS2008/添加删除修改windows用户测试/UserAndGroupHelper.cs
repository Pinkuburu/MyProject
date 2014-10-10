using System;
using System.DirectoryServices;

namespace 添加删除修改windows用户测试
{
    /// <summary>
    /// 计算机用户和组操作类
    /// </summary>
    public class UserAndGroupHelper
    {
        private static readonly string PATH = "WinNT://" + Environment.MachineName;

        #region 添加windows用户
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
        #endregion 添加windows用户

        #region 更改windows用户密码
        /// <summary>
        /// 更改windows用户密码
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="newpassword">新密码</param>
        public static void UpdateUserPassword(string username, string newpassword)
        {
            using (DirectoryEntry dir = new DirectoryEntry(PATH))
            {
                using (DirectoryEntry user = dir.Children.Find(username, "user"))
                {
                    user.Invoke("SetPassword", new object[] { newpassword });
                    user.CommitChanges();
                }
            }
        }
        #endregion 更改windows用户密码

        #region 删除windows用户
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
        #endregion 删除windows用户

        #region 添加windows用户组
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
        #endregion 添加windows用户组

        #region 删除windows用户组
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
        #endregion 删除windows用户组
    }
}
