using Kontent.Ai.Management.Serialization.Converters;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
    public AsyncValidationTaskIssueType IssueType { get; init; }

    /// <summary>
    /// Gets information about issues found in specific elements.
    /// </summary>
    [JsonPropertyName("issues")]
    public List<ElementIssue> Issues { get; init; }
}
