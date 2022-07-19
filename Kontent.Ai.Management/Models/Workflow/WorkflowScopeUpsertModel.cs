using System.Collections.Generic;
using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Represents the workflow scope upsert model.
/// </summary>
public class WorkflowScopeUpsertModel
{
    /// <summary>
    /// Gets or sets the workflow scope's content types.
    /// </summary>
    [JsonProperty("content_types")]
    public IReadOnlyList<Reference> ContentTypes { get; set; }
}
