using Newtonsoft.Json;
using ServerCore.Settings;
using System.IO;
using System.Threading.Tasks;

namespace ServerGUI.Services.Settings
{
    public class FileSettingsReader : FileSettingsBase, ISettingsReader
    {
        public async Task<ISettings> ReadAsync()
        {
            var json = "";

            var path = GetFilePath();
            var directory = Path.GetDirectoryName(path);

            if(!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (File.Exists(path))
            {
                using (var reader = new StreamReader(path))
                {
                    json = await reader.ReadToEndAsync().ConfigureAwait(false);
                }

                return await Task.Run(() =>
                {
                    return JsonConvert.DeserializeObject<JsonSettings>(json);
                }).ConfigureAwait(false);
            }

            return await Task.FromResult<ISettings>(new JsonSettings()).ConfigureAwait(false);
        }
    }
}
