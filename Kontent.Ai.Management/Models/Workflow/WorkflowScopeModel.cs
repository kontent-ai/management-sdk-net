using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Represents the workflow scope response model.
/// </summary>
public sealed record WorkflowScopeModel
{
    /// <summary>
    /// Gets the workflow scope's collections.
    /// </summary>
    [JsonProperty("collections")]
    public IReadOnlyList<Reference> Collections { get; init; }

    /// <summary>
    /// Gets the workflow scope's content types.
    /// </summary>
    [JsonProperty("content_types")]
    public IReadOnlyList<Reference> ContentTypes { get; init; }
}
