using ControlWorks.Logging;
using ControlWorks.Services.Business;
using ControlWorks.Services.Configuration;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using Owin;
using System.Web.Http;

namespace ControlWorks.Service.Rest
{
    public class WebApiApplication
    {
        public static TypeRepository Locator { get; set; }
        public static ILogger Logger { get; set; }

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

            // Configure Web API for self-host. 
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ActionRoute",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );


            app.UseWebApi(config);
        }

        public static void Start()
        {
            var hostUrl = $"http://*:{ConfigurationProvider.Port}";
            Logger.Log(new LogEntry(LoggingEventType.Information, $"Starting WebApi at host {hostUrl}"));

            WebApp.Start<WebApiApplication>(hostUrl);

        }
    }
}
