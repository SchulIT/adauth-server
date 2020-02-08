
using Newtonsoft.Json;

namespace AuthServer.Core.Response
{
    /// <summary>
    /// Interface all responses must implement
    /// </summary>
    public interface IResponse
    {
        [JsonProperty("success")]
        bool Success { get; set; }
    }
}
