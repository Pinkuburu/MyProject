using System;
using System.Collections;

namespace AutoUpdate
{
    class Program
    {
        static AutoUnRAR atRAR = new AutoUnRAR();
        static AutoCopyFile atCopy = new AutoCopyFile();
        static MainClass mc = new MainClass();
        static IniFile ini = new IniFile(@"Config.ini");

        static void Main(string[] args)
        {
            //====== ini ======
            //更新包存放路径
            //UpdatePackagePath = D:\[UpdateTemp]
            //更新包通配符
            //UpdatePackageName = [*]*.rar or [*].rar
            //文件覆盖对应
            //WEB = D:\MaitiamFootball2*
            //UPDATE = D:\Update*

            //string UpdatePackagePath = @"D:\[UpdateTemp]";
            //string UpdatePackageName = "[*]*.rar";
            
            //判定配置文件是否存在
            if (mc.CheckFile("Config.ini"))
            {
                mc.LogTrace("发现配置文件，准备开始更新。");
                mc.LogTrace("正在读取配置文件");
                #region 读取配置文件

                //更新包路径
                string strUpdatePackagePath = ini.GetString("PackagePath", "UpdatePackagePath", "");
                string strUpdatePackageName = ini.GetString("PackagePath", "UpdatePackageName", "");
                //覆盖文件路径
                string strWEB_DRIVE = ini.GetString("CopyPath", "WEB_DRIVE", "");
                string strWEB_DIR = ini.GetString("CopyPath", "WEB_DIR", "");
                string strUPDATE_DRIVE = ini.GetString("CopyPath", "UPDATE_DRIVE", "");
                string strUPDATE_DIR = ini.GetString("CopyPath", "UPDATE_DIR", "");
                string strSQL_DRIVE = ini.GetString("CopyPath", "SQL_DRIVE", "");
                string strSQL_DIR = ini.GetString("CopyPath", "SQL_DIR", "");
                //关闭进程名称
                string strProcessName = ini.GetString("UpdateName", "Name", "");

                #endregion 读取配置文件
                mc.LogTrace("配置文件读取完成");

                int ErrorLevel = 0;
                ArrayList alUpdatePackage = new ArrayList();

                if (mc.CheckDirectory(strUpdatePackagePath))
                {
                    alUpdatePackage = mc.FindUpdatePackage(strUpdatePackagePath, strUpdatePackageName);
                    foreach (string UpPkgPath in alUpdatePackage)
                    {
                        ErrorLevel = atRAR.UnRAR(UpPkgPath, UpPkgPath.Replace(".rar", ""));

                        //[WEB]标识处理方法
                        if (UpPkgPath.Replace(".rar", "").IndexOf("[WEB]") > -1 && ErrorLevel == 0)
                        {
                            ArrayList alWEB = new ArrayList();//存储[WEB]标识要覆盖的目标地址

                            alWEB = mc.FindDirectory(strWEB_DRIVE, strWEB_DIR);//枚举通配符目的地址存入ArrayList
                            foreach (string webPath in alWEB)//遍历目的地址
                            {
                                //atCopy.CopyFile(UpPkgPath.Replace(".rar", ""), webPath);//覆盖文件
                                atCopy.CopyFile(UpPkgPath.Replace(".rar", ""), webPath, true, true);
                            }
                            mc.DeleteFiles(UpPkgPath.Replace(".rar", ""), true);
                        }

                        //[UPDATE]标识处理方法
                        if (UpPkgPath.Replace(".rar", "").IndexOf("[UPDATE]") > -1 && ErrorLevel == 0)
                        {
                            //此处添加日常更新程序进程关闭流程
                            ArrayList alUPDATE = new ArrayList();//存储[UPDATE]标识要覆盖的目标地址

                            alUPDATE = mc.FindDirectory(strUPDATE_DRIVE, strUPDATE_DIR);//枚举通配符目的地址存入ArrayList
                            foreach (string updatePath in alUPDATE)//遍历目的地址
                            {
                                //atCopy.CopyFile(UpPkgPath.Replace(".rar", ""), updatePath);//覆盖文件
                                atCopy.CopyFile(UpPkgPath.Replace(".rar", ""), updatePath, true, true);
                            }
                            mc.DeleteFiles(UpPkgPath.Replace(".rar", ""), true);
                            //此处添加日常更新程序开启流程
                        }

                        //[SQL]标识处理方法?????????????????
                        //if (UpPkgPath.Replace(".rar", "").IndexOf("[SQL]") > -1 && ErrorLevel == 0)
                        //{
                        //    ArrayList alSQL = new ArrayList();
                        //    Console.WriteLine("【正在复制文件】|" + UpPkgPath.Replace(".rar", ""));
                        //}                        
                    }
                    Console.WriteLine("【更新完成】");
                    mc.LogTrace("【更新完成】");
                }
                else
                {
                    mc.LogTrace("更新包路径：" + strUpdatePackagePath + " 不存在");
                    Console.WriteLine(strUpdatePackagePath + " 不存在");
                }
            }
            else
            {
                Console.WriteLine("配置文件不存在");
            }

            Console.ReadKey();
        }
    }
}
