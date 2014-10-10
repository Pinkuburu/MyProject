using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ComputerInfo
{
    ///<summary>
    ///计算机信息类
    ///</summary>

    public class ComputerInfo
    {
        private string IpAddress;
        private static ComputerInfo _instance;

        internal static ComputerInfo Instance()
        {
            if (_instance == null)
                _instance = new ComputerInfo();
            return _instance;
        }

        internal ComputerInfo()
        {
            IpAddress = GetIPAddress();
        }
        
        
        /// <summary>
        /// 获取IP地址（IPv4）
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddress()
        {
            try
            {
                IPAddress[] arrIPAddresses = Dns.GetHostAddresses(Dns.GetHostName());
                foreach (IPAddress ip in arrIPAddresses)
                {
                    if (ip.AddressFamily.Equals(AddressFamily.InterNetwork))//IPv4
                    {
                        return ip.ToString();
                    }
                }
                return "unknow";
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }
    }
}
