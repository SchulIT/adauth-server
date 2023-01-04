using Newtonsoft.Json;
using System;
using System.IO;

namespace AuthServer.Core.Settings
{
    public class JsonSettings : ISettings
    {
        [JsonProperty("server")]
        public IServerSettings Server { get; } = new JsonServerSettings();

        [JsonProperty("tls")]
        public ITlsSettings Tls { get; } = new JsonTlsSettings();

        [JsonProperty("ldap")]
        public ILdapSettings Ldap { get; } = new JsonLdapSettings();

        public static ISettings LoadSettings()
        {
            var file = GetPath();

            var path = Path.GetDirectoryName(file);
            Directory.CreateDirectory(path);

            if (!File.Exists(file))
            {
                using (var stream = new StreamWriter(file))
                {
                    stream.Write(JsonConvert.SerializeObject(new JsonSettings(), Formatting.Indented));
                }
            }

            using (var reader = new StreamReader(file))
            {
                var settings = reader.ReadToEnd();
                var settingsObj = new JsonSettings();
                JsonConvert.PopulateObject(settings, settingsObj);

                return settingsObj;
            }
        }

        public static string GetPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "SchulIT", "AD Auth Server", "settings.json");
        }
    }

    public class JsonServerSettings : IServerSettings
    {
        [JsonProperty("ipv6")]
        public bool IPv6 { get; set; } = false;

        [JsonProperty("port")]
        public int Port { get; set; } = 55117;
    }

    public class JsonTlsSettings : ITlsSettings
    {

        [JsonProperty("cert")]
        public string Certificate { get; set; }

        [JsonProperty("psk")]
        public string PreSharedKey { get; set; }
    }

    public class JsonLdapSettings : ILdapSettings
    {
        [JsonProperty("server")]
        public string Server { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; } = 389;

        [JsonProperty("ssl")]
        public bool UseSSL { get; set; } = false;

        [JsonProperty("tls")]
        public bool UseTLS { get; set; } = true;

        [JsonProperty("domain_netbios")]
        public string DomainNetBIOS { get; set; }

        [JsonProperty("domain_fqdn")]
        public string DomainFQDN { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("certificate_thumbprint")]
        public string CertificateThumbprint { get; set; }

        [JsonProperty("username_property")]
        public UsernameProperty UsernameProperty { get; set; } = UsernameProperty.UserPrincipalName;

        [JsonProperty("allowed_upn_suffixes")]
        public string[] AllowedUpnSuffixes { get; set; }
    }
}
