using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace 备份机文件删除
{
    class Program
    {
        static void Main(string[] args)
        {
            DeleteBackupServerFile();
            Console.ReadKey();
        }

        #region 删除备份机备份文件 DeleteBackupServerFile()
        /// <summary>
        /// DeleteBackupServerFile()
        /// </summary>
        private static void DeleteBackupServerFile()
        {
            string[] dirs = Directory.GetDirectories(@"D:\");
            DateTime dt = DateTime.Now;
            foreach (string dir in dirs)
            {
                DirectoryInfo dir2 = new DirectoryInfo(dir);
                Console.WriteLine(dir + "      " + dir2.LastWriteTime);
                if (dir2.Name != "System Volume Information")
                {
                    string[] arrFiles = Directory.GetFiles(dir);
                    foreach (string strFile in arrFiles)
                    {
                        DirectoryInfo dir3 = new DirectoryInfo(strFile);
                        if (dir3.LastWriteTime < dt.AddDays(-20))
                        {
                            if (dir3.Extension == ".rar")
                            {
                                Console.WriteLine(strFile + "      开始删除");
                            }                            
                            //try
                            //{
                            //    FileDirectoryUtility.DeleteDirectory(dir1);
                            //}
                            //catch (Exception ex)
                            //{
                            //    Console.WriteLine(ex);
                            //}
                        }
                    }
                }                
            }
        }
        #endregion 删除备份机备份文件 DeleteBackupServerFile()
    }
}
