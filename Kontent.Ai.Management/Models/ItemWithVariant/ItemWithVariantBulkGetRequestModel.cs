namespace Kontent.Ai.Management.Models.ItemWithVariant;

/// <summary>
/// Request payload for the bulk-get items-with-variants endpoint.
/// </summary>
public sealed record ItemWithVariantBulkGetRequestModel
{
    /// <summary>
    /// Variant identifiers (item + language pairs).
    /// </summary>
    [JsonPropertyName("variants")]
    public required IReadOnlyList<VariantIdentifierModel> Variants { get; init; }
}
