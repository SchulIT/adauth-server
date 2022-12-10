using AuthServer.Core.Settings;
using AuthServer.GUI.Settings;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace AuthServer.GUI.Settings
{
    public class FileSettingsReader : FileSettingsBase, ISettingsReader
    {
        public async Task<ISettings> ReadAsync()
        {
            return JsonSettings.LoadSettings(); 
        }

        Task<ISettings> ISettingsReader.ReadAsync()
        {
            return ReadAsync();
        }
    }
}
