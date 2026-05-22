
namespace Kontent.Ai.Management.Models.EnvironmentValidation;

/// <summary>
/// Async validation task variant issue.
/// </summary>
public sealed record AsyncValidationTaskVariantIssueModel : AsyncValidationTaskIssueModel
{
    /// <summary>
    /// Gets item reference.
    /// </summary>
    [JsonPropertyName("item")]
    public Metadata Item { get; init; }

    /// <summary>
    /// Gets language reference.
    /// </summary>
    [JsonPropertyName("language")]
    public Metadata Language { get; init; }
}
