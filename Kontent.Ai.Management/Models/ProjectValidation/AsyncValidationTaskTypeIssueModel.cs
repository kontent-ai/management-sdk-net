using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.ProjectValidation;

/// <summary>
/// Async validation task type issue.
/// </summary>
public sealed class AsyncValidationTaskTypeIssueModel : AsyncValidationTaskIssueModel
{
    /// <summary>
    /// Gets or sets item reference.
    /// </summary>
    [JsonProperty("type")]
    public Metadata Type { get; set; }
}
