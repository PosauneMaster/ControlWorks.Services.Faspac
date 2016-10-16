using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlWorks.Services.Configuration
{
    public static class ConfigurationProvider
    {
        public static string ServiceLoggerName { get; } = ConfigurationManager.AppSettings["ServiceLoggerName"] ?? "ServiceLogger";
        public static string Port { get; } = ConfigurationManager.AppSettings["Port"] ?? "8080";
        public static string CpuSettingsFile { get; } = ConfigurationManager.AppSettings["CpuSettingsFile"] ?? "cpu.config";

    }
}
