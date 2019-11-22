using System;
using Kentico.Kontent.Management.Models.Items;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.StronglyTyped
{
    /// <summary>
    /// Represents strongly typed content item variant model.
    /// </summary>
    public sealed class ContentItemVariantModel<T> where T : new()
    {
        /// <summary>
        /// Gets or sets item of the variant.
        /// </summary>
        [JsonProperty("item")]
        public ObjectIdentifier Item { get; set; }

        /// <summary>
        /// Gets or sets elements of the variant as custom class.
        /// </summary>
        [JsonProperty("elements")]
        public T Elements { get; set; }

        /// <summary>
        /// Gets or sets language of the variant.
        /// </summary>
        [JsonProperty("language")]
        public LanguageIdentifier Language { get; set; }

        /// <summary>
        /// Gets or sets last modified timestamp of the content item.
        /// </summary>
        [JsonProperty("last_modified")]
        public DateTime? LastModified { get; set; }
    }
}
