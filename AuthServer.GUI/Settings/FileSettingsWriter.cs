using AuthServer.Core.Settings;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace AuthServer.GUI.Settings
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
            ldapSettings.Server = settings.Ldap.Server;
            ldapSettings.DomainFQDN = settings.Ldap.DomainFQDN;
            ldapSettings.DomainNetBIOS = settings.Ldap.DomainNetBIOS;
            ldapSettings.Username = settings.Ldap.Username;
            ldapSettings.Password = settings.Ldap.Password;

            var jsonTlsSettings = jsonSettings.Tls as JsonTlsSettings;

            jsonTlsSettings.Certificate = settings.Tls.Certificate;
            jsonTlsSettings.PreSharedKey = settings.Tls.PreSharedKey;

            var jsonServerSettings = jsonSettings.Server as JsonServerSettings;
            jsonServerSettings.IPv6 = settings.Server.IPv6;
            jsonServerSettings.Port = settings.Server.Port;

            return jsonSettings;
        }
    }
}
