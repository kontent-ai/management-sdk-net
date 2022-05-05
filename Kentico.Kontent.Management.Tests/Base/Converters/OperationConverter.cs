using Kentico.Kontent.Management.Models.Types.Patch;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using static Kentico.Kontent.Management.Tests.Base.Converters.ConverterHelper;

namespace Kentico.Kontent.Management.Tests.Base.Converters;

internal class ContentTypeOperationBaseModelConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => IsCollectionOf<ContentTypeOperationBaseModel>(objectType);

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var result = new List<ContentTypeOperationBaseModel>();

        var jArray = JArray.Load(reader);

        var operationTypeMap = typeof(ContentTypeOperationBaseModel)
            .Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(ContentTypeOperationBaseModel)) && !t.IsAbstract)
            .Select(t =>
            {
                var instance = (ContentTypeOperationBaseModel)Activator.CreateInstance(t);
                return (Key: instance.Op, Type: t);
            }).ToDictionary(tuple => tuple.Key, tuple => tuple.Type);

        foreach (var item in jArray)
        {
            var operation = item["op"].ToString();

            result.Add((ContentTypeOperationBaseModel)item.ToObject(operationTypeMap[operation]));
        }

        return result;
    }

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();
}

