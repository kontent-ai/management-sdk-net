using Kentico.Kontent.Management.Models.Types.Elements;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Kentico.Kontent.Management.Modules.ActionInvoker
{
    internal class ElementMetadataConverter : JsonConverter
    {
        private static readonly JsonSerializerSettings _specifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new BaseSpecifiedConcreteClassConverter() };


        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(ElementMetadataBase));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            var type = jObject["type"].ToObject<ElementMetadataType>();

            switch (type)
            {
                case ElementMetadataType.Text:
                    return JsonConvert.DeserializeObject<TextElementMetadataModel>(jObject.ToString(), _specifiedSubclassConversion);
                case ElementMetadataType.RichText:
                    return JsonConvert.DeserializeObject<RichTextElementMetadataModel>(jObject.ToString(), _specifiedSubclassConversion);
                case ElementMetadataType.Number:
                    return JsonConvert.DeserializeObject<NumberElementMetadataModel>(jObject.ToString(), _specifiedSubclassConversion);
                case ElementMetadataType.MultipleChoice:
                    return JsonConvert.DeserializeObject<MultipleChoiceElementMetadataModel>(jObject.ToString(), _specifiedSubclassConversion);
                case ElementMetadataType.DateTime:
                    return JsonConvert.DeserializeObject<DateTimeElementMetadataModel>(jObject.ToString(), _specifiedSubclassConversion);
                case ElementMetadataType.Asset:
                    return JsonConvert.DeserializeObject<AssetElementMetadataModel>(jObject.ToString(), _specifiedSubclassConversion);
                case ElementMetadataType.LinkedItems:
                    return JsonConvert.DeserializeObject<LinkedItemsElementMetadataModel>(jObject.ToString(), _specifiedSubclassConversion);
                case ElementMetadataType.Guidelines:
                    return JsonConvert.DeserializeObject<GuidelinesElementMetadataModel>(jObject.ToString(), _specifiedSubclassConversion);
                case ElementMetadataType.Taxonomy:
                    return JsonConvert.DeserializeObject<TaxonomyElementMetadataModel>(jObject.ToString(), _specifiedSubclassConversion);
                case ElementMetadataType.UrlSlug:
                    return JsonConvert.DeserializeObject<UrlSlugElementMetadataModel>(jObject.ToString(), _specifiedSubclassConversion);
                case ElementMetadataType.ContentTypeSnippet:
                    return JsonConvert.DeserializeObject<ContentTypeSnippetElementMetadataModel>(jObject.ToString(), _specifiedSubclassConversion);
                case ElementMetadataType.Custom:
                    return JsonConvert.DeserializeObject<CustomElementMetadataModel>(jObject.ToString(), _specifiedSubclassConversion);
            }

            throw new NotImplementedException();
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException(); // won't be called because CanWrite returns false
        }
    }
}
