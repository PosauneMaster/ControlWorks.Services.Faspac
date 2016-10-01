using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace ControlWorks.Logging
{
    public interface ILogger
    {
        void Log(LogEntry entry);
    }

    public enum LoggingEventType { Debug, Information, Warning, Error, Fatal };

    public class LogEntry
    {
        public readonly LoggingEventType Severity;
        public readonly string Message;
        public readonly Exception Exception;

        public LogEntry(LoggingEventType severity, string message, Exception exception = null)
        {
            this.Severity = severity;
            this.Message = message;
            this.Exception = exception;
        }
    }

    public static class LoggerExtensions
    {
        public static void Log(this ILogger logger, string message)
        {
            logger.Log(new LogEntry(LoggingEventType.Information, message));
        }

        public static void Log(this ILogger logger, Exception exception)
        {
            logger.Log(new LogEntry(LoggingEventType.Error, exception.Message, exception));
        }
    }

    public class Log4netAdapter : ILogger
    {
        private readonly log4net.ILog m_Adaptee;

        public Log4netAdapter(log4net.ILog adaptee)
        {
            m_Adaptee = adaptee;
        }

        public void Log(LogEntry entry)
        {
            if (entry.Severity == LoggingEventType.Information)
                m_Adaptee.Info(entry.Message, entry.Exception);
            else if (entry.Severity == LoggingEventType.Warning)
                m_Adaptee.Warn(entry.Message, entry.Exception);
            else if (entry.Severity == LoggingEventType.Error)
                m_Adaptee.Error(entry.Message, entry.Exception);
            else
                m_Adaptee.Fatal(entry.Message, entry.Exception);
        }
    }
}
