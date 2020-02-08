using AuthServer.Core.Request;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace AuthServer.Core.Json
{
    /// <summary>
    /// JSON converter which converts JSON into objects based on the "action" property:
    /// 
    /// * "auth" -> AuthRequest
    /// * "ping" -> PingRequest
    /// </summary>
    internal class RequestConverter : JsonCreationConverter<IRequest>
    {
        protected override IRequest Create(JsonReader reader, JObject jObject)
        {
            if (!FieldExists("action", jObject))
            {
                throw new ArgumentException("jObject must have property action");
            }

            var action = jObject["action"].ToString();

            switch (action)
            {
                case "authenticate":
                    return new AuthenticationRequest();

                case "ping":
                    return new PingRequest();

                case "status":
                    return new StatusRequest();
            }

            throw new ArgumentException(string.Format("Unknown action {0}", action));
        }
    }
}
