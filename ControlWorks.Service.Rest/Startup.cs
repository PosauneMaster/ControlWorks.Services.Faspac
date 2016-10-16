using ControlWorks.Logging;
using ControlWorks.Services.Configuration;
using Microsoft.Owin.Hosting;
using Owin;
using System.Web.Http;

namespace ControlWorks.Service.Rest
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            // Configure Web API for self-host. 
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            app.UseWebApi(config);
        }

        public static void Start(ILogger logger)
        {
            var hostUrl = $"http://*:{ConfigurationProvider.Port}";
            logger.Log(new LogEntry(LoggingEventType.Information, $"Starting WebApi at host {hostUrl}"));

            WebApp.Start<Startup>(hostUrl);

        }
    }
}
