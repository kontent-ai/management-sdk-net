namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Represents the workflow response model.
/// </summary>
public sealed record WorkflowModel
{
    /// <summary>
    /// Gets the workflow's internal ID.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the workflow's name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the workflow's codename.
    /// </summary>
    [JsonPropertyName("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the workflow's scopes.
    /// </summary>
    [JsonPropertyName("scopes")]
    public IReadOnlyList<WorkflowScopeModel> Scopes { get; init; }

    /// <summary>
    /// Gets the workflow's steps.
    /// </summary>
    [JsonPropertyName("steps")]
    public IReadOnlyList<WorkflowStepModel> Steps { get; init; }

    /// <summary>
    /// Gets the workflow's Published step.
    /// </summary>
    [JsonPropertyName("published_step")]
    public WorkflowPublishedStepModel PublishedStep { get; init; }

    /// <summary>
    /// Gets the workflow's Scheduled step.
    /// </summary>
    [JsonPropertyName("scheduled_step")]
    public WorkflowScheduledStepModel ScheduledStep { get; init; }

    /// <summary>
    /// Gets the workflow's Archived step.
    /// </summary>
    [JsonPropertyName("archived_step")]
    public WorkflowArchivedStepModel ArchivedStep { get; init; }
}