using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;

namespace AuthServer.Core.Json
{
    /// <summary>
    /// Abstract converter which helps converting JSON strings into objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal abstract class JsonCreationConverter<T> : JsonConverter where T : class
    {
        public override bool CanWrite
        {
            get { return false; }
        }

        public override bool CanRead
        {
            get { return true; }
        }

        protected abstract T Create(JsonReader reader, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            var targetObject = Create(reader, jObject);

            serializer.Populate(jObject.CreateReader(), targetObject);
            return targetObject;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        protected bool FieldExists(string fieldName, JObject jObject)
        {
            return jObject[fieldName] != null;
        }
    }
}
