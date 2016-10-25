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
    public class VariablesController : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> GetVariables()
        {
            try
            {
                var variableProcessor = WebApiApplication.Locator.GetInstance<IVariableProcessor>();

                var settings = await variableProcessor.GetAll();

                if (settings == null)
                {
                    var message = "Variables not found";
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
                }
                return Ok(settings);
            }
            catch (Exception ex)
            {
                ex.Data.Add("VariableController.Operation", "GetVariables");
                WebApiApplication.Logger.Log(new LogEntry(LoggingEventType.Error, ex.Message, ex));
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetByCpuName(string id)
        {
            try
            {
                var variableProcessor = WebApiApplication.Locator.GetInstance<IVariableProcessor>();

                var settings = await variableProcessor.FindByCpuName(id);

                if (settings == null)
                {
                    var message = "Variables not found";
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
                }
                return Ok(settings);
            }
            catch (Exception ex)
            {
                ex.Data.Add("VariableController.Operation", "GetByCpuName");
                WebApiApplication.Logger.Log(new LogEntry(LoggingEventType.Error, ex.Message, ex));
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IHttpActionResult> Add(VariableDetailRequest request)
        {
            try
            {
                var variableProcessor = WebApiApplication.Locator.GetInstance<IVariableProcessor>();
                await variableProcessor.Add(request.CpuName, request.Variables);
                return Ok();
            }
            catch (Exception ex)
            {
                ex.Data.Add("VariableController.Operation", "Add");
                WebApiApplication.Logger.Log(new LogEntry(LoggingEventType.Error, ex.Message, ex));
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPut]
        public async Task<IHttpActionResult> Remove(VariableDetailRequest request)
        {
            try
            {
                var variableProcessor = WebApiApplication.Locator.GetInstance<IVariableProcessor>();
                await variableProcessor.Remove(request.CpuName, request.Variables);
                return Ok();
            }
            catch (Exception ex)
            {
                ex.Data.Add("VariableController.Operation", "Add");
                WebApiApplication.Logger.Log(new LogEntry(LoggingEventType.Error, ex.Message, ex));
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("Copy")]
        public async Task<IHttpActionResult> Copy(VariableCopyRequest request)
        {
            try
            {
                var variableProcessor = WebApiApplication.Locator.GetInstance<IVariableProcessor>();
                var response = await variableProcessor.Copy(request.Source, request.Destination);
                return Ok();
            }
            catch (Exception ex)
            {
                ex.Data.Add("VariableController.Operation", "Add");
                WebApiApplication.Logger.Log(new LogEntry(LoggingEventType.Error, ex.Message, ex));
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
