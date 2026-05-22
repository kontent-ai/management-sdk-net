using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.ItemWithVariant;

/// <summary>
/// Represents the request model for bulk-getting items with variants.
/// </summary>
public sealed record ItemWithVariantBulkGetRequestModel
{
    /// <summary>
    /// Gets the variant identifiers (item + language pairs).
    /// </summary>
    [JsonPropertyName("variants")]
    public IEnumerable<VariantIdentifierModel> Variants { get; init; }
}
