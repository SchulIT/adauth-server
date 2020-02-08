using Newtonsoft.Json;
using System.Collections.Generic;

namespace AuthServer.Core.Response
{
    public class StatusResponse : IResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; } = true;

        [JsonProperty("users")]
        public Dictionary<string, UserStatusResponse> Users { get; } = new Dictionary<string, UserStatusResponse>();
    }
}
