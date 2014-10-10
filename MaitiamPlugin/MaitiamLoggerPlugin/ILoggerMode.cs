using System;
using System.ServiceProcess;
using System.Data.SqlClient;

using pGina.Shared.Types;

namespace pGina.Plugin.MySqlLogger
{
    interface ILoggerMode
    {
        //Logs the specified event/properties
        bool Log(SessionChangeDescription changeDescription, SessionProperties properties);

        //Sets the connection to the Sql server, so that multiple loggers can share one stream
        void SetConnection(SqlConnection m_conn);
    }

    class LoggerModeFactory
    {
        static private SqlConnection m_conn = null;

        private LoggerModeFactory() { }

        public static ILoggerMode getLoggerMode(LoggerMode mode)
        {
            if (m_conn == null || m_conn.State != System.Data.ConnectionState.Open)
            {
                string connStr = BuildConnectionString();
                m_conn = new SqlConnection(connStr);
            }

            ILoggerMode logger = null;
            if (mode == LoggerMode.EVENT)
                logger = new EventLoggerMode();
            else if (mode == LoggerMode.SESSION)
                logger = new SessionLogger();
            else
                throw new ArgumentException("Invalid LoggerMode");

            logger.SetConnection(m_conn);
            return logger;
        }

        public static void closeConnection()
        {
            if (m_conn != null)
                m_conn.Close();
            m_conn = null;
        }

        private static string BuildConnectionString()
        {
            SqlConnectionStringBuilder bldr = new SqlConnectionStringBuilder();
            bldr.DataSource = "ishow.xba.com.cn,1595";
            bldr.InitialCatalog = "Maitiam_UAMS";
            bldr.UserID = "cupid";
            bldr.Password = "qweqwe123";

            return bldr.ToString();
        }
    }
}
