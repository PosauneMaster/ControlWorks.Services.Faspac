using ControlWorks.Logging;
using ControlWorks.Services.Configuration;
using log4net;
using System;
using Topshelf;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace ControlWorks.Service.Faspac
{
    class Program
    {
        static void Main(string[] args)
        {
            ILog log = LogManager.GetLogger(ConfigurationProvider.ServiceLoggerName);
            log.Info("Hello");

            ILogger logger = new Log4netAdapter(LogManager.GetLogger("ServiceLogger"));

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            const string name = "ControlWorksFaspacService";
            const string description = "Control Works communication service";

            logger.Log(new LogEntry(LoggingEventType.Information, $"Initializing Service {name} - {description}"));

            try
            {
                var host = HostFactory.New(configuration =>
                {
                    configuration.Service<Host>(callback =>
                    {
                        callback.ConstructUsing(s => new Host(logger));
                        callback.WhenStarted(service => service.Start());
                        callback.WhenStopped(service => service.Stop());
                    });
                    configuration.SetDisplayName(name);
                    configuration.SetServiceName(name);
                    configuration.SetDescription(description);
                    configuration.RunAsLocalService();
                });
                host.Run();
            }
            catch (Exception ex)
            {
                logger.Log(new LogEntry(LoggingEventType.Fatal, "ControlWorksFaspacService Service fatal exception."));
                logger.Log(new LogEntry(LoggingEventType.Fatal, ex.Message, ex));
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ILog Log = LogManager.GetLogger(ConfigurationProvider.ServiceLoggerName);
            Log.Fatal("Unhandled Application Domain Error");
            var ex = e.ExceptionObject as Exception;
            Log.Fatal(ex.Message, ex);
        }
    }
}
