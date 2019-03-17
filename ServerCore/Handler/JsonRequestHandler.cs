using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServerCore.Auth;
using ServerCore.Json;
using ServerCore.Request;
using ServerCore.Response;
using ServerCore.Settings;
using System.Threading.Tasks;

namespace ServerCore.Handler
{
    /// <summary>
    /// The request handler for the JSON protocol.
    /// </summary>
    public class JsonRequestHandler : IRequestHandler
    {
        private ISettings settings;
        private IAuthenticationService authenticationService;

        public JsonRequestHandler(IAuthenticationService authenticationService, ISettings settings)
        {
            this.authenticationService = authenticationService;
            this.settings = settings;
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
                /*
                 * Daten parsen und entsprechend antworten
                 */
                var request = JsonConvert.DeserializeObject<IRequest>(json, new RequestConverter());
                IResponse response = new PongResponse { Success = false };

                if (request is AuthRequest)
                {
                    var authRequest = (request as AuthRequest);

                    response = authenticationService.Login(authRequest.Username, authRequest.Password);
                }
                else if (request is PingRequest)
                {
                    response = new PongResponse { Success = true };
                }

                var jsonResponse = JsonConvert.SerializeObject(response);
                return jsonResponse;
            });
        }
    }
}
