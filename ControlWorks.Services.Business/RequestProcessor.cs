using ControlWorks.Logging;
using ControlWorks.Services.Data;
using ControlWorks.Services.Pvi;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ControlWorks.Services.Business
{
    public interface IRequestProcessor
    {
        Task<ServiceDetail> GetServiceDetails();
        Task<List<CpuDetailResponse>> GetCpuDetails();
        Task<CpuDetailResponse> GetCpuByName(string name);
        Task<CpuDetailResponse> GetCpuByIp(string ip);
        Task Add(CpuInfoRequest request);
    }
    public class RequestProcessor : IRequestProcessor
    {
        IPviApplication _application;
        ILogger logger = new Log4netAdapter(LogManager.GetLogger("ServiceLogger"));

        public RequestProcessor() { }

        public RequestProcessor(IPviApplication application)
        {
            _application = application;
        }
        public async Task<ServiceDetail> GetServiceDetails()
        {
            logger.Log(new LogEntry(LoggingEventType.Information, "RequestProcessor Operation=GetServiceDetails"));

            var result = await _application.GetServiceDetails();

            logger.Log(new LogEntry(LoggingEventType.Debug, ToJson(result)));

            return result;
        }
        public async Task<List<CpuDetailResponse>> GetCpuDetails()
        {
            logger.Log(new LogEntry(LoggingEventType.Information, "RequestProcessor Operation=GetCpuDetails"));

            var result = await _application.GetCpuDetails();

            logger.Log(new LogEntry(LoggingEventType.Debug, ToJson(result)));

            return result;
        }

        public async Task<CpuDetailResponse> GetCpuByName(string name)
        {
            logger.Log(new LogEntry(LoggingEventType.Information, $"RequestProcessor Operation=GetCpuByName Name={name}"));

            var result = await _application.GetCpuByName(name);

            logger.Log(new LogEntry(LoggingEventType.Debug, ToJson(result)));

            return result;

        }

        public async Task<CpuDetailResponse> GetCpuByIp(string ip)
        {
            logger.Log(new LogEntry(LoggingEventType.Information, $"RequestProcessor Operation=GetCpuByIp IP={ip}"));

            var result = await _application.GetCpuByIp(ip);

            logger.Log(new LogEntry(LoggingEventType.Debug, ToJson(result)));

            return result;
        }

        public async Task Add(CpuInfoRequest request)
        {
            var info = new CpuInfo()
            {
                Name = request.Name,
                Description = request.Description,
                IpAddress = request.IpAddress
            };

            logger.Log(new LogEntry(LoggingEventType.Information, $"RequestProcessor Operation=Add request={ToJson(request)}"));

            await _application.AddCpu(info);
        }

        private string ToJson(object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch { return String.Empty; }
        }
    }
}
