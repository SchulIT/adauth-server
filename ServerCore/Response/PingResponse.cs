
using Newtonsoft.Json;

namespace ServerCore.Response
{
    /// <summary>
    /// Represents a pong response (which is sent in case of a ping request)
    /// </summary>
    public class PongResponse : IResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
