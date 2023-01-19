using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Represents the workflow scope upsert model.
/// </summary>
public class WorkflowScopeUpsertModel
{
    /// <summary>
    /// Gets or sets the workflow scope's collections.
    /// </summary>
    [JsonProperty("collections")]
    public IReadOnlyList<Reference> Collections { get; set; }
    
    /// <summary>
    /// Gets or sets the workflow scope's content types.
    /// </summary>
    [JsonProperty("content_types")]
    public IReadOnlyList<Reference> ContentTypes { get; set; }
}
