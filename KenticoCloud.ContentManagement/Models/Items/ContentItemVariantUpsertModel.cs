using System.Collections.Generic;
using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Items
{
    public sealed class ContentItemVariantUpsertModel
    {
        [JsonProperty("elements", Required = Required.Always)]
        public Dictionary<string,  object> Elements { get; set; }

        public ContentItemVariantUpsertModel()
        {
        }

        internal ContentItemVariantUpsertModel(ContentItemVariantModel contentItemVariant)
        {
            Elements = contentItemVariant.Elements;
        }
    }
}
