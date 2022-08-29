using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Kontent.Ai.Management.Modules.ActionInvoker;

internal class AssetWithRenditionsReferenceConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => objectType == typeof(AssetWithRenditionsReference);

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);
        var id = jObject["id"]?.ToObject<string>() 
                 ?? throw new ArgumentException("Object does not contain 'id' property or it is null.", nameof(reader));

        return new AssetWithRenditionsReference(Reference.ById(Guid.Parse(id)));
    }

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) =>
        throw new NotImplementedException(); // won't be called because CanWrite returns false
}
