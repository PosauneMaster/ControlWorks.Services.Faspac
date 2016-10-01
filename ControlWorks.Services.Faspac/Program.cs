using ControlWorks.Services.Configuration;
using log4net;
using System;
using Topshelf;

namespace ControlWorks.Service.Faspac
{
    class Program
    {
        protected static ILog Log = LogManager.GetLogger(ConfigurationProvider.ServiceLoggerName); 

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            const string name = "ControlWorksFaspacService";
            const string description = "Control Works communication service";

            Log.Info($"Initializing Service {name} - {description}");

            try
            {
                var host = HostFactory.New(configuration =>
                {
                    configuration.Service<Host>(callback =>
                    {
                        callback.ConstructUsing(s => new Host());
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
                Log.Fatal("ControlWorksFaspacService Service fatal exception.");
                Log.Fatal(ex.Message, ex);
            }

        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Fatal("Unhandled Application Domain Error");
            var ex = e.ExceptionObject as Exception;
            Log.Fatal(ex.Message, ex);
        }
    }
}
