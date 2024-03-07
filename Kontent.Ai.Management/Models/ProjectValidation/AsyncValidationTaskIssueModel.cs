using Kontent.Ai.Management.Modules.ActionInvoker;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.EnvironmentValidation;

/// <summary>
/// Async validation task issue.
/// </summary>
[JsonConverter(typeof(AsyncValidationTaskIssueConverter))]
public abstract class AsyncValidationTaskIssueModel
{
    /// <summary>
    /// Gets or sets the type of the async validation task issue.
    /// </summary>
    [JsonProperty("issue_type")]
    public AsyncValidationTaskIssueType IssueType { get; set; }

    /// <summary>
    /// Gets or sets information about issues found in specific elements.
    /// </summary>
    [JsonProperty("issues")]
    public List<ElementIssue> Issues { get; set; }
}
