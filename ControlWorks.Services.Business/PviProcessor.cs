using ControlWorks.Services.Pvi;
using ControlWorks.Services.Sql;
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
        IEventNotifier _notifier;
        ISqlApi _sqlApi;

        public PviProcessor(IPviApplication pviApplication, IEventNotifier notifier, ISqlApi sqlApi)
        {
            _pviApplication = pviApplication;
            _notifier = notifier;
            _sqlApi = sqlApi;
            _notifier.Shutdown += _notifier_Shutdown;
        }

        private void _notifier_Shutdown(object sender, ShutdownEventArgs e)
        {
            _sqlApi.AddSnapshot(e.Details);
        }

        public void Connect()
        {
            _pviApplication.Connect(_notifier);
        }

        public async Task RefreshVariables()
        {
            await _pviApplication.UpdateVariables();
        }

    }
}
