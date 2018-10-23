
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace Cogworks.Meganav.Converters
{
    public class ChildConverter<T> : JsonConverter
    {
        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer
        )
        {
            return serializer.Deserialize<List<T>>(reader);
        }
        public override bool CanConvert(Type objectType)
        {
            return true;
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}