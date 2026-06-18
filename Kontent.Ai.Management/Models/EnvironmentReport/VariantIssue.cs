namespace Kontent.Ai.Management.Models.EnvironmentReport;

/// <summary>
/// Problems found in a single language variant.
/// </summary>
public sealed record VariantIssue
{
    /// <summary>
    /// The content item the variant belongs to.
    /// </summary>
    [JsonPropertyName("item")]
    public required Metadata Item { get; init; }

    /// <summary>
    /// The variant's language.
    /// </summary>
    [JsonPropertyName("language")]
    public required Metadata Language { get; init; }

    /// <summary>
    /// Issues found in the variant's elements.
    /// </summary>
    [JsonPropertyName("issues")]
    public required IReadOnlyList<ElementIssue> Issues { get; init; }
}
