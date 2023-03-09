using Kontent.Ai.Management.Models.AssetFolders.Patch;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using static Kontent.Ai.Management.Tests.Base.Converters.ConverterHelper;

namespace Kontent.Ai.Management.Tests.Base.Converters;

internal class AssetFolderOperationBaseModelConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => IsCollectionOf<AssetFolderOperationBaseModel>(objectType);

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var result = new List<AssetFolderOperationBaseModel>();

        var jArray = JArray.Load(reader);

        var operationTypeMap = typeof(AssetFolderOperationBaseModel)
            .Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(AssetFolderOperationBaseModel)) && !t.IsAbstract)
            .Select(t =>
            {
                var instance = (AssetFolderOperationBaseModel)Activator.CreateInstance(t);
                return (Key: instance.Op, Type: t);
            }).ToDictionary(tuple => tuple.Key, tuple => tuple.Type);

        foreach (var item in jArray)
        {
            var operation = item["op"].ToString();

            result.Add((AssetFolderOperationBaseModel)item.ToObject(operationTypeMap[operation]));
        }

        return result;
    }

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();
}

