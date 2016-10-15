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
                var requestPricessor = ServiceLocator.GetService<IRequestProcessor>();
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
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
