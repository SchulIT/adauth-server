using Newtonsoft.Json;
using System.Collections.Generic;

namespace AuthServer.Core.Response
{
    public class StatusResponse : IResponse
    {
        [JsonProperty("users")]
        public Dictionary<string, UserStatusResponse> Users { get; } = new Dictionary<string, UserStatusResponse>();
    }
}
