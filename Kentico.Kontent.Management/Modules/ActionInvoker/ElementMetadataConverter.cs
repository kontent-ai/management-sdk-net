using Kentico.Kontent.Management.Models.Types.Elements;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Modules.ActionInvoker
{
    internal class ElementMetadataConverter : JsonConverter
    {
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new BaseSpecifiedConcreteClassConverter() };


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
                case ElementMetadataType.Undefined:
                    throw new Exception();
                case ElementMetadataType.Text:
                    return JsonConvert.DeserializeObject<TextElementMetadataModel>(jObject.ToString(), SpecifiedSubclassConversion);
                case ElementMetadataType.RichText:
                    return JsonConvert.DeserializeObject<RichTextElementMetadataModel>(jObject.ToString(), SpecifiedSubclassConversion);
                case ElementMetadataType.Number:
                    return JsonConvert.DeserializeObject<NumberElementMetadataModel>(jObject.ToString(), SpecifiedSubclassConversion);
                case ElementMetadataType.MultipleChoice:
                    return JsonConvert.DeserializeObject<MultipleChoiceElementMetadataModel>(jObject.ToString(), SpecifiedSubclassConversion);
                case ElementMetadataType.DateTime:
                    return JsonConvert.DeserializeObject<DateTimeElementMetadataModel>(jObject.ToString(), SpecifiedSubclassConversion);
                case ElementMetadataType.Asset:
                    return JsonConvert.DeserializeObject<AssetElementMetadataModel>(jObject.ToString(), SpecifiedSubclassConversion);
                case ElementMetadataType.LinkedItems:
                    return JsonConvert.DeserializeObject<LinkedItemsElementMetadataModel>(jObject.ToString(), SpecifiedSubclassConversion);
                case ElementMetadataType.Guidelines:
                    return JsonConvert.DeserializeObject<GuidelinesElementMetadataModel>(jObject.ToString(), SpecifiedSubclassConversion);
                case ElementMetadataType.Taxonomy:
                    return JsonConvert.DeserializeObject<TaxonomyElementMetadataModel>(jObject.ToString(), SpecifiedSubclassConversion);
                case ElementMetadataType.UrlSlug:
                    return JsonConvert.DeserializeObject<UrlSlugElementMetadataModel>(jObject.ToString(), SpecifiedSubclassConversion);
                case ElementMetadataType.Snippet:
                    return JsonConvert.DeserializeObject<ContentTypeSnippetElementMetadataModel>(jObject.ToString(), SpecifiedSubclassConversion);
                case ElementMetadataType.Custom:
                    return JsonConvert.DeserializeObject<CustomElementMetadataModel>(jObject.ToString(), SpecifiedSubclassConversion);
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
