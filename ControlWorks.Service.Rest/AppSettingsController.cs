using System;
using ControlWorks.Logging;
using ControlWorks.Services.Business;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ControlWorks.Services.Data;
namespace ControlWorks.Service.Rest
{
    public class AppSettingsController : ApiController
    {
        [HttpPut]
        public async Task<IHttpActionResult> ChangePort(int id)
        {
            try
            {
                var configProcessor = WebApiApplication.Locator.GetInstance<IConfigurationProcessor>();

                var response = await configProcessor.ChangePort(id);

                if (response == null)
                {
                    var message = "Encountered an error updating Port";
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                ex.Data.Add("AppSettingsController.Operation", "ChangePort");
                WebApiApplication.Logger.Log(new LogEntry(LoggingEventType.Error, ex.Message, ex));
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
