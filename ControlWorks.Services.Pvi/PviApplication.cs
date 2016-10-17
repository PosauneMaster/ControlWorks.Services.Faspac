using BR.AN.PviServices;
using ControlWorks.Logging;
using ControlWorks.Services.Data;
using log4net;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ControlWorks.Services.Pvi
{
    public interface IPviApplication
    {
        void Connect();
        ServiceDetail GetServiceDetails();
        System.Threading.Tasks.Task<List<CpuDetailResponse>> GetCpuDetails();
        CpuDetailResponse GetCpuByName(string name);
        CpuDetailResponse GetCpuByIp(string ip);
        void AddCpu(CpuInfo info);
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

        public CpuDetailResponse GetCpuByName(string name)
        {
            var api = new CpuApi(_context.PviService.Cpus);
            return api.FindByName(name);
        }

        public async System.Threading.Tasks.Task<List<CpuDetailResponse>> GetCpuDetails()
        {
            var api = new CpuApi(_context.PviService.Cpus);

            return await System.Threading.Tasks.Task.FromResult(api.GetAll());
        }

        public CpuDetailResponse GetCpuByIp(string ip)
        {
            var api = new CpuApi(_context.PviService.Cpus);
            return api.FindByIp(ip);
        }

        public void AddCpu(CpuInfo info)
        {
            var api = new CpuApi();
            if (api.Add(info))
            {
                _context.LoadCpu(info);
            }
        }

        public ServiceDetail GetServiceDetails()
        {
            var details = new ServiceDetail()
            {
                Name = _context.PviService.Name,
                IsConnected = _context.PviService.IsConnected,
                Cpus = _context.PviService.Cpus.Count,
                ConnectTime = _connectionTime,
                License = _context.PviService.LicenceInfo.ToString()
            };

            return details;
        }
    }
}
