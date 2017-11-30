using System.Collections.Generic;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Items
{
    public sealed class ContentItemVariantUpdateModel
    {
        [JsonProperty("elements", Required = Required.Always)]
        public object Elements { get; set; }

        public ContentItemVariantUpdateModel()
        {
        }

        internal ContentItemVariantUpdateModel(ContentItemVariantModel contentItemVariant)
        {
            Elements = contentItemVariant.Elements;
        }
    }
}
