using Newtonsoft.Json;

namespace AuthServer.Core.Request
{
    public class AbstractPasswordRequest : IRequest
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("new_password")]
        public string NewPassword { get; set; }
    }
}
