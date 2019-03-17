
using Newtonsoft.Json;

namespace ServerCore.Response
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
