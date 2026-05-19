using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.EnvironmentReport;

/// <summary>
/// Represents environment report model
/// </summary>
public sealed record EnvironmentReportModel
{
    /// <summary>
    /// Gets information about the specified environment
    /// </summary>
    [JsonProperty("project")]
    public Environment Environment { get; init; }

    /// <summary>
    /// Gets reports of the problems found in the environment's content
    /// </summary>
    [JsonProperty("variant_issues")]
    public List<VariantIssue> VariantIssues { get; init; }

    /// <summary>
    /// Gets reports of the problems found in the environment's content types
    /// </summary>
    [JsonProperty("type_issues")]
    public List<TypeIssue> TypeIssues { get; init; }
}

