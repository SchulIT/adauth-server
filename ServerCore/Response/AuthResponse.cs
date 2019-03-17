using Newtonsoft.Json;
using System.Collections.Generic;

namespace ServerCore.Response
{
    /// <summary>
    /// Represents an authentication responses (which is sent in case of an authentication request)
    /// </summary>
    public class AuthResponse : IResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("display_name")]
        public string DispalyName { get; set; }

        [JsonProperty("unique_id")]
        public string UniqueId { get; set; }

        [JsonProperty("ou")]
        public string OU { get; set; }

        [JsonProperty("groups")]
        public List<string> Groups { get; set; } = new List<string>();
    }
}
