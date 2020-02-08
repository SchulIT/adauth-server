using AuthServer.Core.Json;
using Newtonsoft.Json;

namespace AuthServer.Core.Request
{
    /// <summary>
    /// Represents a ping request.
    /// </summary>
    [JsonConverter(typeof(RequestConverter))]
    public class PingRequest : IRequest { }
}
