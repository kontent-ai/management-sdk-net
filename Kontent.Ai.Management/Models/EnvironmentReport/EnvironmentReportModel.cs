using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.EnvironmentReport;

/// <summary>
/// Represents environment report model
/// </summary>
public sealed class EnvironmentReportModel
{
    /// <summary>
    /// Gets or sets information about the specified environment
    /// </summary>
    [JsonProperty("project")]
    public Environment Environment { get; set; }

    /// <summary>
    /// Gets or sets reports of the problems found in the environment's content
    /// </summary>
    [JsonProperty("variant_issues")]
    public List<VariantIssue> VariantIssues { get; set; }

    /// <summary>
    /// Gets or sets reports of the problems found in the environment's content types
    /// </summary>
    [JsonProperty("type_issues")]
    public List<TypeIssue> TypeIssues { get; set; }
}

