using BR.AN.PviServices;
using ControlWorks.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlWorks.Services.Pvi
{
    public class ShutdownEventArgs : EventArgs
    {
        public VariableShutdownDetail Details { get; set; }
    }
}
