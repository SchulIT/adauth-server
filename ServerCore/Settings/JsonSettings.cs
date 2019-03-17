using Newtonsoft.Json;
using System;
using System.IO;

namespace ServerCore.Settings
{
    public class JsonSettings : ISettings
    {
        [JsonProperty("server")]
        public IServerSettings Server { get; } = new JsonServerSettings();

        [JsonProperty("tls")]
        public ITlsSettings Tls { get; } = new JsonTlsSettings();

        [JsonProperty("ldap")]
        public ILdapSettings Ldap { get; } = new JsonLdapSettings();

        [JsonProperty("interal_id")]
        public string UniqueIdAttributeName { get; set; }

        public static ISettings LoadSettings()
        {
            var file = GetPath();

            if (!File.Exists(file))
            {
                return new JsonSettings();
            }

            using (var stream = File.Open(file, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(stream))
                {
                    var settings = reader.ReadToEnd();
                    var settingsObj = new JsonSettings();
                    JsonConvert.PopulateObject(settings, settingsObj);

                    return settingsObj;
                }
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
        [JsonProperty("enabled")]
        public bool IsEnabled { get; set; } = false;

        [JsonProperty("cert")]
        public string Certificate { get; set; }

        [JsonProperty("psk")]
        public string PreSharedKey { get; set; }
    }

    public class JsonLdapSettings : ILdapSettings
    {
        [JsonProperty("dc")]
        public string DC { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
