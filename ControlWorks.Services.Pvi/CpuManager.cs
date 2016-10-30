using BR.AN.PviServices;
using ControlWorks.Logging;
using log4net;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ControlWorks.Services.Pvi
{

    public class CpuManager
    {
        private readonly byte _sourceStationId = 100;
        private AutoResetEvent _disconnectWaitHandle;

        private ILogger _logger;
        private Service _service;

        private List<string> _cpuLoaded;
        private int _expectedCpus;
        public bool _shouldNotify = true;

        public event EventHandler<CpusLoadedEventArgs> CpusLoaded;

        public CpuManager(Service service)
        {
            _logger = new Log4netAdapter(LogManager.GetLogger("ServiceLogger"));
            _service = service;
            _cpuLoaded = new List<string>();
            _expectedCpus = 0;
        }

        private void UpdateLoaded(string cpuName)
        {
            if (!_cpuLoaded.Contains(cpuName))
            {
                _cpuLoaded.Add(cpuName);
            }

            if (_cpuLoaded.Count == _expectedCpus && _shouldNotify)
            {
                _shouldNotify = false;
                EventHandler<CpusLoadedEventArgs> temp = CpusLoaded;
                if (temp != null)
                {
                    _logger.Log(new LogEntry(LoggingEventType.Information, $"Cpu finished loading, total = {_cpuLoaded.Count} Raising CpusLoaded event"));

                    temp(this, new CpusLoadedEventArgs() { Cpus = new List<string>(_cpuLoaded) });
                }
            }
        }

        public void LoadCpuCollection(IList<CpuInfo> cpuCollection)
        {
            _expectedCpus = cpuCollection.Count;
            foreach (var cpu in cpuCollection)
            {
                CreateCpu(cpu.Name, cpu.IpAddress);
            }
        }

        public void CreateCpu(string name, string ipAddress)
        {
            _logger.Log(new LogEntry(LoggingEventType.Information, $"Creating Cpu. Name={name}; IpAddress={ipAddress}"));

            Cpu cpu = null;
            if (_service.Cpus.ContainsKey(name))
            {

                _logger.Log(new LogEntry(LoggingEventType.Information, $"A Cpu with the name {name} already exists. Disconnecting and updating"));
                cpu = _service.Cpus[name];
                DisconnectCpu(name);
            }
            else
            {
                cpu = new Cpu(_service, name);
            }

            cpu.Connection.DeviceType = DeviceType.TcpIp;
            cpu.Connection.TcpIp.SourceStation = _sourceStationId;
            cpu.Connection.TcpIp.DestinationIpAddress = ipAddress;

            cpu.Connected += cpu_Connected;
            cpu.Error += cpu_Error;
            cpu.Disconnected += cpu_Disconnected;

            cpu.Connect();

        }

        public void DisconnectCpu(string name)
        {
            if (_service.Cpus.ContainsKey(name))
            {

                _logger.Log(new LogEntry(LoggingEventType.Information, $"CpuManager.DisconnectCpu Name={name}"));

                Cpu cpu = _service.Cpus[name];

                if (cpu.IsConnected)
                {
                    _disconnectWaitHandle = new AutoResetEvent(false);

                    cpu.Disconnect();

                    _disconnectWaitHandle.WaitOne(1000);
                    _disconnectWaitHandle.Dispose();
                    _disconnectWaitHandle = null;
                }

                _service.Cpus.Remove(cpu.Name);
            }
            else
            {
                _logger.Log(new LogEntry(LoggingEventType.Information, $"CpuManager.DisconnectCpu Name={name} Not found"));
            }
        }

        private void cpu_Connected(object sender, PviEventArgs e)
        {
            Cpu cpu = sender as Cpu;
            if (cpu != null)
            {
                UpdateLoaded(e.Name);
            }

            _logger.Log(new LogEntry(LoggingEventType.Information, PviMessage.FormatMessage("Cpu Connected", e)));
        }
        private void cpu_Error(object sender, PviEventArgs e)
        {
            _logger.Log(new LogEntry(LoggingEventType.Information, PviMessage.FormatMessage("Cpu Error", e)));

            Cpu cpu = sender as Cpu;
            if (cpu != null)
            {
                cpu.Connected -= cpu_Connected;
                cpu.Error -= cpu_Error;
                cpu.Disconnected -= cpu_Disconnected;

                UpdateLoaded(e.Name);
            }
        }

        private void cpu_Disconnected(object sender, PviEventArgs e)
        {
            _logger.Log(new LogEntry(LoggingEventType.Information, PviMessage.FormatMessage("Cpu Disconnected", e)));

            Cpu cpu = sender as Cpu;
            if (cpu != null)
            {
                cpu.Connected -= cpu_Connected;
                cpu.Error -= cpu_Error;
                cpu.Disconnected -= cpu_Disconnected;

                UpdateLoaded(e.Name);
            }

            if (_disconnectWaitHandle != null)
            {
                _disconnectWaitHandle.Set();
            }
        }
    }

    public class CpusLoadedEventArgs : EventArgs
    {
        public List<string> Cpus { get; set; }
    }
}
