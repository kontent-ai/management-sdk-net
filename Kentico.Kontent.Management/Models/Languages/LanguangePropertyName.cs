using System.Runtime.Serialization;

namespace Kentico.Kontent.Management.Models.Languages
{
    /// <summary>
    /// Represents properties that can be modified on the content language.
    /// </summary>
    public enum LanguagePropertyName
    {
        /// <summary>
        /// The language's codename
        /// </summary>
        [EnumMember(Value = "codename")]
        Codename,

        /// <summary>
        /// The language's display name.
        /// </summary>
        [EnumMember(Value = "name")]
        Name,

        /// <summary>
        /// Fall back language.
        /// Language to use when the current language contains no content.
        /// </summary>
        [EnumMember(Value = "fallback_language")]
        FallbackLanguage,

        /// <summary>
        /// A flag determining whether the language is active.
        /// </summary>
        [EnumMember(Value = "is_active")]
        IsActive,
    }
}
