using ControlWorks.Services.Data;
using ControlWorks.Services.Pvi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlWorks.Services.Business
{
    public interface IRequestProcessor
    {
        ServiceDetail GetServiceDetails();
        List<CpuDetailResponse> GetCpuDetails();
        CpuDetailResponse GetCpuByName(string name);
        CpuDetailResponse GetCpuByIp(string ip);
        void Add(CpuInfoRequest request);

    }
    public class RequestProcessor : IRequestProcessor
    {
        IPviApplication _application;

        public RequestProcessor() { }

        public RequestProcessor(IPviApplication application)
        {
            _application = application;
        }
        public ServiceDetail GetServiceDetails()
        {
            return _application.GetServiceDetails();
        }
        public List<CpuDetailResponse> GetCpuDetails()
        {
            return _application.GetCpuDetails();
        }

        public CpuDetailResponse GetCpuByName(string name)
        {
            return _application.GetCpuByName(name);
        }

        public CpuDetailResponse GetCpuByIp(string ip)
        {
            return _application.GetCpuByIp(ip);
        }

        public void Add(CpuInfoRequest request)
        {
            var info = new CpuInfo()
            {
                Name = request.Name,
                Description = request.Description,
                IpAddress = request.IpAddress
            };

            _application.AddCpu(info);
        }
    }
}
