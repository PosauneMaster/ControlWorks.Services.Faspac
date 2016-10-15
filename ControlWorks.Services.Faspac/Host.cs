using ControlWorks.Logging;
using ControlWorks.Service.Rest;
using ControlWorks.Services.Business;
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

            var pviApp = ServiceLocator.GetService<IPviProcessor>();
            var factory = new TaskFactory();
            factory.StartNew(() => pviApp.Connect(), TaskCreationOptions.LongRunning);
            Startup.Start(Logger);
        }

        public void Stop()
        {
            Logger.Log(new LogEntry(LoggingEventType.Information, "Host: Stop"));

        }
    }
}
