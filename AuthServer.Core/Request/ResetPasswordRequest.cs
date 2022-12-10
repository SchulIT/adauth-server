using Newtonsoft.Json;

namespace AuthServer.Core.Request
{
    /// <summary>
    /// Request for resetting a password - in case the old password is not known. Requires administrator
    /// credentials.
    /// </summary>
    public class ResetPasswordRequest : AbstractPasswordRequest
    {
        [JsonProperty("admin_username")]
        public string AdminUsername { get; set; }

        [JsonProperty("admin_password")]
        public string AdminPassword { get; set; }
    }
}
