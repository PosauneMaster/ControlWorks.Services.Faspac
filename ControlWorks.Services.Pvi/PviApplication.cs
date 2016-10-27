using ControlWorks.Logging;
using ControlWorks.Services.Data;
using log4net;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Dynamic;

namespace ControlWorks.Services.Pvi
{
    public interface IPviApplication
    {
        void Connect();
        Task<ServiceDetail> GetServiceDetails();
        Task<List<CpuDetailResponse>> GetCpuDetails();
        Task<CpuDetailResponse> GetCpuByName(string name);
        Task<CpuDetailResponse> GetCpuByIp(string ip);
        Task AddCpu(CpuInfo info);
        Task UpdateCpu(CpuInfo info);
        Task DeleteCpuByName(string name);
        Task DeleteCpuByIp(string ip);
        Task<DataResponse> GetCpuData(string cpuName);

    }

    public class PviApplication : IPviApplication
    {
        PviContext _context;
        DateTime _connectionTime;

        private ILogger _logger;

        public string ServiceName { get; set; }

        public PviApplication()
        {
            _logger = new Log4netAdapter(LogManager.GetLogger("ServiceLogger"));
        }

        public void Connect()
        {
            _context = new PviContext(_logger);
            _connectionTime = DateTime.Now;
            Application.Run(_context);
        }

        public async Task<CpuDetailResponse> GetCpuByName(string name)
        {
            var api = new CpuApi(_context.PviService.Cpus);

            return await Task.Run(() => api.FindByName(name));
        }

        public async Task<List<CpuDetailResponse>> GetCpuDetails()
        {
            var api = new CpuApi(_context.PviService.Cpus);

            return await Task.Run(() => api.GetAll());
        }

        public async Task<CpuDetailResponse> GetCpuByIp(string ip)
        {
            var api = new CpuApi(_context.PviService.Cpus);

            return await Task.Run(() => api.FindByIp(ip));
        }

        public async Task AddCpu(CpuInfo info)
        {
            var api = new CpuApi();

            await Task.Run(() =>
            {
                api.Add(info);
                _context.CpuService.CreateCpu(info.Name, info.IpAddress);
            });
        }

        public async Task UpdateCpu(CpuInfo info)
        {
            var api = new CpuApi();

            await Task.Run(() =>
            {
                api.Update(info);
                _context.CpuService.CreateCpu(info.Name, info.IpAddress);
            });
        }

        public async Task DeleteCpuByName(string name)
        {
            var api = new CpuApi();
            await Task.Run(() =>
            {
                if (api.RemoveByName(name))
                {
                    _context.CpuService.DisconnectCpu(name);
                }
            });
        }

        public async Task DeleteCpuByIp(string ip)
        {
            var api = new CpuApi();
            await Task.Run(() =>
            {
                var cpu = api.FindByIp(ip);
                if (api.RemoveByIp(ip))
                {
                    _context.CpuService.DisconnectCpu(cpu.Name);
                }
            });
        }


        public async Task<ServiceDetail> GetServiceDetails()
        {
            var service = _context.PviService;

            var detail = await Task.Run(() =>
                {
                    return new ServiceDetail()
                    {
                        Name = service.Name,
                        IsConnected = service.IsConnected,
                        Cpus = service.Cpus.Count,
                        ConnectTime = _connectionTime,
                        License = service.LicenceInfo.ToString()
                    };
                 });

            return detail;
        }

        public async Task<DataResponse> GetCpuData(string cpuName)
        {
            var dataResponse = await Task.Run(() =>
            {
                dynamic variables = new ExpandoObject();
                var variableDict = (IDictionary<string, object>)variables;

                var service = _context.PviService;
                var variableApi = new VariableApi();
                var variableInfo = variableApi.FindByCpuName(cpuName);

                if (variableInfo.Errors == null || !service.Cpus.ContainsKey(cpuName))
                {
                    return new DataResponse
                    {
                        Name = cpuName,
                        Data = null,
                        Error = new ErrorResponse
                        {
                            Error = $"A Cpu with the name {cpuName} is not found"
                        }
                    };
                }

                int index = 0;
                foreach (var setting in variableInfo.VariableNames)
                {
                    variableDict.Add(setting, $"datapoint_{++index}");
                }

                return new DataResponse
                {
                    Name = cpuName,
                    Data = variableDict as ExpandoObject
                };
            }).ConfigureAwait(false);

            return dataResponse;
        }
    }
}
