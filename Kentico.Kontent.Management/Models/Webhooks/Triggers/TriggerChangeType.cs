using System.Runtime.Serialization;

namespace Kentico.Kontent.Management.Models.Webhooks.Triggers
{
    /// Represents content types for which the webhook should be triggered.
    public enum TriggerChangeType
    {
        /// <summary>
        /// Content item variant.
        /// </summary>
        [EnumMember(Value = "content_item_variant")]
        ContentItemVariant,

        /// <summary>
        /// Taxonomy.
        /// </summary>
        [EnumMember(Value = "taxonomy")]
        Taxonomy,
    }
}
