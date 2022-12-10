using Newtonsoft.Json;

namespace AuthServer.Core.Settings
{
    /// <summary>
    /// Settings
    /// </summary>
    public interface ISettings
    {
        [JsonProperty("server")]
        IServerSettings Server { get; }

        [JsonProperty("tls")]
        ITlsSettings Tls { get; }

        [JsonProperty("ldap")]
        ILdapSettings Ldap { get; }
    }
}
