using System.IO;
using System.Threading.Tasks;

namespace ServerGUI.Services.License
{
    public class FileLicenseLoader : ILicenseLoader
    {
        private const string LicenseFile = "GUI_LICENSES.txt";

        public async Task<string> LoadLicenseAsync()
        {
            var path = Path.Combine(
                Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location),
                LicenseFile
            );

            using (var reader = new StreamReader(path))
            {
                var content = await reader.ReadToEndAsync().ConfigureAwait(false);
                return content;
            }
        }
    }
}
