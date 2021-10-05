using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.LanguageVariants
{
    /// <summary>
    /// Represents language variant upsert model.
    /// </summary>
    public sealed class LanguageVariantUpsertModel
    {
        /// <summary>
        /// Gets or sets elements of the variant.
        /// </summary>
        [JsonProperty("elements", Required = Required.Always)]
        public IEnumerable<dynamic> Elements { get; set; }

        /// <summary>
        /// Creates an instance of the language variant upsert model.
        /// </summary>
        public LanguageVariantUpsertModel()
        {
        }

        internal LanguageVariantUpsertModel(LanguageVariantModel contentItemVariant)
        {
            Elements = contentItemVariant.Elements;
        }
    }
}
