using Newtonsoft.Json;
using ServerCore.Settings;
using System.IO;
using System.Threading.Tasks;

namespace ServerGUI.Services.Settings
{
    public class FileSettingsWriter : FileSettingsBase, ISettingsWriter
    {
        public async Task WriteAsync(ISettings settings)
        {
            var json = await Task.Run(() =>
            {
                if(!(settings is JsonSettings))
                {
                    settings = ToJsonSettings(settings);
                }

                return JsonConvert.SerializeObject(settings, Formatting.Indented);
            }).ConfigureAwait(false);

            using (var writer = new StreamWriter(GetFilePath()))
            {
                await writer.WriteAsync(json).ConfigureAwait(false);
            }
        }

        private JsonSettings ToJsonSettings(ISettings settings)
        {
            var jsonSettings = new JsonSettings();
            jsonSettings.UniqueIdAttributeName = settings.UniqueIdAttributeName;

            var ldapSettings = jsonSettings.Ldap as JsonLdapSettings;
            ldapSettings.DC = settings.Ldap.DC;
            ldapSettings.Domain = settings.Ldap.Domain;
            ldapSettings.Username = settings.Ldap.Username;
            ldapSettings.Password = settings.Ldap.Password;

            var jsonTlsSettings = jsonSettings.Tls as JsonTlsSettings;

            jsonTlsSettings.Certificate = settings.Tls.Certificate;
            jsonTlsSettings.IsEnabled = settings.Tls.IsEnabled;
            jsonTlsSettings.PreSharedKey = settings.Tls.PreSharedKey;

            var jsonServerSettings = jsonSettings.Server as JsonServerSettings;
            jsonServerSettings.IPv6 = settings.Server.IPv6;
            jsonServerSettings.Port = settings.Server.Port;

            return jsonSettings;
        }
    }
}
