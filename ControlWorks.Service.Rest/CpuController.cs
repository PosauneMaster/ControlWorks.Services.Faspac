using ControlWorks.Logging;
using ControlWorks.Services.Business;
using ControlWorks.Services.Data;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ControlWorks.Service.Rest
{
    public class CpuController : ApiController
    {

        [HttpGet]
        public async Task<IHttpActionResult> GetDetails()
        {
            try
            {
                var requestProcessor = WebApiApplication.Locator.GetInstance<IRequestProcessor>();

                var details = await requestProcessor.GetCpuDetails();

                if (details == null)
                {
                    var message = "Cpu services not found";
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
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

        [HttpGet]
        public async Task<IHttpActionResult> FindByName(string id)
        {
            try
            {
                var requestProcessor = WebApiApplication.Locator.GetInstance<IRequestProcessor>();

                var details = await requestProcessor.GetCpuByName(id);

                if (details == null)
                {
                    var message = "Cpu not found";
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
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

        [HttpGet]
        public async Task<IHttpActionResult> FindByIp(string id)
        {
            try
            {
                var requestProcessor = WebApiApplication.Locator.GetInstance<IRequestProcessor>();

                var details = await requestProcessor.GetCpuByIp(id);

                if (details == null)
                {
                    var message = "Cpu not found";
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
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

        [HttpPost]
        public async Task<IHttpActionResult> Add(CpuInfoRequest info)
        {
            try
            {
                var requestProcessor = WebApiApplication.Locator.GetInstance<IRequestProcessor>();

                await requestProcessor.Add(info);

                return Ok();
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

