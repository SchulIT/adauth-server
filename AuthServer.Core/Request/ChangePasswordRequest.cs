using Newtonsoft.Json;

namespace AuthServer.Core.Request
{
    /// <summary>
    /// Request for changing a password - if the old password is known
    /// </summary>
    public class ChangePasswordRequest : AbstractPasswordRequest
    {
        [JsonProperty("old_password")]
        public string OldPassword { get; set; }
    }
}
