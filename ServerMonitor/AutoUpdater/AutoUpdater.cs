using System; 
using System.ComponentModel; 
using System.Data; 
using System.Globalization; 
using System.IO; 
using System.Net; 
using System.Text; 
using System.Windows.Forms; 
using System.Xml;
using System.Diagnostics;
using System.Threading;

namespace AutoUpdater 
{ 
    public partial class AutoUpdater : Form 
    {
        private WebClient downWebClient = new WebClient();
        private static IniFile ini = new IniFile(Application.StartupPath + "\\Client.ini");
        private static string dirPath; 
        private static long size;//所有文件大小 
        private static int count;//文件总数 
        private static string[] fileNames; 
        private static int num;//已更新文件数 
        private static long upsize;//已更新文件大小
        private static string fileName;//当前文件名 
        private static long filesize;//当前文件大小 
         
        public AutoUpdater() 
        { 
            InitializeComponent();
            if (!Directory.Exists(Application.StartupPath + "\\AutoUpdater"))
            {
                Directory.CreateDirectory(Application.StartupPath + "\\AutoUpdater");
            }
        } 

        private void ComCirUpdate_Load(object sender, EventArgs e) 
        {
            dirPath = GetConfigValue("Update", "Url");
            string thePreUpdateDate = GetTheLastVersion(dirPath);
            string localUpDate = GetConfigValue("Update", "Version"); 
            if (Convert.ToInt32(thePreUpdateDate) > Convert.ToInt32(localUpDate)) 
            { 
                UpdaterStart();
            } 
            else 
            {
                Application.Exit();
            }
        } 

        /// <summary> 
        /// 开始更新 
        /// </summary> 
        private void UpdaterStart() 
        {
            //Process[] localByNameApp = Process.GetProcessesByName("Client");//获取程序名的所有进程
            //if (localByNameApp.Length > 0)
            //{
            //    foreach (var app in localByNameApp)
            //    {
            //        if (!app.HasExited)
            //        {
            //            app.Kill();//关闭进程
            //        }
            //    }
            //}

            float tempf; 
            //委托下载数据时事件 
            this.downWebClient.DownloadProgressChanged += delegate(object wcsender, DownloadProgressChangedEventArgs ex) 
            { 
                this.label2.Text = String.Format( 
                    CultureInfo.InvariantCulture, 
                    "正在下载:{0}  [ {1}/{2} ]", 
                    fileName, 
                    ConvertSize(ex.BytesReceived), 
                    ConvertSize(ex.TotalBytesToReceive)); 

                filesize = ex.TotalBytesToReceive; 
                tempf = ((float)(upsize + ex.BytesReceived) / size); 
                this.progressBar1.Value = Convert.ToInt32(tempf * 100); 
                this.progressBar2.Value = ex.ProgressPercentage; 
            }; 
            //委托下载完成时事件 
            this.downWebClient.DownloadFileCompleted += delegate(object wcsender, AsyncCompletedEventArgs ex) 
            { 
                if (ex.Error != null) 
                { 
                    MeBox("1" + ex.Error.Message); 
                } 
                else 
                {
                    if (File.Exists(Application.StartupPath + "\\" + fileName))
                    {
                        try
                        {
                            File.Delete(Application.StartupPath + "\\" + fileName);
                        }
                        catch
                        {
                            RunCMD("del Client.exe", 2000);
                        }
                    }

                    try
                    {
                        File.Move(Application.StartupPath + "\\AutoUpdater\\" + fileName, Application.StartupPath + "\\" + fileName);
                    }
                    catch
                    {
                        RunCMD("move .\\AutoUpdater\\Client.exe Client.exe", 2000);
                    }
                    
                    
                    upsize += filesize; 
                    if (fileNames.Length > num) 
                    { 
                        DownloadFile(num); 
                    } 
                    else 
                    {
                        SetConfigValue("Update", "Version", GetTheLastVersion(dirPath)); 
                        UpdaterClose(); 
                    } 
                } 
            }; 

            size = GetUpdateSize(dirPath + "UpdateSize.ashx"); 
            if (size == 0) 
                UpdaterClose(); 
            num = 0; 
            upsize = 0; 
            UpdateList(); 
            if (fileNames != null) 
                DownloadFile(0); 
        } 

        /// <summary> 
        /// 获取更新文件大小统计 
        /// </summary> 
        /// <param name="filePath">更新文件数据XML</param> 
        /// <returns>返回值</returns> 
        private static long GetUpdateSize(string filePath) 
        { 
            long len; 
            len = 0; 
            try 
            { 
                WebClient wc = new WebClient(); 
                Stream sm = wc.OpenRead(filePath); 
                XmlTextReader xr = new XmlTextReader(sm); 
                while (xr.Read()) 
                { 
                    if (xr.Name == "UpdateSize") 
                    { 
                        len = Convert.ToInt64(xr.GetAttribute("Size"), CultureInfo.InvariantCulture); 
                        break; 
                    } 
                } 
                xr.Close(); 
                sm.Close(); 
            } 
            catch (WebException ex) 
            { 
                MeBox("2" + ex.Message); 
            } 
            return len; 
        } 

