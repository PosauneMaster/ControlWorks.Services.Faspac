using ControlWorks.Logging;
using ControlWorks.Services.Configuration;
using ControlWorks.Services.Data;
using ControlWorks.Services.Pvi;
using ControlWorks.Services.Sql;
using log4net;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlWorks.Services.Business
{
    public interface IDataProcessor
    {
        Task<DataResponse> GetCpuData(string cpuName);
        Task<DataResponse> Snapshot(string cpuName);
    }

    public class DataProcessor : BaseProcessor, IDataProcessor
    {
        IPviApplication _pviApplication;
        ISqlApi _sqlApi;
        ILogger _logger;

        public DataProcessor(IPviApplication pviApplication, ISqlApi sqlApi)
        {
            _pviApplication = pviApplication;

            var loggerName = ConfigurationProvider.ServiceLoggerName;
            _logger = new Log4netAdapter(LogManager.GetLogger(loggerName));
            _sqlApi = sqlApi;

        }

        public async Task<DataResponse> GetCpuData(string cpuName)
        {
            _logger.Log(new LogEntry(LoggingEventType.Information,
                $"VariableProcessor Operation=GetCpuData. cpuName={cpuName}"));

            var response = await _pviApplication.GetCpuData(cpuName);

            _logger.Log(new LogEntry(LoggingEventType.Debug, ToJson(response)));

            return response;
        }

        public async Task<DataResponse> Snapshot(string cpuName)
        {
            _logger.Log(new LogEntry(LoggingEventType.Information,
                $"VariableProcessor Operation=WriteToDb. cpuName={cpuName}"));

            var response = await _pviApplication.GetCpuData(cpuName);
            var cpuDetails = await _pviApplication.GetCpuByName(cpuName);

            if (response.Error == null)
            {
                var detail = new VariableShutdownDetail();


                var variableDict = (IDictionary<string, object>)response.Data;


                dynamic data = response.Data as ExpandoObject;


                detail.MachineIp = cpuDetails.IpAddress;
                detail.MachineName = cpuDetails.Name;

                if (variableDict.ContainsKey("PouchPerMinute"))
                {
                    int pouch;
                    Int32.TryParse(variableDict["PouchPerMinute"].ToString(), out pouch);
                    detail.PouchesPerMinute = pouch;
                }

                if (variableDict.ContainsKey("TimeDisplay.CyclingTime"))
                {
                    detail.CyclingTime = variableDict["TimeDisplay.CyclingTime"].ToString();
                }

                if (variableDict.ContainsKey("CycleCount"))
                {
                    int cycle;
                    Int32.TryParse(variableDict["CycleCount"].ToString(), out cycle);
                    detail.CycleCount = cycle;
                }

                if (variableDict.ContainsKey("HourMeter"))
                {
                    int meter;
                    Int32.TryParse(variableDict["HourMeter"].ToString(), out meter);
                    detail.HourMeter = meter;
                }

                detail.Comment = "API Request";

                await _sqlApi.AddSnapshot(detail);
            }

            return response;
        }
    }
}
