using System.Threading.Tasks;

namespace ServerGUI.Services.License
{
    public interface ILicenseLoader
    {
        Task<string> LoadLicenseAsync();
    }
}
