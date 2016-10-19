﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.IO;
using System.Reflection;

namespace ControlWorks.Services.Configuration
{
    public interface IConfigurationSettings
    {
        ExpandoObject GetSettings(string logFileName);
        string GetSettingFile();
        string GetConfigSource();
        void AddConnectionString(string name, string connectionString);

    }
    public class ConfigurationSettings : IConfigurationSettings
    {
        public ExpandoObject GetSettings(string logFileName)
        {
            dynamic main = new ExpandoObject();
            var mainDict = (IDictionary<string, object>)main;

            mainDict.Add("appSettingsFile", GetSettingFile());
            mainDict.Add("configSourceFile", GetConfigSource());
            mainDict.Add("LogFile", logFileName);

            dynamic settings = new ExpandoObject();
            var settingsDict = (IDictionary<string, object>)settings;

            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                settingsDict.Add(key, ConfigurationManager.AppSettings[key]);
            }

            mainDict.Add("AppSettings", settings);

            dynamic connections = new ExpandoObject();
            var connectionsDict = (IDictionary<string, object>)connections;


            ConnectionStringSettingsCollection collection = ConfigurationManager.ConnectionStrings;

            if (connections != null)
            {
                foreach (ConnectionStringSettings cs in collection)
                {

                    connectionsDict.Add(cs.Name, cs.ConnectionString);
                }

                mainDict.Add("ConnectionStrings", connections);
            }

            return main;
        }

        public string GetSettingFile()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            AppSettingsSection appSettings = config.AppSettings;

            return Path.Combine(Directory.GetCurrentDirectory(), appSettings.File);
        }

        public string GetConfigSource()
        {
            var connectionStringsSection = ConfigurationManager.GetSection("connectionStrings") as ConnectionStringsSection;

            return Path.Combine(Directory.GetCurrentDirectory(), connectionStringsSection.SectionInformation.ConfigSource);
        }

        public void AddConnectionString(string name, string connectionString)
        {
            var setting = new ConnectionStringSettings(name, connectionString);

            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConnectionStringsSection connectionStringsSection = configuration.ConnectionStrings;
            connectionStringsSection.ConnectionStrings.Add(setting);
            configuration.Save(ConfigurationSaveMode.Minimal);

        }
    }
}
