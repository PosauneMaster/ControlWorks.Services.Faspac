using ControlWorks.Services.Configuration;
using ControlWorks.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlWorks.Services.Pvi
{
    public interface IVariableApi
    {
        Task<List<VariableDetailRespose>> GetAll();
        Task<VariableDetailRespose> FindByCpuName(string name);
        Task AddRange(string cpuName, IEnumerable<string> variableNames);
        Task RemoveRange(string cpuName, IEnumerable<string> variableNames);


    }
    public class VariableApi : IVariableApi
    {
        public async Task<List<VariableDetailRespose>> GetAll()
        {
            var list = new List<VariableDetailRespose>();
            var response = await Task.Run(() =>
            {
                
                var collection = new VariableInfoCollection();
                collection.Open(ConfigurationProvider.VariableSettingsFile);
                return collection.GetAll();
            });

            foreach (var v in response)
            {
                list.Add(new VariableDetailRespose()
                {
                    CpuName = v.CpuName,
                    VariableNames = v.Variables
                });
            }

            return list;
        }

        public async Task<VariableDetailRespose> FindByCpuName(string name)
        {
            var response = await Task.Run(() =>
            {
                var collection = new VariableInfoCollection();
                collection.Open(ConfigurationProvider.VariableSettingsFile);

                return collection.FindByCpu(name);
            });

            if (response == null)
            {
                return new VariableDetailRespose
                {
                    CpuName = name,
                    VariableNames = null,
                    Errors = new ErrorResponse
                    {
                         Error = $"No entry exists with the Cpu name {name}"
                    }
                };
            }

            return new VariableDetailRespose
            {
                CpuName = response.CpuName,
                VariableNames = response.Variables
            };
        }

        public async Task AddRange(string cpuName, IEnumerable<string> variableNames)
        {
            await Task.Run(() =>
            {
                var collection = new VariableInfoCollection();
                collection.Open(ConfigurationProvider.VariableSettingsFile);
                collection.AddRange(cpuName, variableNames);
                collection.Save(ConfigurationProvider.VariableSettingsFile);
            });
        }

        public async Task RemoveRange(string cpuName, IEnumerable<string> variableNames)
        {
            await Task.Run(() =>
            {
                var collection = new VariableInfoCollection();
                collection.Open(ConfigurationProvider.VariableSettingsFile);
                collection.RemoveRange(cpuName, variableNames);
                collection.Save(ConfigurationProvider.VariableSettingsFile);
            });
        }


    }
}
