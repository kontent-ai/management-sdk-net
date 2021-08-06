using System.Runtime.Serialization;

namespace Kentico.Kontent.Management.Models.Webhooks.Triggers
{
    public enum TriggerChangeType
    {
        [EnumMember(Value = "content_item_variant")]
        ContentItemVariant,
        [EnumMember(Value = "taxonomy")]
        Taxonomy,
    }
}
