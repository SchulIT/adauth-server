using AuthServer.Core.Settings;
using System.Threading.Tasks;

namespace AuthServer.GUI.Settings
{
    public interface ISettingsWriter
    {
        Task WriteAsync(ISettings settings);
    }
}
