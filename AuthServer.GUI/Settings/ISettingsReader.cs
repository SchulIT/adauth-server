using AuthServer.Core.Settings;
using System.Threading.Tasks;

namespace AuthServer.GUI.Settings
{
    public interface ISettingsReader
    {
        Task<ISettings> ReadAsync();
    }
}
