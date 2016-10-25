using ControlWorks.Logging;
using ControlWorks.Services.Data;
using ControlWorks.Services.Pvi;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlWorks.Services.Business
{
    public interface IServiceProcessor
    {
        Task<ServiceDetail> GetServiceDetails();
    }
    public class ServiceProcessor : BaseProcessor, IServiceProcessor
    {
        IPviApplication _application;
        ILogger logger = new Log4netAdapter(LogManager.GetLogger("ServiceLogger"));

        public ServiceProcessor() { }

        public ServiceProcessor(IPviApplication application)
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
    }
}
