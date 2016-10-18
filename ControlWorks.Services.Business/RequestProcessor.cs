using ControlWorks.Logging;
using ControlWorks.Services.Data;
using ControlWorks.Services.Pvi;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlWorks.Services.Business
{
    public interface IRequestProcessor
    {
        ServiceDetail GetServiceDetails();
        Task<List<CpuDetailResponse>> GetCpuDetails();
        CpuDetailResponse GetCpuByName(string name);
        CpuDetailResponse GetCpuByIp(string ip);
        void Add(CpuInfoRequest request);

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
        public ServiceDetail GetServiceDetails()
        {
            logger.Log(new LogEntry(LoggingEventType.Information, "RequestProcessor Operation=GetServiceDetails"));

            var result = _application.GetServiceDetails();

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

        public CpuDetailResponse GetCpuByName(string name)
        {
            return _application.GetCpuByName(name);
        }

        public CpuDetailResponse GetCpuByIp(string ip)
        {
            return _application.GetCpuByIp(ip);
        }

        public void Add(CpuInfoRequest request)
        {
            var info = new CpuInfo()
            {
                Name = request.Name,
                Description = request.Description,
                IpAddress = request.IpAddress
            };

            _application.AddCpu(info);
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
