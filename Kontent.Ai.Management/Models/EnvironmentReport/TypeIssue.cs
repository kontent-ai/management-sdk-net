namespace Kontent.Ai.Management.Models.EnvironmentReport;

/// <summary>
/// Problems found in a single content type.
/// </summary>
public sealed record TypeIssue
{
    /// <summary>
    /// The content type the issues belong to.
    /// </summary>
    [JsonPropertyName("type")]
    public required Metadata Type { get; init; }

    /// <summary>
    /// Issues found in the content type's elements.
    /// </summary>
    [JsonPropertyName("issues")]
    public required IReadOnlyList<ElementIssue> Issues { get; init; }
}
