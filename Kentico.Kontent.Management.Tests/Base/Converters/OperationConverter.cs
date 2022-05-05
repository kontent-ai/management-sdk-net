using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using static Kentico.Kontent.Management.Tests.Base.Converters.ConverterHelper;

namespace Kentico.Kontent.Management.Tests.Base.Converters;

internal class OperationConverter<T> : JsonConverter
{
    private const string _propertyName = "Op"; 

    public override bool CanConvert(Type objectType) => HasProperty<T>(_propertyName) && IsOperationModel<T>() && IsCollectionOf<T>(objectType);

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var result = new List<T>();

        var jArray = JArray.Load(reader);

        var operationTypeMap = typeof(T)
            .Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(T)) && !t.IsAbstract)
            .Select(t =>
            {
                var instance = (T)Activator.CreateInstance(t);
                return (Key: instance.GetPropertyValue(_propertyName), Type: t);
            }).ToDictionary(tuple => tuple.Key, tuple => tuple.Type);

        foreach (var item in jArray)
        {
            var operation = item["op"].ToString();

            result.Add((T)item.ToObject(operationTypeMap[operation]));
        }

        return result;
    }

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();
}

