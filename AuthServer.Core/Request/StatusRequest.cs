using Newtonsoft.Json;
using System.Collections.Generic;

namespace AuthServer.Core.Request
{
    public class StatusRequest : IRequest
    {
        [JsonProperty("users")]
        public List<string> Users { get; set; }
    }
}
