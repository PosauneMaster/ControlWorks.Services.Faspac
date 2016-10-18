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

            var locator = new TypeRepository();

            var pviApp = locator.GetInstance<IPviProcessor>();
            var factory = new TaskFactory();
            factory.StartNew(() => pviApp.Connect(), TaskCreationOptions.LongRunning);

            WebApiApplication.Locator = locator;
            WebApiApplication.Logger = Logger;
            WebApiApplication.Start();
        }

        public void Stop()
        {
            Logger.Log(new LogEntry(LoggingEventType.Information, "Host: Stop"));

        }
    }
}
