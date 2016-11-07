using BR.AN.PviServices;
using ControlWorks.Logging;
using ControlWorks.Services.Configuration;
using System;
using System.Windows.Forms;

namespace ControlWorks.Services.Pvi
{
    public class PviContext : ApplicationContext
    {
        public Service PviService { get; private set; }
        public CpuManager CpuService { get; private set; }

        private ILogger _logger;
        private IEventNotifier _notifier;

        public PviContext() { }
        public PviContext(ILogger logger, IEventNotifier notifier)
        {
            _logger = logger;
            _notifier = notifier;
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
                CpuService.CpusLoaded += CpuService_CpusLoaded;
                CpuService.LoadCpuCollection(collection.GetAll());
            }
            catch(System.Exception ex)
            {
                _logger.Log(new LogEntry(LoggingEventType.Error, "Error Loading Cpu Settings", ex));
            }
        }

        private void CpuService_CpusLoaded(object sender, CpusLoadedEventArgs e)
        {
            var variable = new VariableApi();
            var task = variable.AddCpuRange(e.Cpus.ToArray()).ConfigureAwait(false);

            var manager = new VariableManager(PviService, variable, _notifier);
            manager.ConnectVariables();
        }

        public void LoadCpu(CpuInfo info)
        {
            CpuService.CreateCpu(info.Name, info.IpAddress);
        }
    }
}
