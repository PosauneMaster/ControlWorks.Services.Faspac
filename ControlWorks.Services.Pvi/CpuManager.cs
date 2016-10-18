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

        public CpuManager(Service service)
        {
            _logger = new Log4netAdapter(LogManager.GetLogger("ServiceLogger"));
            _service = service;
        }

        public void LoadCpuCollection(IEnumerable<CpuInfo> cpuCollection)
        {
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
                _disconnectWaitHandle = new AutoResetEvent(false);

                _logger.Log(new LogEntry(LoggingEventType.Information, $"A Cpu with the name {name} already exists. Disconnecting and updating"));
                cpu = _service.Cpus[name];
                cpu.Disconnect();
                _disconnectWaitHandle.WaitOne(1000);
                _disconnectWaitHandle.Dispose();
                _disconnectWaitHandle = null;
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

        private void cpu_Connected(object sender, PviEventArgs e)
        {
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
            }

            if (_disconnectWaitHandle != null)
            {
                _disconnectWaitHandle.Set();
            }
        }
    }
}
