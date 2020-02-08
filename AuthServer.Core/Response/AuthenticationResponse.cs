using Newtonsoft.Json;

namespace AuthServer.Core.Response
{
    /// <summary>
    /// Represents an authentication responses (which is sent in case of an authentication request)
    /// </summary>
    public class AuthenticationResponse : AbstractUserResponse, IResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
