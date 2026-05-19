using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.EnvironmentReport;

/// <summary>
/// Represents environment report model
/// </summary>
public sealed record EnvironmentReportModel
{
    /// <summary>
    /// Gets information about the specified environment
    /// </summary>
    [JsonPropertyName("project")]
    public Environment Environment { get; init; }

    /// <summary>
    /// Gets reports of the problems found in the environment's content
    /// </summary>
    [JsonPropertyName("variant_issues")]
    public List<VariantIssue> VariantIssues { get; init; }

    /// <summary>
    /// Gets reports of the problems found in the environment's content types
    /// </summary>
    [JsonPropertyName("type_issues")]
    public List<TypeIssue> TypeIssues { get; init; }
}

