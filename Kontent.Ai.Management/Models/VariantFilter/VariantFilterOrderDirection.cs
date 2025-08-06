using System.Runtime.Serialization;

namespace Kontent.Ai.Management.Models.VariantFilter;

/// <summary>
/// Represents the order direction for variant filter results.
/// </summary>
public enum VariantFilterOrderDirection
{
    /// <summary>
    /// Ascending order.
    /// </summary>
    [EnumMember(Value = "asc")]
    Ascending,

    /// <summary>
    /// Descending order.
    /// </summary>
    [EnumMember(Value = "desc")]
    Descending
}