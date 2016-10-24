using ControlWorks.Logging;
using ControlWorks.Services.Configuration;
using ControlWorks.Services.Data;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlWorks.Services.Business
{
    public interface IConfigurationProcessor
    {
        Task<ExpandoObject> GetSettings();
        Task<ResponseMessage> AddOrUpdateConnectionString(ConnectionInfo data);
        Task<ResponseMessage> Add(ConnectionInfo data);
        Task<ResponseMessage> Update(ConnectionInfo data);
        Task<ResponseMessage> ChangePort(int port);

    }

    public class ConfigurationProcessor : BaseProcessor, IConfigurationProcessor
    {
        IConfigurationService _configurationService;
        ILogger _logger;

        public ConfigurationProcessor(IConfigurationService configurationSettings)
        {
            _configurationService = configurationSettings;

            var loggerName = ConfigurationProvider.ServiceLoggerName;
            _logger = new Log4netAdapter(LogManager.GetLogger(loggerName));
        }

        public async Task<ExpandoObject> GetSettings()
        {
            _logger.Log(new LogEntry(LoggingEventType.Information, "ConfigurationProcessor Operation=GetSettings"));

            ExpandoObject settings = null;
            await Task.Run(() => settings = _configurationService.GetSettings(_logger.GetLogFileName()));

            _logger.Log(new LogEntry(LoggingEventType.Debug, ToJson(settings)));

            return settings;
        }

        public async Task<ResponseMessage> Add(ConnectionInfo data)
        {
            var response = await AddOrUpdateConnectionString(data);
            response.Message = $"Connection string {data.Name} added";
            return response;
        }


        public async Task<ResponseMessage> Update(ConnectionInfo data)
        {
            var response = await AddOrUpdateConnectionString(data);
            response.Message = $"Connection string {data.Name} updated";
            return response;
        }

        public async Task<ResponseMessage> AddOrUpdateConnectionString(ConnectionInfo data)
        {
            var errors = ValidateConnectionInfo(data);

            if (errors.Count > 0)
            {
                var response = new ResponseMessage()
                {
                    IsSuccess = false,
                    Message = "Required Fields are missing",
                    Errors = errors.ToArray()
                };

                return response;

            }

            var builder = new SqlConnectionStringBuilder();
            builder.UserID = data.UserName;
            builder.Password = data.Password;
            if (data.Timeout.HasValue)
            {
                builder.ConnectTimeout = data.Timeout.Value;
            }
            builder.DataSource = data.DataSource;
            builder.InitialCatalog = data.InitialCatalog;
            builder.IntegratedSecurity = data.IntegratedSecurity ?? false;

            await Task.Run(() => _configurationService.AddOrUpdateConnectionString(data.Name, builder.ConnectionString));

            var success = new ResponseMessage()
            {
                IsSuccess = true,
                Message = ""
            };


            return success;

        }

        private List<ErrorResponse> ValidateConnectionInfo(ConnectionInfo data)
        {
            var errors = new List<ErrorResponse>();
            if (String.IsNullOrEmpty(data.Name))
            {
                errors.Add(new ErrorResponse() { Error = "Name is required" });
            }

            if (String.IsNullOrEmpty(data.DataSource))
            {
                errors.Add(new ErrorResponse() { Error = "Data Source is required" });
            }

            return errors;
        }

        public async Task<ResponseMessage> ChangePort(int port)
        {
            await Task.Run(() => _configurationService.ChangePort(port));

            var response = new ResponseMessage()
            {
                IsSuccess = true,
                Message = $"Port changed to {port}.  Service restart is required to take effect."
            };

            return response;
        }

    }
}
