using Kentico.Kontent.Management.Models.Types.Elements;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;

namespace Kentico.Kontent.Management.Modules.ActionInvoker
{
    internal class ElementMetadataConverter : JsonConverter
    {
        private static readonly JsonSerializerSettings _specifiedSubclassConversion = new() 
        { 
            ContractResolver = new BaseSpecifiedConcreteClassConverter()
        };


        public override bool CanConvert(Type objectType) => (objectType == typeof(ElementMetadataBase));

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var type = jObject["type"]?.ToObject<ElementMetadataType>() 
                ?? throw new ArgumentException("Object does not contain 'type' property or it is null.", nameof(reader));

            return type switch
            {
                ElementMetadataType.Text => JsonConvert.DeserializeObject<TextElementMetadataModel>(jObject.ToString(),
                    _specifiedSubclassConversion),
                ElementMetadataType.RichText => JsonConvert.DeserializeObject<RichTextElementMetadataModel>(
                    jObject.ToString(), _specifiedSubclassConversion),
                ElementMetadataType.Number => JsonConvert.DeserializeObject<NumberElementMetadataModel>(
                    jObject.ToString(), _specifiedSubclassConversion),
                ElementMetadataType.MultipleChoice => JsonConvert.DeserializeObject<MultipleChoiceElementMetadataModel>(
                    jObject.ToString(), _specifiedSubclassConversion),
                ElementMetadataType.DateTime => JsonConvert.DeserializeObject<DateTimeElementMetadataModel>(
                    jObject.ToString(), _specifiedSubclassConversion),
                ElementMetadataType.Asset => JsonConvert.DeserializeObject<AssetElementMetadataModel>(
                    jObject.ToString(), _specifiedSubclassConversion),
                ElementMetadataType.LinkedItems => JsonConvert.DeserializeObject<LinkedItemsElementMetadataModel>(
                    jObject.ToString(), _specifiedSubclassConversion),
                ElementMetadataType.Guidelines => JsonConvert.DeserializeObject<GuidelinesElementMetadataModel>(
                    jObject.ToString(), _specifiedSubclassConversion),
                ElementMetadataType.Taxonomy => JsonConvert.DeserializeObject<TaxonomyElementMetadataModel>(
                    jObject.ToString(), _specifiedSubclassConversion),
                ElementMetadataType.UrlSlug => JsonConvert.DeserializeObject<UrlSlugElementMetadataModel>(
                    jObject.ToString(), _specifiedSubclassConversion),
                ElementMetadataType.ContentTypeSnippet => JsonConvert
                    .DeserializeObject<ContentTypeSnippetElementMetadataModel>(jObject.ToString(),
                        _specifiedSubclassConversion),
                ElementMetadataType.Custom => JsonConvert.DeserializeObject<CustomElementMetadataModel>(
                    jObject.ToString(), _specifiedSubclassConversion),
                ElementMetadataType.Subpages => JsonConvert.DeserializeObject<SubpagesElementMetadataModel>(
                    jObject.ToString(), _specifiedSubclassConversion),
                _ => throw new InvalidEnumArgumentException(nameof(type), Convert.ToInt32(type), typeof(ElementMetadataType))
            };
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException(); // won't be called because CanWrite returns false
    }
}
