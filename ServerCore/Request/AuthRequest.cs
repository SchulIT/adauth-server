using Newtonsoft.Json;
using ServerCore.Json;

namespace ServerCore.Request
{
    /// <summary>
    /// Represents an authentication request
    /// </summary>
    [JsonConverter(typeof(RequestConverter))]
    public class AuthRequest : IRequest
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
