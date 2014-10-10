using System;
using System.IO;
using System.Collections;
using System.Diagnostics;

namespace AutoUpdate
{
    class MainClass
    {
        #region 变量
        //[WEB]包是否存在
        private bool bl_web = false;
        //[UPDATE]包是否存在
        private bool bl_update = false;
        //[SQL]包是否存在
        private bool bl_sql = false;

        public bool bl_WEB
        {
            get { return bl_web; }
            set { bl_web = value; }
        }

        public bool bl_UPDATE
        {
            get { return bl_update; }
            set { bl_update = value; }
        }

        public bool bl_SQL
        {
            get { return bl_sql; }
            set { bl_sql = value; }
        }
        #endregion 变量

        #region 运行CMD RunCMD(string dosCommand, int milliseconds)
        /// <summary>
        /// 运行CMD
        /// </summary>
        /// <param name="dosCommand">命令内容</param>
        /// <param name="milliseconds">等待时间，为0则无限等待</param>
        /// <returns></returns>
        public string RunCMD(string dosCommand, int milliseconds)
        {
            string output = "";     //输出字符串
            if (dosCommand != null && dosCommand != "")
            {
                Process process = new Process();     //创建进程对象
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe";      //设定需要执行的命令
                startInfo.Arguments = "/C " + dosCommand;   //设定参数，其中的“/C”表示执行完命令后马上退出
                startInfo.UseShellExecute = false;     //不使用系统外壳程序启动
                startInfo.RedirectStandardInput = false;   //不重定向输入
                startInfo.RedirectStandardOutput = true;   //重定向输出
                startInfo.CreateNoWindow = true;     //不创建窗口
                process.StartInfo = startInfo;
                try
                {
                    if (process.Start())       //开始进程
                    {
                        if (milliseconds == 0)
                            process.WaitForExit(0);     //这里无限等待进程结束
                        else
                            process.WaitForExit(milliseconds);  //这里等待进程结束，等待时间为指定的毫秒
                        output = process.StandardOutput.ReadToEnd();//读取进程的输出
                    }
                }
                catch
                {
                }
                finally
                {
                    if (process != null)
                        process.Close();
                }
            }
            return output;
        }
        #endregion 运行CMD RunCMD(string dosCommand, int milliseconds)

        #region 命令控制台调用 CMD(string dosCommand)
        /// <summary>
        /// 命令控制台调用
        /// </summary>
        /// <param name="dosCommand"></param>
        /// <returns></returns>
        public int CMD(string dosCommand)
        {
            int errorLevel = -1;
            //string pathToScannerProgram = Path.Combine(virusCheckFolder, "scan.exe");
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + dosCommand;
            startInfo.RedirectStandardOutput = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.Start();
                //process.StandardOutput.ReadToEnd();
                string output = process.StandardOutput.ReadToEnd();
                Console.WriteLine(output);
                LogTrace(output);
                errorLevel = process.ExitCode;//返回ERRORLEVEL
                process.WaitForExit();
            }
            return errorLevel;
        }
        #endregion 命令控制台调用 CMD(string dosCommand)

        #region 查找指定目录下的更新包 FindUpdatePackage(string strDisk, string dirName, string filePackageName)
        /// <summary>
        /// 查找指定目录下的更新包 
        /// </summary>
        /// <param name="strDisk">要查询的磁盘 例：c:\</param>
        /// <param name="dirName">要查询的更新包存放目录 例：[UpdateTemp]</param>
        /// <param name="filePackageName">要找带有标识的更新包 例：[Update]*.rar</param>
        /// <returns></returns>
        public ArrayList FindUpdatePackage(string strDisk, string filePackageName)
        {
            ArrayList alDirectory = new ArrayList();
            DirectoryInfo dir = new DirectoryInfo(strDisk);
            Console.WriteLine("【正在检索" + dir.FullName + "文件夹】");
            LogTrace("【正在检索" + dir.FullName + "文件夹】");
            foreach (FileInfo Path in dir.GetFiles(filePackageName))
            {
                TagFile(Path.Name);
                alDirectory.Add(Path.FullName);
                Console.WriteLine("【发现更新包】|" + Path.Name);
                LogTrace("【发现更新包】|" + Path.Name);
            }
            return alDirectory;
        }
        #endregion 查找指定目录下的更新包 FindUpdatePackage(string strDisk, string dirName, string filePackageName)

