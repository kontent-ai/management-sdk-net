using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.EnvironmentReport;

/// <summary>
/// Represents the report of the problems found in the environment's content types.
/// </summary>
public sealed record TypeIssue
{
    /// <summary>
    /// Gets information about content type
    /// </summary>
    [JsonPropertyName("type")]
    public Metadata Type { get; init; }

    /// <summary>
    /// Gets information about environment language
    /// </summary>
    [JsonPropertyName("issues")]
    public IEnumerable<ElementIssue> Issues { get; init; }
}
