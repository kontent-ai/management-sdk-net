using System;
using KenticoCloud.ContentManagement.Models.Items;
using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.StronglyTyped
{
    public sealed class ContentItemVariantModel<T> where T : new()
    {
        [JsonProperty("item")]
        public ManageApiReference Item { get; set; }

        [JsonProperty("elements")]
        public T Elements { get; set; }

        [JsonProperty("language")]
        public LanguageIdentifier Language { get; set; }

        [JsonProperty("last_modified")]
        public DateTime? LastModified { get; set; }
    }
}
