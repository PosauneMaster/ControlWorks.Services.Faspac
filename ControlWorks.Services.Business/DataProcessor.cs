using ControlWorks.Logging;
using ControlWorks.Services.Configuration;
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
    public interface IDataProcessor
    {
        Task<DataResponse> GetCpuData(string cpuName);
    }

    public class DataProcessor : BaseProcessor, IDataProcessor
    {
        IPviApplication _pviApplication;
        ILogger _logger;

        public DataProcessor(IPviApplication pviApplication)
        {
            _pviApplication = pviApplication;

            var loggerName = ConfigurationProvider.ServiceLoggerName;
            _logger = new Log4netAdapter(LogManager.GetLogger(loggerName));

        }

        public async Task<DataResponse> GetCpuData(string cpuName)
        {
            _logger.Log(new LogEntry(LoggingEventType.Information,
                $"VariableProcessor Operation=GetCpuData. cpuName={cpuName}"));

            var response = await _pviApplication.GetCpuData(cpuName);

            _logger.Log(new LogEntry(LoggingEventType.Debug, ToJson(response)));

            return response;
        }
    }
}
