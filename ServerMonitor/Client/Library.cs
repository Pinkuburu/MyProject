using System;
using System.Net;
using System.IO;
using System.Xml;
using System.Diagnostics;
using Microsoft.Win32;
using System.Windows.Forms;

namespace Client
{
    class Library
    {
        IniFile ini = new IniFile(Application.StartupPath + "\\Client.ini");
        private string DecryptKey = "1vu(enex";//密钥KEY 长度8位

        #region 读写配置文件
        public string ReadINI(string sectionName, string keyName)
        {
            string strValue = null;
            strValue = ini.GetString(sectionName, keyName, "");
            return strValue;
        }

        public void SaveINI(string sectionName, string keyName, string strValue)
        {
            ini.WriteValue(sectionName, keyName, strValue);
        }
        #endregion 读写配置文件

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

        #region 判断软件的版本 GetTheLastVersion(string Dir)
        /// <summary> 
        /// 判断软件的版本
        /// </summary> 
        /// <param name="Dir">服务器地址</param> 
        /// <returns>返回版本</returns> 
        private string GetTheLastVersion(string Dir)
        {
            string LastUpdateTime = "";
            string AutoUpdaterFileName = Dir + "AutoUpdater/AutoUpdater.xml";
            try
            {
                WebClient wc = new WebClient();
                Stream sm = wc.OpenRead(AutoUpdaterFileName);
                XmlTextReader xml = new XmlTextReader(sm);
                while (xml.Read())
                {
                    if (xml.Name == "Update")
                    {
                        LastUpdateTime = xml.GetAttribute("Version");
                        break;
                    }
                }
                xml.Close();
                sm.Close();
                wc.Dispose();
            }
            catch (WebException ex)
            {
                LastUpdateTime = ReadINI("Update", "Version");
                Log.WriteLog(LogFile.Error, ex.ToString());
            }
            return LastUpdateTime;
        }
        #endregion 判断软件的版本 GetTheLastVersion(string Dir)

        #region 检测更新 CheckUpdate()
        /// <summary>
        /// 检测更新
        /// </summary>
        /// <returns></returns>
        public bool CheckUpdate()
        {
            string strRemoteVersion = null;
            string strLocalVersion = null;

            strRemoteVersion = GetTheLastVersion(ReadINI("Update", "Url"));
            strLocalVersion = ReadINI("Update", "Version");

            if (Convert.ToInt32(strRemoteVersion) > Convert.ToInt32(strLocalVersion))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion 检测更新 CheckUpdate()

        #region 加解密方法
        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="strContent"></param>
        /// <returns></returns>
        public string DESDecrypt(string strContent)
        {
            strContent = Cryptography.DESDecrypt(strContent, this.DecryptKey);
            return strContent;
        }

        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="strContent"></param>
        /// <returns></returns>
        public string DESEncrypt(string strContent)
        {
            strContent = Cryptography.DESEncrypt(strContent, this.DecryptKey);
            return strContent;
        }
        #endregion 加解密方法        

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

        #region 开机自动运行 AutoRun(string Path, bool blRun)
        /// <summary>
        /// 开机自动运行
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="blRun"></param>
        public void AutoRun(string Path, bool blRun)
        {
            string FullPath = Path + " AutoLogin";
            string ShortFileName = "MT_Client";//FullPath.Substring(FullPath.LastIndexOf("\\") + 1);
            
            try
            {
                //打开子键节点
                RegistryKey Reg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                //如果子键节点不存在，则创建之
                if (Reg == null)
                {
                    Reg = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                }
                //在注册表中设置自启动程序
                if (blRun)
                {
                    if (Reg.GetValue(ShortFileName) == null)
                    {
                        Reg.SetValue(ShortFileName, FullPath);
                    }                    
                }
                else
                {
                    if (Reg.GetValue(ShortFileName) != null)
                    {
                        Reg.DeleteValue(ShortFileName);
                    }                    
                }                
            }
            catch (Exception ex)
            {
                LogError(ex.ToString());
            }
        }
        #endregion 开机自动运行 AutoRun(string Path, bool blRun)

        #region 创建指定目录 CreateDirectory(string targetDir)
        /// <summary>
        /// 创建指定目录
        /// </summary>
        /// <param name="targetDir"></param>
        public void CreateDirectory(string targetDir)
        {
            DirectoryInfo dir = new DirectoryInfo(targetDir);
            if (!dir.Exists)
                dir.Create();
        }
        #endregion
    }
}
