using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AutoUpdate
{
    class AutoCopyFile
    {
        MainClass mc = new MainClass();

        #region 复制文件到指定路径 CopyFile(string srcFullPath, string desFullPath)
        /// <summary>
        /// 文件复制
        /// </summary>
        /// <param name="srcFullPath">原文件路径</param>
        /// <param name="desFullPath">目标文件路径</param>
        /// <returns>返回ERRORLEVEL值</returns>
        public int CopyFile(string srcFullPath, string desFullPath)
        {
            //0 文件复制没有错误。
            //1 没有找到要复制的文件。
            //2 用户按 Ctrl+C 终止了“xcopy”。
            //4 出现了初始化错误。没有足够的内存或磁盘空间，或命令行上输入了无效的驱动器名称或语法。
            //5 出现了磁盘写入错误。

            int errorLevel = 0;
            Console.WriteLine("【正在复制文件】" + srcFullPath + " To: " + desFullPath);
            mc.LogTrace("【正在复制文件】" + srcFullPath + " To: " + desFullPath);
            errorLevel = mc.CMD("C:\\xcopy /e /y " + srcFullPath + " \"" + desFullPath + "\"");

            switch (errorLevel)
            {
                case 0:
                    Console.WriteLine("【复制文件-成功操作】" + desFullPath);
                    mc.LogTrace("【复制文件-成功操作】" + desFullPath);
                    break;
                case 1:
                    Console.WriteLine("【复制文件-没有找到要复制的文件】");
                    mc.LogTrace("【复制文件-没有找到要复制的文件】");
                    break;
                case 2:
                    Console.WriteLine("【复制文件-用户按 Ctrl+C 终止了复制】");
                    mc.LogTrace("【复制文件-用户按 Ctrl+C 终止了复制】");
                    break;
                case 4:
                    Console.WriteLine("【复制文件-出现了初始化错误】");
                    mc.LogTrace("【复制文件-出现了初始化错误】");
                    break;
                case 5:
                    Console.WriteLine("【复制文件-出现了磁盘写入错误】");
                    mc.LogTrace("【复制文件-出现了磁盘写入错误】");
                    break;
            }

            return errorLevel;
        }
        #endregion 复制文件到指定路径 CopyFile(string srcFullPath, string desFullPath)

        #region 复制指定目录的所有文件 CopyFile(string sourceDir, string targetDir, bool overWrite, bool copySubDir)
        /// <summary>
        /// 复制指定目录的所有文件
        /// </summary>
        /// <param name="sourceDir">原始目录</param>
        /// <param name="targetDir">目标目录</param>
        /// <param name="overWrite">如果为true,覆盖同名文件,否则不覆盖</param>
        /// <param name="copySubDir">如果为true,包含目录,否则不包含</param>
        public void CopyFile(string sourceDir, string targetDir, bool overWrite, bool copySubDir)
        {
            //复制当前目录文件
            foreach (string sourceFileName in Directory.GetFiles(sourceDir))
            {
                string targetFileName = Path.Combine(targetDir, sourceFileName.Substring(sourceFileName.LastIndexOf("\\") + 1));

                if (File.Exists(targetFileName))
                {
                    if (overWrite == true)
                    {
                        File.SetAttributes(targetFileName, FileAttributes.Normal);
                        File.Copy(sourceFileName, targetFileName, overWrite);
                        Console.WriteLine("【正在复制】" + sourceFileName + " To: " + targetFileName);
                        mc.LogTrace("【正在复制】" + sourceFileName + " To: " + targetFileName);
                    }
                }
                else
                {
                    File.Copy(sourceFileName, targetFileName, overWrite);
                }
            }
            //复制子目录
            if (copySubDir)
            {
                foreach (string sourceSubDir in Directory.GetDirectories(sourceDir))
                {
                    string targetSubDir = Path.Combine(targetDir, sourceSubDir.Substring(sourceSubDir.LastIndexOf("\\") + 1));
                    if (!Directory.Exists(targetSubDir))
                        Directory.CreateDirectory(targetSubDir);
                    CopyFile(sourceSubDir, targetSubDir, overWrite, true);
                }
            }
        }
        #endregion
    }
}
