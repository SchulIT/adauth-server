using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace AuthServer.GUI.Service
{
    public class ServiceHelper : IServiceHelper
    {
        private const string ServiceName = "AD Auth Server";

        private Task<ServiceController> GetServiceAsync()
        {
            return Task.Run(() => ServiceController.GetServices().FirstOrDefault(x => x.ServiceName == ServiceName));
        }

        public async Task<bool> IsServiceInstalledAsync()
        {
            var service = await GetServiceAsync();
            return service != null;
        }

        public async Task<bool> IsServiceRunningAsync()
        {
            var service = await GetServiceAsync();

            if(service == null)
            {
                return false;
            }

            return service.Status == ServiceControllerStatus.Running;
        }

        public async Task StartServiceAsync()
        {
            var service = await GetServiceAsync();

            if (service == null)
            {
                return;
            }

            if (service.Status == ServiceControllerStatus.Stopped)
            {
                service.Start();
            }
        }

        public async Task StopServiceAsync()
        {
            var service = await GetServiceAsync();

            if (service == null)
            {
                return;
            }

            if (service.CanStop)
            {
                service.Stop();
            }
        }
    }
}
