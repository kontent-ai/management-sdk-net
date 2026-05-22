using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.EnvironmentReport;

/// <summary>
/// Represents information necessary to identify 
/// the language variant and lists the content elements
/// </summary>
public sealed record VariantIssue
{
    /// <summary>
    /// Gets information about the content item
    /// </summary>
    [JsonPropertyName("item")]
    public Metadata Item { get; init; }

    /// <summary>
    /// Gets information about environment language
    /// </summary>
    [JsonPropertyName("language")]
    public Metadata Language { get; init; }

    /// <summary>
    /// Gets information about issues
    /// found in specific content elements
    /// </summary>
    [JsonPropertyName("issues")]
    public List<ElementIssue> Issues { get; init; }
}
