using AuthServer.Core.Json;
using Newtonsoft.Json;

namespace AuthServer.Core.Request
{
    /// <summary>
    /// Represents an authentication request
    /// </summary>
    [JsonConverter(typeof(RequestConverter))]
    public class AuthenticationRequest : IRequest
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
