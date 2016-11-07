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
        public static string VariableSettingsFile { get; } = ConfigurationManager.AppSettings["VariableSettingsFile"] ?? "variables.config";
        public static string ShutdownTriggerVariable { get; } = ConfigurationManager.AppSettings["ShutdownTriggerVariable"] ?? "MainDriveRun";
        public static string ShutdownTrigger { get; } = ConfigurationManager.AppSettings["ShutdownTrigger"] ?? "true";
        public static string ConnectionStringName { get; } = ConfigurationManager.AppSettings["ConnectionStringName"] ?? "MyConnectionString";

        public static string GetConnectionString(string name)
        {

            if (ConfigurationManager.ConnectionStrings[name] != null)
            {
                return ConfigurationManager.ConnectionStrings[name].ConnectionString;
            }

            return String.Empty;
        }

    }
}
