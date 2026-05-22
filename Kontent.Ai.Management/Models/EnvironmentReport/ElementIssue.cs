using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.EnvironmentReport;

/// <summary>
/// Represents element issue with messages and element metadata
/// </summary>
public sealed record ElementIssue
{
    /// <summary>
    /// Gets information about the content element
    /// </summary>
    [JsonPropertyName("element")]
    public Metadata Element { get; init; }

    /// <summary>
    /// Gets validation messages
    /// for the content element
    /// </summary>
    [JsonPropertyName("messages")]
    public List<string> Messages { get; init; }
}
