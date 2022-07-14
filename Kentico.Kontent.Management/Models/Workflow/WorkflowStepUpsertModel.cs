using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Workflow;

/// <summary>
/// Represents the custom workflow step upsert model.
/// </summary>
public class WorkflowStepUpsertModel
{
    /// <summary>
    /// Gets or sets the workflow step's identifier. 
    /// </summary>
    /// <remarks>
    /// Not applicable for creating a new workflow because the property is used
    /// to identify already existing steps within the edited workflow
    /// </remarks>
    [JsonProperty("id")]
    public Guid? Id { get; set; }

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
    /// Gets or sets the workflow step's color.
    /// </summary>
    [JsonProperty("color")]
    public WorkflowStepColorModel Color { get; set; }

    /// <summary>
    /// Gets or sets the workflow steps that this step can transition to.
    /// </summary>
    [JsonProperty("transitions_to")]
    public IReadOnlyList<WorkflowStepTransitionToUpsertModel> TransitionsTo { get; set; }

    /// <summary>
    /// Gets or sets the roles which can work with an item in this step.
    /// </summary>
    [JsonProperty("role_ids")]
    public IReadOnlyCollection<Guid> RoleIds { get; set; }
}
