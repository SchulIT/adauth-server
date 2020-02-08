using AuthServer.Core.Handler;
using AuthServer.Core.Json;
using AuthServer.Core.Request;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace AuthServer.Core.Protocol
{
    /// <summary>
    /// The request handler for the JSON protocol.
    /// </summary>
    public class JsonProtocol : IProtocol
    {
        private IProtocolHandler protocolHandler;

        public JsonProtocol(IProtocolHandler protocolHandler)
        {
            this.protocolHandler = protocolHandler;
        }

        public Task<bool> CanHandleAsync(string json)
        {
            json = json.Trim();

            /**
             * Optimisation: JSON of length 0 or JSON not ending with } is not valid
             * as we assume to receive an JSON object.
             */
            if (json.Length == 0 || json[json.Length - 1] != '}')
            {
                return Task.FromResult(false);
            }

            return Task.Run(() =>
            {
                try
                {
                    JObject.Parse(json);
                }
                catch
                {
                    return false;
                }

                return true;
            });
        }

        public Task<string> HandleAsync(string json)
        {
            return Task.Run(() =>
            {
                var request = JsonConvert.DeserializeObject<IRequest>(json, new RequestConverter());
                var response = protocolHandler.Handle(request);

                var jsonResponse = JsonConvert.SerializeObject(response);
                return jsonResponse;
            });
        }
    }
}
