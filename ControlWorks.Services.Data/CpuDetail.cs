using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlWorks.Services.Data
{
    public class CpuDetail
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string IpAddress { get; set; }
        public bool IsConnected { get; set; }
        public bool HasError { get; set; }
        public bool ErrorCode { get; set; }
        public bool ErrorText { get; set; }
    }
}
