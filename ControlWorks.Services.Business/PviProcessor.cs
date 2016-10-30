using ControlWorks.Services.Pvi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlWorks.Services.Business
{
    public interface IPviProcessor
    {
        void Connect();
        Task RefreshVariables();

    }

    public class PviProcessor : BaseProcessor, IPviProcessor
    {
        IPviApplication _pviApplication;

        public PviProcessor(IPviApplication pviApplication)
        {
            _pviApplication = pviApplication;
        }

        public void Connect()
        {
            _pviApplication.Connect();
        }

        public async Task RefreshVariables()
        {
            await _pviApplication.UpdateVariables();
        }

    }
}
