using Kontent.Ai.Management.Models.TypeSnippets.Patch;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using static Kontent.Ai.Management.Tests.Base.Converters.ConverterHelper;

namespace Kontent.Ai.Management.Tests.Base.Converters;

internal class ContentTypeSnippetOperationBaseModelConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => IsCollectionOf<ContentTypeSnippetOperationBaseModel>(objectType);

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var result = new List<ContentTypeSnippetOperationBaseModel>();

        var jArray = JArray.Load(reader);

        var operationTypeMap = typeof(ContentTypeSnippetOperationBaseModel)
            .Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(ContentTypeSnippetOperationBaseModel)) && !t.IsAbstract)
            .Select(t =>
            {
                var instance = (ContentTypeSnippetOperationBaseModel)Activator.CreateInstance(t);
                return (Key: instance.Op, Type: t);
            }).ToDictionary(tuple => tuple.Key, tuple => tuple.Type);

        foreach (var item in jArray)
        {
            var operation = item["op"].ToString();

            result.Add((ContentTypeSnippetOperationBaseModel)item.ToObject(operationTypeMap[operation]));
        }

        return result;
    }

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();
}
