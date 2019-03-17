using ServerCore.Settings;
using System.Threading.Tasks;

namespace ServerGUI.Services.Settings
{
    public interface ISettingsWriter
    {
        Task WriteAsync(ISettings settings);
    }
}
