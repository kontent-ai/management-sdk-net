using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.EnvironmentValidation;

/// <summary>
/// Async validation task variant issue.
/// </summary>
public sealed record AsyncValidationTaskVariantIssueModel : AsyncValidationTaskIssueModel
{
    /// <summary>
    /// Gets item reference.
    /// </summary>
    [JsonProperty("item")]
    public Metadata Item { get; init; }

    /// <summary>
    /// Gets language reference.
    /// </summary>
    [JsonProperty("language")]
    public Metadata Language { get; init; }
}
