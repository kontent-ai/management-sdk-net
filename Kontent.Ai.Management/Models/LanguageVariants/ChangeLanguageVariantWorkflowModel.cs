using System.Diagnostics.CodeAnalysis;

namespace Kontent.Ai.Management.Models.LanguageVariants;

/// <summary>
/// Payload for the change-workflow operation on a language variant. The API applies <see cref="DueDate"/>, <see cref="Note"/>, and <see cref="Contributors"/> alongside the workflow change when supplied.
/// </summary>
public sealed record ChangeLanguageVariantWorkflowModel
{
    /// <summary>
    /// Reference to the target workflow.
    /// </summary>
    [JsonPropertyName("workflow_identifier")]
    public required Reference Workflow { get; init; }

    /// <summary>
    /// Reference to the target step within the workflow.
    /// </summary>
    [JsonPropertyName("step_identifier")]
    public required Reference Step { get; init; }

    /// <summary>
    /// Due date to set. Optional.
    /// </summary>
    [JsonPropertyName("due_date")]
    public DueDateModel? DueDate { get; init; }

    /// <summary>
    /// Free-form note to set. Optional.
    /// </summary>
    [JsonPropertyName("note")]
    public string? Note { get; init; }

    /// <summary>
    /// Contributors to assign. Optional.
    /// </summary>
    [JsonPropertyName("contributors")]
    public IReadOnlyList<UserIdentifier>? Contributors { get; init; }

    /// <summary>
    /// Creates the payload targeting the given workflow and step; other properties are optional.
    /// </summary>
    [SetsRequiredMembers]
    public ChangeLanguageVariantWorkflowModel(Reference workflow, Reference step)
    {
        Workflow = workflow;
        Step = step;
    }
}
