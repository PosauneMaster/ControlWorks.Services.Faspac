using ControlWorks.Logging;
using ControlWorks.Services.Configuration;
using log4net;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlWorks.Services.Business
{
    public interface IConfigurationProcessor
    {
        Task<ExpandoObject> GetSettings();
    }

    public class ConfigurationProcessor : BaseProcessor, IConfigurationProcessor
    {
        IConfigurationSettings _configurationSettings;
        ILogger _logger;

        public ConfigurationProcessor(IConfigurationSettings configurationSettings)
        {
            _configurationSettings = configurationSettings;

            var loggerName = ConfigurationProvider.ServiceLoggerName;
            _logger = new Log4netAdapter(LogManager.GetLogger(loggerName));
        }

        public async Task<ExpandoObject> GetSettings()
        {
            _logger.Log(new LogEntry(LoggingEventType.Information, "ConfigurationProcessor Operation=GetSettings"));

            ExpandoObject settings = null;
            await Task.Run(() => settings = _configurationSettings.GetSettings(_logger.GetLogFileName()));

            _logger.Log(new LogEntry(LoggingEventType.Debug, ToJson(settings)));

            return settings;
        }
    }
}
