
namespace Kontent.Ai.Management.Models.VariantFilter;

/// <summary>
/// Ordering specification for the items-with-variants filter endpoint.
/// </summary>
public sealed record VariantFilterOrderModel
{
    /// <summary>
    /// Column to order by. Supported values: <c>name</c>, <c>due_date</c>, <c>last_modified</c>.
    /// </summary>
    [JsonPropertyName("by")]
    public required string By { get; init; }

    /// <summary>
    /// Sort direction. Defaults to ascending.
    /// </summary>
    [JsonPropertyName("direction")]
    public VariantFilterOrderDirection Direction { get; init; } = VariantFilterOrderDirection.Ascending;
}