        #region 标记各更新包是否存在 TagFile(string PackageName)
        /// <summary>
        /// 标记各更新包是否存在
        /// </summary>
        /// <param name="PackageName"></param>
        private void TagFile(string PackageName)
        {
            if (PackageName.IndexOf("[WEB]") > -1)
            {
                bl_WEB = true;
            }
            if (PackageName.IndexOf("[UPDATE]") > -1)
            {
                bl_UPDATE = true;
            }
            if (PackageName.IndexOf("[SQL]") > -1)
            {
                bl_SQL = true;
            }
        }
        #endregion 标记各更新包是否存在 TagFile(string PackageName)

        #region 检查目录是否存在 CheckDirectory(string dirPath)
        /// <summary>
        /// 检查目录是否存在
        /// </summary>
        /// <param name="dirPath">要查找的文件夹名称 例：D:\ABC</param>
        /// <returns></returns>
        public bool CheckDirectory(string dirPath)
        {
            bool blDir = false;
            
            blDir = Directory.Exists(dirPath);

            return blDir;
        }
        #endregion 检查目录是否存在 CheckDirectory(string dirPath)

        #region 检查文件是否存在 CheckFile(string fileName)
        /// <summary>
        /// 检查文件是否存在
        /// </summary>
        /// <param name="fileName">要查找的文件名称 例：Config.ini</param>
        /// <returns></returns>
        public bool CheckFile(string fileName)
        {
            bool blFile = false;
            blFile = File.Exists(fileName);
            return blFile;
        }
        #endregion 检查文件是否存在 CheckFile(string fileName)

        #region 批量查找文件夹 FindDirectory(string strDisk, string dirName)
        /// <summary>
        /// 批量查找文件夹
        /// </summary>
        /// <param name="strDisk">要查找的目录 例：F:\[UpadteTemp]</param>
        /// <param name="dirName">目录名称，可用通配符 例：[WEB]*</param>
        /// <returns></returns>
        public ArrayList FindDirectory(string strDisk, string dirName)
        {
            ArrayList alDirectory = new ArrayList();
            DirectoryInfo dir = new DirectoryInfo(strDisk);
            Console.WriteLine("【正在检索 " + dir.FullName + " 文件夹】");
            LogTrace("【正在检索 " + dir.FullName + " 文件夹】");
            foreach (DirectoryInfo dChild in dir.GetDirectories(dirName))
            {
                Console.WriteLine("【发现更新目录】" + dChild.FullName);
                LogTrace("【发现更新目录】" + dChild.FullName);
                alDirectory.Add(dChild.FullName);
            }
            return alDirectory;
        }
        #endregion 批量查找文件夹 FindDirectory(string strDisk, string dirName)

        #region 结束指定进程 KillProcess(string strProcessName)
        /// <summary>
        /// 结束指定进程
        /// </summary>
        /// <param name="strProcessName">进程名</param>
        public void KillProcess(string strProcessName)
        {
            Process[] localByNameApp = Process.GetProcessesByName("notepad");//获取程序名的所有进程
            if (localByNameApp.Length > 0)
            {
                foreach (var app in localByNameApp)
                {
                    if (!app.HasExited)
                    {                        
                        app.Kill();//关闭进程
                        LogTrace("进程：" + strProcessName + "被终止");
                    }
                }
            }
        }
        #endregion 结束指定进程 KillProcess(string strProcessName)

        #region 日志方法
        public void LogTrace(string strContent)
        {
            Log.WriteLog(LogFile.Trace, strContent);
        }

        public void LogError(string strContent)
        {
            Log.WriteLog(LogFile.Error, strContent);
        }
        #endregion 日志方法

        #region 删除指定目录的所有文件和子目录 DeleteFiles(string targetDir, bool delSubDir)
        /// <summary>  
        /// 删除指定目录的所有文件和子目录  
        /// </summary>  
        /// <param name="targetDir">操作目录</param>  
        /// <param name="delSubDir">如果为true,包含对子目录的操作</param>  
        public void DeleteFiles(string targetDir, bool delSubDir)
        {
            foreach (string fileName in Directory.GetFiles(targetDir))
            {
                File.SetAttributes(fileName, FileAttributes.Normal);
                File.Delete(fileName);
                Console.WriteLine("【正在删除】" + fileName);
                LogTrace("【正在删除】" + fileName);
            }
            if (delSubDir)
            {
                DirectoryInfo dir = new DirectoryInfo(targetDir);
                foreach (DirectoryInfo subDi in dir.GetDirectories())
                {
                    DeleteFiles(subDi.FullName, true);
                    subDi.Delete();
                }
            }
        }
        #endregion 删除指定目录的所有文件和子目录 DeleteFiles(string targetDir, bool delSubDir)
    }
}
