namespace Kontent.Ai.Management.Models.EnvironmentReport;

/// <summary>
/// The validation report for an environment (response shape).
/// </summary>
public sealed record EnvironmentReportModel
{
    /// <summary>
    /// Information about the validated environment.
    /// </summary>
    [JsonPropertyName("project")]
    public required Environment Environment { get; init; }

    /// <summary>
    /// Problems found in the environment's content. May be empty.
    /// </summary>
    [JsonPropertyName("variant_issues")]
    public required List<VariantIssue> VariantIssues { get; init; }

    /// <summary>
    /// Problems found in the environment's content types. May be empty.
    /// </summary>
    [JsonPropertyName("type_issues")]
    public required List<TypeIssue> TypeIssues { get; init; }
}
