namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// A workflow definition (response shape).
/// </summary>
public sealed record WorkflowModel
{
    /// <summary>
    /// Server-generated workflow ID.
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

    /// <summary>
    /// Scopes this workflow applies to. May be empty (workflow scoped to nothing).
    /// </summary>
    [JsonPropertyName("scopes")]
    public required IReadOnlyList<WorkflowScopeModel> Scopes { get; init; }

    /// <summary>
    /// Custom workflow steps.
    /// </summary>
    [JsonPropertyName("steps")]
    public required IReadOnlyList<WorkflowStepModel> Steps { get; init; }

    /// <summary>
    /// The Published system step.
    /// </summary>
    [JsonPropertyName("published_step")]
    public required WorkflowPublishedStepModel PublishedStep { get; init; }

    /// <summary>
    /// The Scheduled system step.
    /// </summary>
    [JsonPropertyName("scheduled_step")]
    public required WorkflowScheduledStepModel ScheduledStep { get; init; }

    /// <summary>
    /// The Archived system step.
    /// </summary>
    [JsonPropertyName("archived_step")]
    public required WorkflowArchivedStepModel ArchivedStep { get; init; }
}
