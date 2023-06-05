using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Represents the Archived workflow step response model. If a variant is in this step, it indicated that it might have
/// been published at some point, however, it is not needed anymore. Such variants are read-only.
/// </summary>
/// <remarks>
/// All <c>Id</c>, <c>Name</c>, and <c>Codename</c> properties are predefined by the system and cannot be changed.
/// </remarks>
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