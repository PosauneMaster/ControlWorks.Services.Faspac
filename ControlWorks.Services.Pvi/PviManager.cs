using BR.AN.PviServices;
using ControlWorks.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlWorks.Services.Pvi
{
    internal interface IPviManager
    {
        void ConnectPvi();

    }

    internal class PviManager : IPviManager, IDisposable
    {
        private ILogger log;

        public Service PviService { get; set; }

        public event EventHandler<PviEventArgs> ServiceConnected;
        public event EventHandler<PviEventArgs> ServiceDisconnected;
        public event EventHandler<PviEventArgs> ServiceError;

        public PviManager() { }
        public PviManager(ILogger logger)
        {
            log = logger;
        }

        public void ConnectPvi()
        {
            var serviceName = Guid.NewGuid().ToString();
            PviService = new Service(serviceName);
            PviService.Connected += PviService_Connected;
            PviService.Disconnected += PviService_Disconnected;
            PviService.Error += PviService_Error;
            PviService.Connect();

        }

        private void PviService_Error(object sender, PviEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PviService_Disconnected(object sender, PviEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PviService_Connected(object sender, PviEventArgs e)
        {
            var pviEventMsg = FormatPviEventMessage("PviService._service_Connected", e);
            log.Log(new LogEntry(LoggingEventType.Information, pviEventMsg));

            OnServiceConnected(sender, e);
        }

        private void OnServiceConnected(object sender, PviEventArgs e)
        {
            Service service = sender as Service;
            if (service != null)
            {
                EventHandler<PviEventArgs> temp = ServiceConnected;
                if (temp != null)
                {
                    temp(sender, e);
                }
            }
        }

        private string FormatPviEventMessage(string message, PviEventArgs e)
        {
            return String.Format("{0}; Action={1}, Address={2}, Error Code={3}, Error Text={4}, Name={5} ",
                message, e.Action, e.Address, e.ErrorCode, e.ErrorText, e.Name);
        }

        #region IDisposable

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (PviService != null)
                    {
                        PviService.Disconnect();
                        PviService.Dispose();
                    }
                }
            }
            disposed = true;
        }

        #endregion

    }
}