        /// <summary> 
        /// 获取文件列表并下载 
        /// </summary> 
        private static void UpdateList() 
        { 
            string xmlPath = dirPath + "AutoUpdater/AutoUpdater.xml"; 
            WebClient wc = new WebClient(); 
            DataSet ds = new DataSet(); 
            ds.Locale = CultureInfo.InvariantCulture; 
             
            try 
            { 
                Stream sm = wc.OpenRead(xmlPath); 
                ds.ReadXml(sm); 
                DataTable dt = ds.Tables["UpdateFileList"]; 
                StringBuilder sb = new StringBuilder(); 
                count = dt.Rows.Count; 
                for (int i = 0; i < dt.Rows.Count; i++) 
                { 
                    if (i == 0) 
                    { 
                        sb.Append(dt.Rows[i]["UpdateFile"].ToString()); 
                    } 
                    else 
                    { 
                        sb.Append("," + dt.Rows[i]["UpdateFile"].ToString()); 
                    } 
                } 
                fileNames = sb.ToString().Split(','); 
                sm.Close(); 
            } 
            catch (WebException ex) 
            { 
                MeBox("3" + ex.Message); 
            } 
        } 

        /// <summary> 
        /// 下载文件 
        /// </summary> 
        /// <param name="arry">下载序号</param> 
        private void DownloadFile(int arry) 
        { 
            try 
            { 
                num++; 
                fileName = fileNames[arry]; 
                this.label1.Text = String.Format( 
                    CultureInfo.InvariantCulture, 
                    "更新进度 {0}/{1}  [ {2} ]",  
                    num,  
                    count,  
                    ConvertSize(size)); 

                this.progressBar2.Value = 0; 
                this.downWebClient.DownloadFileAsync( 
                    new Uri(dirPath + "AutoUpdater/" + fileName), 
                    Application.StartupPath + "\\AutoUpdater\\" + fileName); 
            } 
            catch (WebException ex) 
            { 
                MeBox("3" + ex.Message); 
            } 
        } 

        /// <summary> 
        /// 转换字节大小 
        /// </summary> 
        /// <param name="byteSize">输入字节数</param> 
        /// <returns>返回值</returns> 
        private static string ConvertSize(long byteSize) 
        { 
            string str = ""; 
            float tempf = (float)byteSize; 
            if (tempf / 1024 > 1) 
            { 
                if ((tempf / 1024) / 1024 > 1) 
                { 
                    str = ((tempf / 1024) / 1024).ToString("##0.00", CultureInfo.InvariantCulture) + "MB"; 
                } 
                else 
                { 
                    str = (tempf / 1024).ToString("##0.00", CultureInfo.InvariantCulture) + "KB"; 
                } 
            } 
            else 
            { 
                str = tempf.ToString(CultureInfo.InvariantCulture) + "B"; 
            } 
            return str; 
        } 

        /// <summary> 
        /// 弹出提示框 
        /// </summary> 
        /// <param name="txt">输入提示信息</param> 
        private static void MeBox(string txt) 
        { 
            MessageBox.Show( 
                txt, 
                "提示信息", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Asterisk, 
                MessageBoxDefaultButton.Button1, 
                MessageBoxOptions.DefaultDesktopOnly); 
        } 

        /// <summary> 
        /// 关闭程序 
        /// </summary> 
        private static void UpdaterClose() 
        {
            //Process[] localByNameApp = Process.GetProcessesByName("Client");//获取程序名的所有进程
            //if (localByNameApp.Length > 0)
            //{
            //    foreach (var app in localByNameApp)
            //    {
            //        if (!app.HasExited)
            //        {
            //            app.Kill();//关闭进程
            //        }
            //    }
            //}

            try 
            {
                Thread.Sleep(2000);
                //System.Diagnostics.Process.Start(Application.StartupPath + "\\Client.exe"); 
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "Client.exe";//需要启动的程序名       
                p.StartInfo.WorkingDirectory = Application.StartupPath;
                p.StartInfo.Arguments = "AutoLogin";//启动参数       
                p.Start();//启动   
            } 
            catch (Win32Exception ex) 
            { 
                MeBox("4" + ex.Message); 
            } 
            Application.Exit(); 
        } 

        /// <summary> 
        /// 读取.exe.config的值 
        /// </summary> 
        /// <param name="path">.exe.config文件的路径</param> 
        /// <param name="appKey">"key"的值</param> 
        /// <returns>返回"value"的值</returns> 
        internal static string GetConfigValue(string path, string appKey) 
        { 
            string strContent = null;
            strContent = ini.GetString(path, appKey, "");
            return strContent;
        }

        /// <summary> 
        /// 设置版本号的值 
        /// </summary> 
        /// <param name="path">.exe.config文件的路径</param> 
        /// <param name="appKey">"key"的值</param> 
        /// <param name="appValue">"value"的值</param> 
        internal static void SetConfigValue(string path, string appKey, string appValue) 
        {
            ini.WriteValue(path,appKey,appValue);
        } 

        /// <summary> 
        /// 判断软件的版本
        /// </summary> 
        /// <param name="Dir">服务器地址</param> 
        /// <returns>返回版本</returns> 
        private static string GetTheLastVersion(string Dir) 
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
            } 
            catch (WebException ex) 
            { 
                MeBox("5" + ex.Message); 
            } 
            return LastUpdateTime; 
        }

        #region 运行CMD RunCMD(string dosCommand, int milliseconds)
        /// <summary>
        /// 运行CMD
        /// </summary>
        /// <param name="dosCommand">命令内容</param>
        /// <param name="milliseconds">等待时间，为0则无限等待</param>
        /// <returns></returns>
        private string RunCMD(string dosCommand, int milliseconds)
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
    } 
}
