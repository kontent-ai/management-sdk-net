namespace Kontent.Ai.Management.Models.EnvironmentReport;

/// <summary>
/// Validation messages for a single content element.
/// </summary>
public sealed record ElementIssue
{
    /// <summary>
    /// The content element the messages belong to.
    /// </summary>
    [JsonPropertyName("element")]
    public required Metadata Element { get; init; }

    /// <summary>
    /// Validation messages for the element.
    /// </summary>
    [JsonPropertyName("messages")]
    public required List<string> Messages { get; init; }
}
