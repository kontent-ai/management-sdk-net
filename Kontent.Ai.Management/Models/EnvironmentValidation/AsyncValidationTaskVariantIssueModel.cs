namespace Kontent.Ai.Management.Models.EnvironmentValidation;

/// <summary>
/// A language variant issue found by an async validation task.
/// </summary>
public sealed record AsyncValidationTaskVariantIssueModel : AsyncValidationTaskIssueModel
{
    /// <summary>
    /// The content item the variant belongs to.
    /// </summary>
    [JsonPropertyName("item")]
    public required Metadata Item { get; init; }

    /// <summary>
    /// The variant's language.
    /// </summary>
    [JsonPropertyName("language")]
    public required Metadata Language { get; init; }
}
