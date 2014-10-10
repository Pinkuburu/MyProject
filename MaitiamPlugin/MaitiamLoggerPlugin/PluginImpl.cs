using System;

using log4net;
using MaitiamLoggerPlugin;
using pGina.Shared.Interfaces;

//注册表搜索“ServicePipeName”
//添加“LocalAdminFallback”字符串值“False”
//解决gina验证失败回滚windows本地验证

namespace pGina.Plugin.MySqlLogger
{
    enum LoggerMode { EVENT, SESSION };

    public class PluginImpl : IPluginConfiguration, IPluginEventNotifications
    {
        private ILog m_logger = LogManager.GetLogger("MaitiamLoggerPlugin");
        public static Guid MaitiamUuid = new Guid("{7FD148C6-54FE-56D9-4E54-FBE6EB3534A3}");

        public string Name
        {
            get { return "Maitiam UAMS Logger"; }
        }

        public string Description
        {
            get { return "UAMS Logger System"; }
        }

        public string Version
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public Guid Uuid
        {
            get { return MaitiamUuid; }
        }

        public void Configure()
        {
            Configuration dlg = new Configuration();
            dlg.ShowDialog();
        }
        
        public void SessionChange(System.ServiceProcess.SessionChangeDescription changeDescription, pGina.Shared.Types.SessionProperties properties)
        {
            m_logger.DebugFormat("SessionChange({0}) - ID: {1}", changeDescription.Reason.ToString(), changeDescription.SessionId);
            m_logger.DebugFormat("Client IP:{0}", TSManager.ListSessions(changeDescription.SessionId));

            //If SessionMode is enabled, send event to it.
            if (true)
            {
                ILoggerMode mode = LoggerModeFactory.getLoggerMode(LoggerMode.SESSION);
                mode.Log(changeDescription, properties);
            }

            //If EventMode is enabled, send event to it.
            if (true)
            {
                ILoggerMode mode = LoggerModeFactory.getLoggerMode(LoggerMode.EVENT);
                mode.Log(changeDescription, properties);
            }

            //Close the connection if it's still open
            LoggerModeFactory.closeConnection();
        }

        public void Starting() { }
        public void Stopping() { }
    }
}
