using System.Collections.Generic;
using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Workflow;

/// <summary>
/// Represents the workflow scope response model.
/// </summary>
public class WorkflowScopeModel
{
    /// <summary>
    /// Gets or sets the workflow scope's content types.
    /// </summary>
    [JsonProperty("content_types")]
    public IReadOnlyList<Reference> ContentTypes { get; set; }
}
