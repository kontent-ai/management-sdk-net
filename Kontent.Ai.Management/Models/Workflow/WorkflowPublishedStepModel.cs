using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Represents the Published workflow step response model.
/// </summary>
public class WorkflowPublishedStepModel
{
    /// <summary>
    /// Gets or sets the workflow step's internal ID.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the workflow step's name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the workflow step's codename.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; set; }

    /// <summary>
    /// Gets or sets the roles which can unpublish the item's variant.
    /// </summary>
    [JsonProperty("unpublish_role_ids")]
    public IReadOnlyCollection<Guid> UnpublishRoleIds { get; set; }

    /// <summary>
    /// Gets or sets the roles which can create new version from published variant.
    /// </summary>
    [JsonProperty("create_new_version_role_ids")]
    public IReadOnlyCollection<Guid> CreateNewVersionRoleIds { get; set; }
}
