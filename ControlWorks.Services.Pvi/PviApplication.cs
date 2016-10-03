using BR.AN.PviServices;
using ControlWorks.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlWorks.Services.Pvi
{
    public interface IPviApplication
    {
        string ServiceName { get; set; }
    }

    public class PviApplication : IPviApplication
    {
        Service _service;
        PviContext _context;

        private ILogger _logger;

        public string ServiceName { get; set; }

        public PviApplication() { }

        public PviApplication(ILogger logger)
        {
            _logger = logger;
        }

        public void Connect()
        {
            _context = new PviContext(_logger);
            Application.Run(_context);
        }
    }
}
