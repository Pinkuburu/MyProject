using System;
using System.Management;
using System.Threading;
using System.Text;

namespace CtalkRobot
{
    public class SysInfoClass
    {
        public string _CPU;
        bool blDebug = false;

        public SysInfoClass()
        {
            DateTime dt = DateTime.Now;
            Thread TdCPU = new Thread(new ThreadStart(CPU));
            TdCPU.Start();
            Console.WriteLine(dt + "|【监控模块线程启动】");
        }

        #region CPU利用率 CPU()
        /// <summary>
        /// CPU利用率
        /// </summary>
        public void CPU()
        {
            SelectQuery sQuery = new SelectQuery("SELECT LoadPercentage FROM Win32_Processor");
            ManagementObjectSearcher mos = new ManagementObjectSearcher(sQuery);
            while (true)
            {
                foreach (ManagementObject mo in mos.Get())
                {
                    try
                    {
                        this._CPU = mo["LoadPercentage"].ToString();
                        DebugInfo(this._CPU, "Info");
                    }
                    catch (System.Exception ex)
                    {
                        this._CPU = "0";
                        DebugInfo(this._CPU, "Error");
                    }             
                }
            }
        }
        #endregion CPU利用率 CPU()

        #region 硬盘信息 DiskInfo()
        /// <summary>
        /// 硬盘信息
        /// </summary>
        public string DiskInfo()
        {
            StringBuilder sb = new StringBuilder();
            SelectQuery sQuery = new SelectQuery("SELECT Name,VolumeName,FreeSpace,Size,DriveType FROM Win32_LogicalDisk");
            ManagementObjectSearcher mos = new ManagementObjectSearcher(sQuery);
            foreach (ManagementObject mo in mos.Get())
            {
                if (mo["DriveType"].ToString() != "5")
                {
                   sb.Append(mo["Name"] + "-" + Convert.ToInt64(mo["FreeSpace"])/1024/1204 + "|");
                }
            }
            return sb.ToString();
        }
        #endregion 硬盘信息 DiskInfo()

        #region 调试信息 DebugInfo(string strContent)
        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="strContent"></param>
        private void DebugInfo(string strContent, string strType)
        {
            DateTime dt = DateTime.Now;
            if (blDebug)
            {
                switch (strType)
                {
                    case "Login":
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case "Error":
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case "SendMsg":
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case "ReciveMsg":
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        break;
                    case "Info":
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;
                }
                Console.WriteLine(dt + "|" + strContent);
            }
        }
        #endregion 调试信息 DebugInfo(string strContent)
    }
}
