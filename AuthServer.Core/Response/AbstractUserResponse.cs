using Newtonsoft.Json;
using System.Collections.Generic;

namespace AuthServer.Core.Response
{
    public class AbstractUserResponse
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("upn")]
        public string UPN { get; set; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("guid")]
        public string Guid { get; set; }

        [JsonProperty("ou")]
        public string OU { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("groups")]
        public IEnumerable<string> Groups { get; set; } = new List<string>();
    }
}
