using System;
using System.Dynamic;
using System.Collections.Generic;

using KenticoCloud.ContentManagement.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KenticoCloud.ContentManagement.Modules.ActionInvoker
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
                var array = property.Value as JArray;
                if (array != null)
                {
                    // Array is always a list of references
                    resultAsDictionary.Add(property.Name, array.ToObject<IEnumerable<ObjectIdentifier>>());
                    continue;
                }

                var value = property.Value as JValue;
                if (value != null)
                {
                    resultAsDictionary.Add(property.Name, value.Value);
                    continue;
                }

                var valueObj = property.Value as JObject;
                if (valueObj != null)
                {
                    resultAsDictionary.Add(property.Name, ConvertToDynamicObject(valueObj));
                    continue;
                }

                throw new NotSupportedException($"Unsupported type of property {value.GetType()}");
            }

            return result;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var defaultResult = (JObject)serializer.Deserialize(reader);

            return ConvertToDynamicObject(defaultResult);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Serialize the object the default way
            serializer.Serialize(writer, value);
        }
    }
}
