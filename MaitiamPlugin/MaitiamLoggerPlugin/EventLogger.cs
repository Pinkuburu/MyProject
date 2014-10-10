using System;
using System.Net;
using System.ServiceProcess;
using System.Data.SqlClient;

using log4net;

using pGina.Shared.Types;
using MaitiamLoggerPlugin;

namespace pGina.Plugin.MySqlLogger
{
    class EventLoggerMode : ILoggerMode
    {
        private ILog m_logger = LogManager.GetLogger("MaitiamLoggerPlugin");
        public static readonly string UNKNOWN_USERNAME = "--Unknown--";
        private SqlConnection m_conn;
        public string strClientIP = null;
        public int intSessionID = -1;

        public EventLoggerMode() { }

        //Logs the event if it's an event we track according to the registry.
        public bool Log(SessionChangeDescription changeDescription, SessionProperties properties)
        {
            //Get the logging message for this event.
            string msg = null;
            switch (changeDescription.Reason)
            {
                case System.ServiceProcess.SessionChangeReason.SessionLogon:
                    msg = LogonEvent(changeDescription.SessionId, properties);
                    break;
                case System.ServiceProcess.SessionChangeReason.SessionLogoff:
                    msg = LogoffEvent(changeDescription.SessionId, properties);
                    break;
                case System.ServiceProcess.SessionChangeReason.SessionLock:
                    msg = SessionLockEvent(changeDescription.SessionId, properties);
                    break;
                case System.ServiceProcess.SessionChangeReason.SessionUnlock:
                    msg = SessionUnlockEvent(changeDescription.SessionId, properties);
                    break;
                case System.ServiceProcess.SessionChangeReason.SessionRemoteControl:
                    msg = SesionRemoteControlEvent(changeDescription.SessionId, properties);
                    break;
                case System.ServiceProcess.SessionChangeReason.ConsoleConnect:
                    msg = ConsoleConnectEvent(changeDescription.SessionId, properties);
                    break;
                case System.ServiceProcess.SessionChangeReason.ConsoleDisconnect:
                    msg = ConsoleDisconnectEvent(changeDescription.SessionId, properties);
                    break;
                case System.ServiceProcess.SessionChangeReason.RemoteConnect:
                    msg = RemoteConnectEvent(changeDescription.SessionId, properties);
                    break;
                case System.ServiceProcess.SessionChangeReason.RemoteDisconnect:
                    msg = RemoteDisconnectEvent(changeDescription.SessionId, properties);
                    break;
            }

            m_logger.DebugFormat("SessionChange({0}) - Message: {1}", changeDescription.Reason.ToString(), msg);            

            //Check if there is a message to log
            if (!string.IsNullOrEmpty(msg))
            {
                if (m_conn == null)
                    throw new InvalidOperationException("No SQL Connection present.");

                //Send it to the server
                logToServer(msg);
            }
            return true; //No msg to log
        }

        //Provides the SQL connection to use
        public void SetConnection(SqlConnection m_conn)
        {
            this.m_conn = m_conn;
        }

        //Connects to the server and logs the message.
        private bool logToServer(string message)
        {
            if (m_conn.State != System.Data.ConnectionState.Open)
                m_conn.Open();

            string hostName = Dns.GetHostName();
            string table = "UAMS_EventLog";            

            // Prepare statement
            string machine = Environment.MachineName;

            m_logger.DebugFormat("Logging: {0}", message);
            string sql = String.Format("INSERT INTO {0}(TimeStamp, Host, Ip, Machine, Message, SessionID, ClientIP) " +
                "VALUES (GETDATE(), '" + hostName + "', '" + getIPAddress() + "', '" + machine + "', '" + message + "', " + this.intSessionID + ", '" + this.strClientIP + "')", table);
            m_logger.DebugFormat("Logging: {0}", message);
            SqlCommand m_command = new SqlCommand(sql, m_conn);
            m_command.Prepare();            
            m_command.ExecuteNonQuery();

            m_logger.DebugFormat("Event logged: {1}", message);

            return true;
        }

        //Returns the current IPv4 address
        private string getIPAddress()
        {
            IPAddress[] ipList = Dns.GetHostAddresses("");
            string m_ip = "";
            // Grab the first IPv4 address in the list
            foreach (IPAddress addr in ipList)
            {
                if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    m_ip = addr.ToString();
                    break;
                }
            }
            return m_ip;
        }


