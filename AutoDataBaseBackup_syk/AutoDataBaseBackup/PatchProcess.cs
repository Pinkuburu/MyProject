using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Diagnostics;

namespace AutoDataBaseBackup
{
    public class PatchProcess
    {
        /// 执行DOS命令，返回DOS命令的输出
        /// 
        /// dos命令
        /// 等待命令执行的时间（单位：毫秒），如果设定为0，则无限等待
        /// 返回输出，如果发生异常，返回空字符串
        public static string RunCMD(string dosCommand, int milliseconds)
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


        //日志
        public static void AddLog(string str)
        {
            string strFileName = AppDomain.CurrentDomain.BaseDirectory + "Log\\Log" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            using (StreamWriter sw = new StreamWriter(File.Open(strFileName, FileMode.Append, FileAccess.Write)))
            {
                sw.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]" + str);
                sw.Close();
            }
        }


        //路径处理
        public static string[] Path(string[] strPath)
        {
            for (int i = 0; i < strPath.Length; i++)
            {
                strPath[i] = strPath[i].Trim();
                if (strPath[i].Substring(strPath[i].Length - 1) != "\\")
                {
                    strPath[i] = strPath[i] + "\\";
                }
            }
            return strPath;
        }


        //目录处理
        public static void DirCreate(string[] strPath)
        {
            for (int i = 0; i < strPath.Length; i++)
            {
                string[] Dir = strPath[i].Split('\\');
                for (int j = 1; j < Dir.Length - 1; j++)
                {
                    int intDirCount = PatchProcess.DirCount(Dir[0].Trim() + "\\", Dir[j].Trim());
                    if (intDirCount == 0)
                    {
                        PatchProcess.MdDir(Dir[0].Trim() + "\\", Dir[j].Trim());
                    }
                    Dir[0] = Dir[0] + "\\" + Dir[j];
                }
            }
        }


        //指定文件夹判断
        public static int DirCount(string strDirPath, string strDirName)
        {
            DirectoryInfo dir = Directory.CreateDirectory(strDirPath);
            List<DirectoryInfo> listDir = new List<DirectoryInfo>(dir.EnumerateDirectories(strDirName));
            return listDir.Count;
        }


        //指定类型文件统计
        public static int FileCount(string strFilePath, string strFileType)
        {
            DirectoryInfo dir = Directory.CreateDirectory(strFilePath);
            List<FileInfo> listFile = new List<FileInfo>(dir.EnumerateFiles(strFileType));
            return listFile.Count;
        }


        //配置文件内容读取
        public static ArrayList ReadData(string strFileName)
        {
            //C#读取TXT文件之创建 FileStream 的对象,说白了告诉程序,文件在那里,对文件如何处理,对文件内容采取的处理方式  
            FileStream fs = new FileStream(strFileName, FileMode.Open, FileAccess.Read);
            //仅对文本进行读写操作  
            StreamReader sr = new StreamReader(fs);
            ArrayList str = new ArrayList();
            //判断是否到文件尾
            while (sr.EndOfStream == false)
            {
                str.Add(sr.ReadLine());
            }
            //C#读取TXT文件之关闭文件，注意顺序，先对文件内部进行关闭，然后才是文件~  
            sr.Close();
            fs.Close();
            return str;
        }


        //批处理生成
        public static void CreBat(string strBakSave,string strRarSave,string[] strDBName,string[] strBackupPatch,string[] strInZipPath)
        {
            string strBatName = AppDomain.CurrentDomain.BaseDirectory + "delbak.bat";
            FileStream fs = new FileStream(strBatName, FileMode.Create);  //抛异常提示路径找不到????
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("@echo off");
            for (int i = 0; i < strBackupPatch.Length; i++)
            {
                for (int j = 0; j < strDBName.Length; j++)
                {
                    sw.WriteLine("for /f \"skip=" + strBakSave.Trim() + "\" %%i in ('dir " + strBackupPatch[i].Trim() + strDBName[j].Trim() + "\\*.bak /tc /o-d /b') do del " + strBackupPatch[i].Trim() + strDBName[j].Trim() + "\\%%i");
                    sw.WriteLine("for /f \"skip=" + strRarSave.Trim() + "\" %%i in ('dir " + strInZipPath[i].Trim() + strDBName[j].Trim() + "\\*.rar /tc /o-d /b') do del " + strInZipPath[i].Trim() + strDBName[j].Trim() + "\\%%i");
                }
            }
            sw.Close();
            sw.Dispose();
        }


        //目录创建
        public static void MdDir(string Path,string DirName)
        {
            Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]   创建" + DirName.Trim() + "目录");
            string strcmd = "md " + Path.Trim() + DirName.Trim();
            PatchProcess.RunCMD(strcmd, 0);
            PatchProcess.AddLog("创建" + DirName.Trim() + "目录");
        }
    }
}
