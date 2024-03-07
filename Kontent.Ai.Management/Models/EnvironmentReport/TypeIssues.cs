using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.EnvironmentReport;

/// <summary>
/// Represents the report of the problems found in the environment's content types.
/// </summary>
public class TypeIssue
{
    /// <summary>
    /// Gets or sets information about content type
    /// </summary>
    [JsonProperty("type")]
    public Metadata Type { get; set; }

    /// <summary>
    /// Gets or sets information about environment language
    /// </summary>
    [JsonProperty("issues")]
    public IEnumerable<ElementIssue> Issues { get; set; }
}
