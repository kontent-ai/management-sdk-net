using System.Collections.Generic;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Items
{
    public sealed class ContentItemVariantUpdateModel
    {
        [JsonProperty("elements", Required = Required.Always)]
        public Dictionary<string, object> Elements { get; set; }

        internal ContentItemVariantUpdateModel()
        {
        }

        internal ContentItemVariantUpdateModel(ContentItemVariantModel contentItemVariant)
        {
            Elements = new Dictionary<string, object>(contentItemVariant.Elements);
        }
    }
}
