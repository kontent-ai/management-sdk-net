using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Represents the Published workflow step upsert model.
/// </summary>
public sealed record WorkflowPublishedStepUpsertModel
{
    /// <summary>
    /// Gets the roles which can create new version from published variant.
    /// </summary>
    [JsonProperty("create_new_version_role_ids")]
    public IReadOnlyCollection<Guid> RoleCreateNewVersionIds { get; init; }

    /// <summary>
    /// Gets the roles which can unpublish the item's variant.
    /// </summary>
    [JsonProperty("unpublish_role_ids")]
    public IReadOnlyCollection<Guid> RolesUnpublishArchivedCancelSchedulingIds { get; init; }
}
