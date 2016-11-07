using BR.AN.PviServices;
using ControlWorks.Services.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlWorks.Services.Pvi
{
    public interface IEventNotifier
    {
        event EventHandler<ShutdownEventArgs> Shutdown;

        void RaiseShutdown(Cpu cpu);
    }

    public class EventNotifier : IEventNotifier
    {
        public event EventHandler<ShutdownEventArgs> Shutdown;

        public void RaiseShutdown(Cpu cpu)
        {
            OnShutdown(cpu);
        }

        private void OnShutdown(Cpu cpu)
        {
            EventHandler<ShutdownEventArgs> temp = Shutdown;

            if (temp != null)
            {
                var details = new VariableShutdownDetail();

                details.MachineIp = cpu.Connection.TcpIp.DestinationIpAddress;
                details.MachineName = cpu.Name;
                details.Comment = "Operator Shutdown";

                if (cpu.Variables.ContainsKey("PouchPerMinute"))
                {
                    details.PouchesPerMinute = cpu.Variables["PouchPerMinute"].Value.ToInt32(CultureInfo.CurrentCulture);
                }

                if (cpu.Variables.ContainsKey("TimeDisplay.CyclingTime"))
                {
                    details.CyclingTime = cpu.Variables["TimeDisplay.CyclingTime"].Value.ToString();
                }

                if (cpu.Variables.ContainsKey("CycleCount"))
                {
                    details.CycleCount = cpu.Variables["CycleCount"].Value.ToInt32(CultureInfo.CurrentCulture);
                }

                if (cpu.Variables.ContainsKey("HourMeter"))
                {
                    details.HourMeter = cpu.Variables["HourMeter"].Value.ToInt32(CultureInfo.CurrentCulture);
                }
                   
                var eventArgs = new ShutdownEventArgs
                {
                    Details = details
                };

                temp(cpu, eventArgs);
            }
        }
    }
}
