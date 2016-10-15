using ControlWorks.Logging;
using ControlWorks.Services.Data;
using log4net;
using System;
using System.Windows.Forms;

namespace ControlWorks.Services.Pvi
{
    public interface IPviApplication
    {
        void Connect();
        ServiceDetail GetServiceDetails();
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
