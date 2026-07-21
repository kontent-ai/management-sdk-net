namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// The Scheduled system step on a workflow (response only). Variants in this step are awaiting automated publishing at a future time and are read-only.
/// </summary>
/// <remarks>
/// All properties are platform-defined and cannot be changed. Not part of the workflow upsert payload.
/// </remarks>
public sealed record WorkflowScheduledStepModel
{
    /// <summary>
    /// Server-generated step ID.
    /// </summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>
    /// Display name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Codename.
    /// </summary>
    [JsonPropertyName("codename")]
    public required string Codename { get; init; }
}
