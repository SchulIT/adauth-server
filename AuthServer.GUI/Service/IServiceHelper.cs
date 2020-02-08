using System.Threading.Tasks;

namespace AuthServer.GUI.Service
{
    public interface IServiceHelper
    {
        Task<bool> IsServiceInstalledAsync();

        Task<bool> IsServiceRunningAsync();

        Task StartServiceAsync();

        Task StopServiceAsync();
    }
}
