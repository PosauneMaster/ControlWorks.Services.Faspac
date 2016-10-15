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
    }
}
