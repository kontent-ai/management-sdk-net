using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.EnvironmentValidation;

/// <summary>
/// Async validation task type issue.
/// </summary>
public sealed record AsyncValidationTaskTypeIssueModel : AsyncValidationTaskIssueModel
{
    /// <summary>
    /// Gets item reference.
    /// </summary>
    [JsonProperty("type")]
    public Metadata Type { get; init; }
}
