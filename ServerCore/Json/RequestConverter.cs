using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServerCore.Request;
using System;

namespace ServerCore.Json
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
                case "auth":
                    return new AuthRequest();

                case "ping":
                    return new PingRequest();
            }

            throw new ArgumentException(string.Format("Unknown action {0}", action));
        }
    }
}
