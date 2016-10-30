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
    public interface IVariableProcessor
    {
        Task<List<VariableDetailRespose>> GetAll();
        Task<VariableDetailRespose> FindByCpuName(string name);
        Task Add(string cpuName, IEnumerable<string> variables);
        Task Remove(string cpuName, IEnumerable<string> variables);
        Task<VariableDetailRespose> Copy(string source, string destination);
        Task AddMaster(string[] variables);
        Task AddCpuRange(string[] cpus);
        Task RemoveCpuRange(string[] cpus);

    }
    public class VariableProcessor : BaseProcessor, IVariableProcessor
    {
        const string VARIABLE_MASTER = "VARIABLE_MASTER";

        IVariableApi _variableApi;
        ILogger _logger;

        public VariableProcessor() { }
        public VariableProcessor(IVariableApi variableApi)
        {
            _variableApi = variableApi;

            var loggerName = ConfigurationProvider.ServiceLoggerName;
            _logger = new Log4netAdapter(LogManager.GetLogger(loggerName));
        }


        public async Task<List<VariableDetailRespose>> GetAll()
        {
            _logger.Log(new LogEntry(LoggingEventType.Information, "VariableProcessor Operation=GetAll"));

            var response = await  _variableApi.GetAll();

            _logger.Log(new LogEntry(LoggingEventType.Debug, ToJson(response)));

            return response;
        }

        public async Task<VariableDetailRespose> FindByCpuName(string name)
        {
            _logger.Log(new LogEntry(LoggingEventType.Information, $"VariableProcessor Operation=FindByCpuName. name={name}"));

            var response = await _variableApi.FindByCpuNameAsync(name);

            _logger.Log(new LogEntry(LoggingEventType.Debug, ToJson(response)));

            return response;
        }

        public async Task Add(string cpuName, IEnumerable<string> variables)
        {
            _logger.Log(new LogEntry(LoggingEventType.Information, 
                $"VariableProcessor Operation=Add. cpuName={cpuName}; variables={String.Join(",", variables)}"));

            await _variableApi.AddRange(cpuName, variables);

        }

        public async Task Remove(string cpuName, IEnumerable<string> variables)
        {
            _logger.Log(new LogEntry(LoggingEventType.Information,
                $"VariableProcessor Operation=Remove. cpuName={cpuName}; variables={String.Join(",", variables)}"));

            await _variableApi.RemoveRange(cpuName, variables);
        }

        public async Task<VariableDetailRespose> Copy(string source, string destination)
        {
            _logger.Log(new LogEntry(LoggingEventType.Information,
                $"VariableProcessor Operation=Copy. source={source}; destination={destination}"));

            var response = await _variableApi.Copy(source, destination);

            _logger.Log(new LogEntry(LoggingEventType.Debug, ToJson(response)));

            return response;
        }

        public async Task AddMaster(string[] variables)
        {
            _logger.Log(new LogEntry(LoggingEventType.Information,
                $"VariableProcessor Operation=AddMaster. cpuName={VARIABLE_MASTER},  variables={String.Join(",", variables)}"));

            await _variableApi.AddRange(VARIABLE_MASTER, variables);
        }

        public async Task AddCpuRange(string[] cpus)
        {
            _logger.Log(new LogEntry(LoggingEventType.Information,
                $"VariableProcessor Operation=AddCpuRange. cpu list={String.Join(",", cpus)}"));

            await _variableApi.AddCpuRange(cpus);
        }

        public async Task RemoveCpuRange(string[] cpus)
        {
            _logger.Log(new LogEntry(LoggingEventType.Information,
                $"VariableProcessor Operation=RemoveCpuRange. cpu list={String.Join(",", cpus)}"));

            await _variableApi.RemoveCpuRange(cpus);
        }

    }
}
