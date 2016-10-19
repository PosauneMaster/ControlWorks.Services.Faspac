using System;
using ControlWorks.Logging;
using ControlWorks.Services.Business;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ControlWorks.Service.Rest
{
    public class SettingsController : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> GetSettings()
        {
            try
            {
                var configProcessor = WebApiApplication.Locator.GetInstance<IConfigurationProcessor>();

                var settings = await configProcessor.GetSettings();

                if (settings == null)
                {
                    var message = "Settings not found";
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
                }
                return Ok(settings);
            }
            catch (Exception ex)
            {
                ex.Data.Add("SettingsController.Operation", "GetSettings");
                WebApiApplication.Logger.Log(new LogEntry(LoggingEventType.Error, ex.Message, ex));
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
}
    }
}
