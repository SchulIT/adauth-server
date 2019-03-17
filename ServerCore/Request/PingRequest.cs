using Newtonsoft.Json;
using ServerCore.Json;

namespace ServerCore.Request
{
    /// <summary>
    /// Represents a ping request.
    /// </summary>
    [JsonConverter(typeof(RequestConverter))]
    public class PingRequest : IRequest { }
}