        //The following functions determine if the specified event should be logged, and returns the logging message if so
        private string LogonEvent(int sessionId, SessionProperties properties)
        {
            bool okToLog = true;

            // Get the username
            string userName = getUsername(properties);

            // Since the username is not available at logoff time, we cache it
            // (tied to the session ID) so that we can get it back at the logoff
            // event.
            //if (userName != null)
            //    m_usernameCache.Add(sessionId, userName);
            if (userName == null)
                userName = UNKNOWN_USERNAME;

            if (okToLog)
                this.intSessionID = sessionId;
                this.strClientIP = TSManager.ListSessions(sessionId);
                return string.Format("[{0}] Logon user: {1}", sessionId, userName);

            return "";
        }

        private string LogoffEvent(int sessionId, SessionProperties properties)
        {
            bool okToLog = true;
            string userName = "";

            userName = getUsername(properties);
            // Delete the username from the cache because we are logging off?

            if (userName == null)
                userName = UNKNOWN_USERNAME;

            if (okToLog)
                this.intSessionID = sessionId;
                this.strClientIP = TSManager.ListSessions(sessionId);
                return string.Format("[{0}] Logoff user: {1}", sessionId, userName);

            return "";
        }

        private string ConsoleConnectEvent(int sessionId, SessionProperties properties)
        {
            bool okToLog = true;

            if (okToLog)
                this.intSessionID = sessionId;
                this.strClientIP = TSManager.ListSessions(sessionId);
                return string.Format("[{0}] Console connect", sessionId);

            return "";
        }

        private string ConsoleDisconnectEvent(int sessionId, SessionProperties properties)
        {
            bool okToLog = true;

            if (okToLog)
                this.intSessionID = sessionId;
                this.strClientIP = TSManager.ListSessions(sessionId);
                return string.Format("[{0}] Console disconnect", sessionId);

            return "";
        }

        private string RemoteDisconnectEvent(int sessionId, SessionProperties properties)
        {
            bool okToLog = true;
            string userName = "";

            userName = getUsername(properties);
            if (userName == null)
                userName = UNKNOWN_USERNAME;


            if (okToLog)
                this.intSessionID = sessionId;
                this.strClientIP = TSManager.ListSessions(sessionId);
                return string.Format("[{0}] Remote disconnect user: {1}", sessionId, userName);

            return "";
        }

        private string RemoteConnectEvent(int sessionId, SessionProperties properties)
        {
            bool okToLog = true;
            string userName = "";

            userName = getUsername(properties);
            if (userName == null)
                userName = UNKNOWN_USERNAME;


            if (okToLog)
                this.intSessionID = sessionId;
                this.strClientIP = TSManager.ListSessions(sessionId);
                return string.Format("[{0}] Remote connect user: {1}", sessionId, userName);

            return "";
        }

        private string SesionRemoteControlEvent(int sessionId, SessionProperties properties)
        {
            bool okToLog = true;
            string userName = "";

            userName = getUsername(properties);
            if (userName == null)
                userName = UNKNOWN_USERNAME;


            if (okToLog)
                this.intSessionID = sessionId;
                this.strClientIP = TSManager.ListSessions(sessionId);
                return string.Format("[{0}] Remote control user: {1}", sessionId, userName);

            return "";
        }

        private string SessionUnlockEvent(int sessionId, SessionProperties properties)
        {
            bool okToLog = true;
            string userName = "";

            userName = getUsername(properties);
            if (userName == null)
                userName = UNKNOWN_USERNAME;


            if (okToLog)
                this.intSessionID = sessionId;
                this.strClientIP = TSManager.ListSessions(sessionId);
                return string.Format("[{0}] Session unlock user: {1}", sessionId, userName);

            return "";
        }

        private string SessionLockEvent(int sessionId, SessionProperties properties)
        {
            bool okToLog = true;
            string userName = "";

            userName = getUsername(properties);
            if (userName == null)
                userName = UNKNOWN_USERNAME;


            if (okToLog)
                this.intSessionID = sessionId;
                this.strClientIP = TSManager.ListSessions(sessionId);
                return string.Format("[{0}] Session lock user: {1}", sessionId, userName);

            return "";
        }

        private string getUsername(SessionProperties properties)
        {
            if (properties == null)
                return UNKNOWN_USERNAME;

            bool useModifiedName = true;
            UserInformation userInfo = properties.GetTrackedSingle<UserInformation>();
            if (useModifiedName)
                return userInfo.Username;
            else
                return userInfo.OriginalUsername;
        }
    }
}
