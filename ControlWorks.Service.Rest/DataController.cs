using ControlWorks.Logging;
using ControlWorks.Services.Business;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;


namespace ControlWorks.Service.Rest
{
    public class DataController : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> GetData(string id)
        {
            try
            {
                var requestProcessor = WebApiApplication.Locator.GetInstance<IDataProcessor>();

                var details = await requestProcessor.GetCpuData(id);

                if (details == null)
                {
                    var message = "Cpu data not found";
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
                }

                return Ok(details);
            }
            catch (Exception ex)
            {
                ex.Data.Add("PviController.Operation", "GetData");
                WebApiApplication.Logger.Log(new LogEntry(LoggingEventType.Error, ex.Message, ex));
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
