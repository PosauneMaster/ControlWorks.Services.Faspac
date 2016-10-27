using ControlWorks.Services.Data;
using ControlWorks.Services.Pvi;
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
        ILogger _logger;

        public DataProcessor(IPviApplication)
        {

        }


        public async Task<DataResponse> GetCpuData(string cpuName)
        {
            _logger.Log(new LogEntry(LoggingEventType.Information,
                $"VariableProcessor Operation=Copy. source={source}; destination={destination}"));

            var response = await _variableApi.Copy(source, destination);

            _logger.Log(new LogEntry(LoggingEventType.Debug, ToJson(response)));

            return response;
        }
    }
}
