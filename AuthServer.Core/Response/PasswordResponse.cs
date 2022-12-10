using AuthServer.Core.Provider;
using Newtonsoft.Json;

namespace AuthServer.Core.Response
{
    public class PasswordResponse : IResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("result")]
        public string Result { get; set; }
    }
}
