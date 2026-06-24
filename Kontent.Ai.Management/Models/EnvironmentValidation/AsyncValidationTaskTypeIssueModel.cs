namespace Kontent.Ai.Management.Models.EnvironmentValidation;

/// <summary>
/// A content type issue found by an async validation task.
/// </summary>
public sealed record AsyncValidationTaskTypeIssueModel : AsyncValidationTaskIssueModel
{
    /// <summary>
    /// The content type the issues belong to.
    /// </summary>
    [JsonPropertyName("type")]
    public required NamedReference Type { get; init; }
}
