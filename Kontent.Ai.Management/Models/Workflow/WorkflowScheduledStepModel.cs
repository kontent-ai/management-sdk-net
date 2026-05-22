using System;

namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Represents the Scheduled workflow step response model. If a variant is in this step, it indicated that it has been
/// scheduled for automated publishing some time in the future. Such variants are read-only.
/// </summary>
/// <remarks>
/// All its properties are predefined by the system and cannot be changed.
/// </remarks>
public sealed record WorkflowScheduledStepModel
{
    /// <summary>
    /// Gets the workflow step's internal ID.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the workflow step's name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the workflow step's codename.
    /// </summary>
    [JsonPropertyName("codename")]
    public string Codename { get; init; }
}