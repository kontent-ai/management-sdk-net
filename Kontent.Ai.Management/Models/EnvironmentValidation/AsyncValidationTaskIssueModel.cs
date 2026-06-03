using Kontent.Ai.Management.Serialization.Converters;

namespace Kontent.Ai.Management.Models.EnvironmentValidation;

/// <summary>
/// Async validation task issue.
/// </summary>
[JsonConverter(typeof(AsyncValidationTaskIssueJsonConverter))]
public abstract record AsyncValidationTaskIssueModel
{
    /// <summary>
    /// Gets the type of the async validation task issue.
    /// </summary>
    [JsonPropertyName("issue_type")]
    public required AsyncValidationTaskIssueType IssueType { get; init; }

    /// <summary>
    /// Gets information about issues found in specific elements.
    /// </summary>
    [JsonPropertyName("issues")]
    public required List<ElementIssue> Issues { get; init; }
}
