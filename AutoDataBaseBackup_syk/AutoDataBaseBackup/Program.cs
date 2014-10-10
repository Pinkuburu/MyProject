using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace AutoDataBaseBackup
{
    class Program
    {
        static void Main(string[] args)
        {
            int inthours = DateTime.Now.Hour;
            string strcmd = "";
            string streturn = "";
            
            //读取配置文件内容
            #region ...
            ArrayList filestr = new ArrayList();
            filestr = PatchProcess.ReadData(AppDomain.CurrentDomain.BaseDirectory + "config.ini");
            string RarPath = filestr[0].ToString().Split('=')[1];
            string BakSaveCount = filestr[1].ToString().Split('=')[1];
            string RarSaveCount = filestr[2].ToString().Split('=')[1];
            string ZipTime = filestr[3].ToString().Split('=')[1];
            string[] DBName = filestr[4].ToString().Split('=')[1].Split('|');
            string[] Port = filestr[5].ToString().Split('=')[1].Split('|');
            string[] BackupPath = filestr[6].ToString().Split('=')[1].Split('|');
            string[] InZipPath = filestr[7].ToString().Split('=')[1].Split('|');
            #endregion


            //路径处理
            #region ...
            BackupPath = PatchProcess.Path(BackupPath);
            InZipPath = PatchProcess.Path(InZipPath);
            #endregion


            //目录处理
            #region ...
            PatchProcess.DirCreate(BackupPath);
            PatchProcess.DirCreate(InZipPath);
            #endregion


            //自动删除备份
            #region ...
            Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]   删除备份...");
            if (PatchProcess.FileCount(AppDomain.CurrentDomain.BaseDirectory, "delbak.bat") == 0)
            {
                PatchProcess.CreBat(BakSaveCount,RarSaveCount, DBName, BackupPath, InZipPath);
            }
            strcmd = AppDomain.CurrentDomain.BaseDirectory + "delbak.bat";
            PatchProcess.RunCMD(strcmd, 0);
            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "delbak.bat");
            PatchProcess.AddLog("删除备份。");
            Console.WriteLine("\n");
            #endregion


            //自动备份
            #region ...
            try
            {
                string strBackupTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                for (int i = 0; i < Port.Length; i++)
                {
                    for (int j = 0; j < DBName.Length; j++)
                    {
                        int intDirCount = PatchProcess.DirCount(BackupPath[i].Trim(), DBName[j].Trim());
                        if (intDirCount == 0)
                        {
                            PatchProcess.MdDir(BackupPath[i].Trim(), DBName[j].Trim());
                        }
                        Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]   开始创建" + DBName[j].Trim() + "数据库备份...");
                        strcmd = "osql -S localhost," + Port[i].Trim() + " -E -d \"" + DBName[j].Trim() + "\" -Q \"backup database \"" + DBName[j].Trim() + "\" to disk='" + BackupPath[i].Trim() + DBName[j].Trim() + "\\" + DBName[j].Trim() + "_Backup" + strBackupTime.Trim() + ".bak'\"";
                        streturn = PatchProcess.RunCMD(strcmd, 0);
                        PatchProcess.AddLog(streturn);
                        Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]   " + DBName[j].Trim() + "数据库备份创建完毕。");
                        if (inthours % Convert.ToInt32(ZipTime.Trim()) == 0)
                        {
                            intDirCount = PatchProcess.DirCount(InZipPath[i].Trim(), DBName[j].Trim());
                            if (intDirCount == 0)
                            {
                                PatchProcess.MdDir(InZipPath[i].Trim(), DBName[j].Trim());
                            }
                            Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]   开始压缩" + DBName[j].Trim() + "数据库备份...");
                            strcmd = RarPath.Trim() + " a -m3 -ap " + InZipPath[i].Trim() + DBName[j].Trim() + "\\" + DBName[j].Trim() + "_Backup" + strBackupTime.Trim() + ".rar " + BackupPath[i].Trim() + DBName[j].Trim() + "\\" + DBName[j].Trim() + "_Backup" + strBackupTime.Trim() + ".bak";
                            streturn = PatchProcess.RunCMD(strcmd, 0);
                            PatchProcess.AddLog(streturn);
                            Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]   " + DBName[j].Trim() + "数据库备份压缩完毕。");
                        }
                    }
                }
            }
            catch (Exception error)
            {
                PatchProcess.AddLog("【错误】" + error.ToString());
            }
            #endregion
        }
    }
}
