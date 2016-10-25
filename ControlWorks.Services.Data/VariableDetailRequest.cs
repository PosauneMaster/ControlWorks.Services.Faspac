using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlWorks.Services.Data
{
    public class VariableDetailRequest
    {
        public string CpuName { get; set; }
        public string[] Variables { get; set; }
    }
}
