using System.Threading.Tasks;

namespace ServerGUI.Services.Service
{
    public interface IServiceHelper
    {
        Task<bool> IsServiceInstalledAsync();

        Task<bool> IsServiceRunningAsync();

        Task StartServiceAsync();

        Task StopServiceAsync();
    }
}
