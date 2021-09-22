using System;
using System.Linq;
using System.Dynamic;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kentico.Kontent.Management.Modules.ActionInvoker
{
    internal class DynamicObjectJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(object);
        }

        private static dynamic ConvertToDynamicObject(JObject obj)
        {
            dynamic result = new ExpandoObject();

            var resultAsDictionary = ((IDictionary<string, object>)result);

            foreach (var property in obj.Properties())
            {
                if (property.Value is JArray array)
                {
                    resultAsDictionary.Add(property.Name, array.Select(arrayObject => ConvertJComplexObject(arrayObject)));
                    continue;
                }

                var value = property.Value as JValue;
                if (value != null)
                {
                    resultAsDictionary.Add(property.Name, value.Value);
                    continue;
                }

                if (property.Value is JObject valueObj)
                {
                    resultAsDictionary.Add(property.Name, ConvertToDynamicObject(valueObj));
                    continue;
                }

                throw new NotSupportedException($"Unsupported type of property {value.GetType()}");
            }

            return result;
        }


        private static object ConvertJComplexObject(object input) 
        {
            switch (input)
            {
                case JObject obj:
                    return ConvertToDynamicObject(obj);
                case JArray array:
                    return array.Select(obj => ConvertJComplexObject(obj));
                default:
                    throw new ArgumentOutOfRangeException("JSON deserialization did not return either object nor array.");
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var defaultResult = serializer.Deserialize(reader);

            return ConvertJComplexObject(defaultResult);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Serialize the object the default way
            serializer.Serialize(writer, value);
        }
    }
}
