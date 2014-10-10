using System;
using System.IO;

namespace AutoUpdate
{
    class AutoUnRAR
    {
        MainClass mc = new MainClass();

        #region 更新包解压缩 UnRAR(string srcFullPath, string desFullPath)
        /// <summary>
        /// 更新包解压缩
        /// </summary>
        /// <param name="srcFullPath">更新包源地址</param>
        /// <param name="desFullPath">更新包解压目标地址</param>
        /// <returns>返回ERRORLEVEL值</returns>
        public int UnRAR(string srcFullPath, string desFullPath)
        {
            //0 成功操作。 
            //1 警告。发生非致命错误。 
            //2 发生致命错误。 
            //3 解压时发生 CRC 错误。 
            //4 尝试修改一个 锁定的压缩文件。  
            //5 写错误。 
            //6 文件打开错误。 
            //7 错误命令行选项。 
            //8 内存不足。 
            //9 文件创建错误。 
            //255 用户中断。 

            int errorLevel = -1;

            if (desFullPath.IndexOf("\\", desFullPath.Length - 1) < 0)
            {
                desFullPath = desFullPath + "\\";
            }

            Console.WriteLine("【正在解压文件】" + srcFullPath + " To: " + desFullPath);
            mc.LogTrace("【正在解压文件】" + srcFullPath + " To: " + desFullPath);
            errorLevel = mc.CMD("c:\\rar x -y " + srcFullPath + " " + desFullPath);

            switch (errorLevel)
            {
                case 0:
                    Console.WriteLine("【RAR-成功操作】" + desFullPath);
                    mc.LogTrace("【RAR-成功操作】" + desFullPath);
                    break;
                case 1:
                    Console.WriteLine("【RAR-警告!发生非致命错误】");
                    mc.LogTrace("【RAR-警告!发生非致命错误】");
                    break;
                case 2:
                    Console.WriteLine("【RAR-发生致命错误】");
                    mc.LogTrace("【RAR-发生致命错误】");
                    break;
                case 3:
                    Console.WriteLine("【RAR-解压时发生CRC错误】");
                    mc.LogTrace("【RAR-解压时发生CRC错误】");
                    break;
                case 4:
                    Console.WriteLine("【RAR-尝试修改一个锁定的压缩文件】");
                    mc.LogTrace("【RAR-尝试修改一个锁定的压缩文件】");
                    break;
                case 5:
                    Console.WriteLine("【RAR-写错误】");
                    mc.LogTrace("【RAR-写错误】");
                    break;
                case 6:
                    Console.WriteLine("【RAR-文件打开错误】");
                    mc.LogTrace("【RAR-文件打开错误】");
                    break;
                case 7:
                    Console.WriteLine("【RAR-错误命令行选项】");
                    mc.LogTrace("【RAR-错误命令行选项】");
                    break;
                case 8:
                    Console.WriteLine("【RAR-内存不足】");
                    mc.LogTrace("【RAR-内存不足】");
                    break;
                case 9:
                    Console.WriteLine("【RAR-文件创建错误】");
                    mc.LogTrace("【RAR-文件创建错误】");
                    break;
                default:
                    Console.WriteLine("【RAR-用户中断】");
                    mc.LogTrace("【RAR-用户中断】");
                    break;
            }
            return errorLevel;
        }
        #endregion 更新包解压缩 UnRAR(string srcFullPath, string desFullPath)

        #region 查找指定磁盘目录下包含[Update]标识的更新文件包 FindRAR(string Disk, string dirName)
        /// <summary>
        /// 查找指定磁盘目录下包含[Update]标识的更新文件包
        /// </summary>
        /// <param name="Disk">磁盘分区 例：c:\</param>
        /// <param name="dirName">更新临时目录名称 例：[UpdateTemp]</param>
        /// <returns>返回更新包绝对路径</returns>
        public string FindRAR(string strDisk, string dirName)
        {
            string rarPath = null;
            DirectoryInfo dir = new DirectoryInfo(strDisk);
            foreach (DirectoryInfo dChild in dir.GetDirectories(dirName))
            {
                Console.WriteLine("【正在检索" + dirName + "文件夹】");
                mc.LogTrace("【正在检索" + dirName + "文件夹】");
                foreach (FileInfo Path in dChild.GetFiles("[Update]*.rar"))
                {
                    rarPath = Path.FullName;
                    Console.WriteLine("【发现更新包】|" + Path.Name);
                    mc.LogTrace("【发现更新包】|" + Path.Name);
                }
            }
            return rarPath;
        }
        #endregion 查找指定磁盘目录下包含[Update]标识的更新文件包 FindRAR(string Disk, string dirName)
    }
}
