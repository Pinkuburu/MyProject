using System;
using System.Collections.Generic;
using System.Text;
using System.DirectoryServices;

namespace 添加删除修改windows用户测试
{
    class Program
    {
        static void Main(string[] args)
        {
            UserAndGroupHelper.RemoveUser("Cupid");
            Console.ReadKey();
        }
    }
}
