using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cogworks.Meganav.Converters
{
    public class DictionaryConverter : JsonConverter
    {
        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            IDictionary<string, object> result;

            if (reader.TokenType == JsonToken.StartArray)
            {
                var legacyArray = (JArray)JToken.ReadFrom(reader);

                result = legacyArray.ToDictionary(
                    el => el["Key"].ToString(),
                    el => (object)el["Value"]);
            }
            else
            {
                result =
                    (IDictionary<string, object>)
                    serializer.Deserialize(reader, typeof(IDictionary<string, object>));
            }

            return result;
        }
        
        public override bool CanConvert(Type objectType)
        {
            return typeof(IDictionary<string, object>).IsAssignableFrom(objectType);
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}