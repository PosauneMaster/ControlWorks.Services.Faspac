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
    public class PviContext : ApplicationContext
    {

        public Service PviService { get; private set; }

        private ILogger _logger;
        public PviContext() { }
        public PviContext(ILogger logger)
        {
            _logger = logger;
            ConnectPvi();
        }

        private void ConnectPvi()
        {
            var pviManager = new PviManager(_logger);
            pviManager.ServiceConnected += PviManager_ServiceConnected;
            pviManager.ServiceDisconnected += PviManager_ServiceDisconnected;
            pviManager.ServiceError += PviManager_ServiceError;
            pviManager.ConnectPvi();
        }

        private void PviManager_ServiceError(object sender, PviEventArgs e)
        {
        }

        private void PviManager_ServiceDisconnected(object sender, PviEventArgs e)
        {
        }

        private void PviManager_ServiceConnected(object sender, PviEventArgs e)
        {
            PviService = sender as Service;
        }
    }
}
