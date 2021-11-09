using System.Collections.Generic;
using System.Linq;
using Kentico.Kontent.Management.Models.LanguageVariants.Elements;
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
        public IEnumerable<BaseElement> Elements { get; set; }

        /// <summary>
        /// Creates an instance of the language variant upsert model.
        /// </summary>
        public LanguageVariantUpsertModel()
        {
        }

        internal LanguageVariantUpsertModel(LanguageVariantModel LanguageVariant)
        {
            Elements = LanguageVariant.Elements;
        }
    }
}
