using System;
using System.Net;
using System.ServiceProcess;
using System.Data.SqlClient;

using log4net;

using pGina.Shared.Types;
using MaitiamLoggerPlugin;


namespace pGina.Plugin.MySqlLogger
{
    class SessionLogger : ILoggerMode
    {
        private ILog m_logger = LogManager.GetLogger("MaitiamLoggerPlugin");
        private SqlConnection m_conn;

        public SessionLogger() { }

        //Logs the session if it's a LogOn or LogOff event.
        public bool Log(SessionChangeDescription changeDescription, SessionProperties properties)
        {
            if (m_conn == null)
                throw new InvalidOperationException("No SQL Connection present.");

            string username = "--UNKNOWN--";
            string strClientIP = null;
            strClientIP = TSManager.ListSessions(changeDescription.SessionId);

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
                if (m_conn.State != System.Data.ConnectionState.Open)
                    m_conn.Open();
                
                string table = "UAMS_SessionLog";

                //Update the existing entry for this machine/ip if it exists.
                string updatesql = string.Format("UPDATE {0} SET LogoutStamp = GETDATE() " +
                    "WHERE LogoutStamp IS NULL and Machine = '" + Environment.MachineName + "' and IpAddress = '" + getIPAddress() + "' and SessionID = " + changeDescription.SessionId + " and ClientIP = '" + strClientIP + "'", table);

                SqlCommand cmd = new SqlCommand(updatesql, m_conn);
                cmd.Prepare();
                cmd.ExecuteNonQuery();

                //Insert new entry for this logon event
                string insertsql = string.Format("INSERT INTO {0} (LoginStamp, LogoutStamp, UserName, Machine, IpAddress, SessionID, ClientIP) " +
                    "VALUES (GETDATE(), NULL, '" + username + "', '" + Environment.MachineName + "', '" + getIPAddress() + "', " + changeDescription.SessionId + ", '" + strClientIP + "')", table);

                cmd = new SqlCommand(insertsql, m_conn);
                cmd.Prepare();
                cmd.ExecuteNonQuery();

                m_logger.DebugFormat("Logged LogOn event for {0} at {1}", username, strClientIP);

            }

            //LogOff Event
            else if (changeDescription.Reason == SessionChangeReason.SessionLogoff)
            {
                if (m_conn.State != System.Data.ConnectionState.Open)
                    m_conn.Open();

                string table = "UAMS_SessionLog";

                string updatesql = string.Format("UPDATE {0} SET LogoutStamp = GETDATE() " +
                    "WHERE LogoutStamp IS NULL AND UserName = '" + username + "' AND Machine = '" + Environment.MachineName + "' AND IpAddress = '" + getIPAddress() + "' and SessionID = " + changeDescription.SessionId + "", table); //and ClientIP = '" + strClientIP + "'
                //m_logger.DebugFormat("Logoff:{0}", updatesql);
                SqlCommand cmd = new SqlCommand(updatesql, m_conn);
                cmd.Prepare();
                cmd.ExecuteNonQuery();

                m_logger.DebugFormat("Logged LogOff event for {0} at {1}", username, strClientIP);
            }

            return true;

        }

        //Provides the MySQL connection to use
        public void SetConnection(SqlConnection m_conn)
        {
            this.m_conn = m_conn;
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
