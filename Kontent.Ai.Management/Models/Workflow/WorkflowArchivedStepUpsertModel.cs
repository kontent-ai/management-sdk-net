using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Represents the Archived workflow step upsert model.
/// </summary>
public sealed record WorkflowArchivedStepUpsertModel
{
    /// <summary>
    /// Gets the roles which can work with an item in this step.
    /// </summary>
    [JsonPropertyName("role_ids")]
    public IReadOnlyCollection<Guid> RoleIds { get; init; }
}
