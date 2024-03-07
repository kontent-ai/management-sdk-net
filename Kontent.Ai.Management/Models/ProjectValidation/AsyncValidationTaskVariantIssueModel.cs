using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.EnvironmentValidation;

/// <summary>
/// Async validation task variant issue.
/// </summary>
public sealed class AsyncValidationTaskVariantIssueModel : AsyncValidationTaskIssueModel
{
    /// <summary>
    /// Gets or sets item reference.
    /// </summary>
    [JsonProperty("item")]
    public Metadata Item { get; set; }

    /// <summary>
    /// Gets or sets language reference.
    /// </summary>
    [JsonProperty("language")]
    public Metadata Language { get; set; }
}
