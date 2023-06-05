using Newtonsoft.Json;
using System;

namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Represents the Scheduled workflow step response model. If a variant is in this step, it indicated that it has been
/// scheduled for automated publishing some time in the future. Such variants are read-only.
/// </summary>
/// <remarks>
/// All its properties are predefined by the system and cannot be changed.
/// </remarks>
public class WorkflowScheduledStepModel
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
}