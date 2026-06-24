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
    public required NamedReference Item { get; init; }

    /// <summary>
    /// The variant's language.
    /// </summary>
    [JsonPropertyName("language")]
    public required NamedReference Language { get; init; }

    /// <summary>
    /// Issues found in the variant's elements.
    /// </summary>
    [JsonPropertyName("issues")]
    public required IReadOnlyList<ElementIssue> Issues { get; init; }
}
