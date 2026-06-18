namespace Kontent.Ai.Management.Models.EnvironmentValidation;

/// <summary>
/// Represents element issue with messages and element metadata.
/// </summary>
public sealed record ElementIssue
{
    /// <summary>
    /// Gets information about the element.
    /// </summary>
    [JsonPropertyName("element")]
    public required Metadata Element { get; init; }

    /// <summary>
    /// Gets validation messages for the element.
    /// </summary>
    [JsonPropertyName("messages")]
    public required IReadOnlyList<string> Messages { get; init; }
}
