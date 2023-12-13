using System.Runtime.Serialization;

namespace Kontent.Ai.Management.Models.LegacyWebhooks.Triggers;

/// Represents content types for which the webhook should be triggered.
public enum TriggerChangeType
{
    /// <summary>
    /// Content item variant.
    /// </summary>
    [EnumMember(Value = "content_item_variant")]
    LanguageVariant,

    /// <summary>
    /// Taxonomy.
    /// </summary>
    [EnumMember(Value = "taxonomy")]
    Taxonomy,
}
