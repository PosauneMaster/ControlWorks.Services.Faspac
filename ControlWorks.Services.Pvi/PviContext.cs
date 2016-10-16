using BR.AN.PviServices;
using ControlWorks.Logging;
using ControlWorks.Services.Configuration;
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
        public CpuManager CpuService { get; private set; }

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
            pviManager.ConnectPvi();
        }

        private void PviManager_ServiceConnected(object sender, PviEventArgs e)
        {
            PviService = sender as Service;

            var settingFile = ConfigurationProvider.CpuSettingsFile;
            var collection = new CpuInfoCollection();

            try
            {
                collection.Open(settingFile);
                CpuService = new CpuManager(PviService);
                CpuService.LoadCpuCollection(collection.GetAll());
            }
            catch(System.Exception ex)
            {
                _logger.Log(new LogEntry(LoggingEventType.Error, "Error Loading Cpu Settings", ex));
            }


        }
    }
}
