using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.StronglyTyped
{
    public sealed class ContentItemVariantUpsertModel<T> where T : new()
    {
        [JsonProperty("elements", Required = Required.Always)]
        public T Elements { get; set; }

        public ContentItemVariantUpsertModel()
        {
        }

        internal ContentItemVariantUpsertModel(ContentItemVariantModel<T> contentItemVariant)
        {
            Elements = contentItemVariant.Elements;
        }
    }
}
