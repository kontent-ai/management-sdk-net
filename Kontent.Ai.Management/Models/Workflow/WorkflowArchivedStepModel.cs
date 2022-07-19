using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Represents the Archived workflow step response model.
/// </summary>
public class WorkflowArchivedStepModel
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
    /// Gets or sets the roles which can work with an item in this step.
    /// </summary>
    [JsonProperty("role_ids")]
    public IReadOnlyCollection<Guid> RoleIds { get; set; }
}
