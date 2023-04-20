using Kontent.Ai.Management.Models.TaxonomyGroups.Patch;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using static Kontent.Ai.Management.Tests.Base.Converters.ConverterHelper;

namespace Kontent.Ai.Management.Tests.Base.Converters;

internal class TaxonomyGroupOperationBaseModelConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => IsCollectionOf<TaxonomyGroupOperationBaseModel>(objectType);

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var result = new List<TaxonomyGroupOperationBaseModel>();

        var jArray = JArray.Load(reader);

        var operationTypeMap = typeof(TaxonomyGroupOperationBaseModel)
            .Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(TaxonomyGroupOperationBaseModel)) && !t.IsAbstract)
            .Select(t =>
            {
                var instance = (TaxonomyGroupOperationBaseModel)Activator.CreateInstance(t);
                return (Key: instance.Op, Type: t);
            }).ToDictionary(tuple => tuple.Key, tuple => tuple.Type);

        foreach (var item in jArray)
        {
            var operation = item["op"].ToString();

            result.Add((TaxonomyGroupOperationBaseModel)item.ToObject(operationTypeMap[operation]));
        }

        return result;
    }

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();
}

