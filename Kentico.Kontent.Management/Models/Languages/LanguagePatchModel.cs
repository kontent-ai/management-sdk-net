using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Languages
{
    /// <summary>
    /// Represents the replace operation on languages.
    /// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-language
    /// </summary>
    public class LanguagePatchModel
    {
        /// <summary>
        /// Represents the replace operation.
        /// </summary>
        [JsonProperty("op")]
        public static string Op => "replace";

        /// <summary>
        /// Gets or sets the name of the language property that you want to modify.
        /// Enum: "name" "codename" "fallback_language" "is_active"
        /// </summary>
        [JsonProperty("property_name")]
        public LanguagePropertyName PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the value or object to insert in the specified property. The format of the value property depends on the value of the property_name property.
        /// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-language
        /// </summary>
        [JsonProperty("value")]
        public dynamic Value { get; set; }
    }
}
