using System;

namespace MaitiamPlugin
{
    interface ILoggerMode
    {
        //Logs the specified event/properties
        bool Log(System.ServiceProcess.SessionChangeDescription changeDescription, pGina.Shared.Types.SessionProperties properties);
    }

    class LoggerModeFactory
    {
        private LoggerModeFactory() { }

        public static ILoggerMode getLoggerMode(LoggerMode mode)
        {

            ILoggerMode logger = null;
            if (mode == LoggerMode.EVENT)
                logger = new EventLoggerMode();
            else if (mode == LoggerMode.SESSION)
                logger = new SessionLogger();
            else
                throw new ArgumentException("Invalid LoggerMode");

            return logger;
        }
    }
}
