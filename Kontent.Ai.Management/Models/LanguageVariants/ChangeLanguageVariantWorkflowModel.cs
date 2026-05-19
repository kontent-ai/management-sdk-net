using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.LanguageVariants;

/// <summary>
/// Represents a change language variant workflow model.
/// </summary>
public sealed record ChangeLanguageVariantWorkflowModel
{
    /// <summary>
    /// Represents the identifier of the workflow.
    /// </summary>
    [JsonProperty("workflow_identifier")]
    public Reference Workflow { get; init; }

    /// <summary>
    /// Represents the identifier of the step in the workflow.
    /// </summary>
    [JsonProperty("step_identifier")]
    public Reference Step { get; init; }

    /// <summary>
    /// Gets due date.
    /// </summary>
    [JsonProperty("due_date")]
    public DueDateModel DueDate { get; init; }

    /// <summary>
    /// Gets a note.
    /// </summary>
    [JsonProperty("note")]
    public string Note { get; init; }

    /// <summary>
    /// Gets the contributors.
    /// </summary>
    [JsonProperty("contributors")]
    public IEnumerable<UserIdentifier> Contributors { get; init; }

    /// <summary>
    /// Creates an instance of the change language variant workflow model.
    /// </summary>
    /// <param name="workflowIdentifier">The identifier of the workflow.</param>
    /// <param name="stepIdentifier">The identifier of the workflow step.</param>
    public ChangeLanguageVariantWorkflowModel(Reference workflowIdentifier, Reference stepIdentifier)
    {
        Workflow = workflowIdentifier;
        Step = stepIdentifier;
    }
}
