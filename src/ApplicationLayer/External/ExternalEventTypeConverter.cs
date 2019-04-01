using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApplicationLayer.External
{
    /// <summary>
    /// Provides conversion to <see cref="ExternalEvent"/> by inspecting the 'type'/'Type' property in the json body.
    /// </summary>
    public sealed class ExternalEventTypeConverter : JsonConverter
    {
        private static readonly Dictionary<string, Type> _typeLookUp = new Dictionary<string, Type>();

        static ExternalEventTypeConverter()
        {
            foreach (var type in typeof(ExternalEvent).Assembly.GetTypes())
            {
                if (typeof(ExternalEvent).IsAssignableFrom(type))
                {
                    _typeLookUp.Add(type.Name.Replace("event", string.Empty), type);
                }
            }
        }

        public override bool CanRead => true;

        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return typeof(ExternalEvent).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject jo = JObject.Load(reader);
                var typeStr = jo["type"].Value<string>() ?? jo["Type"].Value<string>();
                if (_typeLookUp.ContainsKey(typeStr))
                {
                    existingValue = jo.ToObject(_typeLookUp[typeStr]);
                }
            }

            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
