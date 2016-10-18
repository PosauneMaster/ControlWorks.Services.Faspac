using ControlWorks.Logging;
using ControlWorks.Services.Data;
using log4net;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading.Tasks;

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

            await Task.Run(() => api.Add(info));
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
    }
}
