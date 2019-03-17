using ServerCore.Settings;
using System.Threading.Tasks;

namespace ServerGUI.Services.Settings
{
    public interface ISettingsReader
    {
        Task<ISettings> ReadAsync();
    }
}
