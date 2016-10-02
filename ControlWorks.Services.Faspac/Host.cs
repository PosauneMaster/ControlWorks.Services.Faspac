using ControlWorks.Logging;
using ControlWorks.Service.Rest;
using ControlWorks.Services.Configuration;
using log4net;
using System.Threading.Tasks;

namespace ControlWorks.Service.Faspac
{
    public class Host
    {
        protected ILogger Logger { get; }

        public Host() { }

        public Host(ILogger logger)
        {
            Logger = logger;
        }

        public void Start()
        {
            Logger.Log(new LogEntry(LoggingEventType.Information, "Host: Start"));

            Startup.Start(Logger);
        }

        public void Stop()
        {
            Logger.Log(new LogEntry(LoggingEventType.Information, "Host: Stop"));

        }
    }
}
