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
        [Route("api/variables/Add")]
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

        [HttpDelete]
        [Route("api/variables/Remove")]
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
                ex.Data.Add("VariableController.Operation", "Remove");
                WebApiApplication.Logger.Log(new LogEntry(LoggingEventType.Error, ex.Message, ex));
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("api/variables/Copy")]
        public async Task<IHttpActionResult> Copy(VariableCopyRequest request)
        {
            try
            {
                var variableProcessor = WebApiApplication.Locator.GetInstance<IVariableProcessor>();
                var response = await variableProcessor.Copy(request.Source, request.Destination);
                return Ok(response);
            }
            catch (Exception ex)
            {
                ex.Data.Add("VariableController.Operation", "Copy");
                WebApiApplication.Logger.Log(new LogEntry(LoggingEventType.Error, ex.Message, ex));
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("api/variables/Master")]
        public async Task<IHttpActionResult> Master(string[] request)
        {
            try
            {
                var variableProcessor = WebApiApplication.Locator.GetInstance<IVariableProcessor>();
                await variableProcessor.AddMaster(request);
                return Ok();
            }
            catch (Exception ex)
            {
                ex.Data.Add("VariableController.Operation", "Master");
                WebApiApplication.Logger.Log(new LogEntry(LoggingEventType.Error, ex.Message, ex));
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("api/variables/AddCpu")]
        public async Task<IHttpActionResult> AddCpu(string[] request)
        {
            try
            {
                var variableProcessor = WebApiApplication.Locator.GetInstance<IVariableProcessor>();
                await variableProcessor.AddCpuRange(request);
                return Ok();
            }
            catch (Exception ex)
            {
                ex.Data.Add("VariableController.Operation", "AddCpu");
                WebApiApplication.Logger.Log(new LogEntry(LoggingEventType.Error, ex.Message, ex));
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpDelete]
        [Route("api/variables/DeleteCpu")]
        public async Task<IHttpActionResult> DeleteCpu(string[] request)
        {
            try
            {
                var variableProcessor = WebApiApplication.Locator.GetInstance<IVariableProcessor>();
                await variableProcessor.RemoveCpuRange(request);
                return Ok();
            }
            catch (Exception ex)
            {
                ex.Data.Add("VariableController.Operation", "DeleteCpu");
                WebApiApplication.Logger.Log(new LogEntry(LoggingEventType.Error, ex.Message, ex));
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
