using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlWorks.Services.Data
{
    public class VariableDetailRespose
    {
        public string CpuName { get; set; }
        public string[] VariableNames { get; set; }
        public ErrorResponse Errors { get; set; }
    }
}
