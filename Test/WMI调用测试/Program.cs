using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Diagnostics;
using System.Threading;

namespace WMI调用测试
{
    class Program
    {
        private static PerformanceCounter pcCpuLoad;

        static void Main(string[] args)
        {
            //PerformanceCounterFun("Processor", "_Total", "% Processor Time");
            DiskInfo();
            //Console.WriteLine(FreeMemoryInfo());
            //Thread.Sleep(1000);
            //DateTime dt = DateTime.Now;
            //Console.Write(dt.DayOfWeek + 1);
            Console.ReadKey();
        }

        private static void DiskInfo()
        {
            SelectQuery sQuery = new SelectQuery("SELECT Name,VolumeName,FreeSpace,Size,DriveType FROM Win32_LogicalDisk");
            ManagementObjectSearcher mos = new ManagementObjectSearcher(sQuery);
            foreach (ManagementObject mo in mos.Get())
            {
                if (mo["DriveType"].ToString() == "3")
                {
                    Console.WriteLine(mo["Name"] + " " + mo["VolumeName"] + " " + mo["DriveType"].ToString() + " " + Convert.ToInt64(mo["FreeSpace"]) / 1024 / 1024 + " " + Convert.ToInt64(mo["Size"]) / 1024 / 1024);
                }
            }
        }

        private static string FreeMemoryInfo()
        {
            string strFreeMemory=null;

            SelectQuery sQuery = new SelectQuery("SELECT FreePhysicalMemory FROM Win32_OperatingSystem");
            ManagementObjectSearcher mos = new ManagementObjectSearcher(sQuery);
            foreach (ManagementObject mo in mos.Get())
            {
                strFreeMemory = mo["FreePhysicalMemory"].ToString();
            }
            return strFreeMemory;
        }

        private static void PerformanceCounterFun(string CategoryName, string InstanceName, string CounterName)
        {
            PerformanceCounter pc = new PerformanceCounter(CategoryName, CounterName, InstanceName);
            while (true)
            {
                Thread.Sleep(1000);
                float cpuLoad = pc.NextValue();
                Console.WriteLine(cpuLoad);
            }
        }
    }
}
