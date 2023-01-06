using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Workflow;

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
