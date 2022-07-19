using System;
using System.ComponentModel;
using Kontent.Ai.Management.Models.AssetRenditions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kontent.Ai.Management.Modules.ActionInvoker;

internal class ImageTransformationConverter : JsonConverter
{
    private static readonly JsonSerializerSettings _specifiedSubclassConversion = new() { ContractResolver = new BaseSpecifiedConcreteClassConverter() };

    public override bool CanConvert(Type objectType) => objectType == typeof(ImageTransformation);

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);
        var type = jObject["mode"]?.ToObject<ImageTransformationMode>() ?? throw new ArgumentException("Object does not contain 'mode' property or it is null.", nameof(reader));

        return type switch
        {
            ImageTransformationMode.Rect =>
                JsonConvert.DeserializeObject<RectangleResizeTransformation>(jObject.ToString(), _specifiedSubclassConversion),
            _ => throw new InvalidEnumArgumentException(nameof(type), Convert.ToInt32(type), typeof(ImageTransformationMode))
        };
    }

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException(); // won't be called because CanWrite returns false
}