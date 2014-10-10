using System;
using System.Data;
using System.Net;
using System.ServiceProcess;

using log4net;

using pGina.Shared.Types;


namespace MaitiamPlugin
{
    class SessionLogger : ILoggerMode
    {
        private ILog m_logger = LogManager.GetLogger("MaitiamPluginLogger");

        public SessionLogger() { }

        //Logs the session if it's a LogOn or LogOff event.
        public bool Log(SessionChangeDescription changeDescription, SessionProperties properties)
        {
            string username = "--UNKNOWN--";
            if (properties != null)
            {
                UserInformation ui = properties.GetTrackedSingle<UserInformation>();
                if (true)
                    username = ui.Username;
                else
                    username = ui.OriginalUsername;
            }

            //Logon Event
            if (changeDescription.Reason == SessionChangeReason.SessionLogon)
            {
                string table = "UAMS_SessionLog";

                //Update the existing entry for this machine/ip if it exists.
                string updatesql = string.Format("UPDATE {0} SET LogoutStamp = GETDATE() " +
                    "WHERE LogoutStamp = 0 and Machine = " + Environment.MachineName + " and IpAddress = " + getIPAddress(), table);
                SqlHelper.ExecuteNonQuery(DBConnection.GetConnString(), CommandType.Text, updatesql);

                //Insert new entry for this logon event
                string insertsql = string.Format("INSERT INTO {0} (LoginStamp, LogoutStamp, UserName, Machine, IpAddress) " +
                    "VALUES (GETDATE(), 0, " + username + ", " + Environment.MachineName + ", " + getIPAddress() + ")", table);
                SqlHelper.ExecuteNonQuery(DBConnection.GetConnString(), CommandType.Text, insertsql);

                m_logger.DebugFormat("Logged LogOn event for {0} at {1}", username, getIPAddress());

            }

            //LogOff Event
            else if (changeDescription.Reason == SessionChangeReason.SessionLogoff)
            {
                string table = "UAMS_SessionLog";

                string updatesql = string.Format("UPDATE {0} SET LogoutStamp = GETDATE() " +
                    "WHERE LogoutStamp = 0 AND UserName = " + username + " AND Machine = " + Environment.MachineName + " AND IpAddress = " + getIPAddress(), table);
                SqlHelper.ExecuteNonQuery(DBConnection.GetConnString(), CommandType.Text, updatesql);

                m_logger.DebugFormat("Logged LogOff event for {0} at {1}", username, getIPAddress());
            }

            return true;

        }
        
        //Returns the IPv4 address of the current machine
        private string getIPAddress()
        {
            IPAddress[] ipList = Dns.GetHostAddresses("");

            // Grab the first IPv4 address in the list
            foreach (IPAddress addr in ipList)
            {
                if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return addr.ToString();
                }
            }
            return "-INVALID IP ADDRESS-";
        }
    }
}
