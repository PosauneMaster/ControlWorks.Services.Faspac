using BR.AN.PviServices;
using ControlWorks.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlWorks.Services.Pvi
{
    public class PviContext : ApplicationContext
    {
        public Service PviService { get; private set; }

        private ILogger _logger;
        public PviContext() { }
        public PviContext(ILogger logger)
        {
            _logger = logger;
        }

        private void ConnectPvi()
        {
        }





    }
}
