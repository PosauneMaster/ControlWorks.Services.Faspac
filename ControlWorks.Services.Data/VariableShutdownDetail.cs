using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlWorks.Services.Data
{
    public class VariableShutdownDetail : BaseDto
    {
        public string MachineIp { get; set; }
        public string MachineName { get; set; }
        public int? PouchesPerMinute { get; set; }
        public string CyclingTime { get; set; }
        public int? CycleCount { get; set; }
        public int? HourMeter { get; set; }
        public string Comment { get; set; }
    }
}
