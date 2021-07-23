using Kentico.Kontent.Management.Models.Types.Elements;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Modules.ActionInvoker
{
    public class ElementMetadataConverter : JsonConverter
    {
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new BaseSpecifiedConcreteClassConverter() };


        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(ElementMetadataBase));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            var type = jo["type"].ToObject<ElementMetadataType>();

            switch (type)
            {
                case ElementMetadataType.Undefined:
                    throw new Exception();
                case ElementMetadataType.Text:
                    return JsonConvert.DeserializeObject<TextElementMetadataModel>(jo.ToString(), SpecifiedSubclassConversion);
                case ElementMetadataType.RichText:
                    return JsonConvert.DeserializeObject<RichTextElementMetadataModel>(jo.ToString(), SpecifiedSubclassConversion);
                case ElementMetadataType.Number:
                    return JsonConvert.DeserializeObject<NumberElementMetadataModel>(jo.ToString(), SpecifiedSubclassConversion);
                case ElementMetadataType.MultipleChoice:
                    return JsonConvert.DeserializeObject<MultipleChoiceElementMetadataModel>(jo.ToString(), SpecifiedSubclassConversion);
                case ElementMetadataType.DateTime:
                    return JsonConvert.DeserializeObject<DateTimeElementMetadataModel>(jo.ToString(), SpecifiedSubclassConversion);
                case ElementMetadataType.Asset:
                    return JsonConvert.DeserializeObject<AssetElementMetadataModel>(jo.ToString(), SpecifiedSubclassConversion);
                case ElementMetadataType.ModularContent:
                    return JsonConvert.DeserializeObject<LinkedItemsElementMetadataModel>(jo.ToString(), SpecifiedSubclassConversion);
                case ElementMetadataType.Guidelines:
                    return JsonConvert.DeserializeObject<GuidelinesElementMetadataModel>(jo.ToString(), SpecifiedSubclassConversion);
                case ElementMetadataType.Taxonomy:
                    return JsonConvert.DeserializeObject<TaxonomyElementMetadataModel>(jo.ToString(), SpecifiedSubclassConversion);
                case ElementMetadataType.UrlSlug:
                    return JsonConvert.DeserializeObject<UrlSlugElementMetadataModel>(jo.ToString(), SpecifiedSubclassConversion);
                case ElementMetadataType.Snippet:
                    return JsonConvert.DeserializeObject<ContentTypeSnippetElementMetadataModel>(jo.ToString(), SpecifiedSubclassConversion);
                case ElementMetadataType.Custom:
                    return JsonConvert.DeserializeObject<CustomElementMetadataModel>(jo.ToString(), SpecifiedSubclassConversion);
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
