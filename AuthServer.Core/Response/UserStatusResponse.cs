using Newtonsoft.Json;

namespace AuthServer.Core.Response
{
    public class UserStatusResponse : AbstractUserResponse
    {
        [JsonProperty("is_active")]
        public bool IsActive { get; set; }
    }
}
