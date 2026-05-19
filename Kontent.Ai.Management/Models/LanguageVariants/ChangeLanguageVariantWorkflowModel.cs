using Kontent.Ai.Management.Models.Shared;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.LanguageVariants;

/// <summary>
/// Represents a change language variant workflow model.
/// </summary>
public sealed record ChangeLanguageVariantWorkflowModel
{
    /// <summary>
    /// Represents the identifier of the workflow.
    /// </summary>
    [JsonPropertyName("workflow_identifier")]
    public Reference Workflow { get; init; }

    /// <summary>
    /// Represents the identifier of the step in the workflow.
    /// </summary>
    [JsonPropertyName("step_identifier")]
    public Reference Step { get; init; }

    /// <summary>
    /// Gets due date.
    /// </summary>
    [JsonPropertyName("due_date")]
    public DueDateModel DueDate { get; init; }

    /// <summary>
    /// Gets a note.
    /// </summary>
    [JsonPropertyName("note")]
    public string Note { get; init; }

    /// <summary>
    /// Gets the contributors.
    /// </summary>
    [JsonPropertyName("contributors")]
    public IEnumerable<UserIdentifier> Contributors { get; init; }

    /// <summary>
    /// Creates an instance of the change language variant workflow model.
    /// </summary>
    /// <param name="workflow">The identifier of the workflow.</param>
    /// <param name="step">The identifier of the workflow step.</param>
    public ChangeLanguageVariantWorkflowModel(Reference workflow, Reference step)
    {
        Workflow = workflow;
        Step = step;
    }
}
