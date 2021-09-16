using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Languages
{
    /// <summary>
    /// Represents the langueage create model.
    /// </summary>
    public class LanguageCreateModel
    {
        /// <summary>
        /// Gets or sets the language's display name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the language's codename.
        /// </summary>
        [JsonProperty("codename")]
        public string Codename { get; set; }

        /// <summary>
        /// Gets or sets the language's external id.
        /// </summary>
        [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets or sets a flag determining whether the language is active.
        /// </summary>
        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets language to use when the current language contains no content. With multiple languages you can create fallback chains.
        /// </summary>
        [JsonProperty("fallback_language")]
        public Reference FallbackLanguage { get; set; }
    }
}
