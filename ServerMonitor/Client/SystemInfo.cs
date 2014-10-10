using System;
using System.Management;
using System.Threading;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.IO;

namespace Client
{
    public class SysInfoClass
    {
        public string _CPU;
        public string _Disk;
        public Thread TdCPU;
        public Thread TdDiskInfo;
        Library lib = new Library();
        PerformanceCounter pc = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        public string strIP;

        public SysInfoClass()
        {
            DateTime dt = DateTime.Now;
            TdCPU = new Thread(new ThreadStart(CPU));
            TdCPU.IsBackground = true;
            TdCPU.Start();
            TdDiskInfo = new Thread(new ThreadStart(Disk));
            TdDiskInfo.IsBackground = true;
            TdDiskInfo.Start();
            this.strIP = ServerIP();
            Log.WriteLog(LogFile.Trace, "【监控模块线程启动】");
        }

        #region 显示信息 ShowInfo()
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <returns></returns>
        public string ShowInfo()
        {
            string strContent = null;
            string strVersion = lib.ReadINI("Update", "Version");

            if (this._CPU == null)
            {
                this._CPU = "0";
            }

            strContent = this.strIP + "|" + this._CPU + "|" + strVersion + "|" + this._Disk;
            strContent = strContent.Replace("|{", "{").Replace("|}", "}");
            //Log.WriteLog(LogFile.Trace, "SysInfoClass | " + strContent);

            return strContent;
        }
        #endregion 显示信息 ShowInfo()

        #region CPU利用率 CPU()
        /// <summary>
        /// CPU利用率
        /// </summary>
        private void CPU()
        {                        
            while (true)
            {
                Thread.Sleep(1000);
                float cpuLoad = pc.NextValue();
                this._CPU = Convert.ToInt32(cpuLoad).ToString();
            }
        }
        #endregion CPU利用率 CPU()

        #region 读取硬盘信息线程 Disk()
        /// <summary>
        /// 读取硬盘信息线程
        /// </summary>
        private void Disk()
        {
            while (true)
            {
                this._Disk = DiskInfo();
                Thread.Sleep(5 * 60 * 1000);
            }
        }
        #endregion 读取硬盘信息线程 Disk()

        #region 硬盘信息 DiskInfo()
        /// <summary>
        /// 硬盘信息
        /// </summary>
        private string DiskInfo()
        {
            long longSpace = 0;
            long longFreeSpace = 0;
            long longSize = 0;
            long longUseSpace = 0;

            StringBuilder sb = new StringBuilder();
            SelectQuery sQuery = new SelectQuery("SELECT Name,VolumeName,FreeSpace,Size,DriveType FROM Win32_LogicalDisk");
            ManagementObjectSearcher mos = new ManagementObjectSearcher(sQuery);
            sb.Append("{");
            foreach (ManagementObject mo in mos.Get())
            {
                if (mo["DriveType"].ToString() == "3")
                {
                    //sb.Append(mo["Name"] + "-" + Convert.ToInt64(mo["FreeSpace"]) / 1024 / 1204 + "-" + Convert.ToInt64(mo["Size"]) / 1024 / 1204 + "|");
                    //{C:-54713-84965|D:-70944-85050|E:-165293-170100|F:-139197-170100|G:-133459-170100|H:-119944-130855|J:-7041-68042|K:-39320-51037|L:-40584-102060|M:-12235-102060|N:-38862-82433}
                    longFreeSpace = Convert.ToInt64(mo["FreeSpace"]) / 1024 / 1204;
                    longSize = Convert.ToInt64(mo["Size"]) / 1024 / 1204;                    
                    longUseSpace = longSize - longFreeSpace;
                    longSpace = longUseSpace * 100 / longSize;
                    sb.Append(mo["Name"] + "|" + longSpace + "|");
                }
            }
            sb.Append("}");
            return sb.ToString();
        }
        #endregion 硬盘信息 DiskInfo()

        #region 获取服务器IP信息 ServerIP()
        /// <summary>
        /// 获取服务器IP信息
        /// </summary>
        /// <returns></returns>
        public string ServerIP()
        {
            string strIP = null;
            try
            {
                WebRequest request = WebRequest.Create("http://ishow.xba.com.cn/ip.aspx");//为指定的 URI 方案初始化新的 System.Net.WebRequest 实例
                request.UseDefaultCredentials = false;//获取或设置一个 System.Boolean 值，该值控制 System.Net.CredentialCache.DefaultCredentials
                WebResponse response = request.GetResponse();//返回对 Internet 请求的响应。
                Stream resStream = response.GetResponseStream();//返回从 Internet 资源返回数据流
                StreamReader sr = new StreamReader(resStream, System.Text.Encoding.Default);//实例华一个流的读写器
                strIP = sr.ReadToEnd();//这就是百度首页的HTML哦 ,字符串形式的流的其余部分（从当前位置到末尾）。如果当前位置位于流的末尾，则返回空字符串 ("")
                resStream.Close();//关闭当前流并释放与之关联的所有资源
                sr.Close();
                return strIP;
            }
            catch
            {
                IPAddress[] ServerIP = Dns.GetHostByName(Dns.GetHostName()).AddressList;
                return ServerIP[0].ToString();
            }
            

            
        }
        #endregion 获取服务器IP信息
    }
}
