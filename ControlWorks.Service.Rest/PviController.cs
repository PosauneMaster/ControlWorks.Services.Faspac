using ControlWorks.Logging;
using ControlWorks.Services.Business;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ControlWorks.Service.Rest
{
    public class PviController : ApiController
    {
        public IHttpActionResult GetDetails()
        {
            try
            {
                var requestPricessor = WebApiApplication.Locator.GetInstance<IRequestProcessor>();

                var details = requestPricessor.GetServiceDetails();

                if (details == null)
                {
                    var message = "Pvi Service not found";
                    return  ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
                }

                return Ok(details);
            }
            catch (Exception ex)
            {
                ex.Data.Add("PviController.Operation", "GetDetails");
                WebApiApplication.Logger.Log(new LogEntry(LoggingEventType.Error, ex.Message, ex));
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
