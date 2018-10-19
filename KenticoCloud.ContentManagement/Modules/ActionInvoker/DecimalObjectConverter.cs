using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace KenticoCloud.ContentManagement.Modules.ActionInvoker
{
    internal class DecimalObjectConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(decimal) || objectType == typeof(decimal?);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            => throw new NotImplementedException($"{nameof(DecimalObjectConverter)} class should be only used for serialization.");

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch (value)
            {
                case decimal dec when dec == 0:
                    // Serialize decimal(0) to 0 instead of 0.0
                    // NOTE: See issue #29
                    JToken.FromObject(0).WriteTo(writer);
                    break;

                default:
                    JToken.FromObject(value).WriteTo(writer);
                    break;
            }
        }
    }
}
